using YTScrapper.Application.Enums;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Result
{
    public class SearchResult
    {
        public SuccessOrFailure<YouTubeModel> Result { get; set; }
        public SearchStatusCode StatusCode { get; set; }
    }
}
