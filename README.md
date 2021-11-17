# YoutubeParser
### Start with
Enter any URL which leads to YouTube video, e.g. https://www.youtube.com/watch?v=dfzBMxXQUOc <br/>
You will get data about this YouTube video, e.g. the following: <br/>
<pre>
<code>
[
  {
    "title": "Using SQLite in C# - Building Simple, Powerful, Portable Databases for Your Application",
    "description": "test",
    "author": "IAmTimCorey",
    "duration": "38:40"
  }
]
</code>
</pre>
Also, you can access already stored data by an id or an author.
### Stack
- Clean architecture
- .NET5
- SqlLite

# MediumApi
### Start with
Enter any URL which leads to Medium post, e.g. https://medium.com/young-coder/7-of-my-favorite-little-javascript-tricks-4f2a1cfe68b4 <br/>
You will get data about this Meduim post, e.g. the following: <br/>
<pre>
<code>
[
  {
    "link": "...",
    "title": "...",
    "author": "...",
    "description": "...",
    "content": "...",
    "thumbnail": "...",
    "pubDate": "...",
    "categories": [
      ...
    ]
  }
]
</code>
</pre>
Also, you can access already stored data by link or an author.
### Stack
- Clean architecture
- .NET6
- SqlLite
