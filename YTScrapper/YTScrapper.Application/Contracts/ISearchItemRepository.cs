using System.Collections.Generic;
using System.Threading.Tasks;
using YTScrapper.Domain.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchItemRepository
    {
        Task<List<YouTubeSearchItem>> Get();
        Task<int> Add(YouTubeSearchItem response);
        Task Update(YouTubeSearchItem response);
        Task Delete(YouTubeSearchItem response);
    }
}
