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

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
