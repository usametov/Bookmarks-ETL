using Bookmarks.Common;
using System;
using System.Collections.Generic;

namespace delicious_parser
{
    public class DeliciousBookmark : IBookmark
    {
        public string LinkUrl
        {
            get;
            set;
        }

        public string LinkText
        {
            get;
            set;
        }

        public DateTime AddDate
        {
            get;
            set;
        }

        public bool IsPrivate
        {
            get;
            set;
        }

        public List<string> Tags
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
