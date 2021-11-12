using System.Collections.Generic;
using System.Threading.Tasks;
using YTScrapper.Domain.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchItemRepository
    {
        Task<List<YouTubeModel>> Get();
        Task<int> Add(YouTubeModel response);
        Task Update(YouTubeModel response);
        Task Delete(YouTubeModel response);
    }
}
