
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
            
            var bookmarkFiles = Directory.EnumerateFiles
                (Path.Combine(filePath, Constants.BOOKMARKS_DIR));

            var tagFiles = Directory.EnumerateFiles
                (Path.Combine(filePath, Constants.TAGS_DIR));
            
            var inverted = tagFiles.Select(tf => new KeyValuePair<TagFormat[], string>(                                                               
                                                            JsonConvert.DeserializeObject<TagFormat[]>
                                                            (string.Format("[{0}]", File.ReadAllText(tf)).Replace("}{", "},{"))
                                                            ,
                                                            Path.GetFileName(tf)
                                                        ));
            
            var tagFormatComparer = new TagFormatComparer();

            //construct IBookmarks and add to result
            foreach (var boo in bookmarkFiles) {

                var parsedBoo = JsonConvert.DeserializeObject<BookmarkFormat>(File.ReadAllText(boo));
                var gitmark = CreateGitmark(parsedBoo);
                //add tags                
                gitmark.Tags = inverted.Where(inv => inv.Key.Contains
                                                (new TagFormat { hash = parsedBoo.hash }, tagFormatComparer))
                                                .Select(inv=>inv.Value).ToList();

                result.Add(gitmark);
            }

            return result;
        }

        public Gitmark CreateGitmark(BookmarkFormat parsedBookmark) {
            var gitmark = new Gitmark
            {
                LinkUrl = parsedBookmark.uri
                   ,
                LinkText = parsedBookmark.title
                   ,
                Description = string.Empty
            };

            DateTime addDate = DateTime.Now;
            if (DateTime.TryParse(parsedBookmark.time, out addDate))
                gitmark.AddDate = addDate;

            return gitmark;
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
