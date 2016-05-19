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
        IMongoClient _client;
        IMongoDatabase _database;

        public MongoProcessor(string connectionString)
        {

            ConnectionString = connectionString;
            _client = new MongoClient(ConnectionString);
            _database = _client.GetDatabase("astanova-bookmarks");

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

        public IEnumerable<TagCount> CalculateTermCounts()
        {
            var bookmarks = _database.GetCollection<BsonDocument>("bookmarks");
                        
            var aggregate = bookmarks.Aggregate(BuildTagCountsPipelineDefinition(0, 1000));

            return aggregate.ToList();
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


    }
}
