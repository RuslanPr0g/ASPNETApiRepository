using YTScrapper.Domain.Models;

namespace YTScrapper.Application.DTOs
{
    public class SearchResult
    {
        public SearchItem Item { get; }
        public string Website { get; set; }
        public string ErrorMessage { get; set; }

        public SearchResult()
        {
            Item = new();
        }

        public SearchResult(SearchItem searchItems)
        {
            Item = searchItems;
        }
    }
}
