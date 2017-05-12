using Bookmarks.Common;
using System.Collections.Generic;
using System;

namespace ChromeBookmarksParser
{
    public class ChromeBookmarks : IBookmark
    {
        public DateTime AddDate
        {
            get;

            set;
        }

        public string Description
        {
            get;

            set;
        }

        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public List<string> Tags { get; set; }
        
    }
}