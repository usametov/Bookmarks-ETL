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
        /// <summary>
        /// reads and parses gitmarks
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<IBookmark> ParseBookmarks(string filePath)
        {
            var result = new List<IBookmark>();
            // enumerate tag files, read the content, sanitize and MAP it to tag name
            var invertedTags = Directory.EnumerateFiles
                (Path.Combine(filePath, Constants.TAGS_DIR)).Select(tf => new KeyValuePair<TagContent[], string>
                                                        (
                                                            JsonConvert.DeserializeObject<TagContent[]>(CleanJson(tf))
                                                            ,
                                                            Path.GetFileName(tf) // value set to tag name \\).
                                                        ));

            var urlHashComparer = new UrlHashComparer();//this will be used to REDUCE tags
            var bookmarkFiles = Directory.EnumerateFiles // enumerate bookmarks
                (Path.Combine(filePath, Constants.BOOKMARKS_DIR));
            //construct IBookmarks and add to result
            foreach (var boo in bookmarkFiles)
            {

                var parsedBoo = JsonConvert.DeserializeObject<BookmarkContent>(File.ReadAllText(boo));
                var gitmark = CreateGitmark(parsedBoo);
                //add tags                
                gitmark.Tags = invertedTags.Where(inv => inv.Key.Contains
                                                (new TagContent { hash = parsedBoo.hash }, urlHashComparer))
                                                .Select(inv => inv.Value).ToList();

                result.Add(gitmark);
            }

            return result;
        }

        private static string CleanJson(string tf)
        {
            //return string.Format("[{0}]", File.ReadAllText(tf)
            //                                    .Replace("[{", "{")
            //                                    .Replace("}]{", "},{")
            //                                    .Replace("}]", "}")
            //                                    .Replace("}{", "},{"));
            return File.ReadAllText(tf);
        }

        /// <summary>
        /// makes bookmark from gitmark content
        /// </summary>
        /// <param name="parsedBookmark"></param>
        /// <returns></returns>
        public Gitmark CreateGitmark(BookmarkContent parsedBookmark) {
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

    public class BookmarkContent {
        /// <summary>
        /// this is a hash of uri below 
        /// </summary>
        public string hash { get; set; }

        public string rights { get; set; }

        public string creator { get; set; }
        /// <summary>
        /// this is link text
        /// </summary>
        public string uri { get; set; }

        public string time { get; set; }
        
        public string title { get; set; }
    }

    /// <summary>
    /// tag content obj should be one-to-one with BookmarkContent
    /// </summary>
    public class TagContent {

        public string ver { get; set; }

        public string creator { get; set; }

        /// <summary>
        /// this is a hash of uri below 
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// this is bookmark 
        /// </summary>
        public string uri { get; set; }

        /// <summary>
        /// this is link text
        /// </summary>        
        public string title { get; set; }
    }

    public class UrlHashComparer : IEqualityComparer<TagContent>
    {
        public bool Equals(TagContent x, TagContent y)
        {
            return x.hash.Equals(y.hash);
        }

        public int GetHashCode(TagContent obj)
        {
            return obj.hash.GetHashCode();
        }
    }
}
