using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.Filters;
using YTScrapper.Domain.Models;

namespace YTScrapper.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchItemRepository _searchItemRepository;

        public SearchService(ISearchItemRepository searchItemRepository)
        {
            _searchItemRepository = searchItemRepository;
        }

        public async Task<int> AddSearchItem(SearchItem searchItem)
        {
            return await _searchItemRepository.Add(searchItem);
        }

        public async Task DeleteSearchItem(SearchItem searchItem)
        {
            await _searchItemRepository.Delete(searchItem);
        }

        public async Task<List<SearchItem>> GetAllSearchItems()
        {
            return await _searchItemRepository.Get();
        }

        public async Task<List<SearchItem>> GetSearchItemsByParameter(SearchItemFilter searchItemFilter)
        {
            if (searchItemFilter.Id is not null)
            {
                return (await _searchItemRepository.Get())
                    .Where(s => s.Id == searchItemFilter.Id).ToList();
            }

            if (searchItemFilter.Author is not null)
            {
                return (await _searchItemRepository.Get())
                    .Where(s => s.Author == searchItemFilter.Author).ToList();
            }

            return new List<SearchItem>();
        }

        public async Task UpdateSearchItem(SearchItem searchItem)
        {
            await _searchItemRepository.Update(searchItem);
        }
    }
}
