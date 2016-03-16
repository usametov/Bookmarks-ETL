using Bookmarks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace BookmarkProcessor
{
    public class Processor
    {
        public static IEnumerable<int> GetTermCounts(Dictionary<string, int> processedTags)
        {
            return processedTags.Values.OrderByDescending(v=>v);            
        }

        public static Tuple<IEnumerable<string>, IEnumerable<KeyValuePair<string, int>>> CalculateTermCounts(List<IBookmark> bookmarks)
        {
            var allTags = bookmarks.SelectMany(b => b.Tags)
                                           .Map(t => t.Replace("-", "").ToLowerInvariant());
                        
            return new Tuple<IEnumerable<string>, IEnumerable<KeyValuePair<string, int>>>
                (allTags.Distinct(), from tag in allTags
                                       group tag by tag into g
                                       orderby g.Count() descending
                                       select new KeyValuePair<string, int>(g.Key, g.Count()));
        }

        public static IEnumerable<KeyValuePair<string, int>> GetMostFrequentTags(IEnumerable<KeyValuePair<string, int>> tagCounts
            , IEnumerable<string> excludeList, int mostFreq)
        {
            var filteredSet = tagCounts.Where(tc=>!excludeList.Contains(tc.Key));

            var lBound = filteredSet.Select(kv => kv.Value).OrderByDescending(v=>v).Take(mostFreq).Min();

            return filteredSet.Where(t => t.Value >= lBound).OrderByDescending(kv => kv.Value);
        }

        public static IEnumerable<KeyValuePair<string, int>> GetAssociatedTerms(IEnumerable<IBookmark> bookmarks
            , IEnumerable<string> umbrellaList, IEnumerable<string> excludeList)
        {            
            var filteredBookmarks = FilterBookmarks(bookmarks, umbrellaList);

            var allTags = filteredBookmarks.SelectMany(b => b.Tags)
                                           .Map(t => t.ToLowerInvariant())
                                           .Filter(t => !umbrellaList.Contains(t)
                                                     && !excludeList.Contains(t));

            Console.WriteLine("Filtered Bookmarks Count: " + filteredBookmarks.Count());

            return from tag in allTags
                          group tag by tag into g
                          orderby g.Count() descending
                          select new KeyValuePair<string, int>(g.Key, g.Count());
        }

        public static IEnumerable<IBookmark> FilterBookmarks
            (IEnumerable<IBookmark> bookmarks, IEnumerable<string> umbrellaList)
        {
            return bookmarks.Where(b=>b.Tags.Select(t=>t.ToLower()).Intersect(umbrellaList.Select(u=>u.ToLower())).Any());
        }
    }
}
