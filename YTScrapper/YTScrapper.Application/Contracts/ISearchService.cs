using System.Collections.Generic;
using System.Threading.Tasks;
using YTSearch.Application.Filters;
using YTSearch.Domain.Models;
using YTSearch.Shared.Models;

namespace YTSearch.Application.Contracts
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
