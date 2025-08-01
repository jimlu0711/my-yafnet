﻿namespace YAF.Lucene.Net.Codecs
{
    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    using FieldInfos = YAF.Lucene.Net.Index.FieldInfos; // javadocs

    /// <summary>
    /// Encodes/decodes <see cref="FieldInfos"/>.
    /// <para/>
    /// @lucene.experimental
    /// </summary>
    public abstract class FieldInfosFormat
    {
        /// <summary>
        /// Sole constructor. (For invocation by subclass
        /// constructors, typically implicit.)
        /// </summary>
        protected FieldInfosFormat()
        {
        }

        /// <summary>
        /// Returns a <see cref="Codecs.FieldInfosReader"/> to read field infos
        /// from the index.
        /// </summary>
        public abstract FieldInfosReader FieldInfosReader { get; }

        /// <summary>
        /// Returns a <see cref="Codecs.FieldInfosWriter"/> to write field infos
        /// to the index.
        /// </summary>
        public abstract FieldInfosWriter FieldInfosWriter { get; }
    }
}