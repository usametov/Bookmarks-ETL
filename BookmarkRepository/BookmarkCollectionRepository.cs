using System;
using System.Collections.Generic;
using Bookmarks5.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookmarkRepository
{
    public class BookmarkCollectionRepository : IBookmarkCollectionRepository
    {
        IMongoClient _client;
        IMongoDatabase _database;

        public BookmarkCollectionRepository() {
            _client = new MongoClient();
            _database = _client.GetDatabase("astanova-bookmarks");
        }

        public string ConnectionString{ get; set; }

        public IEnumerable<string> GetBookmarkCollections()
        {
            throw new NotImplementedException();
        }

        public void SetDefaultBookmarkCollection(string bookmarkCollection)
        {
            throw new NotImplementedException();
        }
    }
}
