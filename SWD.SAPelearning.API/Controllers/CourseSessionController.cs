using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CourseSessionDTO;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseSessionController : ControllerBase
    {
        private readonly ICourseSession course_session;

        public CourseSessionController(ICourseSession course_session)
        {
            this.course_session = course_session;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.course_session.GetAllCourseSession();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourseSession([FromBody] CourseSessionDTO request)
        {
            if (request == null)
            {
                return BadRequest("CourseSessionDTO cannot be null.");
            }

            try
            {
                var createdSession = await this.course_session.CreateCourseSession(request);

                if (createdSession == null)
                {
                    return StatusCode(500, "There was a problem creating the course session.");
                }

                return Ok(createdSession);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // Get course session by ID
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCourseSessionById(int id)
        {
            try
            {
                var session = await this.course_session.GetCourseSessionById(id);
                if (session == null)
                {
                    return NotFound($"Course session with ID {id} not found.");
                }

                return Ok(session);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Update course session
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCourseSession(int id, [FromBody] CourseSessionDTO request)
        {
            if (request == null)
            {
                return BadRequest("CourseSessionDTO cannot be null.");
            }

            try
            {
                var updatedSession = await this.course_session.UpdateCourseSession(id, request);

                if (updatedSession == null)
                {
                    return NotFound($"Course session with ID {id} not found.");
                }

                return Ok(updatedSession);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // Conflict status for duplicate session names
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Delete course session by ID
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourseSession(int id)
        {
            try
            {
                var result = await this.course_session.DeleteCourseSession(id);

                if (!result)
                {
                    return NotFound($"Course session with ID {id} not found.");
                }

                return Ok($"Course session with ID {id} was successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
