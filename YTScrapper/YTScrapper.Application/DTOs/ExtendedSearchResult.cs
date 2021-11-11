using YTScrapper.Application.Enums;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.DTOs
{
    public class ExtendedSearchResult
    {
        public ValueOrNull<SearchResult> Result { get; set; }
        public ScraperStatusCode StatusCode { get; set; }
    }
}
