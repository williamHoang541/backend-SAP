using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollment enrollment;

        public EnrollmentController(IEnrollment enrollment)
        {
            this.enrollment = enrollment;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.enrollment.GetAllEnrollment();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
