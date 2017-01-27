Bookmarks ETL utility
=====================

The purpose of this project is to manage bookmarks from a different sources.
It provides functionality to import/export bookmarks from/to different formats.

Currently you can import bookmarks from 
* Bibsonomy, 
* Gitmarks, 
* Pinterest, 
* Delicious web sites.

Once you exported bookmarks from these web sites to a file, you feed the that file to BookmarksBatchConsole.    
The ouput file will have a format defined by Bookmarks.Common.Bookmark class.
Have a look at examples in TestMongoDbImport project.

This project is bundled with [TagSortService] (https://github.com/usametov/TagSortService), which is another project of mine.





