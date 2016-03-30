
namespace Web.Model
{
    public class TagBundleRequest
    {
        public string[] TagBundle { get; set; }
        public string[] ExcludeList { get; set; }        
        public string BundleName { get; set; }
        public int Threshold { get; set; }
    }
}
