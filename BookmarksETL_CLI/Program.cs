using MongoDbImportUtil;
using System;
using System.IO;

namespace BookmarksETL_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseBookmarks(args);
        }

        private static void ParseBookmarks(string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("src or dest is missing");

            string bookmarksFile = args[0];
            string outputPath = args[1];

            //var converter = new Bookmarks2Json(new DeliciousParser.Parser());
            //var converter = new Bookmarks2Json(new BibsonomyParser.Parser());
            var converter = new Bookmarks2Json(new PinterestParser.Parser());
            var content = converter.Write(bookmarksFile, outputPath);
            using (var writer = new StreamWriter(outputPath))
            {
                writer.Write(content);
            }
        }
    }
}
