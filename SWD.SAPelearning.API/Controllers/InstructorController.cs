using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;

namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructor instructor;

        public InstructorController(IInstructor instructor)
        {
            this.instructor = instructor;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.instructor.GetAllInstructor();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
