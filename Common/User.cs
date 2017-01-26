namespace Bookmarks.Common
{
    public class User
    {
        //should be set to hash of email        
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
