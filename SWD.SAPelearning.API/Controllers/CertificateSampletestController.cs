using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertifficateTestAttempt;
using SWD.SAPelearning.Repository.DTO.SampleTest;


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

        [AllowAnonymous]
        [Route("create-sample-test")]
        [HttpPost]
        public async Task<IActionResult> CreateSampleTest(CreateSampleTestDTO request)
        {
            try
            {
                var sampleTest = await this.certificate_sample_test.CreateSampleTest(request);
                return Ok(sampleTest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("delete-attempt")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAttempt(RemoveSDTO sampleId)
        {
            try
            {
                var result = await this.certificate_sample_test.DeleteAttempt(sampleId);

                if (result)
                {
                    return Ok("Attempt deleted successfully.");
                }
                else
                {
                    return NotFound("Attempt not found or could not be deleted.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
