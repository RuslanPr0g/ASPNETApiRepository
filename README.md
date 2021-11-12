# YoutubeParser
### Start with
Enter any URL which leads to YouTube video, e.g. https://www.youtube.com/watch?v=dfzBMxXQUOc <br/>
You will get data about this YouTube video, e.g. the following: <br/>
<pre>
<code>
{
  "item": {
    "imagePreviewUrl": "https://i.ytimg.com/vi/dfzBMxXQUOc/hqdefault.jpg?sqp=-oaymwEiCKgBEF5IWvKriqkDFQgBFQAAAAAYASUAAMhCPQCAokN4AQ==\\u0026rs=AOn4CLBVPll2Q7spOBSMX6d66pUu3iLUWA",
    "searchItemUrl": "https://www.youtube.com/watch?v=dfzBMxXQUOc",
    "title": "What's new in C# 10",
    "description": "C# 10 brings many improvements focused around enabling cleaner and simpler code in many scenarios. \\n\\nhttps://get.dot.net/6",
    "author": "dotNET",
    "duration": "0:29:27"
  },
  "website": "YouTube",
  "errorMessage": null
}
</code>
</pre>
Also, you can access already scrapped data (I store in DB) by author or any other filter.
### Stack
- Clean architecture
- .NET5
- SqlLite
