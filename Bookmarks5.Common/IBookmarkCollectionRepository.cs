
using System.Collections.Generic;

namespace Bookmarks5.Common
{
    public interface IBookmarkCollectionRepository
    {
        IEnumerable<string> GetBookmarkCollections();

        void SetDefaultBookmarkCollection(string bookmarkCollection);
    }
}
