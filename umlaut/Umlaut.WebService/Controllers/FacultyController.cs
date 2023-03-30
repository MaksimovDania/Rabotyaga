using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umlaut.Database.Models;

namespace Umlaut.WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : Controller
    {
        [HttpGet("{facultyId}/statistics")]
        public async Task<IActionResult> GetAverageValuesForCurrentFaculty(int facultyId) 
        {
            IActionResult responce;
            responce = Ok($"{facultyId} + smth");
            return responce;
        }

        [HttpGet("{facultyId}/specializations")]
        public async Task<IActionResult> GetSpecializationsForCurrentFaculty(int facultyId) 
        {
            IActionResult responce;
            responce = Ok($"{facultyId} + smth");
            return responce;
        }

        [HttpGet("percentages")]
        public async Task<IActionResult> GetPercentageMatchFacultyWithSpecialization() 
        {
            IActionResult responce;
            responce = Ok("smth");
            return responce;
        }

        [HttpGet("{facultyId}/salary/experience")]
        public async Task<IActionResult> GetSalaryToExperienceRatio(int facultyId) 
        {
            IActionResult responce;
            responce = Ok($"{facultyId} + smth");
            return responce;
        }

        [HttpGet("{facultyId}/year_graduations")]
        public async Task<IActionResult> GetYearGraduations(int facultyId) 
        {
            IActionResult responce;
            responce = Ok($"{facultyId} + smth");
            return responce;
        }
    }
}
