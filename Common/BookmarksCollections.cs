using System.Security.Cryptography;

namespace Bookmarks.Common
{
    /// <summary>
    /// maps to bookmarks table
    /// </summary>
    public class BookmarksCollections
    {
        /// <summary>
        /// should be set to hash of name
        /// </summary>
        public string Id { get; set; }

        public string Name { get; set; }

    }
}
