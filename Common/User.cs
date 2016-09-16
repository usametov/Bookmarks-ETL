using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bookmarks.Common
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
