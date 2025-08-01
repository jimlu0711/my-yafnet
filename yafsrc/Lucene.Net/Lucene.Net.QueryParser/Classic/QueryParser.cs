﻿using YAF.Lucene.Net.Analysis;
using YAF.Lucene.Net.Search;
using YAF.Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
#if FEATURE_SERIALIZABLE_EXCEPTIONS
using System.ComponentModel;
using System.Runtime.Serialization;
#endif
using JCG = J2N.Collections.Generic;

namespace YAF.Lucene.Net.QueryParsers.Classic
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

    /// <summary> This class is generated by JavaCC.  The most important method is
    /// <see cref="QueryParserBase.Parse(string)" />.
    /// <para/>
    /// The syntax for query strings is as follows:
    /// A Query is a series of clauses.
    /// A clause may be prefixed by:
    /// <list type="bullet">
    /// <item><description> a plus (<c>+</c>) or a minus (<c>-</c>) sign, indicating
    /// that the clause is required or prohibited respectively; or</description></item>
    /// <item><description> a term followed by a colon, indicating the field to be searched.
    /// This enables one to construct queries which search multiple fields.</description></item>
    /// </list>
    ///
    /// <para/>
    /// A clause may be either:
    /// <list type="bullet">
    /// <item><description> a term, indicating all the documents that contain this term; or</description></item>
    /// <item><description> a nested query, enclosed in parentheses.  Note that this may be used
    /// with a <c>+</c>/<c>-</c> prefix to require any of a set of
    /// terms.</description></item>
    /// </list>
    ///
    /// <para/>
    /// Thus, in BNF, the query grammar is:
    /// <code>
    ///     Query  ::= ( Clause )*
    ///     Clause ::= ["+", "-"] [&lt;TERM&gt; ":"] ( &lt;TERM&gt; | "(" Query ")" )
    /// </code>
    ///
    /// <para>
    /// Examples of appropriately formatted queries can be found in the <a
    /// href="../../../../../../queryparsersyntax.html">query syntax
    /// documentation</a>.
    /// </para>
    ///
    /// <para>
    /// In <see cref="TermRangeQuery" />s, QueryParser tries to detect date values, e.g.
    /// <tt>date:[6/1/2005 TO 6/4/2005]</tt> produces a range query that searches
    /// for "date" fields between 2005-06-01 and 2005-06-04. Note that the format
    /// of the accepted input depends on the <see cref="System.Globalization.CultureInfo" />.
    /// A <see cref="Documents.DateResolution" /> has to be set,
    /// if you want to use <see cref="Documents.DateTools"/> for date conversion.<p/>
    /// </para>
    /// <para>
    /// The date resolution that shall be used for RangeQueries can be set
    /// using <see cref="QueryParserBase.SetDateResolution(Documents.DateResolution)" />
    /// or <see cref="QueryParserBase.SetDateResolution(string, Documents.DateResolution)" />. The former
    /// sets the default date resolution for all fields, whereas the latter can
    /// be used to set field specific date resolutions. Field specific date
    /// resolutions take, if set, precedence over the default date resolution.
    /// </para>
    /// <para>
    /// If you don't use <see cref="Documents.DateTools" /> in your index, you can create your own
    /// query parser that inherits <see cref="QueryParser"/> and overwrites
    /// <see cref="QueryParserBase.GetRangeQuery(string, string, string, bool, bool)" /> to
    /// use a different method for date conversion.
    /// </para>
    ///
    /// <para>Note that <see cref="QueryParser"/> is <em>not</em> thread-safe.</para>
    ///
    /// <para><b>NOTE</b>: there is a new QueryParser in contrib, which matches
    /// the same syntax as this class, but is more modular,
    /// enabling substantial customization to how a query is created.
    /// </para>
    ///
    /// <b>NOTE</b>: You must specify the required <see cref="LuceneVersion" /> compatibility when
    /// creating QueryParser:
    /// <list type="bullet">
    /// <item><description>As of 3.1, <see cref="QueryParserBase.AutoGeneratePhraseQueries"/> is false by default.</description></item>
    /// </list>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This class is based on generated code")]
    public class QueryParser : QueryParserBase
    {
        // NOTE: This was moved into the QueryParserBase class.

        // * The default operator for parsing queries.
        // * Use <see cref="QueryParser.DefaultOperator"/> to change it.
        // */

        //public enum Operator
        //{
        //    OR,
        //    AND
        //}

        /// <summary>
        /// Constructs a query parser.
        /// </summary>
        /// <param name="matchVersion">Lucene version to match.</param>
        /// <param name="f">the default field for query terms.</param>
        /// <param name="a">used to find terms in the query text.</param>
        public QueryParser(LuceneVersion matchVersion, string f, Analyzer a)
            : this(new FastCharStream(new StringReader("")))
        {
            Init(matchVersion, f, a);
        }

        // *   Query  ::= ( Clause )*
        // *   Clause ::= ["+", "-"] [<TermToken> ":"] ( <TermToken> | "(" Query ")" )
        public int Conjunction()
        {
            int ret = CONJ_NONE;
            switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
            {
                case RegexpToken.AND:
                case RegexpToken.OR:
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.AND:
                            Jj_consume_token(RegexpToken.AND);
                            ret = CONJ_AND;
                            break;
                        case RegexpToken.OR:
                            Jj_consume_token(RegexpToken.OR);
                            ret = CONJ_OR;
                            break;
                        default:
                            jj_la1[0] = jj_gen;
                            Jj_consume_token(-1);
                            throw new ParseException();
                    }
                    break;
                default:
                    jj_la1[1] = jj_gen;
                    break;
            }
            {
                if (true) return ret;
            }
            throw Error.Create("Missing return statement in function");
        }

        public int Modifiers()
        {
            int ret = MOD_NONE;
            switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
            {
                case RegexpToken.NOT:
                case RegexpToken.PLUS:
                case RegexpToken.MINUS:
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.PLUS:
                            Jj_consume_token(RegexpToken.PLUS);
                            ret = MOD_REQ;
                            break;
                        case RegexpToken.MINUS:
                            Jj_consume_token(RegexpToken.MINUS);
                            ret = MOD_NOT;
                            break;
                        case RegexpToken.NOT:
                            Jj_consume_token(RegexpToken.NOT);
                            ret = MOD_NOT;
                            break;
                        default:
                            jj_la1[2] = jj_gen;
                            Jj_consume_token(-1);
                            throw new ParseException();
                    }
                    break;
                default:
                    jj_la1[3] = jj_gen;
                    break;
            }
            {
                if (true) return ret;
            }
            throw Error.Create("Missing return statement in function");
        }

        // This makes sure that there is no garbage after the query string
        public override sealed Query TopLevelQuery(string field)
        {
            Query q;
            q = Query(field);
            Jj_consume_token(0);
            {
                if (true) return q;
            }
            throw Error.Create("Missing return statement in function");
        }

        public Query Query(string field)
        {
            IList<BooleanClause> clauses = new JCG.List<BooleanClause>();
            Query q, firstQuery = null;
            int conj, mods;
            mods = Modifiers();
            q = Clause(field);
            AddClause(clauses, CONJ_NONE, mods, q);
            if (mods == MOD_NONE)
                firstQuery = q;
            while (true)
            {
                switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                {
                    case RegexpToken.AND:
                    case RegexpToken.OR:
                    case RegexpToken.NOT:
                    case RegexpToken.PLUS:
                    case RegexpToken.MINUS:
                    case RegexpToken.BAREOPER:
                    case RegexpToken.LPAREN:
                    case RegexpToken.STAR:
                    case RegexpToken.QUOTED:
                    case RegexpToken.TERM:
                    case RegexpToken.PREFIXTERM:
                    case RegexpToken.WILDTERM:
                    case RegexpToken.REGEXPTERM:
                    case RegexpToken.RANGEIN_START:
                    case RegexpToken.RANGEEX_START:
                    case RegexpToken.NUMBER:
                        break;
                    default:
                        jj_la1[4] = jj_gen;
                        goto label_1;
                }

                conj = Conjunction();
                mods = Modifiers();
                q = Clause(field);
                AddClause(clauses, conj, mods, q);
            }

        label_1:

            if (clauses.Count == 1 && firstQuery != null)
            {
                if (true) return firstQuery;
            }

            if (true) return GetBooleanQuery(clauses);
            throw Error.Create("Missing return statement in function");
        }

        public Query Clause(string field)
        {
            Query q;
            Token fieldToken, boost = null; // LUCENENET: IDE0059: Remove unnecessary value assignment
            if (Jj_2_1(2))
            {
                switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                {
                    case RegexpToken.TERM:
                        fieldToken = Jj_consume_token(RegexpToken.TERM);
                        Jj_consume_token(RegexpToken.COLON);
                        field = DiscardEscapeChar(fieldToken.Image);
                        break;
                    case RegexpToken.STAR:
                        Jj_consume_token(RegexpToken.STAR);
                        Jj_consume_token(RegexpToken.COLON);
                        field = "*";
                        break;
                    default:
                        jj_la1[5] = jj_gen;
                        Jj_consume_token(-1);
                        throw new ParseException();
                }
            }
            else
            {
                /* LUCENENET: intentionally blank */
            }
            switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
            {
                case RegexpToken.BAREOPER:
                case RegexpToken.STAR:
                case RegexpToken.QUOTED:
                case RegexpToken.TERM:
                case RegexpToken.PREFIXTERM:
                case RegexpToken.WILDTERM:
                case RegexpToken.REGEXPTERM:
                case RegexpToken.RANGEIN_START:
                case RegexpToken.RANGEEX_START:
                case RegexpToken.NUMBER:
                    q = Term(field);
                    break;
                case RegexpToken.LPAREN:
                    Jj_consume_token(RegexpToken.LPAREN);
                    q = Query(field);
                    Jj_consume_token(RegexpToken.RPAREN);
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.CARAT:
                            Jj_consume_token(RegexpToken.CARAT);
                            boost = Jj_consume_token(RegexpToken.NUMBER);
                            break;
                        default:
                            jj_la1[6] = jj_gen;
                            break;
                    }
                    break;
                default:
                    jj_la1[7] = jj_gen;
                    Jj_consume_token(-1);
                    throw new ParseException();
            }
            {
                if (true) return HandleBoost(q, boost);
            }
            throw Error.Create("Missing return statement in function");
        }

        public Query Term(string field)
        {
            Token term, boost = null, fuzzySlop = null, goop1, goop2;
            bool prefix = false;
            bool wildcard = false;
            bool fuzzy = false;
            bool regexp = false;
            bool startInc = false;
            bool endInc = false;
            Query q;
            switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
            {
                case RegexpToken.BAREOPER:
                case RegexpToken.STAR:
                case RegexpToken.TERM:
                case RegexpToken.PREFIXTERM:
                case RegexpToken.WILDTERM:
                case RegexpToken.REGEXPTERM:
                case RegexpToken.NUMBER:
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.TERM:
                            term = Jj_consume_token(RegexpToken.TERM);
                            break;
                        case RegexpToken.STAR:
                            term = Jj_consume_token(RegexpToken.STAR);
                            wildcard = true;
                            break;
                        case RegexpToken.PREFIXTERM:
                            term = Jj_consume_token(RegexpToken.PREFIXTERM);
                            prefix = true;
                            break;
                        case RegexpToken.WILDTERM:
                            term = Jj_consume_token(RegexpToken.WILDTERM);
                            wildcard = true;
                            break;
                        case RegexpToken.REGEXPTERM:
                            term = Jj_consume_token(RegexpToken.REGEXPTERM);
                            regexp = true;
                            break;
                        case RegexpToken.NUMBER:
                            term = Jj_consume_token(RegexpToken.NUMBER);
                            break;
                        case RegexpToken.BAREOPER:
                            term = Jj_consume_token(RegexpToken.BAREOPER);
                            term.Image = term.Image.Substring(0, 1);
                            break;
                        default:
                            jj_la1[8] = jj_gen;
                            Jj_consume_token(-1);
                            throw new ParseException();
                    }
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.FUZZY_SLOP:
                            fuzzySlop = Jj_consume_token(RegexpToken.FUZZY_SLOP);
                            fuzzy = true;
                            break;
                        default:
                            jj_la1[9] = jj_gen;
                            break;
                    }
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.CARAT:
                            Jj_consume_token(RegexpToken.CARAT);
                            boost = Jj_consume_token(RegexpToken.NUMBER);
                            switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                            {
                                case RegexpToken.FUZZY_SLOP:
                                    fuzzySlop = Jj_consume_token(RegexpToken.FUZZY_SLOP);
                                    fuzzy = true;
                                    break;
                                default:
                                    jj_la1[10] = jj_gen;
                                    break;
                            }
                            break;
                        default:
                            jj_la1[11] = jj_gen;
                            break;
                    }
                    q = HandleBareTokenQuery(field, term, fuzzySlop, prefix, wildcard, fuzzy, regexp);
                    break;
                case RegexpToken.RANGEIN_START:
                case RegexpToken.RANGEEX_START:
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.RANGEIN_START:
                            Jj_consume_token(RegexpToken.RANGEIN_START);
                            startInc = true;
                            break;
                        case RegexpToken.RANGEEX_START:
                            Jj_consume_token(RegexpToken.RANGEEX_START);
                            break;
                        default:
                            jj_la1[12] = jj_gen;
                            Jj_consume_token(-1);
                            throw new ParseException();
                    }
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.RANGE_GOOP:
                            goop1 = Jj_consume_token(RegexpToken.RANGE_GOOP);
                            break;
                        case RegexpToken.RANGE_QUOTED:
                            goop1 = Jj_consume_token(RegexpToken.RANGE_QUOTED);
                            break;
                        default:
                            jj_la1[13] = jj_gen;
                            Jj_consume_token(-1);
                            throw new ParseException();
                    }
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.RANGE_TO:
                            Jj_consume_token(RegexpToken.RANGE_TO);
                            break;
                        default:
                            jj_la1[14] = jj_gen;
                            break;
                    }
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.RANGE_GOOP:
                            goop2 = Jj_consume_token(RegexpToken.RANGE_GOOP);
                            break;
                        case RegexpToken.RANGE_QUOTED:
                            goop2 = Jj_consume_token(RegexpToken.RANGE_QUOTED);
                            break;
                        default:
                            jj_la1[15] = jj_gen;
                            Jj_consume_token(-1);
                            throw new ParseException();
                    }
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.RANGEIN_END:
                            Jj_consume_token(RegexpToken.RANGEIN_END);
                            endInc = true;
                            break;
                        case RegexpToken.RANGEEX_END:
                            Jj_consume_token(RegexpToken.RANGEEX_END);
                            break;
                        default:
                            jj_la1[16] = jj_gen;
                            Jj_consume_token(-1);
                            throw new ParseException();
                    }
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.CARAT:
                            Jj_consume_token(RegexpToken.CARAT);
                            boost = Jj_consume_token(RegexpToken.NUMBER);
                            break;
                        default:
                            jj_la1[17] = jj_gen;
                            break;
                    }
                    bool startOpen = false;
                    bool endOpen = false;
                    if (goop1.Kind == RegexpToken.RANGE_QUOTED)
                    {
                        goop1.Image = goop1.Image.Substring(1, goop1.Image.Length - 2);
                    }
                    else if ("*".Equals(goop1.Image, StringComparison.Ordinal))
                    {
                        startOpen = true;
                    }
                    if (goop2.Kind == RegexpToken.RANGE_QUOTED)
                    {
                        goop2.Image = goop2.Image.Substring(1, goop2.Image.Length - 2);
                    }
                    else if ("*".Equals(goop2.Image, StringComparison.Ordinal))
                    {
                        endOpen = true;
                    }
                    q = GetRangeQuery(field, startOpen ? null : DiscardEscapeChar(goop1.Image), endOpen ? null : DiscardEscapeChar(goop2.Image), startInc, endInc);
                    break;
                case RegexpToken.QUOTED:
                    term = Jj_consume_token(RegexpToken.QUOTED);
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.FUZZY_SLOP:
                            fuzzySlop = Jj_consume_token(RegexpToken.FUZZY_SLOP);
                            break;
                        default:
                            jj_la1[18] = jj_gen;
                            break;
                    }
                    switch ((jj_ntk == -1) ? Jj_ntk() : jj_ntk)
                    {
                        case RegexpToken.CARAT:
                            Jj_consume_token(RegexpToken.CARAT);
                            boost = Jj_consume_token(RegexpToken.NUMBER);
                            break;
                        default:
                            jj_la1[19] = jj_gen;
                            break;
                    }
                    q = HandleQuotedTerm(field, term, fuzzySlop);
                    break;
                default:
                    jj_la1[20] = jj_gen;
                    Jj_consume_token(-1);
                    throw new ParseException();
            }
            { if (true) return HandleBoost(q, boost); }
            throw Error.Create("Missing return statement in function");
        }

        private bool Jj_2_1(int xla)
        {
            jj_la = xla;
            jj_lastpos = jj_scanpos = Token;
            try
            {
                return !Jj_3_1();
            }
            catch (LookaheadSuccess)
            {
                return true;
            }
            finally
            {
                Jj_save(0, xla);
            }
        }

        private bool Jj_3R_2()
        {
            if (Jj_scan_token(RegexpToken.TERM)) return true;
            if (Jj_scan_token(RegexpToken.COLON)) return true;
            return false;
        }

        private bool Jj_3_1()
        {
            Token xsp;
            xsp = jj_scanpos;
            if (Jj_3R_2())
            {
                jj_scanpos = xsp;
                if (Jj_3R_3()) return true;
            }
            return false;
        }

        private bool Jj_3R_3()
        {
            if (Jj_scan_token(RegexpToken.STAR)) return true;
            if (Jj_scan_token(RegexpToken.COLON)) return true;
            return false;
        }

        /// <summary>Generated Token Manager.</summary>
        public QueryParserTokenManager TokenSource { get; set; }
        /// <summary>Current token.</summary>
        public Token Token { get; set; }
        /// <summary>Next token.</summary>
        public Token Jj_nt { get; set; }
        private int jj_ntk;
        private Token jj_scanpos, jj_lastpos;
        private int jj_la;
        private int jj_gen;
        private readonly int[] jj_la1 = new int[21]; // LUCENENET: marked readonly
        private static readonly uint[] jj_la1_0 = new uint[] // LUCENENET: marked readonly // LUCENENET: Avoid static constructors (see https://github.com/apache/lucenenet/pull/224#issuecomment-469284006)
        {
            0x300, 0x300, 0x1c00, 0x1c00, 0xfda7f00, 0x120000, 0x40000, 0xfda6000, 0x9d22000, 0x200000,
            0x200000, 0x40000, 0x6000000, 0x80000000, 0x10000000, 0x80000000, 0x60000000, 0x40000,
            0x200000, 0x40000, 0xfda2000,
        };
        private static readonly int[] jj_la1_1 = new int[] // LUCENENET: marked readonly // LUCENENET: Avoid static constructors (see https://github.com/apache/lucenenet/pull/224#issuecomment-469284006)
        {
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1, 0x0,
            0x1, 0x0, 0x0, 0x0, 0x0, 0x0,
        };

        // LUCENENET: Avoid static constructors (see https://github.com/apache/lucenenet/pull/224#issuecomment-469284006)
        //static QueryParser()
        //{
        //    {
        //        Jj_la1_init_0();
        //        Jj_la1_init_1();
        //    }
        //}

        //private static void Jj_la1_init_0()
        //{
        //    jj_la1_0 = new uint[]
        //    {
        //        0x300, 0x300, 0x1c00, 0x1c00, 0xfda7f00, 0x120000, 0x40000, 0xfda6000, 0x9d22000, 0x200000,
        //        0x200000, 0x40000, 0x6000000, 0x80000000, 0x10000000, 0x80000000, 0x60000000, 0x40000,
        //        0x200000, 0x40000, 0xfda2000,
        //    };
        //}

        //private static void Jj_la1_init_1()
        //{
        //    jj_la1_1 = new int[]
        //    {
        //        0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1, 0x0,
        //        0x1, 0x0, 0x0, 0x0, 0x0, 0x0,
        //    };
        //}

        private readonly JJCalls[] jj_2_rtns = new JJCalls[1];
        private bool jj_rescan = false;
        private int jj_gc = 0;

        /// <summary>Constructor with user supplied <see cref="ICharStream"/>. </summary>
        protected internal QueryParser(ICharStream stream)
        {
            TokenSource = new QueryParserTokenManager(stream);
            Token = new Token();
            jj_ntk = -1;
            jj_gen = 0;
            for (int i = 0; i < 21; i++) jj_la1[i] = -1;
            for (int i = 0; i < jj_2_rtns.Length; i++) jj_2_rtns[i] = new JJCalls();
        }

        /// <summary>Reinitialize. </summary>
        public override void ReInit(ICharStream stream)
        {
            TokenSource.ReInit(stream);
            Token = new Token();
            jj_ntk = -1;
            jj_gen = 0;
            for (int i = 0; i < 21; i++) jj_la1[i] = -1;
            for (int i = 0; i < jj_2_rtns.Length; i++) jj_2_rtns[i] = new JJCalls();
        }

        /// <summary>Constructor with generated Token Manager. </summary>
        protected QueryParser(QueryParserTokenManager tm)
        {
            TokenSource = tm;
            Token = new Token();
            jj_ntk = -1;
            jj_gen = 0;
            for (int i = 0; i < 21; i++) jj_la1[i] = -1;
            for (int i = 0; i < jj_2_rtns.Length; i++) jj_2_rtns[i] = new JJCalls();
        }

        /// <summary>Reinitialize. </summary>
        public virtual void ReInit(QueryParserTokenManager tm)
        {
            TokenSource = tm;
            Token = new Token();
            jj_ntk = -1;
            jj_gen = 0;
            for (int i = 0; i < 21; i++) jj_la1[i] = -1;
            for (int i = 0; i < jj_2_rtns.Length; i++) jj_2_rtns[i] = new JJCalls();
        }

        private Token Jj_consume_token(int kind)
        {
            Token oldToken;
            if ((oldToken = Token).Next != null) Token = Token.Next;
            else Token = Token.Next = TokenSource.GetNextToken();
            jj_ntk = -1;
            if (Token.Kind == kind)
            {
                jj_gen++;
                if (++jj_gc > 100)
                {
                    jj_gc = 0;
                    for (int i = 0; i < jj_2_rtns.Length; i++)
                    {
                        JJCalls c = jj_2_rtns[i];
                        while (c != null)
                        {
                            if (c.gen < jj_gen) c.first = null;
                            c = c.next;
                        }
                    }
                }
                return Token;
            }
            Token = oldToken;
            jj_kind = kind;
            throw GenerateParseException();
        }

        // LUCENENET: It is no longer good practice to use binary serialization.
        // See: https://github.com/dotnet/corefx/issues/23584#issuecomment-325724568
#if FEATURE_SERIALIZABLE_EXCEPTIONS
        [Serializable]
#endif
        private sealed class LookaheadSuccess : Exception
        {
            public LookaheadSuccess()
            { }

#if FEATURE_SERIALIZABLE_EXCEPTIONS
            /// <summary>
            /// Initializes a new instance of this class with serialized data.
            /// </summary>
            /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
            [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.")]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public LookaheadSuccess(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
#endif
        }

        private readonly LookaheadSuccess jj_ls = new LookaheadSuccess(); // LUCENENET: marked readonly
        private bool Jj_scan_token(int kind)
        {
            if (jj_scanpos == jj_lastpos)
            {
                jj_la--;
                if (jj_scanpos.Next is null)
                {
                    jj_lastpos = jj_scanpos = jj_scanpos.Next = TokenSource.GetNextToken();
                }
                else
                {
                    jj_lastpos = jj_scanpos = jj_scanpos.Next;
                }
            }
            else
            {
                jj_scanpos = jj_scanpos.Next;
            }
            if (jj_rescan)
            {
                int i = 0;
                Token tok = Token;
                while (tok != null && tok != jj_scanpos)
                {
                    i++;
                    tok = tok.Next;
                }
                if (tok != null) Jj_add_error_token(kind, i);
            }
            if (jj_scanpos.Kind != kind) return true;
            if (jj_la == 0 && jj_scanpos == jj_lastpos) throw jj_ls;
            return false;
        }

        /// <summary>Get the next Token. </summary>
        public Token GetNextToken()
        {
            if (Token.Next != null) Token = Token.Next;
            else Token = Token.Next = TokenSource.GetNextToken();
            jj_ntk = -1;
            jj_gen++;
            return Token;
        }

        /// <summary>Get the specific Token. </summary>
        public Token GetToken(int index)
        {
            Token t = Token;
            for (int i = 0; i < index; i++)
            {
                if (t.Next != null) t = t.Next;
                else t = t.Next = TokenSource.GetNextToken();
            }
            return t;
        }

        private int Jj_ntk()
        {
            if ((Jj_nt = Token.Next) is null)
                return (jj_ntk = (Token.Next = TokenSource.GetNextToken()).Kind);
            else
                return (jj_ntk = Jj_nt.Kind);
        }

        private readonly IList<int[]> jj_expentries = new JCG.List<int[]>(); // LUCENENET: marked readonly
        private int[] jj_expentry;
        private int jj_kind = -1;
        private readonly int[] jj_lasttokens = new int[100]; // LUCENENET: marked readonly
        private int jj_endpos;

        private void Jj_add_error_token(int kind, int pos)
        {
            if (pos >= 100) return;
            if (pos == jj_endpos + 1)
            {
                jj_lasttokens[jj_endpos++] = kind;
            }
            else if (jj_endpos != 0)
            {
                jj_expentry = new int[jj_endpos];
                for (int i = 0; i < jj_endpos; i++)
                {
                    jj_expentry[i] = jj_lasttokens[i];
                }

                foreach (var oldentry in jj_expentries)
                {
                    if (oldentry.Length == jj_expentry.Length)
                    {
                        for (int i = 0; i < jj_expentry.Length; i++)
                        {
                            if (oldentry[i] != jj_expentry[i])
                            {
                                goto jj_entries_loop_continue;
                            }
                        }
                        jj_expentries.Add(jj_expentry);
                        goto jj_entries_loop_break;
                    }
                jj_entries_loop_continue: {/* LUCENENET: intentionally blank */}
                }
            jj_entries_loop_break:
                if (pos != 0) jj_lasttokens[(jj_endpos = pos) - 1] = kind;
            }
        }

        /// <summary>Generate ParseException. </summary>
        public virtual ParseException GenerateParseException()
        {
            jj_expentries.Clear();
            bool[] la1tokens = new bool[33];
            if (jj_kind >= 0)
            {
                la1tokens[jj_kind] = true;
                jj_kind = -1;
            }
            for (int i = 0; i < 21; i++)
            {
                if (jj_la1[i] == jj_gen)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        if ((jj_la1_0[i] & (1 << j)) != 0)
                        {
                            la1tokens[j] = true;
                        }
                        if ((jj_la1_1[i] & (1 << j)) != 0)
                        {
                            la1tokens[32 + j] = true;
                        }
                    }
                }
            }
            for (int i = 0; i < 33; i++)
            {
                if (la1tokens[i])
                {
                    jj_expentry = new int[1];
                    jj_expentry[0] = i;
                    jj_expentries.Add(jj_expentry);
                }
            }
            jj_endpos = 0;
            Jj_rescan_token();
            Jj_add_error_token(0, 0);
            int[][] exptokseq = new int[jj_expentries.Count][];
            for (int i = 0; i < jj_expentries.Count; i++)
            {
                exptokseq[i] = jj_expentries[i];
            }
            return new ParseException(Token, exptokseq, QueryParserConstants.TokenImage);
        }


        /// <summary>Enable tracing. </summary>
        public void Enable_tracing()
        {
            // LUCENENET: Intentionally blank
        }

        /// <summary>Disable tracing. </summary>
        public void Disable_tracing()
        {
            // LUCENENET: Intentionally blank
        }

        private void Jj_rescan_token()
        {
            jj_rescan = true;
            for (int i = 0; i < 1; i++)
            {
                try
                {
                    JJCalls p = jj_2_rtns[i];
                    do
                    {
                        if (p.gen > jj_gen)
                        {
                            jj_la = p.arg;
                            jj_lastpos = jj_scanpos = p.first;
                            switch (i)
                            {
                                case 0:
                                    Jj_3_1();
                                    break;
                            }
                        }
                        p = p.next;
                    } while (p != null);
                }
                catch (LookaheadSuccess)
                {
                    // ignored
                }
            }
            jj_rescan = false;
        }

        private void Jj_save(int index, int xla)
        {
            JJCalls p = jj_2_rtns[index];
            while (p.gen > jj_gen)
            {
                if (p.next is null)
                {
                    p = p.next = new JJCalls();
                    break;
                }
                p = p.next;
            }
            p.gen = jj_gen + xla - jj_la;
            p.first = Token;
            p.arg = xla;
        }

        internal sealed class JJCalls
        {
            internal int gen;
            internal Token first;
            internal int arg;
            internal JJCalls next;
        }
    }
}
