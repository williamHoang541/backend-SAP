using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.SapModuleDTO;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SapModuleController : ControllerBase
    {
        private readonly ISapModule certificate_module;

        public SapModuleController(ISapModule certificate_module)
        {
            this.certificate_module = certificate_module;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate_module.GetAllSapModule();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateSapModule([FromBody] SapModuleDTO request)
        {
            if (request == null)
            {
                return BadRequest("SapModuleDTO is null.");
            }

            try
            {
                var createdModule = await this.certificate_module.CreateSapModule(request);
                return CreatedAtAction(nameof(CreateSapModule), new { id = createdModule.Id }, createdModule); // Return the created module with 201 status
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SapModuleDTO request)
        {
            try
            {
                var updatedModule = await this.certificate_module.UpdateSapModule(id, request);
                return Ok(updatedModule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Delete
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await this.certificate_module.DeleteSapModule(id);
                if (result)
                {
                    return Ok("Successfully Deleted"); // Return custom success message
                }
                return NotFound($"Module with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSapModuleById(int id)
        {
            try
            {
                var module = await this.certificate_module.GetSapModuleById(id);
                return Ok(module);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
