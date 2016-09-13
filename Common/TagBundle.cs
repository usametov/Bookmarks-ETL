
using MongoDB.Bson;

namespace Bookmarks.Common
{    
    public class TagBundle 
    {
        public ObjectId Id { get; set; }
        public string[] Tags { get; set; }
        public string[] ExcludeTags { get; set; }
        public string Name { get; set; }        
    }
}
