using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bookmarks.Common
{    
    public class TagBundle 
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string[] Tags { get; set; }
        public string[] ExcludeTags { get; set; }
        public string Name { get; set; }        
    }
}
