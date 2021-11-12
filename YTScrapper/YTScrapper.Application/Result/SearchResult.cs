using YTScrapper.Application.Enums;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Result
{
    public class SearchResult
    {
        public SuccessOrFailure<YouTubeSearchItem> Result { get; set; }
        public SearchStatusCode StatusCode { get; set; }
    }
}
