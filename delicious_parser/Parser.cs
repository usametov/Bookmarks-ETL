using System;
using System.Collections.Generic;
using Bookmarks.Common;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace DeliciousParser
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
            
            var nodes = document.QuerySelectorAll(Constants.DOC_NODE);
            var result = new List<IBookmark>();

            foreach (var node in nodes) 
            {
                var bookmark = new DeliciousBookmark
                {
                    IsPrivate = node.ChildNodes[1].Attributes[Constants.PRIVATE_ATTR].ToString().Equals(Constants.ONE_STR)
                    ,
                    LinkUrl = node.ChildNodes[1].Attributes[Constants.HREF_ATTR].ToString()
                    ,
                    LinkText = node.ChildNodes[1].InnerText.Trim()
                    ,
                    Tags = new List<string>(node.ChildNodes[1].Attributes[Constants.TAGS_ATTR].ToString().Split(','))
                    ,
                    Description = node.ChildNodes.Count > 2 ? node.ChildNodes[2].InnerText.Trim() : string.Empty
                };

                result.Add(bookmark);
            }
                        
            Console.WriteLine("nodes processed: "+ result.Count);
            Console.ReadLine();

            return result;
        }
    }
}
