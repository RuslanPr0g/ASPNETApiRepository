using System.Collections.Generic;
using System.Threading.Tasks;
using YTScrapper.Application.Filters;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchService
    {
        Task<SuccessOrFailure<YouTubeSearchItem>> GetSearchItemByYoutubeUrl(string url);
        Task<List<YouTubeSearchItem>> GetAllSearchItems();
        Task<List<YouTubeSearchItem>> GetSearchItemsByParameter(SearchItemFilter searchItemFilter);
        Task<int> AddSearchItem(YouTubeSearchItem searchItem);
        Task UpdateSearchItem(YouTubeSearchItem searchItem);
        Task DeleteSearchItem(YouTubeSearchItem searchItem);
    }
}
