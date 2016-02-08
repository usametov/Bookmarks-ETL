﻿using System;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BookmarkProcessor;

namespace BookmarkProcessorUnitTest
{
    [TestFixture]
    public class TestProcessor
    {
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\mstech-top-tags.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-android.txt")]
        public void TestLoadTagBundle(string path) 
        {
            LoadTagBundle(path);
        }

        private List<string> LoadTagBundle(string path)
        {
            var result = new List<string>();
            using (var reader = File.OpenText(path))
            {
                var txt = reader.ReadToEnd();
                var split = txt.Split(new char[] { '\r', '\n', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            
                foreach (var str in split)
                {
                    string tmp = str.Replace("\"", "").Trim().ToLower();
                    //add only non-empty strings
                    if(!tmp.Equals(string.Empty)) 
                        result.Add(tmp);
                }

                
            }
            return result;
        }

        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "mstech-associated.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\mstech-top-tags.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4mstech.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "security-associated.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\security-top-tags.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4security.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "android-associated.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\mobile-top-tags.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-android.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "ML-associated.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\ML-top-tags.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-ML.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "diy-associated.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\diy-top-tags.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-diy.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "computer-networks-associated.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\computer-networks-top-tags.txt"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-computer-networks.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "books-associated.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\books-top-tags.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-books.txt")]   
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "linux-associated.txt" 
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\linux-top-tags.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-linux.txt")]  
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "cryptocurrencies-associated.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\cryptocurrencies-top-tags.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-cryptocurrencies.txt")]  
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "cryptography-associated.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\cryptography-top-tags.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-cryptography.txt")]
        [TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
               , "video-associated.txt"
               , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\video-top-tags.txt"
               , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-video.txt")]  
        public void TestGetAssociatedTerms(string bookmarksFile
            , string outputPath, string tagBundleFile, string excludeFile) 
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);

            var associatedTerms = Processor.GetAssociatedTerms(bookmarks
                , LoadTagBundle(tagBundleFile), LoadTagBundle(excludeFile));

            PrintTerms(associatedTerms, outputPath);
        }

        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\ML-top-tags.txt")]
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

        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //        , "mstech-top50.txt"                
        ////        , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4mstech.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "security-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4security.txt")]
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "android-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-android.txt")]   
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "ML-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-ML.txt")] 
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "diy-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-diy.txt")]   
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "computer-networks-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-computer-networks.txt")]   
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "books-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-books.txt")]  
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "linux-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-linux.txt")]  
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "cryptocurrencies-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-cryptocurrencies.txt")]  
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , "video-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-video.txt")]  
        public void TestGetMostFrequentTags(string bookmarksFile, string outputPath, string excludeFile)
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(bookmarksFile);
                        
            var processedTags = Processor.CalculateTermCounts(bookmarks);
            
            var freqTerms = Processor.GetMostFrequentTags(processedTags.Item2, LoadTagBundle(excludeFile), 80);
            PrintTerms(freqTerms, outputPath);
        }
        
        //[TestCase(@"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\delicious-feb4-2016.html"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\security-top-tags.txt" 
        //       , "cryptography-top-counts.txt"
        //       , @"C:\code\asta-nova\Card Sort Util\solr_import_util\storage\exclude-list4-cryptography.txt")]
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
    }
}
