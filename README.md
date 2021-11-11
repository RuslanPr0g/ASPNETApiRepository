# YoutubeParser
### Start with
Enter any URL which leads to YouTube video, e.g. https://www.youtube.com/watch?v=dfzBMxXQUOc <br/>
You will get data about this YouTube video, e.g. the following: <br/>
<pre>
<code>
{
	"title": What's new in C#",
	"description": "C# 10 brings many improvements focused around enabling cleaner and simpler code in many scenarios.",
	"author": "dotNET",
	"duration": "00:29:27"
}
</code>
</pre>
Also, you can access already scrapped data (I store in DB) by author or any other filter.
### Stack
- Clean architecture
- .NET5
- SqlLight
