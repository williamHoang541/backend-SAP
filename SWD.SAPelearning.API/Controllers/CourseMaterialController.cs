using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.CourseMaterialDTO;
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
        public async Task<IActionResult> GetAllCourseMaterials([FromQuery] GetAllDTO getAllDTO)
        {
            // Validate pagination input
            if (getAllDTO.PageNumber < 1 || getAllDTO.PageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            // Fetch course materials using the service
            List<CourseMaterialDTO> courseMaterials = await this.course_material.GetAllCourseMaterialsAsync(getAllDTO);

            // Check if course materials are found
            if (courseMaterials == null || courseMaterials.Count == 0)
            {
                return NotFound("No course materials found matching the criteria.");
            }

            return Ok(courseMaterials);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourseMaterial([FromBody] CourseMateriaCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("CourseMaterialDTO cannot be null.");
            }

            try
            {
                var createdMaterial = await this.course_material.CreateCourseMaterial(request);

                if (createdMaterial == null)
                {
                    return StatusCode(500, "There was a problem creating the course material.");
                }

                return Ok(createdMaterial);
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

        // Get course material by ID
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCourseMaterialById(int id)
        {
            try
            {
                var material = await this.course_material.GetCourseMaterialById(id);
                if (material == null)
                {
                    return NotFound($"Course material with ID {id} not found.");
                }

                return Ok(material);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Update course material
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCourseMaterial(int id, [FromBody] CourseMateriaCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("CourseMaterialDTO cannot be null.");
            }

            try
            {
                var updatedMaterial = await this.course_material.UpdateCourseMaterial(id, request);

                if (updatedMaterial == null)
                {
                    return NotFound($"Course material with ID {id} not found.");
                }

                return Ok(updatedMaterial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Delete course material by ID
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCourseMaterial(int id)
        {
            try
            {
                var result = await this.course_material.DeleteCourseMaterial(id);

                if (!result)
                {
                    return NotFound($"Course material with ID {id} not found.");
                }

                return Ok($"Course material with ID {id} was successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

       
    }
}
    

