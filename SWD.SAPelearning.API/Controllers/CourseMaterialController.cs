using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CourseMaterialDTO;


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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCourseMaterialById(int id)
        {
            try
            {
                var material = await this.course_material.GetCourseMaterialById(id);
                if (material == null)
                {
                    return NotFound(new { Message = $"Course material with ID {id} not found." });
                }

                return Ok(material);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourseMaterial([FromBody] CourseMaterialDTO request)
        {
            if (request == null)
            {
                return BadRequest("CourseMaterialDTO cannot be null.");
            }

            try
            {
                var createdMaterial = await this.course_material.CreateCourseMaterial(request);
                return CreatedAtAction(nameof(GetCourseMaterialById), new { id = createdMaterial.Id }, createdMaterial);
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

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCourseMaterial(int id, [FromBody] CourseMaterialDTO request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("CourseMaterialDTO cannot be null.");
                }

                var updatedMaterial = await this.course_material.UpdateCourseMaterial(id, request);
                return Ok(updatedMaterial); // Return the updated material
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Return 400 Bad Request if the input is null
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the material: {ex.Message}"); // Handle unexpected errors
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCourseMaterial(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid material ID."); // Return 400 Bad Request if the ID is not valid
            }

            try
            {
                bool result = await this.course_material.DeleteCourseMaterial(id);

                if (result)
                {
                    return Ok($"Course material with ID {id} was successfully deleted."); // Return 200 OK with a success message
                }

                return NotFound($"Course material with ID {id} not found."); // Return 404 Not Found if the material does not exist
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 Internal Server Error if an exception occurs
            }
        }

        [HttpGet]
        [Route("course/{courseId}")]
        public async Task<IActionResult> GetMaterialsByCourseId(int courseId)
        {
            try
            {
                var materials = await this.course_material.GetMaterialsByCourseId(courseId);
                if (materials == null || !materials.Any())
                {
                    return NotFound(new { Message = $"No materials found for course ID {courseId}." });
                }
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
    

