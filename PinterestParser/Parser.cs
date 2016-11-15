using System;
using System.Collections.Generic;
using Bookmarks.Common;
using Newtonsoft.Json;
using System.IO;

namespace PinterestParser
{
    public class Parser : IBookmarkParser
    {
        public List<IBookmark> ParseBookmarks(string filePath)
        {
            var pinterestBookmarks = JsonConvert.DeserializeObject<PinterestPost[]>
                                                                (File.ReadAllText(filePath));

            var result = new List<IBookmark>();

            foreach (var pb in pinterestBookmarks)
            {
                string url = pb.original_link;

                var title = pb.note;

                var tags = new List<string> { pb.board.name };

                var bookmark = new PinterestBookmark
                {
                    LinkUrl = url
                    ,
                    LinkText = title
                    ,
                    Tags = tags
                    ,
                    AddDate = DateTime.Now
                };

                result.Add(bookmark);
            }

            return result;
        }
    }

    /// <summary>
    /// this is needed for json parsing
    /// </summary>
    public class PinterestPost
    {

        public string note { get; set; }

        public string id { get; set; }

        public string original_link { get; set; }

        public Board board { get; set; }
    }

    public class Board
    {

        public string url { get; set; }

        public string id { get; set; }

        public string name { get; set; }
    }
}
