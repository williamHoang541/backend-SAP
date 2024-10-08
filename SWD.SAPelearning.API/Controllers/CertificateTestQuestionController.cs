using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertificateQuestion;
using SWD.SAPelearning.Repository.DTO.TestQuestion;


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
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate_test_question.GetAllCertificateTestQuestion();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTestQuestion([FromBody] CreateTestQuestionDTO request)
        {
            try
            {
                var result = await this.certificate_test_question.CreateTestQuestion(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/CertificateTestQuestion/update
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateTestQuestion([FromBody] UpdateTestQuestionDTO request)
        {
            try
            {
                var result = await this.certificate_test_question.UpdateTestQuestion(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/CertificateTestQuestion/delete/{id}
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteTestQuestion(int id)
        {
            try
            {
                var result = await this.certificate_test_question.DeleteTestQuestion(id);
                if (result)
                {
                    return Ok("Test question deleted successfully.");
                }
                return NotFound("Test question not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
