using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;

namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificate certificate;

        public CertificateController(ICertificate certificate)
        {
            this.certificate = certificate;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate.GetAllCertificate();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
