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
namespace YAF.Types.Objects;

using YAF.Types.Constants;

/// <summary>
/// The Message Notification
/// </summary>
[Serializable]
public class MessageNotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageNotification"/> class.
    /// </summary>
    public MessageNotification()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageNotification"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="messageType">Type of the message.</param>
    public MessageNotification(string message, MessageTypes messageType)
    {
        this.Message = message;
        this.MessageType = messageType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageNotification"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="messageType">Type of the message.</param>
    public MessageNotification(string message, MessageTypes messageType, string script)
    {
        this.Message = message;
        this.MessageType = messageType;
        this.Script = script;
    }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>
    /// The message.
    /// </value>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the type of the message.
    /// </summary>
    /// <value>
    /// The type of the message.
    /// </value>
    public MessageTypes MessageType { get; set; }

    public string Script { get; set; }
}