using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
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
        public async Task<IActionResult> GetAll([FromQuery] GetAllDTO getAllDTO)
        {
            var modules = await this.certificate_module.GetAllSapModulesAsync(getAllDTO);
            if (modules == null || !modules.Any())
            {
                return NotFound("No SAP modules found.");
            }
            return Ok(modules);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateSapModule([FromBody] SapModuleCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("SapModuleCreateDTO is null.");
            }

            try
            {
                var createdModule = await this.certificate_module.CreateSapModule(request);
                return CreatedAtAction(nameof(GetSapModuleById), new { id = createdModule.Id }, createdModule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SapModuleCreateDTO request)
        {
            if (request == null)
            {
                return BadRequest("SapModuleCreateDTO is null.");
            }

            try
            {
                var updatedModule = await this.certificate_module.UpdateSapModule(id, request);
                if (updatedModule == null)
                {
                    return NotFound($"Module with ID {id} not found.");
                }
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
