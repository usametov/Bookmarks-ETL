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
                var anchor = node.QuerySelector(Constants.ANCHOR_NODE);
                
                var url = anchor.Attributes[Constants.HREF_ATTR].Value;
                
                var title = anchor.InnerText.Trim();
                
                var tags = new List<string>(anchor.Attributes[Constants.TAGS_ATTR].Value.Replace("-","").ToLower().Split(','));
                
                var innerTxt = node.InnerText.Split(new string[]{"---"}, StringSplitOptions.RemoveEmptyEntries);
                var descTxt =  string.Empty;

                if(innerTxt.Length > 1)
                    descTxt = string.Join("---", innerTxt, 1, innerTxt.Length - 1);     
                
                var bookmark = new DeliciousBookmark
                {
                    LinkUrl = url
                    ,
                    LinkText = title
                    ,
                    Tags = tags
                    ,
                    Description = descTxt
                };

                result.Add(bookmark);
            }
            
            return result;
        }
    }
}
