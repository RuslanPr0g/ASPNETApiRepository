using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.DTOs;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Runners
{
    public interface IScraperRunner
    {
        Task<List<ValueOrNull<SearchResult>>> Run(SearchRequest searchRequest, CancellationToken token = default);
    }
}
