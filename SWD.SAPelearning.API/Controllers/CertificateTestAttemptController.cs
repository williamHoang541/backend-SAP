using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateTestAttemptController : ControllerBase
    {
        private readonly ICertificateTestAttempt certificate_test_attempt;

        public CertificateTestAttemptController(ICertificateTestAttempt certificate_test_attempt)
        {
            this.certificate_test_attempt = certificate_test_attempt;
        }

        [HttpGet]
        [Route("get-all-certificate-test-attempt")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate_test_attempt.GetAllCertificateTestAttempt();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
