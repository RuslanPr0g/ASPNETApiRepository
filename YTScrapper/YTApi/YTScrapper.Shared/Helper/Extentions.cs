using System;
using System.Web;

namespace YTSearch.Shared.Helper
{
    public static class Extentions
    {
        public static string GetYouTubeVideoIdFromUrl(this string url)
        {
            string videoId = string.Empty;
            
            var uri = new Uri(url);

            var queryCollection = HttpUtility.ParseQueryString(uri.Query);

            if (url.Contains("https://www.youtube.com/watch?v="))
            {
                videoId = queryCollection.Get("v");
            }
            else if (url.Contains("https://youtu.be/"))
            {
                videoId = uri.Segments[1];
            }

            return videoId;
        }
    }
}
