using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.CourseDTO;
using SWD.SAPelearning.Services;


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
        public async Task<IActionResult> GetAllCourses([FromQuery] GetAllDTO getAllDTO)
        {
            // Validate the input if necessary
            if (getAllDTO.PageNumber < 1 || getAllDTO.PageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            // Fetch the courses using the service
            List<CourseDTO> courses = await this.course.GetAllCourseAsync(getAllDTO);

            // Check if courses are found
            if (courses == null || courses.Count == 0)
            {
                return NotFound("No courses found matching the criteria.");
            }

            return Ok(courses);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetCourseCount()
        {
            try
            {
                int totalCourses = await this.course.CountCoursesAsync();
                return Ok(new { TotalCourses = totalCourses });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
            [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("CourseCreateDTO cannot be null.");
            }

            try
            {
                // Use the service to create the course
                var createdCourse = await this.course.CreateCourse(request);

                if (createdCourse == null)
                {
                    return StatusCode(500, "There was a problem creating the course.");
                }

                // Return the created course details to the client
                return Ok(createdCourse);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Input error: {ex.Message}");
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
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("CourseCreateDTO cannot be null.");
            }

            try
            {
                // Use the service to update the course
                var updatedCourse = await this.course.UpdateCourse(id, request);

                if (updatedCourse == null)
                {
                    return StatusCode(500, "There was a problem updating the course.");
                }

                // Return the updated course details to the client
                return Ok(updatedCourse);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Input error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
