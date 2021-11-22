using System;
using System.Web;

namespace MediumApi.Application
{
    public static class Extentions
    {
        public static string GetMediumUsernameFromUrl(this string url)
        {
            string username = string.Empty;

            var uri = new Uri(url);

            if (url.Contains("https://medium.com/"))
            {
                username = uri.Segments[1].Trim('/');
            }

            return username;
        }
    }
}
