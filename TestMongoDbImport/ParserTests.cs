using System;
using NUnit.Framework;

namespace TestMongoDbImport
{
    [TestFixture]
    public class ParserTests
    {
        //[TestCase(@"C:\code\csharp6\Tagging-Util\storage\delicious-2016-03-16.html")]        
        public void TestDeliciousParser(string bookmarksFile)
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);
            Assert.IsNotEmpty(bookmarks);
        }

        //[TestCase(@"C:\code\csharp6\Tagging-Util\storage\bibsonomy.xml")]
        public void TestBibsonomyParser(string bookmarksFile)
        {
            var parser = new BibsonomyParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);
            Assert.IsNotEmpty(bookmarks);
        }

        //[TestCase(@"C:\code\csharp6\Tagging-Util\storage\pins.txt")]
        public void TestPinterestParser(string bookmarksFile)
        {
            var parser = new PinterestParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);
            Assert.IsNotEmpty(bookmarks);
        }

        //[TestCase(@"C:\code\csharp6\Tagging-Util\storage\gitmarks\")]
        public void TestGitmarksParser(string bookmarksRepo)
        {
            var parser = new GitmarksParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksRepo);
            Assert.IsNotEmpty(bookmarks);
        }
    }
}
