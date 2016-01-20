using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarks.Common
{
    public interface IBookmark
    {
        string LinkUrl { get; set; }
        
        string LinkText { get; set; }
        
        DateTime AddDate { get; set; }

        bool IsPrivate { get; set; }

        List<string> Tags { get; set; }
    }
}
