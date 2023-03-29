using Microsoft.AspNetCore.Mvc;

namespace Umlaut.WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetLocations() { return View(); }
    }
}
