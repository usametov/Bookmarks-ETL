using Bookmarks.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookmarkProcessor
{
    public class MongoProcessor
    {
        public const string DEFAULT_BOOKMARKS_COLLECTION = "bookmarks";
        private const string DEFAULT_BOOKMARK_DB = "astanova-bookmarks";
        private const int TAG_COUNTS_PAGE_SIZE = 1000;

        public string BookmarksCollection { get; set; }
        IMongoClient _client;
        IMongoDatabase _database;

        public MongoProcessor(string connectionString, string bookmarksCollection = DEFAULT_BOOKMARKS_COLLECTION)
        {

            ConnectionString = connectionString;
            _client = new MongoClient(ConnectionString);
            _database = _client.GetDatabase(DEFAULT_BOOKMARK_DB);
            BookmarksCollection = bookmarksCollection; 

            Init();
        }

        private static void Init()
        {
            if(!BsonClassMap.IsClassMapRegistered(typeof(TagBundle)))
                BsonClassMap.RegisterClassMap<TagBundle>(cm =>
                                {
                                    cm.AutoMap();
                                    cm.MapCreator(t =>
                                    new TagBundle
                                    {
                                        Id = t.Id
                                        ,
                                        ExcludeTags = t.ExcludeTags
                                        ,
                                        Name = t.Name
                                        ,
                                        Tags = t.Tags
                                    });
                                    cm.SetIgnoreExtraElements(true);
                                });
        }

        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// TODO: should this be cached?
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TagCount> CalculateTermCounts()
        {
            var bookmarks = _database.GetCollection<BsonDocument>(BookmarksCollection);
            int bufferSize = GetTagCountsBufferSize();
            var aggregate = bookmarks.Aggregate(BuildTagCountsPipelineDefinition(0, bufferSize));

            return aggregate.ToList();
        }

        private static int GetTagCountsBufferSize()
        {
            return TAG_COUNTS_PAGE_SIZE;
        }

        /// <summary>
        /// builds pipeline definitions
        /// TODO: try using memoization here
        /// </summary>
        /// <param name="skipNum"></param>
        /// <param name="take">TODO: this should be controlled by providing proper buffer size</param>
        /// <returns></returns>
        private PipelineDefinition<BsonDocument, TagCount> BuildTagCountsPipelineDefinition(int skipNum, int take)
        {
            var projectTags = new BsonDocument
            {
                {
                    "$project", new BsonDocument
                    {
                        {"_id", 0},
                        { "Tags", 1 }
                    }
                }
            };

            var unwindTags = new BsonDocument
            {
                {
                    "$unwind", "$Tags"
                }
            };

            var groupByTag = new BsonDocument
            {
                {"$group",
                    new BsonDocument
                    {
                        {"_id",  new BsonDocument
                                             {
                                                 {
                                                     "Tag","$Tags"
                                                 }
                                             }
                        },
                        {"Count", new BsonDocument
                                            {
                                                {
                                                    "$sum", 1
                                                }
                                            }
                        }
                    }
                }
            };

            var projectRename = new BsonDocument
            {
                {
                    "$project", new BsonDocument
                    {
                        {"_id", 0 },
                        {"Tag", "$_id.Tag"},
                        { "Count", 1 }
                    }
                }
            };

            var sort = new BsonDocument
            {
                {"$sort",
                    new BsonDocument { { "Count", -1 } }
                }
            };

            var skip = new BsonDocument
            {
                {
                    "$skip", skipNum
                }
            };

            var limit = new BsonDocument
            {
                {
                    "$limit", take
                }
            };

            return new[] 
            { projectTags, unwindTags, groupByTag
            , projectRename, sort, skip, limit };
            
        }

        public void CreateTagBundle(TagBundle tagBundle) {

            var tagBundles = _database.GetCollection<TagBundle>("tagBundles");

            tagBundles?.InsertOne(tagBundle);
        }

        public void UpdateTagBundle(TagBundle tagBundle){

            var tagBundles = _database.GetCollection<TagBundle>("tagBundles");
            var builder = Builders<TagBundle>.Filter;
            var filter = builder.Eq(t => t.Name, tagBundle.Name);
            var update = Builders<TagBundle>.Update
                .Set(t=>t.Tags, tagBundle.Tags)
                .Set(t => t.ExcludeTags, tagBundle.ExcludeTags) 
                .CurrentDate("lastModified");

            tagBundles.UpdateOne(filter, update);
        }
        
        /// <summary>
        /// gets tag bundle(s)
        /// </summary>
        /// <param name="name">if this is null or empty then get all</param>
        /// <returns></returns>
        public IEnumerable<TagBundle> GetTagBundles(string name)
        {
            IEnumerable<TagBundle> result = null;
            var tagBundles = _database.GetCollection<TagBundle>("tagBundles");
            
            if (string.IsNullOrEmpty(name)){
                result = tagBundles.Find(new BsonDocument()).ToList();
            }
            else {
                var builder = Builders<TagBundle>.Filter;
                var filter = builder.Eq(t => t.Name, name);
                result = tagBundles.Find(filter).ToList();
            }

            return result;
        }

        public TagBundle GetTagBundleById(string objId)
        {
            if (string.IsNullOrEmpty(objId))
                throw new ArgumentNullException("objId");
                        
            var tagBundles = _database.GetCollection<TagBundle>("tagBundles");            
                        
            var builder = Builders<TagBundle>.Filter;
            var filter = builder.Eq(t => t.Id, new ObjectId(objId));

            var resultTask = tagBundles.Find(filter).FirstAsync();            

            return resultTask.Result;
        }

        public IEnumerable<TagCount> GetNextMostFrequentTags(string tagBundleName)
        {            
            var tagCounts = CalculateTermCounts();
            //get tag bundle by name
            var tagBundle = GetTagBundles(tagBundleName).FirstOrDefault();

            if (tagBundle == null)
                throw new ApplicationException("tagBundle not found");
            
            var filteredTags = tagCounts.Filter(tc => !tagBundle.ExcludeTags.Contains(tc.Tag)
                                                      && !tagBundle.Tags.Contains(tc.Tag))
                                        .OrderByDescending(tc => tc.Count).ToList();

            return filteredTags;
        }
    }
}
