
using Bookmarks.Common;
using Newtonsoft.Json;
//using System.IO;

namespace MongoDbImportUtil
{
    public class Bookmarks2Json
    {
        public Bookmarks2Json(IBookmarkParser pars) {
            Parser = pars;
        }

        public IBookmarkParser Parser { get; set; }

        public string Write(string bookmarksFile, string outputPath) {
            
            var bookmarks = Parser.ParseBookmarks(bookmarksFile);
            return JsonConvert.SerializeObject(bookmarks);
        }
    }
}
