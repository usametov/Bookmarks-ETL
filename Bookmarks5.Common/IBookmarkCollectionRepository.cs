
using System.Collections.Generic;

namespace Bookmarks5.Common
{
    public interface IBookmarkCollectionRepository
    {
        string ConnectionString{get;set;}

        IEnumerable<string> GetBookmarkCollections();

        void SetDefaultBookmarkCollection(string bookmarkCollection);
    }
}
