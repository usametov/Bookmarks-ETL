using System.Collections.Generic;

namespace Bookmarks5.Common
{
    public interface IBookmarkParser
    {
        List<IBookmark> ParseBookmarks(string filePath);
    }
}
