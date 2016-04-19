using System;
using System.Collections.Generic;
using Bookmarks.Common;

namespace PinterestParser
{
    public class PinterestBookmark : IBookmark
    {
        public DateTime AddDate
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string LinkText
        {
            get; set;
        }

        public string LinkUrl
        {
            get; set;
        }

        public List<string> Tags
        {
            get; set;
        }
    }
}
