using System;
using NUnit.Framework;
using MongoDbImportUtil;
using System.IO;
using Newtonsoft.Json;

namespace TestMongoDbImport
{
    [TestFixture]
    public class MergeTests
    {
        //[TestCase(@"C:\code\csharp6\Tagging-Util\storage\delicious-2016-03-16.json"
        //        , @"C:\code\csharp6\Tagging-Util\storage\gitmarks-apr20-2016.json"
        //        , @"C:\code\csharp6\Tagging-Util\storage\bookmarks-merge.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\storage\backup-nov21.txt"
        //        , @"C:\code\csharp6\Tagging-Util\storage\gitmarks-nov21-2016.json"
        //        , @"C:\code\csharp6\Tagging-Util\storage\bookmarks-merge.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\storage\bookmarks-merge-nov21-2016.json"
        //        , @"C:\code\csharp6\Tagging-Util\storage\pinterest-nov21-2016.json"
        //        , @"C:\code\csharp6\Tagging-Util\storage\bookmarks-merge.json")]
        [TestCase(@"C:\code\csharp6\Tagging-Util\storage\bookmarks-merge-nov22-2016.json"
                , @"C:\code\csharp6\Tagging-Util\storage\gitmarks-jan25.json"
                , @"C:\code\csharp6\Tagging-Util\storage\bookmarks-merge-jan26.json")]
        public void TestMergeBookmarks(string file1, string file2, string outputPath)
        {
            var bookmarks = MergeBookmarks.Merge(file1, file2);
            var content = JsonConvert.SerializeObject(bookmarks);
            using (var writer = new StreamWriter(outputPath))
            {
                writer.Write(content);
            }
        }
    }
}
