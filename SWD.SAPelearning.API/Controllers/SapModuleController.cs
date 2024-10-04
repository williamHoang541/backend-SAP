using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


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
    }
}
