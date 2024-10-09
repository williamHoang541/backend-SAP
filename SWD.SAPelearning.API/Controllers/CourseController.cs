using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;


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
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.course.GetAllCourse();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDTO request)
        {
            if (request == null)
            {
                return BadRequest("CourseDTO is null.");
            }

            try
            {
                var createdCourse = await this.course.CreateCourse(request);
                return CreatedAtAction(nameof(CreateCourse), new { id = createdCourse.Id }, createdCourse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                var course = await this.course.GetCourseById(id);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(404, $"Error: {ex.Message}");
            }
        }
    }
}
