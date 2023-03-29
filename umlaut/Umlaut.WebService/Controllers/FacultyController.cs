using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Umlaut.WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : Controller
    {
        [HttpGet("{facultyId}/statistics")]
        public async Task<IActionResult> GetAverageValuesForCurrentFaculty(int facultyId) { return View(); }

        [HttpGet("{facultyId}/specializations")]
        public async Task<IActionResult> GetSpecializationsForCurrentFaculty(int facultyId) { return View(); }

        [HttpGet("percentages")]
        public async Task<IActionResult> GetPercentageMatchFacultyWithSpecialization() { return View(); }

        [HttpGet("salary/experience")]
        public async Task<IActionResult> GetSalaryToExperienceRatio() { return View(); }

        [HttpGet("{facultyId}/year_graduations")]
        public async Task<IActionResult> GetYearGraduations(int facultyId) { return View(); }
    }
}
