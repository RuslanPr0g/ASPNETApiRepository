using System;

namespace YTScrapper.Application.Exceptions
{
    [Serializable]
    public class YoutubeWrongVideoUrlException : Exception
    {
        public YoutubeWrongVideoUrlException()
        {

        }

        public YoutubeWrongVideoUrlException(string url) : base(String.Format("Youtube video url is incorrect: {0}", url))
        {

        }
    }
}
