using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Bookmarks5.Common;
using Web.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class BookmarkCollectionsController : Controller
    {
        private IBookmarkCollectionRepository bookmarksCollectionRepo;

        public BookmarkCollectionsController(IBookmarkCollectionRepository repo) {
            bookmarksCollectionRepo = repo;
        }

        [HttpGet("GetBookmarkCollections")]
        public IEnumerable<BookmarkCollection> GetBookmarkCollections()
        {
            var result = new List<BookmarkCollection> {
                new BookmarkCollection { Name = "delicious", IsDefault = true }
                , new BookmarkCollection { Name = "bibsonomy", IsDefault = false}
                , new BookmarkCollection { Name = "pinterest" , IsDefault = false}
            };
            //TODO: add service call here
            return result;
        }

        [HttpPost("SetDefaultBookmarkCollection")]
        public void SetDefaultBookmarkCollection([FromBody]BookmarkCollection defaultBookmarkCollection)
        {
            //TODO: add service call here
        }
    }
}
