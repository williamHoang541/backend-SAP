using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourse course;

        public CourseController(ICourse course)
        {
            this.course = course;
        }

        [HttpGet]
        [Route("get-all-course")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.course.GetAllCourse();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
