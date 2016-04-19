using System;
using System.Collections.Generic;

namespace Bookmarks.Common
{
    public interface IBookmark
    {
        string LinkUrl { get; set; }
        
        string LinkText { get; set; }
        
        DateTime AddDate { get; set; }
                
        List<string> Tags { get; set; }

        string Description { get; set; }
    }
}
