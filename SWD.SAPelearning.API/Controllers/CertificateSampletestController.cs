using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateSampletestController : ControllerBase
    {
        private readonly ICertificateSampletest certificate_sample_test;

        public CertificateSampletestController(ICertificateSampletest certificate_sample_test)
        {
            this.certificate_sample_test = certificate_sample_test;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate_sample_test.GetAllCertificateSampletest();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
