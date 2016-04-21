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
    }
}
