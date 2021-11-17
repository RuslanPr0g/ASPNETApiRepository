using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSearch.Application.Contracts;
using YTSearch.Application.DTOs;
using YTSearch.Application.Filters;
using YTSearch.Domain.Models;
using YTSearch.Shared.Models;

namespace YTSearch.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchItemRepository _searchItemRepository;
        private readonly ISearchRunner _searchRunner;

        public SearchService(ISearchItemRepository searchItemRepository, ISearchRunner searchRunner)
        {
            _searchItemRepository = searchItemRepository;
            _searchRunner = searchRunner;
        }

        public async Task<int> AddSearchItem(YouTubeModel searchItem)
        {
            var searches = await GetAllSearchItems();

            if (searches.Any(s => s.Url == searchItem.Url))
            {
                return -1;
            }

            return await _searchItemRepository.Add(searchItem);
        }

        public async Task DeleteSearchItem(YouTubeModel searchItem)
        {
            await _searchItemRepository.Delete(searchItem);
        }

        public async Task<List<YouTubeModel>> GetAllSearchItems()
        {
            return await _searchItemRepository.Get();
        }

        public async Task<SuccessOrFailure<YouTubeModel>> GetSearchItemByYoutubeUrl(string url)
        {
            var searches = await GetAllSearchItems();

            if (searches.Any(s => s.Url == url))
            {
                return searches.First(s => s.Url == url);
            }

            var result = (await _searchRunner.Run(new SearchRunnerRequest { Url = url }));

            if (result.HasValue)
            {
                var youtubeModel = new YouTubeModel
                {
                    Url = url,
                    Title = result.Value.Title,
                    Description = result.Value.Description,
                    Author = result.Value.Author,
                    Duration = result.Value.Duration,
                };
                var id = await AddSearchItem(youtubeModel);
                youtubeModel.Id = id;

                return SuccessOrFailure<YouTubeModel>.CreateValue(youtubeModel);
            }
            else
            {
                return SuccessOrFailure<YouTubeModel>.CreateNull(result.NullMessage);
            }
        }

        public async Task<List<YouTubeModel>> GetSearchItemsByParameter(SearchItemFilter searchItemFilter)
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

            return new List<YouTubeModel>();
        }

        public async Task UpdateSearchItem(YouTubeModel searchItem)
        {
            await _searchItemRepository.Update(searchItem);
        }
    }
}
