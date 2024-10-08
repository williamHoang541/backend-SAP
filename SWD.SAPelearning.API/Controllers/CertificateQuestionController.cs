using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertificateQuestion;


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
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate_question.GetAllCertificateQuestion();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateCertificateQuestionDTO request)
        {
            try
            {
                var result = await this.certificate_question.CreateQuestion(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateCertificateQuestionDTO request)
        {
            try
            {
                var result = await this.certificate_question.UpdateQuestion(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/CertificateQuestion/delete/{id}
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                var result = await this.certificate_question.DeleteQuestion(id);
                if (result)
                {
                    return Ok("Question deleted successfully.");
                }
                return NotFound("Question not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}