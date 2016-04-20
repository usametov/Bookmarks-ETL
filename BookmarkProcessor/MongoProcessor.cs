using MongoDB.Bson;
using MongoDB.Driver;

namespace BookmarkProcessor
{
    public class MongoProcessor
    {
        IMongoClient _client;
        IMongoDatabase _database;

        public MongoProcessor() {
            _client = new MongoClient();
            _database = _client.GetDatabase("astanova-bookmarks");
        }

        public string ConnectionString
        {
            get;
            set;
        }



    }
}
