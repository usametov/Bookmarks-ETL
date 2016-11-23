using ReactiveETL;
using Bookmarks.Common;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System;
using GitmarksParser;

namespace BookmarksETL_CLI
{
    public static class GitmarksExportUtil
    {        
        public static void ExportBookmarks(string bookmarksFile, string bookmarksDir)
        {
            var bookmarks = JsonConvert.DeserializeObject<Bookmark[]>(File.ReadAllText(bookmarksFile)).AsEnumerable();

            var result = Input.From(bookmarks).Apply(row => {

                                                        string fileName = Path.Combine(bookmarksDir, row["Id"].ToString());

                                                        var bookmark = new BookmarkContent
                                                        {
                                                            hash = row["Id"].ToString()
                                                            ,
                                                            creator = "batch"
                                                            ,
                                                            rights = "CC BY"
                                                            ,
                                                            time = DateTime.Now.ToShortDateString()
                                                            ,
                                                            title = row["LinkText"].ToString()
                                                            ,
                                                            uri = row["LinkUrl"].ToString()
                                                        };
                                                        File.WriteAllText(fileName, JsonConvert.SerializeObject(bookmark)); 
                                                    }).Execute();
        }

        public static void ExportTags(string bookmarksFile, string tagsDir)
        {
            var bookmarks = JsonConvert.DeserializeObject<Bookmark[]>(File.ReadAllText(bookmarksFile)).AsEnumerable();
            var invertedTags = bookmarks.SelectMany
                (b => {
                        return b.Tags.Select(t => 
                                            {
                                                var invalidFileNameChars = Path.GetInvalidFileNameChars()
                                                                               .Concat(new[] { '[', ']', '#', '!' });

                                                t = invalidFileNameChars.Aggregate(t, (current, c) => 
                                                                                       current.Replace(c.ToString(), string.Empty));

                                                return Tuple.Create(t, new TagContent
                                                                    {
                                                                        creator = "batch"
                                                                    ,
                                                                        hash = b.Id
                                                                    ,
                                                                        title = b.LinkText
                                                                    ,
                                                                        uri = b.LinkUrl
                                                                    ,
                                                                        ver = "BookmarksEtl.0.0.1"
                                                                    });
                                            });
                    })
                    .GroupBy(it => it.Item1)
                    .OrderBy(it=>it.Key);//group by tag and sort
            
            var result = Input.From(invertedTags.ToList()).Apply(row => 
            {
                var tagName = row["Key"].ToString().Trim();                                

                if (string.IsNullOrEmpty(tagName)) return;

                string fileName = Path.Combine(tagsDir, tagName);
                
                var tagContentTuples = row["elements"] as Tuple<string, TagContent>[];

                var tagContent = tagContentTuples.Where(tpl=>tpl!=null).Select(tpl => tpl.Item2);

                var content = JsonConvert.SerializeObject(tagContent.ToArray());

                File.WriteAllText(fileName, content);
            }).Execute();
        }
    }
}
