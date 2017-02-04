using Bookmarks.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace MongoDbImportUtil
{
    public class Bookmarks2Json
    {
        public Bookmarks2Json(IBookmarkParser pars) {
            Parser = pars;
        }

        public IBookmarkParser Parser { get; private set; }

        public string Write(string bookmarksFile) {
            
            var bookmarks = Parser.ParseBookmarks(bookmarksFile);
            return JsonConvert.SerializeObject(bookmarks);
        }

        public static string ExportToMongoFormat(List<Bookmark> bookmarks) {

            var mappings = new Dictionary<string, string>
            {
                {"Id", "_id"},    
            };

            var settings = new JsonSerializerSettings();                        
            settings.ContractResolver = new CustomContractResolver(mappings);
            return JsonConvert.SerializeObject(bookmarks, settings);
        } 
    }

    public class CustomContractResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public CustomContractResolver(Dictionary<string, string> mappings)
        {
            PropertyMappings = mappings;
        }
        
        protected override string ResolvePropertyName(string propertyName)
        {
            string resolvedName = null;
            var resolved = PropertyMappings.TryGetValue(propertyName, out resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
}
