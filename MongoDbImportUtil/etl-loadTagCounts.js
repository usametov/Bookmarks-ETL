//calculate tag counts in MongoDB
//this is mostly for debugging purposes
function loadTagCounts(){
	db.bookmarks.aggregate(
		{
			$project:
			{"_id":0, "Tags":1}
		},
		{ $unwind: "$Tags" },
		{$group:{_id:"$Tags", count:{$sum:1}}},
		{$project:{_id:0, Tag:"$_id",count:1}}, 
		{$sort:{count:-1}},
		{$out:"tagCounts"});
}
