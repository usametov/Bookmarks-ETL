using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Bookmarks.Common
{
    public static class Utils
    {
        static Regex regex = new Regex(@"[-!_\s]+");

        public static string SanitizeStr(string t)
        {
            return regex.Replace(t, "").ToLowerInvariant();
        }

        public static string ComputeHash(string str2hash, HashAlgorithm hashAlgo)
        {
            if(string.IsNullOrEmpty(str2hash))
                return string.Empty;

            return string.Join(string.Empty
                             , hashAlgo.ComputeHash(Encoding.ASCII.GetBytes(str2hash))
                                                                  .Select(b => b.ToString("X2")));
        }

        public static bool SafeEquals(this string s1, string s2) {

            return (s1 ?? string.Empty).Equals(s2 ?? string.Empty);
        }
    }
}
