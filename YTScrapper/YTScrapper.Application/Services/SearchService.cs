﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.Filters;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchItemRepository _searchItemRepository;
        private readonly ISearchRunner _scraperRunner;

        public SearchService(ISearchItemRepository searchItemRepository, ISearchRunner scraperRunner)
        {
            _searchItemRepository = searchItemRepository;
            _scraperRunner = scraperRunner;
        }

        public async Task<int> AddSearchItem(YouTubeSearchItem searchItem)
        {
            var searches = await GetAllSearchItems();

            if (searches.Any(s => s.Url == searchItem.Url))
            {
                return -1;
            }

            return await _searchItemRepository.Add(searchItem);
        }

        public async Task DeleteSearchItem(YouTubeSearchItem searchItem)
        {
            await _searchItemRepository.Delete(searchItem);
        }

        public async Task<List<YouTubeSearchItem>> GetAllSearchItems()
        {
            return await _searchItemRepository.Get();
        }

        public async Task<SuccessOrFailure<YouTubeSearchItem>> GetSearchItemByYoutubeUrl(string url)
        {
            var searches = await GetAllSearchItems();

            if (searches.Any(s => s.Url == url))
            {
                return searches.First(s => s.Url == url);
            }

            var result = (await _scraperRunner.Run(url));

            if (result.HasValue)
            {
                var id = await AddSearchItem(result.Value);
                return SuccessOrFailure<YouTubeSearchItem>.CreateValue(new YouTubeSearchItem
                {
                    Id = id,
                    Url = url,
                    Title = result.Value.Title,
                    Description = result.Value.Description,
                    Author = result.Value.Author,
                    Duration = result.Value.Duration,
                });
            }
            else
            {
                return SuccessOrFailure<YouTubeSearchItem>.CreateNull(result.NullMessage);
            }
        }

        public async Task<List<YouTubeSearchItem>> GetSearchItemsByParameter(SearchItemFilter searchItemFilter)
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

            return new List<YouTubeSearchItem>();
        }

        public async Task UpdateSearchItem(YouTubeSearchItem searchItem)
        {
            await _searchItemRepository.Update(searchItem);
        }
    }
}
