using YTSearch.Application.Enums;
using YTSearch.Domain.Models;
using YTSearch.Shared.Models;

namespace YTSearch.Application.Result
{
    public class SearchResult
    {
        public SuccessOrFailure<YouTubeModel> Result { get; set; }
        public SearchStatusCode StatusCode { get; set; }
    }
}
