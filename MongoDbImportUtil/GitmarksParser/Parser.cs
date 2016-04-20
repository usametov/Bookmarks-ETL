
using System.IO;
using System.Collections.Generic;
using Bookmarks.Common;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace GitmarksParser
{
    public class Parser : IBookmarkParser
    {
        public List<IBookmark> ParseBookmarks(string filePath)
        {
            var result = new List<IBookmark>();

            string bookmarksDir = Path.Combine(filePath, Constants.BOOKMARKS_DIR);
            string tagsDir = Path.Combine(filePath, Constants.TAGS_DIR);

            var bookmarkFiles = Directory.EnumerateFiles(bookmarksDir);
            var tagFiles = Directory.EnumerateFiles(tagsDir);

            var parsedBookmarks = new List<BookmarkFormat>();
            var parsedTags = new Dictionary<string, TagFormat[]>();
            
            foreach (var tg in tagFiles) {
                var jsonStrFix = string.Format("[{0}]", File.ReadAllText(tg)).Replace("}{", "},{");
                parsedTags.Add
                    (Path.GetFileName(tg)
                    ,JsonConvert.DeserializeObject<TagFormat[]>(jsonStrFix));
            }

            var inverted = parsedTags.Select(pt => new { TF = pt.Value, Tag = pt.Key });
            var tagFormatComparer = new TagFormatComparer();

            //construct IBookmarks and add to result
            foreach (var boo in bookmarkFiles) {

                var parsedBoo = JsonConvert.DeserializeObject<BookmarkFormat>(File.ReadAllText(boo));
                var gitmark = new Gitmark
                {
                    LinkUrl = parsedBoo.uri
                    ,
                    LinkText = parsedBoo.title
                    ,
                    Description = string.Empty
                };

                DateTime addDate = DateTime.Now;
                if(DateTime.TryParse(parsedBoo.time, out addDate))
                    gitmark.AddDate = addDate;

                //add tags                
                gitmark.Tags = inverted.Where(inv => inv.TF.Contains
                                                (new TagFormat { hash = parsedBoo.hash }, tagFormatComparer))
                                                .Select(inv=>inv.Tag).ToList();

                result.Add(gitmark);
            }

            return result;
        }
    }

    public class BookmarkFormat {

        public string hash { get; set; }

        public string rights { get; set; }

        public string creator { get; set; }

        public string uri { get; set; }

        public string time { get; set; }

        public string title { get; set; }
    }

    public class TagFormat {

        public string ver { get; set; }

        public string creator { get; set; }

        public string hash { get; set; }

        public string uri { get; set; }

        public string title { get; set; }
    }

    public class TagFormatComparer : IEqualityComparer<TagFormat>
    {
        public bool Equals(TagFormat x, TagFormat y)
        {
            return x.hash.Equals(y.hash);
        }

        public int GetHashCode(TagFormat obj)
        {
            return obj.hash.GetHashCode();
        }
    }
}
