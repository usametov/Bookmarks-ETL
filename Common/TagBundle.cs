namespace Bookmarks.Common
{    
    public class TagBundle 
    {
        /// <summary>
        /// should be set to hash of Name + BookmarkCollectionId
        /// </summary>
        public string Id { get; set; }

        public string BookmarkCollectionId { get; set; }
        public string[] Tags { get; set; }
        public string[] ExcludeTags { get; set; }
        public string Name { get; set; }        
    }
}
