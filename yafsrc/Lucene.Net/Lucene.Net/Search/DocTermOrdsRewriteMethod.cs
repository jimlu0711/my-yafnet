using YAF.Lucene.Net.Diagnostics;
using System;
using System.Collections.Generic;

namespace YAF.Lucene.Net.Search
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

    using AtomicReaderContext = YAF.Lucene.Net.Index.AtomicReaderContext;
    using BytesRef = YAF.Lucene.Net.Util.BytesRef;
    using IBits = YAF.Lucene.Net.Util.IBits;
    using IndexReader = YAF.Lucene.Net.Index.IndexReader;
    using Int64BitSet = YAF.Lucene.Net.Util.Int64BitSet;
    using SortedSetDocValues = YAF.Lucene.Net.Index.SortedSetDocValues;
    using Terms = YAF.Lucene.Net.Index.Terms;
    using TermsEnum = YAF.Lucene.Net.Index.TermsEnum;

    /// <summary>
    /// Rewrites <see cref="MultiTermQuery"/>s into a filter, using DocTermOrds for term enumeration.
    /// <para>
    /// This can be used to perform these queries against an unindexed docvalues field.
    /// </para>
    /// @lucene.experimental
    /// </summary>
    public sealed class DocTermOrdsRewriteMethod : MultiTermQuery.RewriteMethod
    {
        public override Query Rewrite(IndexReader reader, MultiTermQuery query)
        {
            Query result = new ConstantScoreQuery(new MultiTermQueryDocTermOrdsWrapperFilter(query));
            result.Boost = query.Boost;
            return result;
        }

        internal class MultiTermQueryDocTermOrdsWrapperFilter : Filter
        {
            protected readonly MultiTermQuery m_query;

            /// <summary>
            /// Wrap a <see cref="MultiTermQuery"/> as a <see cref="Filter"/>.
            /// </summary>
            protected internal MultiTermQueryDocTermOrdsWrapperFilter(MultiTermQuery query)
            {
                this.m_query = query;
            }

            public override string ToString()
            {
                // query.toString should be ok for the filter, too, if the query boost is 1.0f
                return m_query.ToString();
            }

            public override sealed bool Equals(object o)
            {
                if (o == this)
                {
                    return true;
                }
                if (o is null)
                {
                    return false;
                }
                if (this.GetType().Equals(o.GetType()))
                {
                    return this.m_query.Equals(((MultiTermQueryDocTermOrdsWrapperFilter)o).m_query);
                }
                return false;
            }

            public override sealed int GetHashCode()
            {
                return m_query.GetHashCode();
            }

            /// <summary>
            /// Returns the field name for this query </summary>
            public string Field => m_query.Field;

            /// <summary>
            /// Returns a <see cref="DocIdSet"/> with documents that should be permitted in search
            /// results.
            /// </summary>
            public override DocIdSet GetDocIdSet(AtomicReaderContext context, IBits acceptDocs)
            {
                SortedSetDocValues docTermOrds = FieldCache.DEFAULT.GetDocTermOrds((context.AtomicReader), m_query.m_field);
                // Cannot use FixedBitSet because we require long index (ord):
                Int64BitSet termSet = new Int64BitSet(docTermOrds.ValueCount);
                TermsEnum termsEnum = m_query.GetTermsEnum(new TermsAnonymousClass(docTermOrds));

                if (Debugging.AssertsEnabled) Debugging.Assert(termsEnum != null);
                if (termsEnum.MoveNext())
                {
                    // fill into a bitset
                    do
                    {
                        termSet.Set(termsEnum.Ord);
                    } while (termsEnum.MoveNext());
                }
                else
                {
                    return null;
                }
                return new FieldCacheDocIdSet(context.Reader.MaxDoc, acceptDocs, (doc) =>
                {
                    docTermOrds.SetDocument(doc);
                    long ord;
                    // TODO: we could track max bit set and early terminate (since they come in sorted order)
                    while ((ord = docTermOrds.NextOrd()) != SortedSetDocValues.NO_MORE_ORDS)
                    {
                        if (termSet.Get(ord))
                        {
                            return true;
                        }
                    }
                    return false;
                });
            }

            private sealed class TermsAnonymousClass : Terms
            {
                private readonly SortedSetDocValues docTermOrds;

                public TermsAnonymousClass(SortedSetDocValues docTermOrds)
                {
                    this.docTermOrds = docTermOrds;
                }

                public override IComparer<BytesRef> Comparer => BytesRef.UTF8SortedAsUnicodeComparer;

                public override TermsEnum GetEnumerator()
                {
                    return docTermOrds.GetTermsEnum();
                }

                public override long SumTotalTermFreq => -1;

                public override long SumDocFreq => -1;

                public override int DocCount => -1;

                public override long Count => -1;

                public override bool HasFreqs => false;

                public override bool HasOffsets => false;

                public override bool HasPositions => false;

                public override bool HasPayloads => false;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj is null)
            {
                return false;
            }
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return 877;
        }
    }
}