using Microsoft.AspNetCore.Mvc;

namespace Umlaut.WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationController : Controller
    {
        [HttpGet("{specializationId}/salary/experience")]
        public async Task<IActionResult> GetSalaryToExperienceRatio(int specializationId) 
        {
            IActionResult responce;
            responce = Ok($"{specializationId} + smth");
            return responce; 
        }

        [HttpGet("{specializationId}/statistics")]
        public async Task<IActionResult> GetAverageValuesForCurrentSpecialization(int specializationId) 
        {
            IActionResult responce;
            responce = Ok($"{specializationId} + smth");
            return responce;
        }
    }
}
