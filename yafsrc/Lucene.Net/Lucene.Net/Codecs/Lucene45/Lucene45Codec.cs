using System;
using System.Runtime.CompilerServices;

namespace YAF.Lucene.Net.Codecs.Lucene45
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

    using Lucene40LiveDocsFormat = YAF.Lucene.Net.Codecs.Lucene40.Lucene40LiveDocsFormat;
    using Lucene40SegmentInfoFormat = YAF.Lucene.Net.Codecs.Lucene40.Lucene40SegmentInfoFormat;
    using Lucene41StoredFieldsFormat = YAF.Lucene.Net.Codecs.Lucene41.Lucene41StoredFieldsFormat;
    using Lucene42FieldInfosFormat = YAF.Lucene.Net.Codecs.Lucene42.Lucene42FieldInfosFormat;
    using Lucene42NormsFormat = YAF.Lucene.Net.Codecs.Lucene42.Lucene42NormsFormat;
    using Lucene42TermVectorsFormat = YAF.Lucene.Net.Codecs.Lucene42.Lucene42TermVectorsFormat;
    using PerFieldDocValuesFormat = YAF.Lucene.Net.Codecs.PerField.PerFieldDocValuesFormat;
    using PerFieldPostingsFormat = YAF.Lucene.Net.Codecs.PerField.PerFieldPostingsFormat;

    /// <summary>
    /// Implements the Lucene 4.5 index format, with configurable per-field postings
    /// and docvalues formats.
    /// <para/>
    /// If you want to reuse functionality of this codec in another codec, extend
    /// <see cref="FilterCodec"/>.
    /// <para/>
    /// See <see cref="Lucene.Net.Codecs.Lucene45"/> package documentation for file format details.
    /// <para/>
    /// @lucene.experimental 
    /// </summary>
    // NOTE: if we make largish changes in a minor release, easier to just make Lucene46Codec or whatever
    // if they are backwards compatible or smallish we can probably do the backwards in the postingsreader
    // (it writes a minor version, etc).
    [Obsolete("Only for reading old 4.3-4.5 segments")]
    [CodecName("Lucene45")] // LUCENENET specific - using CodecName attribute to ensure the default name passed from subclasses is the same as this class name
    public class Lucene45Codec : Codec
    {
        private readonly StoredFieldsFormat fieldsFormat = new Lucene41StoredFieldsFormat();
        private readonly TermVectorsFormat vectorsFormat = new Lucene42TermVectorsFormat();
        private readonly FieldInfosFormat fieldInfosFormat = new Lucene42FieldInfosFormat();
        private readonly SegmentInfoFormat infosFormat = new Lucene40SegmentInfoFormat();
        private readonly LiveDocsFormat liveDocsFormat = new Lucene40LiveDocsFormat();

        private readonly PostingsFormat postingsFormat;

        private sealed class PerFieldPostingsFormatAnonymousClass : PerFieldPostingsFormat
        {
            private readonly Lucene45Codec outerInstance;

            public PerFieldPostingsFormatAnonymousClass(Lucene45Codec outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override PostingsFormat GetPostingsFormatForField(string field)
            {
                return outerInstance.GetPostingsFormatForField(field);
            }
        }

        private readonly DocValuesFormat docValuesFormat;

        private sealed class PerFieldDocValuesFormatAnonymousClass : PerFieldDocValuesFormat
        {
            private readonly Lucene45Codec outerInstance;

            public PerFieldDocValuesFormatAnonymousClass(Lucene45Codec outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override DocValuesFormat GetDocValuesFormatForField(string field)
            {
                return outerInstance.GetDocValuesFormatForField(field);
            }
        }

        /// <summary>
        /// Sole constructor. </summary>
        public Lucene45Codec()
            : base()
        {
            postingsFormat = new PerFieldPostingsFormatAnonymousClass(this);
            docValuesFormat = new PerFieldDocValuesFormatAnonymousClass(this);
        }

        public override sealed StoredFieldsFormat StoredFieldsFormat => fieldsFormat;

        public override sealed TermVectorsFormat TermVectorsFormat => vectorsFormat;

        public override sealed PostingsFormat PostingsFormat => postingsFormat;

        public override FieldInfosFormat FieldInfosFormat => fieldInfosFormat;

        public override SegmentInfoFormat SegmentInfoFormat => infosFormat;

        public override sealed LiveDocsFormat LiveDocsFormat => liveDocsFormat;

        /// <summary>
        /// Returns the postings format that should be used for writing
        /// new segments of <paramref name="field"/>.
        /// <para/>
        /// The default implementation always returns "Lucene41"
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual PostingsFormat GetPostingsFormatForField(string field)
        {
            // LUCENENET specific - lazy initialize the codec to ensure we get the correct type if overridden.
            if (defaultFormat is null)
            {
                defaultFormat = Codecs.PostingsFormat.ForName("Lucene41");
            }
            return defaultFormat;
        }

        /// <summary>
        /// Returns the docvalues format that should be used for writing
        /// new segments of <paramref name="field"/>.
        /// <para/>
        /// The default implementation always returns "Lucene45"
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual DocValuesFormat GetDocValuesFormatForField(string field)
        {
            // LUCENENET specific - lazy initialize the codec to ensure we get the correct type if overridden.
            if (defaultDVFormat is null)
            {
                defaultDVFormat = Codecs.DocValuesFormat.ForName("Lucene45");
            }
            return defaultDVFormat;
        }

        public override sealed DocValuesFormat DocValuesFormat => docValuesFormat;

        // LUCENENET specific - lazy initialize the codecs to ensure we get the correct type if overridden.
        private PostingsFormat defaultFormat;
        private DocValuesFormat defaultDVFormat;

        private readonly NormsFormat normsFormat = new Lucene42NormsFormat();

        public override sealed NormsFormat NormsFormat => normsFormat;
    }
}