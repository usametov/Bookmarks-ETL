using System;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BookmarkProcessor;
using MongoDbImportUtil;
using System.Configuration;
using Bookmarks.Common;

namespace BookmarkProcessorUnitTest
{
    [TestFixture]
    public class TestMongoProcessor
    {
        string connectionString;

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
                                    .Filter(str => !str.Equals(string.Empty));
            }
        }

        [SetUp]
        public void SetupConnectionString()
        {
            string appConfigFilePath = 
                @"C:\code\csharp6\Tagging-Util\BookmarkProcessorUnitTest\app.config";

            Configuration config = ConfigurationManager.OpenExeConfiguration
                (ConfigurationUserLevel.None);

            AppDomain.CurrentDomain.SetData
                ("APP_CONFIG_FILE", appConfigFilePath);

            ConnectionStringsSection section =
                config.GetSection("connectionStrings")
                as ConnectionStringsSection;

            connectionString = section.ConnectionStrings[0].ConnectionString;
            
        }

        //[TestCase]
        public void TestGetMostFrequentTags() {
            var processor = new MongoProcessor(connectionString);
            
            var processedTags = processor.CalculateTermCounts();

            Assert.IsNotEmpty(processedTags);
        }

        //[TestCase("mstech")]
        //[TestCase("security")]
        //[TestCase("android")]
        //[TestCase("machine-learning")]
        //[TestCase("diy")]
        //[TestCase("computer-networks")]
        //[TestCase("books")]
        //[TestCase("linux")] 
        //[TestCase("cryptocurrencies")]
        //[TestCase("video")]
        //[TestCase("tools")]
        //[TestCase("cryptography")]
        //[TestCase("moocs")]
        //[TestCase("webdev")]
        //[TestCase("virtualization")]
        //[TestCase("communication")]
        //[TestCase("sourcecode")]
        public void TestCreateTagBundle(string name) {

            var processor = new MongoProcessor(connectionString);
            processor.CreateTagBundle(new TagBundle { Name = name });
        }

        //[TestCase("mstech"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\mstech-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4mstech.txt")]
        //[TestCase("security"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\security-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4security.txt")]
        //[TestCase("android"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\mobile-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-android.txt")]
        //[TestCase("machine-learning"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\ML-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-ML.txt")]
        //[TestCase("diy"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\diy-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-diy.txt")]
        //[TestCase("computer-networks"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\computer-networks-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-computer-networks.txt")]
        //[TestCase("books"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\books-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-books.txt")]
        //[TestCase("linux"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\linux-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-linux.txt")]
        //[TestCase("cryptocurrencies"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\cryptocurrencies-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-cryptocurrencies.txt")]
        //[TestCase("video"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\video-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-video.txt")]
        //[TestCase("tools", @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\tools-top-tags.txt"
        //        , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-tools.txt")]  
        //[TestCase("sourcecode"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\sourcecode-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-sourcecode.txt")]
        //[TestCase("communication"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\communication-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-communication.txt")]
        //[TestCase("virtualization"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\virtualization-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-virtualization.txt")]
        //[TestCase("webdev"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\webdev-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-webdev.txt")]
        //[TestCase("moocs"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\moocs-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-moocs.txt")]
        //[TestCase("cryptography", @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\cryptography-top-tags.txt"
        //       , @"C:\code\csharp6\Tagging-Util\solr_import_util\storage\exclude-list4-cryptography.txt")]
        public void TestUpdateTagBundle(string name
                                      , string tagBundleFile
                                      , string excludeFile) {

            var tagBundle2Update = new TagBundle { Name = name
                                                , Tags = new string[] { }
                                                , ExcludeTags = new string[] { } };

            tagBundle2Update.Tags = LoadTagBundle(tagBundleFile).ToArray();
            tagBundle2Update.ExcludeTags = LoadTagBundle(excludeFile).ToArray();

            var processor = new MongoProcessor(connectionString);
            processor.UpdateTagBundle(tagBundle2Update);
        }

        //[TestCase(null)]
        //[TestCase("webdev")]
        public void TestGetTagBundles(string name) {

            var processor = new MongoProcessor(connectionString);
            Assert.IsNotEmpty(processor.GetTagBundles(name));
        }
    }
}
