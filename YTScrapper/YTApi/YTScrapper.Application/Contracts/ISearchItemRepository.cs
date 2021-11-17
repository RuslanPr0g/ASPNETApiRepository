using System.Collections.Generic;
using System.Threading.Tasks;
using YTSearch.Domain.Models;

namespace YTSearch.Application.Contracts
{
    public interface ISearchItemRepository
    {
        Task<List<YouTubeModel>> Get();
        Task<int> Add(YouTubeModel response);
        Task Update(YouTubeModel response);
        Task Delete(YouTubeModel response);
    }
}
