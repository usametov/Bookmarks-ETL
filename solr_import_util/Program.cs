using System;
namespace solr_import_util
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new DeliciousParser.Parser();
            //var bookmarks = parser.ParseBookmarks("../../storage/delicious-2016-01-13.html");
            var bookmarks = parser.ParseBookmarks("../../storage/delicious-2015-12-19.html");
            Console.WriteLine("Bookmarks Count: " + bookmarks.Count);
        }
    }
}
