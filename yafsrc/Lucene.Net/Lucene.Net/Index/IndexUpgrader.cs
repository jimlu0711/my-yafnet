﻿using YAF.Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace YAF.Lucene.Net.Index
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

    using CommandLineUtil = YAF.Lucene.Net.Util.CommandLineUtil;
    using Constants = YAF.Lucene.Net.Util.Constants;
    using Directory = YAF.Lucene.Net.Store.Directory;
    using FSDirectory = YAF.Lucene.Net.Store.FSDirectory;
    using InfoStream = YAF.Lucene.Net.Util.InfoStream;

    // LUCENENET: Not used
    ///// <code>
    /////  java -cp lucene-core.jar Lucene.Net.Index.IndexUpgrader [-delete-prior-commits] [-verbose] indexDir
    ///// </code>

    /// <summary>
    /// This is an easy-to-use tool that upgrades all segments of an index from previous Lucene versions
    /// to the current segment file format. It can be used from command line.
    /// <para />
    /// LUCENENET specific: In the Java implementation, this class' Main method
    /// was intended to be called from the command line. However, in .NET a
    /// method within a DLL can't be directly called from the command line so we
    /// provide a <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools">.NET tool</see>,
    /// <see href="https://www.nuget.org/packages/lucene-cli">lucene-cli</see>,
    /// with a command that maps to that method:
    /// index upgrade
    /// <para />
    /// Alternatively this class can be instantiated and <see cref="Upgrade()"/> invoked. It uses <see cref="UpgradeIndexMergePolicy"/>
    /// and triggers the upgrade via an <see cref="IndexWriter.ForceMerge(int)"/> request to <see cref="IndexWriter"/>.
    /// <para />
    /// This tool keeps only the last commit in an index; for this
    /// reason, if the incoming index has more than one commit, the tool
    /// refuses to run by default. Specify <c>-delete-prior-commits</c>
    /// to override this, allowing the tool to delete all but the last commit.
    /// From .NET code this can be enabled by passing <c>true</c> to
    /// <see cref="IndexUpgrader(Directory, LuceneVersion, TextWriter, bool)"/>.
    /// <para />
    /// <b>Warning:</b> this tool may reorder documents if the index was partially
    /// upgraded before execution (e.g., documents were added). If your application relies
    /// on &quot;monotonicity&quot; of doc IDs (which means that the order in which the documents
    /// were added to the index is preserved), do a full ForceMerge instead.
    /// The <see cref="MergePolicy"/> set by <see cref="IndexWriterConfig"/> may also reorder
    /// documents.
    /// </summary>
    public sealed class IndexUpgrader
    {
        private static void PrintUsage()
        {
            // LUCENENET specific - our wrapper console shows the correct usage
            throw new ArgumentException("One or more arguments was invalid");
            //Console.Error.WriteLine("Upgrades an index so all segments created with a previous Lucene version are rewritten.");
            //Console.Error.WriteLine("Usage:");
            //Console.Error.WriteLine("  java " + nameof(IndexUpgrader) + " [-delete-prior-commits] [-verbose] [-dir-impl X] indexDir");
            //Console.Error.WriteLine("this tool keeps only the last commit in an index; for this");
            //Console.Error.WriteLine("reason, if the incoming index has more than one commit, the tool");
            //Console.Error.WriteLine("refuses to run by default. Specify -delete-prior-commits to override");
            //Console.Error.WriteLine("this, allowing the tool to delete all but the last commit.");
            //Console.Error.WriteLine("Specify a " + nameof(FSDirectory) + " implementation through the -dir-impl option to force its use. If no package is specified the " + typeof(FSDirectory).Namespace + " package will be used.");
            //Console.Error.WriteLine("WARNING: this tool may reorder document IDs!");
            //Environment.FailFast("1");
        }

        /// <summary>
        /// Main method to run <see cref="IndexUpgrader"/> from the
        /// command-line.
        /// <para />
        /// LUCENENET specific: In the Java implementation, this Main method
        /// was intended to be called from the command line. However, in .NET a
        /// method within a DLL can't be directly called from the command line so we
        /// provide a <see href="https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools">.NET tool</see>,
        /// <see href="https://www.nuget.org/packages/lucene-cli">lucene-cli</see>,
        /// with a command that maps to this method:
        /// index upgrade
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <exception cref="ArgumentException">Thrown if any incorrect arguments are provided</exception>
        public static void Main(string[] args)
        {
            ParseArgs(args).Upgrade();
        }

        public static IndexUpgrader ParseArgs(string[] args)
        {
            string path = null;
            bool deletePriorCommits = false;
            TextWriter @out = null;
            string dirImpl = null;
            int i = 0;
            while (i < args.Length)
            {
                string arg = args[i];
                if ("-delete-prior-commits".Equals(arg, StringComparison.Ordinal))
                {
                    deletePriorCommits = true;
                }
                else if ("-verbose".Equals(arg, StringComparison.Ordinal))
                {
                    @out = Console.Out;
                }
                else if ("-dir-impl".Equals(arg, StringComparison.Ordinal))
                {
                    if (i == args.Length - 1)
                    {
                        throw new ArgumentException("ERROR: missing value for -dir option");
                        //Console.WriteLine("ERROR: missing value for -dir-impl option");
                        //Environment.FailFast("1");
                    }
                    i++;
                    dirImpl = args[i];
                }
                else if (path is null)
                {
                    path = arg;
                }
                else
                {
                    PrintUsage();
                }
                i++;
            }
            if (path is null)
            {
                PrintUsage();
            }

            Directory dir/* = null*/; // LUCENENET: IDE0059: Remove unnecessary value assignment
            if (dirImpl is null)
            {
                dir = FSDirectory.Open(new DirectoryInfo(path));
            }
            else
            {
                dir = CommandLineUtil.NewFSDirectory(dirImpl, new DirectoryInfo(path));
            }
#pragma warning disable 612, 618
            return new IndexUpgrader(dir, LuceneVersion.LUCENE_CURRENT, @out, deletePriorCommits);
#pragma warning restore 612, 618
        }

        internal readonly Directory dir; // LUCENENET specific - made internal for testing CLI arguments
        internal readonly IndexWriterConfig iwc; // LUCENENET specific - made internal for testing CLI arguments
        internal readonly bool deletePriorCommits; // LUCENENET specific - made internal for testing CLI arguments

        /// <summary>
        /// Creates index upgrader on the given directory, using an <see cref="IndexWriter"/> using the given
        /// <paramref name="matchVersion"/>. The tool refuses to upgrade indexes with multiple commit points.
        /// </summary>
        public IndexUpgrader(Directory dir, LuceneVersion matchVersion)
            : this(dir, new IndexWriterConfig(matchVersion, null), false)
        {
        }

        /// <summary>
        /// Creates index upgrader on the given directory, using an <see cref="IndexWriter"/> using the given
        /// <paramref name="matchVersion"/>. You have the possibility to upgrade indexes with multiple commit points by removing
        /// all older ones. If <paramref name="infoStream"/> is not <c>null</c>, all logging output will be sent to this stream.
        /// </summary>
        public IndexUpgrader(Directory dir, LuceneVersion matchVersion, TextWriter infoStream, bool deletePriorCommits)
            : this(dir, new IndexWriterConfig(matchVersion, null), deletePriorCommits)
        {
            if (null != infoStream)
            {
                this.iwc.SetInfoStream(infoStream);
            }
        }

        /// <summary>
        /// Creates index upgrader on the given directory, using an <see cref="IndexWriter"/> using the given
        /// config. You have the possibility to upgrade indexes with multiple commit points by removing
        /// all older ones.
        /// </summary>
        public IndexUpgrader(Directory dir, IndexWriterConfig iwc, bool deletePriorCommits)
        {
            this.dir = dir;
            this.iwc = iwc;
            this.deletePriorCommits = deletePriorCommits;
        }

        /// <summary>
        /// Perform the upgrade. </summary>
        public void Upgrade()
        {
            if (!DirectoryReader.IndexExists(dir))
            {
                throw new IndexNotFoundException(dir.ToString());
            }

            if (!deletePriorCommits)
            {
                ICollection<IndexCommit> commits = DirectoryReader.ListCommits(dir);
                if (commits.Count > 1)
                {
                    throw new ArgumentException("this tool was invoked to not delete prior commit points, but the following commits were found: " + commits);
                }
            }

            IndexWriterConfig c = (IndexWriterConfig)iwc.Clone();
            c.MergePolicy = new UpgradeIndexMergePolicy(c.MergePolicy);
            c.IndexDeletionPolicy = new KeepOnlyLastCommitDeletionPolicy();

            IndexWriter w = new IndexWriter(dir, c);
            try
            {
                InfoStream infoStream = c.InfoStream;
                if (infoStream.IsEnabled("IndexUpgrader"))
                {
                    infoStream.Message("IndexUpgrader", "Upgrading all pre-" + Constants.LUCENE_MAIN_VERSION + " segments of index directory '" + dir + "' to version " + Constants.LUCENE_MAIN_VERSION + "...");
                }
                w.ForceMerge(1);
                if (infoStream.IsEnabled("IndexUpgrader"))
                {
                    infoStream.Message("IndexUpgrader", "All segments upgraded to version " + Constants.LUCENE_MAIN_VERSION);
                }
            }
            finally
            {
                w.Dispose();
            }
        }
    }
}
