﻿using System;
using System.IO;
using System.Threading;

namespace YAF.Lucene.Net.Store
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

    /// <summary>
    /// A <see cref="DataInput"/> wrapping a plain <see cref="Stream"/>.
    /// </summary>
    public class InputStreamDataInput : DataInput, IDisposable
    {
        private readonly Stream _is;
        private int disposed; // LUCENENET specific - allow double-dispose
        private readonly bool leaveOpen; // LUCENENET specific - added to allow the stream to be left open

        /// <summary>
        /// Initializes a new instance of <see cref="InputStreamDataInput"/> with the specified <paramref name="is"/> (input stream).
        /// </summary>
        /// <param name="is">The input stream to read from.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="is"/> is <c>null</c>.</exception>
        public InputStreamDataInput(Stream @is)
        {
            this._is = @is ?? throw new ArgumentNullException(nameof(@is)); // LUCENENET specific - added null guard clause
        }

        /// <inheritdoc cref="InputStreamDataInput(Stream)"/>
        /// <summary>
        /// Initializes a new instance of <see cref="InputStreamDataInput"/> with the specified <paramref name="is"/> (input stream) and <paramref name="leaveOpen"/> flag.
        /// </summary>
        /// <param name="leaveOpen">If <c>true</c>, the stream will not be disposed when this instance is disposed.</param>
        /// <remarks>
        /// LUCENENET specific - added to allow the stream to be left open.
        /// </remarks>
        public InputStreamDataInput(Stream @is, bool leaveOpen)
            : this(@is)
        {
            this.leaveOpen = leaveOpen;
        }

        public override byte ReadByte()
        {
            int v = _is.ReadByte();
            if (v == -1)
            {
                throw EOFException.Create();
            }
            return (byte)v;
        }

        public override void ReadBytes(byte[] b, int offset, int len)
        {
            while (len > 0)
            {
                int cnt = _is.Read(b, offset, len);
                if (cnt < 0)
                {
                    // Partially read the input, but no more data available in the stream.
                    throw EOFException.Create();
                }
                len -= cnt;
                offset += cnt;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (0 != Interlocked.CompareExchange(ref this.disposed, 1, 0)) return; // LUCENENET specific - allow double-dispose

            if (disposing)
            {
                if (!leaveOpen)
                {
                    _is.Dispose();
                }
            }
        }
    }
}
