/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2025 Ingo Herbote
 * https://www.yetanotherforum.net/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * https://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Core.Handlers;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.SessionState;

using YAF.Core.Model;
using YAF.Types.Models;
using YAF.Types.Objects;

using MimeTypes = Utilities.MimeTypes;

/// <summary>
/// The File Upload Handler
/// </summary>
public class FileUploader : IHttpHandler, IReadOnlySessionState, IHaveServiceLocator
{
    /// <summary>
    /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
    /// </summary>
    public bool IsReusable => false;

    /// <summary>
    /// Gets the ServiceLocator.
    /// </summary>
    public IServiceLocator ServiceLocator => BoardContext.Current.ServiceLocator;

    /// <summary>
    /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
    /// </summary>
    /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
    public void ProcessRequest(HttpContext context)
    {
        context.Response.AddHeader("Pragma", "no-cache");
        context.Response.AddHeader("Cache-Control", "private, no-cache");

        this.HandleMethod(context);
    }

    /// <summary>
    /// Returns the options.
    /// </summary>
    /// <param name="context">The context.</param>
    private static void ReturnOptions(HttpContext context)
    {
        context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
        context.Response.StatusCode = 200;
    }

    /// <summary>
    /// Writes the JSON iFrame safe.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="statuses">The statuses.</param>
    private static void WriteJsonIframeSafe(HttpContext context, List<FilesUploadStatus> statuses)
    {
        context.Response.AddHeader("Vary", "Accept");

        context.Response.ContentType = "application/json";

        var jsonObject = new JavaScriptSerializer().Serialize(statuses.ToArray());
        context.Response.Write(jsonObject);
    }

    /// <summary>
    /// Handle request based on method
    /// </summary>
    /// <param name="context">The context.</param>
    private void HandleMethod(HttpContext context)
    {
        switch (context.Request.HttpMethod)
        {
            case "POST":
            case "PUT":
                this.UploadFile(context);
                break;

            case "OPTIONS":
                ReturnOptions(context);
                break;

            default:
                context.Response.ClearHeaders();
                context.Response.StatusCode = 405;
                break;
        }
    }

    /// <summary>
    /// Uploads the file.
    /// </summary>
    /// <param name="context">The context.</param>
    private void UploadFile(HttpContext context)
    {
        var statuses = new List<FilesUploadStatus>();

        this.UploadWholeFile(context, statuses);

        WriteJsonIframeSafe(context, statuses);
    }

    /// <summary>
    /// Uploads the whole file.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="statuses">The statuses.</param>
    private void UploadWholeFile(HttpContext context, ICollection<FilesUploadStatus> statuses)
    {
            var yafUserId = BoardContext.Current.PageUserID;
            var uploadFolder = BoardContext.Current.Get<BoardFolders>().Uploads;

            if (!BoardContext.Current.UploadAccess)
            {
                throw new HttpRequestValidationException("No Access");
            }

            try
            {
                var allowedExtensions = this.Get<BoardSettings>().AllowedFileExtensions.ToLower().Split(',');

                for (var i = 0; i < context.Request.Files.Count; i++)
                {
                    var file = context.Request.Files[i];

                    // *** 關鍵修改點 1：將原始 InputStream 複製到 MemoryStream ***
                    // 這樣原始數據就可以被多次讀取
                    using (var originalFileMemoryStream = new MemoryStream())
                    {
                        file.InputStream.CopyTo(originalFileMemoryStream);
                        originalFileMemoryStream.Position = 0; // 重置 MemoryStream 的讀取位置到開頭

                        var fileName = Path.GetFileName(file.FileName);

                        var extension = Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

                        if (!allowedExtensions.Contains(extension))
                        {
                            statuses.Add(
                                new FilesUploadStatus
                                {
                                    error = "Invalid File"
                                });

                            return;
                        }

                        if (!MimeTypes.FileMatchContentType(file))
                        {
                            statuses.Add(
                                new FilesUploadStatus
                                {
                                    error = "Invalid File"
                                });

                            return;
                        }

                        if (fileName.IsSet())
                        {
                            // Check for Illegal Chars
                            if (FileHelper.ValidateFileName(fileName))
                            {
                                fileName = FileHelper.CleanFileName(fileName);
                            }
                        }
                        else
                        {
                            statuses.Add(
                                new FilesUploadStatus
                                {
                                    error = "File does not have a name"
                                });

                            return;
                        }

                        if (fileName.Length > 220)
                        {
                            fileName = fileName.Substring(fileName.Length - 220);
                        }

                        // verify the size of the attachment
                        // 注意：這裡的 file.ContentLength 仍然是原始檔案的大小，用於判斷
                        if (this.Get<BoardSettings>().MaxFileSize > 0
                            && file.ContentLength > this.Get<BoardSettings>().MaxFileSize)
                        {
                            statuses.Add(new FilesUploadStatus
                            {
                                error = this.Get<ILocalization>().GetTextFormatted(
                                    "UPLOAD_TOOBIG",
                                    file.ContentLength / 1024,
                                    this.Get<BoardSettings>().MaxFileSize / 1024)
                            });

                            return;
                        }

                        Stream resized = null; // 儲存調整大小後的圖像流

                        try
                        {
                            // *** 關鍵修改點 2：圖像處理使用複製後的 MemoryStream ***
                            originalFileMemoryStream.Position = 0; // 確保每次讀取時都從頭開始
                            using var img = Image.FromStream(originalFileMemoryStream);

                            if (img.Width > this.Get<BoardSettings>().ImageAttachmentResizeWidth ||
                                img.Height > this.Get<BoardSettings>().ImageAttachmentResizeHeight)
                            {
                                resized = ImageHelper.GetResizedImageStreamFromImage(img,
                                    this.Get<BoardSettings>().ImageAttachmentResizeWidth,
                                    this.Get<BoardSettings>().ImageAttachmentResizeHeight);
                            }
                        }
                        catch (Exception)
                        {
                            // 如果圖像處理失敗，將 resized 設為 null，表示不進行圖像處理
                            // 這裡的 catch 塊是原始程式碼的，如果需要詳細錯誤，可以記錄到日誌
                            resized = null;
                        }

                        int newAttachmentId = 0; // *** 這裡添加了初始值 ***

                        if (this.Get<BoardSettings>().UseFileTable)
                        {
                            // 這裡的邏輯是將檔案數據儲存到資料庫檔案表
                            if (resized is null)
                            {
                                // 如果沒有調整大小，從原始 MemoryStream 讀取數據
                                originalFileMemoryStream.Position = 0; // 重置位置
                                byte[] fileData = originalFileMemoryStream.ToArray(); // 將 MemoryStream 轉為 byte 陣列

                                newAttachmentId = this.GetRepository<Attachment>().Save(
                                    yafUserId,
                                    fileName,
                                    fileData.Length.ToType<int>(),
                                    file.ContentType,
                                    fileData.ToArray());
                            }
                            else
                            {
                                // 如果有調整大小，從 resized Stream 讀取數據
                                resized.Position = 0; // 重置位置
                                byte[] fileData = ((MemoryStream)resized).ToArray(); // 假設 resized 也是 MemoryStream

                                newAttachmentId = this.GetRepository<Attachment>().Save(
                                    yafUserId,
                                    fileName,
                                    fileData.Length.ToType<int>(), // 使用 fileData.Length
                                    file.ContentType,
                                    fileData.ToArray());

                                resized.Dispose(); // 釋放資源
                            }
                        }
                        else
                        {
                            // 這裡的邏輯是將檔案儲存到磁碟 (你的問題點所在分支)
                            var previousDirectory = this.Get<HttpRequestBase>()
                                .MapPath(Path.Combine(BaseUrlBuilder.ServerFileRoot, uploadFolder));

                            // check if Uploads folder exists
                            if (!Directory.Exists(previousDirectory))
                            {
                                Directory.CreateDirectory(previousDirectory);
                            }

                            // 先保存 Attachment 記錄到資料庫，獲取 newAttachmentId
                            // 這裡假設 Save 方法會返回 ID
                            newAttachmentId = this.GetRepository<Attachment>().Save(
                                yafUserId,
                                fileName,
                                file.ContentLength, // 這裡使用原始檔案長度
                                file.ContentType);

                            // *** 關鍵修改點 3：使用 try-catch 捕獲 FileStream 創建或寫入時的異常 ***
                            try
                            {
                                using (var fs = new FileStream(
                                           $"{previousDirectory}/u{yafUserId}-{newAttachmentId}.{fileName}.yafupload",
                                           FileMode.Create,
                                           FileAccess.Write)) // 使用 FileAccess.Write 即可
                                {
                                    if (resized is null)
                                    {
                                        // 儲存原始檔案到磁碟
                                        originalFileMemoryStream.Position = 0; // 確保流從頭開始讀取
                                        originalFileMemoryStream.CopyTo(fs); // 將 MemoryStream 內容寫入檔案
                                    }
                                    else
                                    {
                                        // 儲存調整大小後的檔案到磁碟
                                        resized.Position = 0; // 確保流從頭開始讀取
                                        resized.CopyTo(fs); // 將 resized 流的內容寫入檔案
                                        resized.Dispose(); // 確保 resized 資源被釋放
                                    }
                                }
                                // 如果執行到這裡，表示檔案寫入成功
                                this.Get<ILoggerService>().Info($"File saved successfully to disk: {previousDirectory}/u{yafUserId}-{newAttachmentId}.{fileName}.yafupload");
                            }
                            catch (Exception ex)
                            {
                                // 捕獲任何在 FileStream 創建或寫入時發生的異常
                                statuses.Add(new FilesUploadStatus { error = $"File write error: {ex.Message}" });
                                // *** 這裡非常重要：將完整的異常訊息寫入日誌 ***
                                this.Get<ILoggerService>().Error(ex, $"Error writing file to disk: {previousDirectory}/u{yafUserId}-{newAttachmentId}.{fileName}.yafupload. Full Exception: {ex.ToString()}");

                                return; // 確保在發生錯誤時退出
                            }
                        }

                        var fullName = Path.GetFileName(fileName);
                        statuses.Add(new FilesUploadStatus(fullName, file.ContentLength, newAttachmentId));
                    } // originalFileMemoryStream 的 using 塊結束
                }
            }
            catch (Exception ex)
            {
                statuses.Add(new FilesUploadStatus
                {
                    error = ex.Message
                });

                this.Get<ILoggerService>().Error(ex, $"Error during Attachment upload. Full Exception: {ex.ToString()}");
            }
    }
}