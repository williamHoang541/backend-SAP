using Microsoft.AspNetCore.Cors.Infrastructure;
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
                return BadRequest("CourseDTO cannot be null.");
            }

            try
            {
                var createdCourse = await this.course.CreateCourse(request);
                return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.CourseId }, createdCourse); // Use CourseId here
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Return 400 for null request
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return 400 for validation errors
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 for any other errors
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                var course = await this.course.GetCourseById(id);
                if (course == null)
                {
                    return NotFound(new { Message = $"Course with ID {id} not found." });
                }

                return Ok(course);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDTO request)
        {
            try
            {
                // Validate the request body
                if (request == null)
                {
                    return BadRequest("CourseDTO cannot be null.");
                }

                // Call the service to update the course
                var updatedCourse = await this.course.UpdateCourse(id, request);

                return Ok(updatedCourse); // Return the updated course
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Return 400 Bad Request if the input is null
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the course: {ex.Message}"); // Handle unexpected errors
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid course ID."); // Return 400 Bad Request if the ID is not valid
            }

            try
            {
                // Call the service method to delete the course
                bool result = await this.course.DeleteCourse(id);

                if (result)
                {
                    return Ok($"Course with ID {id} was successfully deleted."); // Return 200 OK with a success message
                }

                return NotFound($"Course with ID {id} not found."); // Return 404 Not Found if the course does not exist
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error if an exception occurs
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
