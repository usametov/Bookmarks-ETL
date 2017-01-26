using Bookmarks.Common;
using MongoDbImportUtil;
using NUnit.Framework;
using BookmarksETL_CLI;

namespace TestMongoDbImport
{
    [TestFixture]
    public class ConvertTests
    {
        //[TestCase("delicious", @"C:\code\csharp6\Tagging-Util\storage\delicious-2016-03-16.html", @"C:\code\csharp6\Tagging-Util\storage\delicious-2016-03-16.json")]
        //[TestCase("gitmarks", @"C:\code\csharp6\Tagging-Util\storage\gitmarks", @"C:\code\csharp6\Tagging-Util\storage\gitmarks-jan25.json")]
        //[TestCase("pinterest", @"C:\code\csharp6\Tagging-Util\storage\pins.txt", @"C:\code\csharp6\Tagging-Util\storage\pinterest-nov21-2016.json")]
        public void TestConvert(string parserType, string bookmarksFile, string outputPath)
        {
            Program.ParseBookmarks(parserType, bookmarksFile, outputPath);
        }        
    }
}
