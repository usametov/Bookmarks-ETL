using System;
using System.Collections.Generic;
using System.Linq;
using Web.Model;
using Microsoft.AspNet.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        // GET: api/tags
        [HttpPost("CalculateMostFreqTerms")]
        public IEnumerable<string> CalculateMostFreqTerms([FromBody]TagBundleRequest tagBundle)
        {
            return @"cryptography
                    encryption
                    ssl
                    tor                    
                    https".Split(new char[] { '\n' }).Select(t => t.Trim());
        }

        [HttpPost("CalculateAssociatedTerms")]
        public IEnumerable<string> CalculateAssociatedTerms(
            [FromBody]TagBundleRequest tagBundle)
        {
            return new string[] { "stub1", "stub2" };
        }

        [HttpGet("GetTagBundle")]
        public IEnumerable<string> GetTagBundle(
            [FromQuery]string bundleName, [FromQuery]string bookmarksCollectionName)
        {
            return new string[] { "tst1", "tst2", "tst3", "tst4" };
        }

        [HttpGet("GetTagBundles")]
        public IEnumerable<string> GetTagBundles(
            [FromQuery]string bookmarksCollectionName)
        {
            return new string[] { "cryptography", "security", "machine-learning", "tools", "linux" };
        }

        [HttpGet("GetExcludeList")]
        public IEnumerable<string> GetExcludeList(
            [FromQuery]string bundleName)
        {
            return new string[] { "books", "!torontopubliclibrary","papers","!filetype:pdf","paper" };
        }

        [HttpPost("SaveTagBundle")]
        public void SaveTagBundle(
           [FromBody]TagBundleRequest tagBundle)
        {
            #region Null Checks
            if (string.IsNullOrEmpty(tagBundle?.BookmarksCollectionName))
            {
                throw new ArgumentException("tagBundle?.BookmarksCollectionName");
            }

            if (string.IsNullOrEmpty(tagBundle?.BundleName))
            {
                throw new ArgumentException("tagBundle?.BundleName");
            }

            if (tagBundle.TagBundle == null && tagBundle.TagBundle.Count() == 0)
            {
                throw new ArgumentException("tagBundle.ExcludeList is null or empty");
            }
            #endregion
            //TODO: add backend call here
        }

        [HttpPost("CreateTagBundle")]
        public void CreateTagBundle(
           [FromBody]TagBundleRequest tagBundle)
        {
            #region Null Checks
            if (string.IsNullOrEmpty(tagBundle?.BookmarksCollectionName))
            {
                throw new ArgumentException("tagBundle?.BookmarksCollectionName");
            }

            if (string.IsNullOrEmpty(tagBundle?.BundleName))
            {
                throw new ArgumentException("tagBundle?.BundleName");
            }
            #endregion
            //TODO: add backend call here
        }

        [HttpPost("SaveExcludeList")]
        public void SaveExcludeList(
           [FromBody]TagBundleRequest tagBundle)
        {
            #region Null Checks
            if (string.IsNullOrEmpty(tagBundle?.BookmarksCollectionName))
            {
                throw new ArgumentException("tagBundle?.BookmarksCollectionName");
            }

            if(string.IsNullOrEmpty(tagBundle?.BundleName))
            {
                throw new ArgumentException("tagBundle?.BundleName");
            }

            if (tagBundle.ExcludeList == null && tagBundle.ExcludeList.Count() == 0)
            {
                throw new ArgumentException("tagBundle.ExcludeList is null or empty");
            }
            #endregion

            //TODO: add backend call here
        }
    }
}
