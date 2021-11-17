namespace YTSearch.Shared.Helper
{
    public static class YouTubeHelper
    {
        public static string GetYouTubeVideoIdFromUrl(this string url)
        {
            string regex = string.Empty;

            // TODO: fix url matching with new Url()

            if (url.Contains("https://www.youtube.com/watch?v="))
            {
                regex = @"^https:\/\/[^\/]+\/watch\?v=([^&^\n]+)";
            }
            else if (url.Contains("https://youtu.be/"))
            {
                regex = @"^https:\/\/[^\/]+\/([^&^\n]+)";
            }

            return RegexHelper.GetNMatchFromRegexPattern(regex, url, 1);
        }
    }
}
