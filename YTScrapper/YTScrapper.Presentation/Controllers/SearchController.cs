using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSearch.Application.Contracts;
using YTSearch.Application.DTOs;
using YTSearch.Application.Filters;
using YTSearch.Domain.Models;

namespace YTSearch.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _service;

        public SearchController(ISearchService service)
        {
            _service = service;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest searchRequest)
        {
            var result = await _service.GetSearchItemByYoutubeUrl(searchRequest.SearchUrl);

            if (result.HasValue)
            {
                return Ok(MapDomainSearchItemToReadDto(result.Value));
            }
            else
            {
                return BadRequest(result.NullMessage);
            }
        }

        [HttpPost("search/specific")]
        public async Task<IActionResult> SearchByFilter([FromBody] SearchSpecificQuery searchRequest)
        {
            var searchItemFilter = new SearchItemFilter
            {
                Id = searchRequest.Id,
                Author = searchRequest.Author,
            };

            var result = await _service.GetSearchItemsByParameter(searchItemFilter);

            if (result.Count > 0)
            {
                return Ok(MapDomainSearchItemListToReadDto(result));
            }
            else
            {
                return NotFound("No searches found by your request.");
            }
        }

        private static YouTubeSearchItemReadDto MapDomainSearchItemToReadDto(YouTubeModel youTubeSearchItem)
        {
            return new YouTubeSearchItemReadDto
            {
                Title = youTubeSearchItem.Title,
                Description = youTubeSearchItem.Description,
                Author = youTubeSearchItem.Author,
                Duration = youTubeSearchItem.Duration,
            };
        }

        private static List<YouTubeSearchItemReadDto> MapDomainSearchItemListToReadDto(List<YouTubeModel> youTubeSearchItems)
        {
            return youTubeSearchItems.Select(y => MapDomainSearchItemToReadDto(y)).ToList();
        }
    }
}
