using System.Collections.Generic;

namespace Bookmarks.Common
{
    public interface IBookmarkParser
    {
        List<IBookmark> ParseBookmarks(string filePath);
    }
}
