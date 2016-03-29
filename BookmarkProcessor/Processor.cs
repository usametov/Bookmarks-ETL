using Bookmarks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static Bookmarks.Common.Utils;

namespace BookmarkProcessor
{
    public class Processor
    {
        //public static IEnumerable<int> GetTermCounts(Dictionary<string, int> processedTags)
        //{
        //    return processedTags.Values.OrderByDescending(v=>v);            
        //}

        /// <summary>
        /// calculates term counts by using bookmarks to map terms collection, 
        /// and then reducing non-unique terms seq to (term, count) pairs
        /// </summary>
        /// <param name="bookmarks"></param>
        /// <returns>reduces non-unique terms seq to (term, count) pairs</returns>
        public static Tuple<IEnumerable<string>, IEnumerable<KeyValuePair<string, int>>> CalculateTermCounts
            (IEnumerable<IBookmark> bookmarks)
        {
            IEnumerable<string> allTags = GetAllTags(bookmarks);

            return new Tuple<IEnumerable<string>, IEnumerable<KeyValuePair<string, int>>>
                (allTags.Distinct(), ReduceTermsCounts(allTags));
        }

        private static IEnumerable<string> GetAllTags(IEnumerable<IBookmark> bookmarks)
        {
            return bookmarks.SelectMany(b => b.Tags)
                                           .Map(t => SanitizeStr(t));
        }

        /// <summary>
        /// tags2count is a collection of non-unique terms
        /// </summary>
        /// <param name="tags2count"></param>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<string, int>> ReduceTermsCounts(IEnumerable<string> tags2count)
        {
            return from tag in tags2count
                   group tag by tag into g
                   orderby g.Count() descending
                   select new KeyValuePair<string, int>(g.Key, g.Count());
        }

        /// <summary>
        /// gets most freq tags (ordered desc) first excluding non-relevant tags 
        /// </summary>
        /// <param name="tagCounts">collection of term-count pairs</param>
        /// <param name="excludeList">non-relevant tags</param>
        /// <param name="mostFreq">threshold param</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, int>> GetMostFrequentTags(IEnumerable<KeyValuePair<string, int>> tagCounts
                                                                                , IEnumerable<string> excludeList
                                                                                , int mostFreq)
        {
            var filteredSet = tagCounts.Where(tc=>!excludeList.Contains(tc.Key));

            var lBound = filteredSet.Select(kv => kv.Value).OrderByDescending(v=>v).Take(mostFreq).Min();

            return filteredSet.Where(t => t.Value >= lBound).OrderByDescending(kv => kv.Value);
        }

        /// <summary>
        /// gets terms associated with umbrella terms and excluding non-relevant terms
        /// </summary>
        /// <param name="bookmarks"></param>
        /// <param name="umbrellaList"></param>
        /// <param name="excludeList"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, int>> GetAssociatedTerms(IEnumerable<IBookmark> bookmarks
            , IEnumerable<string> umbrellaList, IEnumerable<string> excludeList)
        {
            //TODO: this method should be improved to show actual term counts
            //currently we reduce term counts after mapping terms from filtered bookmarks
            //to reduce proper term counts we need to get allTerms seq from CalculateTermCounts 
            //and then intersect allTerms with filteredTags below
            //at this moment this cannot be done because of thread stack limits
            var filteredBookmarks = FilterBookmarks(bookmarks, umbrellaList);

            var filteredTags = filteredBookmarks.SelectMany(b => b.Tags)
                                           .Map(t => SanitizeStr(t))
                                           .Filter(t => !umbrellaList.Contains(t)
                                                     && !excludeList.Contains(t));
                        
            return ReduceTermsCounts(filteredTags);
        }

        /// <summary>
        /// filters bookmarks having umbrella terms
        /// </summary>
        /// <param name="bookmarks"></param>
        /// <param name="umbrellaList"></param>
        /// <returns></returns>
        public static IEnumerable<IBookmark> FilterBookmarks
            (IEnumerable<IBookmark> bookmarks, IEnumerable<string> umbrellaList)
        {
            return bookmarks.Where(b=>b.Tags.Select(t=>SanitizeStr(t))
                                            .Intersect(umbrellaList.Select(u=>SanitizeStr(u)))
                                            .Any());   
        }
    }
}
