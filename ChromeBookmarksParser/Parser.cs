using Bookmarks.Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;

namespace ChromeBookmarksParser
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

            var nodes = document.QuerySelectorAll($"{Constants.DT_NODE} {Constants.ANCHOR_NODE}");
            
            return nodes.Select(n =>
                    new ChromeBookmarks
                    {
                        LinkUrl = n.Attributes[Constants.HREF_ATTR].Value
                        ,
                        LinkText = n.InnerText
                        ,
                        Tags = n.InnerText.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Where(s => s.StartsWith(":"))
                                          .Select(t => t.TrimStart(':')).ToList()
                        ,
                        Description = string.Empty
                        ,
                        AddDate = DateTime.Now
                    } as IBookmark
                ).ToList();
        }
    }
}
