using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Bookmarks.Common
{
    /// <summary>
    /// maps to bookmarks table
    /// </summary>
    public class BookmarksCollections
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

    }
}
