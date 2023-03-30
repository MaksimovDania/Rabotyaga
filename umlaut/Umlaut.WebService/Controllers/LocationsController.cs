using Microsoft.AspNetCore.Mvc;

namespace Umlaut.WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetLocations() 
        {
            IActionResult responce;
            responce = Ok("smth");
            return responce;
        }
    }
}
