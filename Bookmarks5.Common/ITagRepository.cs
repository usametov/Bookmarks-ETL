using System.Collections.Generic;

namespace Bookmarks5.Common
{
    public interface ITagRepository
    {
        IEnumerable<string> GetTagBundle(string bundleName
            , string bookmarksCollectionName);

        IEnumerable<string> GetTagBundles(string bookmarksCollectionName);

        IEnumerable<string> GetMostFrequentTags(string bundleName
            , string bookmarksCollectionName, int threshold);

        IEnumerable<string> GetTagAssociations(string bundleName
            , string bookmarksCollectionName);

        IEnumerable<string> GetExcludeList(string bundleName
            , string bookmarksCollectionName);

        void SaveExcludeList(string tagBundleName
            , IEnumerable<string> excludeList, string bookmarksCollectionName);

        void SaveTagBundleList(string tagBundleName
            , IEnumerable<string> topTags, string bookmarksCollectionName);

        void CreateTagBundle(string bundleName, string bookmarksCollectionName);
    }
}
