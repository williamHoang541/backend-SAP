using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


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
    }
}
