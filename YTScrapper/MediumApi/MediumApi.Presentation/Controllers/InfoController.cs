using Microsoft.AspNetCore.Mvc;

namespace MediumApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet("post")]
        public async Task<IActionResult> Get()
        {

            return Ok();
        }
    }
}
