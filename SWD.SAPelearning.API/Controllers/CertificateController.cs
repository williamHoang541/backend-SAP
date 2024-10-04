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

        // GET: api/certificate/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCertificateById(int id)
        {
            try
            {
                var certificate = await this.certificate.GetCertificateById(id);
                if (certificate == null)
                {
                    return NotFound($"No certificate found with ID {id}.");
                }
                return Ok(certificate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCertificate([FromBody] CertificateDTO request)
        {
            if (request == null)
            {
                return BadRequest("CertificateDTO is null.");
            }

            try
            {
                var createdCertificate = await this.certificate.CreateCertificate(request);

                // Return the created certificate with the appropriate HTTP status code
                return CreatedAtAction(nameof(GetCertificateById), new { id = createdCertificate.Id }, new
                {
                    certificateName = createdCertificate.CertificateName,
                    description = createdCertificate.Description,
                    level = createdCertificate.Level,
                    environment = createdCertificate.Environment,
                    status = createdCertificate.Status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCertificate(int id, [FromBody] CertificateDTO request)
        {
            if (request == null)
            {
                return BadRequest("CertificateDTO is null.");
            }
            try
            {
                var updatedCertificate = await this.certificate.UpdateCertificate(id, request);
                return Ok(updatedCertificate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
