﻿// Lucene version compatibility level 4.8.1
/*

Copyright (c) 2001, Dr Martin Porter
Copyright (c) 2002, Richard Boulton
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
    * this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
    * notice, this list of conditions and the following disclaimer in the
    * documentation and/or other materials provided with the distribution.
    * Neither the name of the copyright holders nor the names of its contributors
    * may be used to endorse or promote products derived from this software
    * without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 */

namespace YAF.Lucene.Net.Tartarus.Snowball.Ext
{
    /// <summary>
    /// This class was automatically generated by a Snowball to Java compiler
    /// It implements the stemming algorithm defined by a snowball script.
    /// </summary>
    public class GermanStemmer : SnowballProgram
    {
        // LUCENENET specific: Factored out methodObject by using Func<bool> instead of Reflection

        private readonly static Among[] a_0 = {
                    new Among ( "", -1, 6 ),
                    new Among ( "U", 0, 2 ),
                    new Among ( "Y", 0, 1 ),
                    new Among ( "\u00E4", 0, 3 ),
                    new Among ( "\u00F6", 0, 4 ),
                    new Among ( "\u00FC", 0, 5 )
                };

        private readonly static Among[] a_1 = {
                    new Among ( "e", -1, 1 ),
                    new Among ( "em", -1, 1 ),
                    new Among ( "en", -1, 1 ),
                    new Among ( "ern", -1, 1 ),
                    new Among ( "er", -1, 1 ),
                    new Among ( "s", -1, 2 ),
                    new Among ( "es", 5, 1 )
                };

        private readonly static Among[] a_2 = {
                    new Among ( "en", -1, 1 ),
                    new Among ( "er", -1, 1 ),
                    new Among ( "st", -1, 2 ),
                    new Among ( "est", 2, 1 )
                };

        private readonly static Among[] a_3 = {
                    new Among ( "ig", -1, 1 ),
                    new Among ( "lich", -1, 1 )
                };

        private readonly static Among[] a_4 = {
                    new Among ( "end", -1, 1 ),
                    new Among ( "ig", -1, 2 ),
                    new Among ( "ung", -1, 1 ),
                    new Among ( "lich", -1, 3 ),
                    new Among ( "isch", -1, 2 ),
                    new Among ( "ik", -1, 2 ),
                    new Among ( "heit", -1, 3 ),
                    new Among ( "keit", -1, 4 )
                };

        private static readonly char[] g_v = { (char)17, (char)65, (char)16, (char)1, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)8, (char)0, (char)32, (char)8 };

        private static readonly char[] g_s_ending = { (char)117, (char)30, (char)5 };

        private static readonly char[] g_st_ending = { (char)117, (char)30, (char)4 };

        private int I_x;
        private int I_p2;
        private int I_p1;

        // LUCENENET: commented out unused private method
        // private void copy_from(GermanStemmer other)
        // {
        //     I_x = other.I_x;
        //     I_p2 = other.I_p2;
        //     I_p1 = other.I_p1;
        //     base.CopyFrom(other);
        // }

        private bool r_prelude()
        {
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            int v_6;
            // (, line 28
            // test, line 30
            v_1 = m_cursor;
            // repeat, line 30

            while (true)
            {
                v_2 = m_cursor;

                do
                {
                    // (, line 30
                    // or, line 33

                    do
                    {
                        v_3 = m_cursor;

                        do
                        {
                            // (, line 31
                            // [, line 32
                            m_bra = m_cursor;
                            // literal, line 32
                            if (!(Eq_S(1, "\u00DF")))
                            {
                                goto lab3;
                            }
                            // ], line 32
                            m_ket = m_cursor;
                            // <-, line 32
                            SliceFrom("ss");
                            goto lab2;
                        } while (false);
                        lab3:
                        m_cursor = v_3;
                        // next, line 33
                        if (m_cursor >= m_limit)
                        {
                            goto lab1;
                        }
                        m_cursor++;
                    } while (false);
                    lab2:
                    // LUCENENET NOTE: continue label is not supported directly in .NET,
                    // so we just need to add another goto to get to the end of the outer loop.
                    // See: http://stackoverflow.com/a/359449/181087

                    // Original code:
                    //continue replab0;

                    goto end_of_outer_loop;

                } while (false);
                lab1:
                m_cursor = v_2;
                goto replab0;
                end_of_outer_loop: { /* LUCENENET: intentionally empty */ }
            }
            replab0:
            m_cursor = v_1;
            // repeat, line 36

            while (true)
            {
                v_4 = m_cursor;

                do
                {
                    // goto, line 36

                    while (true)
                    {
                        v_5 = m_cursor;

                        do
                        {
                            // (, line 36
                            if (!(InGrouping(g_v, 97, 252)))
                            {
                                goto lab7;
                            }
                            // [, line 37
                            m_bra = m_cursor;
                            // or, line 37

                            do
                            {
                                v_6 = m_cursor;

                                do
                                {
                                    // (, line 37
                                    // literal, line 37
                                    if (!(Eq_S(1, "u")))
                                    {
                                        goto lab9;
                                    }
                                    // ], line 37
                                    m_ket = m_cursor;
                                    if (!(InGrouping(g_v, 97, 252)))
                                    {
                                        goto lab9;
                                    }
                                    // <-, line 37
                                    SliceFrom("U");
                                    goto lab8;
                                } while (false);
                                lab9:
                                m_cursor = v_6;
                                // (, line 38
                                // literal, line 38
                                if (!(Eq_S(1, "y")))
                                {
                                    goto lab7;
                                }
                                // ], line 38
                                m_ket = m_cursor;
                                if (!(InGrouping(g_v, 97, 252)))
                                {
                                    goto lab7;
                                }
                                // <-, line 38
                                SliceFrom("Y");
                            } while (false);
                            lab8:
                            m_cursor = v_5;
                            goto golab6;
                        } while (false);
                        lab7:
                        m_cursor = v_5;
                        if (m_cursor >= m_limit)
                        {
                            goto lab5;
                        }
                        m_cursor++;
                    }
                    golab6:
                    // LUCENENET NOTE: continue label is not supported directly in .NET,
                    // so we just need to add another goto to get to the end of the outer loop.
                    // See: http://stackoverflow.com/a/359449/181087

                    // Original code:
                    //continue replab4;

                    goto end_of_outer_loop_2;

                } while (false);
                lab5:
                m_cursor = v_4;
                goto replab4;
                end_of_outer_loop_2: { /* LUCENENET: intentionally empty */ }
            }
            replab4:
            return true;
        }

        private bool r_mark_regions()
        {
            int v_1;
            // (, line 42
            I_p1 = m_limit;
            I_p2 = m_limit;
            // test, line 47
            v_1 = m_cursor;
            // (, line 47
            // hop, line 47
            {
                int c = m_cursor + 3;
                if (0 > c || c > m_limit)
                {
                    return false;
                }
                m_cursor = c;
            }
            // setmark x, line 47
            I_x = m_cursor;
            m_cursor = v_1;
            // gopast, line 49

            while (true)
            {

                do
                {
                    if (!(InGrouping(g_v, 97, 252)))
                    {
                        goto lab1;
                    }
                    goto golab0;
                } while (false);
                lab1:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab0:
            // gopast, line 49

            while (true)
            {

                do
                {
                    if (!(OutGrouping(g_v, 97, 252)))
                    {
                        goto lab3;
                    }
                    goto golab2;
                } while (false);
                lab3:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab2:
            // setmark p1, line 49
            I_p1 = m_cursor;
            // try, line 50

            do
            {
                // (, line 50
                if (!(I_p1 < I_x))
                {
                    goto lab4;
                }
                I_p1 = I_x;
            } while (false);
            lab4:
            // gopast, line 51

            while (true)
            {
                do
                {
                    if (!(InGrouping(g_v, 97, 252)))
                    {
                        goto lab6;
                    }
                    goto golab5;
                } while (false);
                lab6:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab5:
            // gopast, line 51

            while (true)
            {

                do
                {
                    if (!(OutGrouping(g_v, 97, 252)))
                    {
                        goto lab8;
                    }
                    goto golab7;
                } while (false);
                lab8:
                if (m_cursor >= m_limit)
                {
                    return false;
                }
                m_cursor++;
            }
            golab7:
            // setmark p2, line 51
            I_p2 = m_cursor;
            return true;
        }

        private bool r_postlude()
        {
            int among_var;
            int v_1;
            // repeat, line 55

            while (true)
            {
                v_1 = m_cursor;

                do
                {
                    // (, line 55
                    // [, line 57
                    m_bra = m_cursor;
                    // substring, line 57
                    among_var = FindAmong(a_0, 6);
                    if (among_var == 0)
                    {
                        goto lab1;
                    }
                    // ], line 57
                    m_ket = m_cursor;
                    switch (among_var)
                    {
                        case 0:
                            goto lab1;
                        case 1:
                            // (, line 58
                            // <-, line 58
                            SliceFrom("y");
                            break;
                        case 2:
                            // (, line 59
                            // <-, line 59
                            SliceFrom("u");
                            break;
                        case 3:
                            // (, line 60
                            // <-, line 60
                            SliceFrom("a");
                            break;
                        case 4:
                            // (, line 61
                            // <-, line 61
                            SliceFrom("o");
                            break;
                        case 5:
                            // (, line 62
                            // <-, line 62
                            SliceFrom("u");
                            break;
                        case 6:
                            // (, line 63
                            // next, line 63
                            if (m_cursor >= m_limit)
                            {
                                goto lab1;
                            }
                            m_cursor++;
                            break;
                    }
                    // LUCENENET NOTE: continue label is not supported directly in .NET,
                    // so we just need to add another goto to get to the end of the outer loop.
                    // See: http://stackoverflow.com/a/359449/181087

                    // Original code:
                    //continue replab0;

                    goto end_of_outer_loop;

                } while (false);
                lab1:
                m_cursor = v_1;
                goto replab0;
                end_of_outer_loop: { /* LUCENENET: intentionally empty */ }
            }
            replab0:
            return true;
        }

        private bool r_R1()
        {
            if (!(I_p1 <= m_cursor))
            {
                return false;
            }
            return true;
        }

        private bool r_R2()
        {
            if (!(I_p2 <= m_cursor))
            {
                return false;
            }
            return true;
        }

        private bool r_standard_suffix()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            int v_6;
            int v_7;
            int v_8;
            int v_9;
            // (, line 73
            // do, line 74
            v_1 = m_limit - m_cursor;
            do
            {
                // (, line 74
                // [, line 75
                m_ket = m_cursor;
                // substring, line 75
                among_var = FindAmongB(a_1, 7);
                if (among_var == 0)
                {
                    goto lab0;
                }
                // ], line 75
                m_bra = m_cursor;
                // call R1, line 75
                if (!r_R1())
                {
                    goto lab0;
                }
                switch (among_var)
                {
                    case 0:
                        goto lab0;
                    case 1:
                        // (, line 77
                        // delete, line 77
                        SliceDel();
                        break;
                    case 2:
                        // (, line 80
                        if (!(InGroupingB(g_s_ending, 98, 116)))
                        {
                            goto lab0;
                        }
                        // delete, line 80
                        SliceDel();
                        break;
                }
            } while (false);
            lab0:
            m_cursor = m_limit - v_1;
            // do, line 84
            v_2 = m_limit - m_cursor;
            do
            {
                // (, line 84
                // [, line 85
                m_ket = m_cursor;
                // substring, line 85
                among_var = FindAmongB(a_2, 4);
                if (among_var == 0)
                {
                    goto lab1;
                }
                // ], line 85
                m_bra = m_cursor;
                // call R1, line 85
                if (!r_R1())
                {
                    goto lab1;
                }
                switch (among_var)
                {
                    case 0:
                        goto lab1;
                    case 1:
                        // (, line 87
                        // delete, line 87
                        SliceDel();
                        break;
                    case 2:
                        // (, line 90
                        if (!(InGroupingB(g_st_ending, 98, 116)))
                        {
                            goto lab1;
                        }
                        // hop, line 90
                        {
                            int c = m_cursor - 3;
                            if (m_limit_backward > c || c > m_limit)
                            {
                                goto lab1;
                            }
                            m_cursor = c;
                        }
                        // delete, line 90
                        SliceDel();
                        break;
                }
            } while (false);
            lab1:
            m_cursor = m_limit - v_2;
            // do, line 94
            v_3 = m_limit - m_cursor;
            do
            {
                // (, line 94
                // [, line 95
                m_ket = m_cursor;
                // substring, line 95
                among_var = FindAmongB(a_4, 8);
                if (among_var == 0)
                {
                    goto lab2;
                }
                // ], line 95
                m_bra = m_cursor;
                // call R2, line 95
                if (!r_R2())
                {
                    goto lab2;
                }
                switch (among_var)
                {
                    case 0:
                        goto lab2;
                    case 1:
                        // (, line 97
                        // delete, line 97
                        SliceDel();
                        // try, line 98
                        v_4 = m_limit - m_cursor;
                        do
                        {
                            // (, line 98
                            // [, line 98
                            m_ket = m_cursor;
                            // literal, line 98
                            if (!(Eq_S_B(2, "ig")))
                            {
                                m_cursor = m_limit - v_4;
                                goto lab3;
                            }
                            // ], line 98
                            m_bra = m_cursor;
                            // not, line 98
                            {
                                v_5 = m_limit - m_cursor;
                                do
                                {
                                    // literal, line 98
                                    if (!(Eq_S_B(1, "e")))
                                    {
                                        goto lab4;
                                    }
                                    m_cursor = m_limit - v_4;
                                    goto lab3;
                                } while (false);
                                lab4:
                                m_cursor = m_limit - v_5;
                            }
                            // call R2, line 98
                            if (!r_R2())
                            {
                                m_cursor = m_limit - v_4;
                                goto lab3;
                            }
                            // delete, line 98
                            SliceDel();
                        } while (false);
                        lab3:
                        break;
                    case 2:
                        // (, line 101
                        // not, line 101
                        {
                            v_6 = m_limit - m_cursor;
                            do
                            {
                                // literal, line 101
                                if (!(Eq_S_B(1, "e")))
                                {
                                    goto lab5;
                                }
                                goto lab2;
                            } while (false);
                            lab5:
                            m_cursor = m_limit - v_6;
                        }
                        // delete, line 101
                        SliceDel();
                        break;
                    case 3:
                        // (, line 104
                        // delete, line 104
                        SliceDel();
                        // try, line 105
                        v_7 = m_limit - m_cursor;
                        do
                        {
                            // (, line 105
                            // [, line 106
                            m_ket = m_cursor;
                            // or, line 106
                            do
                            {
                                v_8 = m_limit - m_cursor;
                                do
                                {
                                    // literal, line 106
                                    if (!(Eq_S_B(2, "er")))
                                    {
                                        goto lab8;
                                    }
                                    goto lab7;
                                } while (false);
                                lab8:
                                m_cursor = m_limit - v_8;
                                // literal, line 106
                                if (!(Eq_S_B(2, "en")))
                                {
                                    m_cursor = m_limit - v_7;
                                    goto lab6;
                                }
                            } while (false);
                            lab7:
                            // ], line 106
                            m_bra = m_cursor;
                            // call R1, line 106
                            if (!r_R1())
                            {
                                m_cursor = m_limit - v_7;
                                goto lab6;
                            }
                            // delete, line 106
                            SliceDel();
                        } while (false);
                        lab6:
                        break;
                    case 4:
                        // (, line 110
                        // delete, line 110
                        SliceDel();
                        // try, line 111
                        v_9 = m_limit - m_cursor;
                        do
                        {
                            // (, line 111
                            // [, line 112
                            m_ket = m_cursor;
                            // substring, line 112
                            among_var = FindAmongB(a_3, 2);
                            if (among_var == 0)
                            {
                                m_cursor = m_limit - v_9;
                                goto lab9;
                            }
                            // ], line 112
                            m_bra = m_cursor;
                            // call R2, line 112
                            if (!r_R2())
                            {
                                m_cursor = m_limit - v_9;
                                goto lab9;
                            }
                            switch (among_var)
                            {
                                case 0:
                                    m_cursor = m_limit - v_9;
                                    goto lab9;
                                case 1:
                                    // (, line 114
                                    // delete, line 114
                                    SliceDel();
                                    break;
                            }
                        } while (false);
                        lab9:
                        break;
                }
            } while (false);
            lab2:
            m_cursor = m_limit - v_3;
            return true;
        }


                public override bool Stem()
        {
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            // (, line 124
            // do, line 125
            v_1 = m_cursor;
             do
            {
                // call prelude, line 125
                if (!r_prelude())
                {
                    goto lab0;
                }
            } while (false);
            lab0:
            m_cursor = v_1;
            // do, line 126
            v_2 = m_cursor;
            do
            {
                // call mark_regions, line 126
                if (!r_mark_regions())
                {
                    goto lab1;
                }
            } while (false);
            lab1:
            m_cursor = v_2;
            // backwards, line 127
            m_limit_backward = m_cursor; m_cursor = m_limit;
            // do, line 128
            v_3 = m_limit - m_cursor;
            do
            {
                // call standard_suffix, line 128
                if (!r_standard_suffix())
                {
                    goto lab2;
                }
            } while (false);
            lab2:
            m_cursor = m_limit - v_3;
            m_cursor = m_limit_backward;                    // do, line 129
            v_4 = m_cursor;
            do
            {
                // call postlude, line 129
                if (!r_postlude())
                {
                    goto lab3;
                }
            } while (false);
            lab3:
            m_cursor = v_4;
            return true;
        }

        public override bool Equals(object o)
        {
            return o is GermanStemmer;
        }

        public override int GetHashCode()
        {
            return this.GetType().FullName.GetHashCode();
        }
    }
}
