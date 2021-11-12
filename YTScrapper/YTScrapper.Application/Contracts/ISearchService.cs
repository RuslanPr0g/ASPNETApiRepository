using System.Collections.Generic;
using System.Threading.Tasks;
using YTScrapper.Application.Filters;
using YTScrapper.Domain.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchService
    {
        Task<List<SearchItem>> GetAllSearchItems();
        Task<List<SearchItem>> GetSearchItemsByParameter(SearchItemFilter searchItemFilter);
        Task AddSearchItem(SearchItem searchItem);
        Task UpdateSearchItem(SearchItem searchItem);
        Task DeleteSearchItem(SearchItem searchItem);
    }
}
