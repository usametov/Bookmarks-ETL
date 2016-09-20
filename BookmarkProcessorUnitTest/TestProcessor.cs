using System;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BookmarkProcessor;
using LanguageExt;
using MongoDbImportUtil;
using System.Configuration;

namespace BookmarkProcessorUnitTest
{
    [TestFixture]
    public class TestProcessor
    {
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\mstech-top-tags.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-android.txt")]
        public void TestLoadTagBundle(string path) 
        {
            LoadTagBundle(path);
        }

        /// <summary>
        /// Loads tags from flat file. Removes quotes, trimming whitespaces and converts everything to lower case        
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<string> LoadTagBundle(string path)
        {
            using (var reader = File.OpenText(path))
            {
                var txt = reader.ReadToEnd();
                return txt.Split(new char[] { '\r', '\n', '\t', ',' }
                               , StringSplitOptions.RemoveEmptyEntries)
                                    .Map(str => str.Replace("\"", "").Trim().ToLower())
                                    .Filter(str=>!str.Equals(string.Empty));
            }            
        }

        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "mstech-associated.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\mstech-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4mstech.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "security-associated.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\security-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4security.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "android-associated.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\mobile-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-android.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "ML-associated.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\ML-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-ML.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "diy-associated.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\diy-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-diy.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "computer-networks-associated.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\computer-networks-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-computer-networks.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "books-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\books-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-books.txt")]   
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "linux-associated.txt" 
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\linux-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-linux.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "cryptocurrencies-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\cryptocurrencies-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-cryptocurrencies.txt")]          
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "video-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\video-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-video.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "sourcecode-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\sourcecode-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-sourcecode.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "tools-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\tools-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-tools.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "communication-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\communication-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-communication.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "virtualization-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\virtualization-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-virtualization.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "webdev-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\webdev-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-webdev.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "moocs-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\moocs-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-moocs.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-2016-03-16.html"
        //       , "cryptography-associated.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\cryptography-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-cryptography.txt")]
        public void TestGetAssociatedTerms(string bookmarksFile
            , string outputPath, string tagBundleFile, string excludeFile) 
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);
            
            var associatedTerms = Processor.GetAssociatedTerms(bookmarks
                , LoadTagBundle(tagBundleFile), LoadTagBundle(excludeFile));

            PrintTerms(associatedTerms, outputPath);
        }

        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\ML-top-tags.txt")]
        public void TestFilterBookmarks(string bookmarksFile, string toptags)
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);

            var filtered = Processor.FilterBookmarks(bookmarks, LoadTagBundle(toptags));
            Console.WriteLine("Filtered Bookmarks Count: " + filtered.Count());
        }

        private void PrintTerms(IEnumerable<KeyValuePair<string, int>> termCounts, string output)
        {            
            using(var writer = new StreamWriter(output))
               foreach (var termPair in termCounts)
                {
                    writer.WriteLine(string.Format("{0} -- {1}", termPair.Key, termPair.Value));
                }
            
        }

        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "mstech-top50.txt"                
        ////        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4mstech.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "security-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4security.txt")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "android-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-android.txt")]   
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "ML-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-ML.txt")] 
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "diy-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-diy.txt")]   
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "computer-networks-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-computer-networks.txt")]   
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "books-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-books.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "linux-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-linux.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "cryptocurrencies-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-cryptocurrencies.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "video-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-video.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "sourcecode-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-sourcecode.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "tools-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-tools.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "communication-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-communication.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "virtualization-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-virtualization.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "webdev-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-webdev.txt")]  
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-2016-03-16.html"
        //       , "moocs-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-moocs.txt")]
        public void TestGetMostFrequentTags(string bookmarksFile
                                            , string outputPath
                                            , string excludeFile
                                            , int threshold)
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);
                        
            var processedTags = Processor.CalculateTermCounts(bookmarks);
            
            var freqTerms = Processor.GetMostFrequentTags(processedTags.Item2, LoadTagBundle(excludeFile), threshold);
            PrintTerms(freqTerms, outputPath);
        }


        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-2016-03-16.html"
        //       , "cryptography-top-counts.txt"               
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-cryptography.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\cryptography-top-tags.txt", 100)]
        public void TestGetMostFrequentTags(string bookmarksFile
                                        , string outputPath
                                        , string excludeFile
                                        , string topTagsFile
                                        , int threshold)
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);

            var processedTags = Processor.CalculateTermCounts(bookmarks);

            var excludeList = LoadTagBundle(excludeFile);
            excludeList = excludeList.Append(LoadTagBundle(topTagsFile));
                        
            var freqTerms = Processor.GetMostFrequentTags(processedTags.Item2, excludeList, threshold);
            PrintTerms(freqTerms, outputPath);
        }

        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-2016-03-16.html"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\security-top-tags.txt"
        //       , "cryptography-top-counts.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-cryptography.txt")]
        public void TestGetMostFrequentTagsFiltered(string bookmarksFile
            , string filterTags, string outputPath, string excludeFile)
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);
            var filteredBookmarks = Processor.FilterBookmarks(bookmarks, LoadTagBundle(filterTags));

            var processedTags = Processor.CalculateTermCounts(filteredBookmarks.ToList());

            var freqTerms = Processor.GetMostFrequentTags(processedTags.Item2, LoadTagBundle(excludeFile), 80);
            PrintTerms(freqTerms, outputPath);
        }

        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-2016-03-16.html"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\delicious-2016-03-16.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\bibsonomy.xml"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\bibsonomy.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest-p1.json"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest1.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest-p2.json"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest2.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest-p3.json"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest3.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest-p4.json"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest4.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest-p5.json"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest5.json")]
        //[TestCase(@"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest-p6.json"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\pinterest6.json")]
        public void TestWriteJson(string bookmarksFile, string outputPath)
        {
            //var converter = new Bookmarks2Json(new DeliciousParser.Parser());
            //var converter = new Bookmarks2Json(new BibsonomyParser.Parser());
            var converter = new Bookmarks2Json(new PinterestParser.Parser());
            var content = converter.Write(bookmarksFile, outputPath);
            using (var writer = new StreamWriter(outputPath))                
            {
                writer.Write(content);
            }
        }

        //[TestCase(@"C:\code\python\gitmarks\gitmarks_2\gitmark_base\public\"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\gitmarks.json")]        
        public void TestWriteJsonFromMultipleFiles(string bookmarksRepo, string outputPath)
        {
            var converter = new Bookmarks2Json(new GitmarksParser.Parser());
            var content = converter.Write(bookmarksRepo, outputPath);
            using (var writer = new StreamWriter(outputPath))
            {
                writer.Write(content);
            }
        }

        [TestCase("mstech")]
        public void TestGetBookmarksByTagBundle(string tagBundleName)
        {

        }
    }
}
