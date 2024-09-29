using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateTestQuestionController : ControllerBase
    {
        private readonly ICertificateTestQuestion certificate_test_question;

        public CertificateTestQuestionController(ICertificateTestQuestion certificate_test_question)
        {
            this.certificate_test_question = certificate_test_question;
        }

        [HttpGet]
        [Route("get-all-certificate-test-question")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate_test_question.GetAllCertificateTestQuestion();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
