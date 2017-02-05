using System.Security.Cryptography;
namespace Bookmarks.Common
{    
    public class TagBundle 
    {
        public string Id { get; set; }

        public string BookmarkCollectionId { get; set; }
        public string[] Tags { get; set; }
        public string[] ExcludeTags { get; set; }
        public string Name { get; set; }

        public static TagBundle Create(string name, string bookmarkCollectionsId)
        {
            var bundle = new TagBundle { Name = name, BookmarkCollectionId = bookmarkCollectionsId };
            bundle.Id = Utils.ComputeHash(name + bookmarkCollectionsId, MD5.Create());
            return bundle;
        }
    }
}
