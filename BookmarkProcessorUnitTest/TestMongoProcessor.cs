using System;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BookmarkProcessor;
using MongoDbImportUtil;
using System.Configuration;

namespace BookmarkProcessorUnitTest
{
    [TestFixture]
    public class TestMongoProcessor
    {
        string connectionString;

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

        [TestCase]
        public void TestGetMostFrequentTags() {
            var processor = new MongoProcessor();
            processor.ConnectionString = connectionString;
        }
    }
}
