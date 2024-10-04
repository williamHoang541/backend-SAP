using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMaterialController : ControllerBase
    {
        private readonly ICourseMaterial course_material;

        public CourseMaterialController(ICourseMaterial course_material)
        {
            this.course_material = course_material;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.course_material.GetAllCourseMaterial();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
