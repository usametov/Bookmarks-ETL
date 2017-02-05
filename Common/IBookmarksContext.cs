using System;
using System.Collections.Generic;

namespace Bookmarks.Common
{
    public interface IBookmarksContext
    {
        string BookmarksCollection { get; set; }
        
        IEnumerable<TagCount> CalculateTermCounts(int bufferSize);

        IEnumerable<TagCount> GetAssociatedTerms(TagBundle tagBundle, int bufferSize);
        
        string ConnectionString { get; set; }
        
        void CreateBookmarksCollection(string name);
        
        void CreateTagBundle(TagBundle tagBundle);

        void EditTagBundle(string tagBundleId, string tagBundleName);

        IEnumerable<string> ExtractExcludeTags(string[] excludeTagBundles
                                            , Bookmarks.Common.TagBundle tagBundle);

        IEnumerable<Bookmark> GetBookmarksByTagBundle(string tagBundleName, int? skip, int? take);

        IEnumerable<BookmarksCollections> GetBookmarksCollections();

        IEnumerable<TagCount> GetNextMostFrequentTags(string tagBundleName, string[] excludeTagBundles, int limitTermCounts);

        TagBundle GetTagBundleById(string objId);

        IEnumerable<TagBundle> GetTagBundles(string name);

        User GetUserByUsernameAndPasswdHash(string userName, string passwordHash);

        void UpdateTagBundle(TagBundle tagBundle);
        
        void UpdateTagBundleById(TagBundle tagBundle);

        void UpdateExcludeList(string tagBundleId, string[] excludeTags);

        void UpdateBookmarkCollectionId(string tagBundleId, string bookmarksCollectionId);

        IEnumerable<TagBundle> GetTagBundleNames(string bookmarksCollectionId);

        IEnumerable<TagCount> CalculateRemainingTermCounts(int bufferSize, string[] excludeTagBundles);

        IEnumerable<Bookmarks.Common.Bookmark> BackupBookmarks();
    }
}
