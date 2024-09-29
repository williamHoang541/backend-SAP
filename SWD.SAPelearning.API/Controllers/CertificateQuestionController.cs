using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateQuestionController : ControllerBase
    {
        private readonly ICertificateQuestion certificate_question;

        public CertificateQuestionController(ICertificateQuestion certificate_question)
        {
            this.certificate_question = certificate_question;
        }

        [HttpGet]
        [Route("get-all-certificate-question")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate_question.GetAllCertificateQuestion();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
