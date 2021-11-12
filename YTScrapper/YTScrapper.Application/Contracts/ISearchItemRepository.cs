using System.Collections.Generic;
using System.Threading.Tasks;
using YTScrapper.Domain.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchItemRepository
    {
        Task<List<SearchItem>> Get();
        Task<int> Add(SearchItem response);
        Task Update(SearchItem response);
        Task Delete(SearchItem response);
    }
}
