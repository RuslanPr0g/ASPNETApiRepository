using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.DTOs;

namespace YTScrapper.Application.Runners
{
    public interface IScraperRunner
    {
        Task<List<SearchResult>> Run(SearchRequest searchRequest, CancellationToken token = default);
    }
}
