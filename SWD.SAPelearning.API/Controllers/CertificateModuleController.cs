using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateModuleController : ControllerBase
    {
        private readonly ICertificateModule certificate_module;

        public CertificateModuleController(ICertificateModule certificate_module)
        {
            this.certificate_module = certificate_module;
        }

        [HttpGet]
        [Route("get-all-certificate-module")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate_module.GetAllCertificateModule();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
