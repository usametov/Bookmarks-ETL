using System;
using System.Collections.Generic;
using Bookmarks.Common;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace BibsonomyParser
{
    public class Parser : IBookmarkParser
    {
        public List<IBookmark> ParseBookmarks(string filePath)
        {
            // Load the document using HTMLAgilityPack
            var html = new HtmlDocument();
            html.Load(filePath);

            // Fizzler for HtmlAgilityPack is implemented as the 
            // QuerySelectorAll extension method on HtmlNode
            var document = html.DocumentNode;

            var nodes = document?.QuerySelectorAll(Constants.DOC_NODE);
            var result = new List<IBookmark>();

            foreach (var node in nodes)
            {
                var url = node.Attributes[Constants.HREF_ATTR].Value;
                
                var title = node.Attributes[Constants.DESC_ATTR].Value;

                var tags = new List<string>(
                    node.Attributes
                    [Constants.TAGS_ATTR].Value.Replace("-", "")
                                                .ToLower()
                                                .Split
                                                (new string[] { " " },  StringSplitOptions.RemoveEmptyEntries));

                var dateTime = DateTime.Now;                    

                var bookmark = new BibsonomyPost
                {
                    LinkUrl = url
                    ,
                    LinkText = title
                    ,
                    Tags = tags
                    ,
                    Description = title
                };

                if (DateTime.TryParse(node?.Attributes[Constants.TIME_ATTR]?.Value, out dateTime))
                    bookmark.AddDate = dateTime;

                result.Add(bookmark);
            }

            return result;
        }
    }
}
