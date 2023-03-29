using Microsoft.AspNetCore.Mvc;

namespace Umlaut.WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationController : Controller
    {
        [HttpGet("salary/experience")]
        public async Task<IActionResult> GetSalaryToExperienceRatio() { return View(); }

        [HttpGet("{specializationId}/statistics")]
        public async Task<IActionResult> GetAverageValuesForCurrentSpecialization(int specializationId) { return View(); }
    }
}
