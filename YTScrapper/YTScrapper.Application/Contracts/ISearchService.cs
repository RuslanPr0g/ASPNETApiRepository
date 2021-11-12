using System.Collections.Generic;
using System.Threading.Tasks;
using YTScrapper.Application.Filters;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchService
    {
        Task<SuccessOrFailure<YouTubeModel>> GetSearchItemByYoutubeUrl(string url);
        Task<List<YouTubeModel>> GetAllSearchItems();
        Task<List<YouTubeModel>> GetSearchItemsByParameter(SearchItemFilter searchItemFilter);
        Task<int> AddSearchItem(YouTubeModel searchItem);
        Task UpdateSearchItem(YouTubeModel searchItem);
        Task DeleteSearchItem(YouTubeModel searchItem);
    }
}
