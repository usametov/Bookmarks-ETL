using System.Security.Cryptography;
namespace Bookmarks.Common
{    
    public class TagBundle 
    {
        public string Id { get; set; }

        public string[] BookmarksCollections { get; set; }
        
        public string[] Tags { get; set; }
        
        public string[] ExcludeTags { get; set; }
        
        public string[] ExcludeTagBundles { get; set; }
        
        public string Name { get; set; }

        public static TagBundle Create(string name, string[] bookmarkCollectionIds)
        {
            var bundle = new TagBundle { Name = name, BookmarksCollections = bookmarkCollectionIds };
            bundle.Id = Utils.ComputeHash(name + bookmarkCollectionIds, MD5.Create());
            return bundle;
        }
    }
}
