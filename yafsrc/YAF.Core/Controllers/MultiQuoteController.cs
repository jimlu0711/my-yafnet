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

namespace YAF.Core.Controllers;

using System.Collections.Generic;
using System.Web.Http;

using YAF.Types.Objects;

/// <summary>
/// The YAF MultiQuote Button controller.
/// </summary>
[RoutePrefix("api")]
public class MultiQuoteController : ApiController, IHaveServiceLocator
{
    /// <summary>
    ///   Gets ServiceLocator.
    /// </summary>
    public IServiceLocator ServiceLocator => BoardContext.Current.ServiceLocator;

    /// <summary>
    /// Handles the multi quote Button.
    /// </summary>
    /// <param name="quoteButton">
    /// The quote Button.
    /// </param>
    /// <returns>
    /// Returns the Message Id and the Updated CSS Class for the Button
    /// </returns>
    [Route("MultiQuote/HandleMultiQuote")]
    [HttpPost]
    public IHttpActionResult HandleMultiQuote(MultiQuoteButton quoteButton)
    {
        var buttonId = quoteButton.ButtonId;
        var isMultiQuoteButton = quoteButton.IsMultiQuoteButton;
        var messageId = quoteButton.MessageId;
        var topicId = quoteButton.TopicId;
        var buttonCssClass = quoteButton.ButtonCssClass;

        var yafSession = this.Get<ISession>();

        var multiQuote = new MultiQuote { MessageID = messageId, TopicID = topicId };

        if (isMultiQuoteButton)
        {
            if (yafSession.MultiQuoteIds != null)
            {
                if (!yafSession.MultiQuoteIds.Exists(m => m.MessageID.Equals(messageId)))
                {
                    yafSession.MultiQuoteIds.Add(multiQuote);
                }
            }
            else
            {
                yafSession.MultiQuoteIds = [multiQuote];
            }

            buttonCssClass += " Checked";
        }
        else
        {
            if (yafSession.MultiQuoteIds != null
                && yafSession.MultiQuoteIds.Exists(m => m.MessageID.Equals(messageId)))
            {
                yafSession.MultiQuoteIds.Remove(multiQuote);
            }

            buttonCssClass = "btn-multiquote form-check btn btn-link";
        }

        return this.Ok(new ReturnClass { Id = buttonId, NewTitle = buttonCssClass });
    }
}