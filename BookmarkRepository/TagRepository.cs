using System;
using System.Collections.Generic;
using Bookmarks5.Common;

namespace BookmarkRepository
{
    public class TagRepository : ITagRepository
    {
        public void CreateTagBundle(string bundleName, string bookmarksCollectionName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetExcludeList(string bundleName, string bookmarksCollectionName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetMostFrequentTags(string bundleName, string bookmarksCollectionName, int threshold)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetTagAssociations(string bundleName, string bookmarksCollectionName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetTagBundle(string bundleName, string bookmarksCollectionName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetTagBundles(string bookmarksCollectionName)
        {
            throw new NotImplementedException();
        }

        public void SaveExcludeList(string tagBundleName, IEnumerable<string> excludeList, string bookmarksCollectionName)
        {
            throw new NotImplementedException();
        }

        public void SaveTagBundleList(string tagBundleName, IEnumerable<string> topTags, string bookmarksCollectionName)
        {
            throw new NotImplementedException();
        }
    }

}
