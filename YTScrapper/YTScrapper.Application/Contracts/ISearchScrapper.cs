using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.DTOs;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchScrapper
    {
        Task<ValueOrNull<SearchResult>> Search(SearchRequest request, CancellationToken token = default);
    }
}
