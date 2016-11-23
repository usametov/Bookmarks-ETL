using System;
using NUnit.Framework;
using MongoDbImportUtil;
using System.IO;
using Newtonsoft.Json;
using BookmarksETL_CLI;

namespace TestMongoDbImport
{
    [TestFixture]
    public class GitmarksExportTests
    {
        //[TestCase(@"C:\code\csharp6\Tagging-Util\storage\bookmarks-merge-nov22-2016.json"
        //        , @"C:\code\csharp6\Tagging-Util\storage\gitmarks2\bookmarks")]
        public void TestExportBookmarks(string bookmarksFile, string bookmarksDir)
        {
            GitmarksExportUtil.ExportBookmarks(bookmarksFile, bookmarksDir);
        }

        [TestCase(@"C:\code\csharp6\Tagging-Util\storage\bookmarks-merge-nov22-2016.json"
                , @"C:\code\csharp6\Tagging-Util\storage\gitmarks2\tags")]
        public void TestExportTags(string bookmarksFile, string tagsDir)
        {
            GitmarksExportUtil.ExportTags(bookmarksFile, tagsDir);
        }
    }
}
