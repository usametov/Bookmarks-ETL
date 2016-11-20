using MongoDbImportUtil;
using System;
using System.IO;
using CommandLine;
using System.Text;

namespace BookmarksETL_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write(@"Usage: 
parse -p [parser_type] -i input -o output_file 
merge --file1 [file1] --file2 [file2]");

                Console.ReadKey();
                return;
            }

            string invokedVerb = null;
            object invokedVerbInstance = null;

            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options, (verb, subOptions) =>
                          {
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
        [Option('p', "parser", HelpText = "provide parser type", Required = true)]
        public string ParserType { get; set; }

        [Option('i', "input", HelpText = "pass input file", Required = true)]
        public string InputFile { get; set; }    

        [Option('o', "output", HelpText = "provide output path", Required = true)]
        public string OutputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("Usage:");
            usage.AppendLine("parse -p [parser_type] -i input -o output_file");
            return usage.ToString();
        }
    }

    internal class MergeOptions
    {
        [Option("file1", HelpText = "pass bookmarksFile1", Required = true)]
        public string File1 { get; set; }

        [Option("file2", HelpText = "pass bookmarksFile2", Required = true)]
        public string File2 { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("Usage:");
            usage.AppendLine("merge --file1 [file1] --file2 [file2]");
            return usage.ToString();
        }
    }

    internal class Options
    {
        [VerbOption("parse", HelpText = "transforms external bookmarks to our format")]
        public ParseOptions ParseVerb { get; set; }

        [VerbOption("merge", HelpText = "merges two bookmark files into one")]
        public MergeOptions MergeVerb { get; set; }
    }
}
