﻿/* Yet Another Forum.NET
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

namespace YAF.Core.Helpers;

using System;

using YAF.Types.Models;

/// <summary>
/// The user helper.
/// </summary>
public static class UserHelper
{
    /// <summary>
    /// Gets the user language file.
    /// </summary>
    /// <param name="user">
    /// The user.
    /// </param>
    /// <returns>
    /// language file name. If null -- use default language
    /// </returns>
    public static string GetUserLanguageFile(User user)
    {
        if (user != null && user.LanguageFile.IsSet() && BoardContext.Current.BoardSettings.AllowUserLanguage)
        {
            return user.LanguageFile;
        }

        return null;
    }

    /// <summary>
    /// Gets the Guest User Language File based on the current Browser Language
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public static string GetGuestUserLanguageFile()
    {
        var languages = BoardContext.Current.Get<IDataCache>().GetOrSet(
            "Languages",
            StaticDataHelper.NeutralCultures,
            TimeSpan.FromDays(30));

        var languageRow = languages.FirstOrDefault(
            row => row.CultureTag.Equals(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));

        return languageRow != null ? languageRow.CultureFile : BoardContext.Current.BoardSettings.Language;
    }
}