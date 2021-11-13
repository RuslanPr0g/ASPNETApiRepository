namespace YTScrapper.Shared.Helper
{
    public static class YouTubeHelper
    {
        public static string GetYouTubeVideoIdFromUrl(this string url)
        {
            string regex = string.Empty;

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
