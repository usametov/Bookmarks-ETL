using Bookmarks.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDbImportUtil
{
    public static class MergeBookmarks
    {
        public static List<Bookmark> Merge(string bookmarksFile1, string bookmarksFile2)
        {            
            var bookmarks1 = JsonConvert.DeserializeObject<List<Bookmark>>(bookmarksFile1);
            var bookmarks2 = JsonConvert.DeserializeObject<List<Bookmark>>(bookmarksFile2);            
            //union all
            var result = bookmarks1.Union(bookmarks2, new EqualityComparer<Bookmark>(Equals));
            //group and merge
            return result.GroupBy(b => b.LinkUrl)
                      .Select(bg =>
                                new Bookmark
                                {
                                    LinkUrl = bg.Key
                                    ,//concatenate descriptions
                                    Description = string.Join(" ... ", bg.SelectMany(b => b.Description).Distinct().ToArray())
                                    ,//concatenate anchor text
                                    LinkText = string.Join(" ... ", bg.SelectMany(b => b.LinkText).Distinct().ToArray())
                                    ,//merge tags
                                    Tags = bg.SelectMany(b => b.Tags).Distinct().ToList()
                                })
                      .ToList();            
        }

        private static bool Equals(Bookmark b1, Bookmark b2)
        {            
            var tag1 = new HashSet<string>(b1.Tags.Distinct());
            var tag2 = new HashSet<string>(b2.Tags.Distinct());
                        
            return (b1.LinkUrl.Equals(b2.LinkUrl)
                    && b1.LinkText.Equals(b2.LinkText)
                    && b1.Description.Equals(b2.Description)
                    && tag1.SetEquals(tag2));
        }
    }
}
