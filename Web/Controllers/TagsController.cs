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
            [FromBody]TagBundleRequest tagBundle)
        {
            return new string[] { "test1", "test2", "test3", "test4" };
        }

        [HttpGet("GetTagBundles")]
        public IEnumerable<string> GetTagBundles(string bookmarksCollection)
        {
            return new string[] { "cryptography", "security", "machine-learning", "tools", "linux" };
        }

        [HttpGet("GetExcludeList")]
        public IEnumerable<string> GetExcludeList(
            [FromBody]TagBundleRequest tagBundle)
        {
            return new string[] { "_test1_", "_test2", "_test3_", "_test4_" };
        }

        [HttpPost("SaveTagBundle")]
        public void SaveTagBundle(
           [FromBody]TagBundleRequest tagBundle)
        {
            
        }

        [HttpPost("SaveExcludeList")]
        public void SaveExcludeList(
           [FromBody]TagBundleRequest tagBundle)
        {

        }        
    }
}
