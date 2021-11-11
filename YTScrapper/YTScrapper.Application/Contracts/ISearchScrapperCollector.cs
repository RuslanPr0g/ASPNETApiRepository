using System.Collections.Generic;
using System.Threading.Tasks;
using YTScrapper.Application.Enums;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchScrapperCollector
    {
        ValueTask<IEnumerable<ISearchScrapper>> CollectFor(List<SupportedWebsite> websites);
    }
}
