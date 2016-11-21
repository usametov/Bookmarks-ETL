
using Bookmarks.Common;
using Newtonsoft.Json;


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
    }
}
