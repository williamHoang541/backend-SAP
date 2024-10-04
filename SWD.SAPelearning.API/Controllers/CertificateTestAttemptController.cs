using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPelearning_bakend.Repositories.Services;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertifficateTestAttempt;
using SWD.SAPelearning.Repository.DTO.UserDTO;
using SWD.SAPelearning.Repository.Models;


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
        
        [AllowAnonymous]
        [Route("create-attempt")]
        [HttpGet]
        public async Task<IActionResult> CreateAttemp(string userId)
        {
            try
            {
                var a = await this.certificate_test_attempt.CreateAttemp(userId);
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [Route("update-status-by-attempt-id")]
        [HttpPatch] // Use POST or PUT for updates
        public async Task<IActionResult> UpdateStatusByAttemptId(int attemptId)
        {
            try
            {
                // Call the service method to update the status by attempt ID
                var updatedAttempt = await this.certificate_test_attempt.UpdateStatusAttempt(attemptId);
                return Ok(updatedAttempt); // Return the updated attempt
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return the error message
            }
        }

        [AllowAnonymous]
        [Route("count-attempts-by-user-id")]
        [HttpGet]
        public async Task<IActionResult> CountAttemptsByUserId(string userId)
        {
            try
            {
                // Call the service method to count attempts
                var count = await this.certificate_test_attempt.CountAttemptsByUserId(userId);
                return Ok(count); // Return the count as a response
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return the error message
            }
        }


        [AllowAnonymous]
        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(RemoveADTO Attempt)
        {
            try
            {
                var a = await this.certificate_test_attempt.DeleteAttempt(Attempt);
                return Ok(a);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
