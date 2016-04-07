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
            return new string[] { "test1", "test2", "test3", "test4" };
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
            //TODO: add backend call here
        }

        [HttpPost("SaveExcludeList")]
        public void SaveExcludeList(
           [FromBody]TagBundleRequest tagBundle)
        {
            //TODO: add backend call here
        }
    }
}
