using System.Net;
using System.Threading.Tasks;

namespace YTScrapper.Application.Contracts
{
    public interface IWebClientService
    {
        Task<WebClient> Provide();
        Task<WebClient> SetDefaultUserString(WebClient client);
        Task<WebClient> SetTls12UserString(WebClient client);
    }
}
