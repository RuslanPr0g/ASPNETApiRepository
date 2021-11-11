using System.Collections.Generic;
using YTScrapper.Application.Enums;

namespace YTScrapper.Application.DTOs
{
    public class SearchRequest
    {
        public string SearchUrl { get; set; }
        public List<SupportedWebsite> Websites { get; set; }
    }
}
