using MongoDbImportUtil;
using System;
using System.IO;
using CommandLine;

namespace BookmarksETL_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string invokedVerb = null;
            object invokedVerbInstance = null;

            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options, (verb, subOptions) =>
                          {
                              // if parsing succeeds the verb name and correct instance
                              // will be passed to onVerbCommand delegate (string,object)
                              invokedVerb = verb;
                              invokedVerbInstance = subOptions;
                          }))
            {
                Environment.Exit(Parser.DefaultExitCodeFail);
            }

            if (invokedVerb == "parse")
            {
                var parseOptions = (ParseOptions)invokedVerbInstance;
                ParseBookmarks(parseOptions.ParserType, parseOptions.InputFile, parseOptions.OutputFile);
            }

            if (invokedVerb == "merge")
            {
                var mergeOptions = (MergeOptions)invokedVerbInstance;
                MergeBookmarks.Merge(mergeOptions.File1, mergeOptions.File2);
            }
        }

        private static void ParseBookmarks(string parserType, string bookmarksFile, string outputPath)
        {            
            Bookmarks2Json converter = null;
            switch (parserType)
            {
                case "delicious":
                    converter = new Bookmarks2Json(new DeliciousParser.Parser());
                    break;
                case "bibsonomy":
                    converter = new Bookmarks2Json(new BibsonomyParser.Parser());
                    break;
                case "pinterest":
                    converter = new Bookmarks2Json(new PinterestParser.Parser());
                    break;
                case "gitmarks":
                    converter = new Bookmarks2Json(new GitmarksParser.Parser());
                    break;
            }
            
            var content = converter.Write(bookmarksFile, outputPath);
            using (var writer = new StreamWriter(outputPath))
            {
                writer.Write(content);
            }
        }
    }
        
    internal class ParseOptions
    {
        [Option('p', "parser", HelpText = "provide parser type")]
        public string ParserType { get; set; }

        [Option('i', "input", HelpText = "pass input file")]
        public string InputFile { get; set; }    

        [Option('o', "output", HelpText = "provide output path")]
        public string OutputFile { get; set; }
    }

    internal class MergeOptions
    {
        [Option("file1", HelpText = "pass bookmarksFile1")]
        public string File1 { get; set; }

        [Option("file2", HelpText = "pass bookmarksFile2")]
        public string File2 { get; set; }
    }

    internal class Options
    {
        [VerbOption("parse", HelpText = "transforms external bookmarks to our format")]
        public ParseOptions ParseVerb { get; set; }

        [VerbOption("merge", HelpText = "merges two bookmark files into one")]
        public MergeOptions MergeVerb { get; set; }
    }
}
