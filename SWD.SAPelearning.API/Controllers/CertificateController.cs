using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.CertificateDTO;

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
        public async Task<IActionResult> GetAll([FromQuery] GetAllDTO getAllDTO)
        {
            try
            {
                // Validate or set default values
                getAllDTO.PageSize ??= 10; // Default page size to 10 if not specified
                getAllDTO.PageNumber ??= 1; // Default page number to 1 if not specified

                // Call the service to retrieve the data with sorting and filtering applied
                var certificates = await this.certificate.GetAllCertificateAsync(getAllDTO);

                // Check if the result is empty
                if (certificates == null || !certificates.Any())
                {
                    return NotFound("No certificates found");
                }

                // Return the result
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                // Handle exception and return a bad request with error details
                return BadRequest(new { message = ex.Message });
            }
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
        public async Task<IActionResult> CreateCertificate([FromBody] CertificateCreateDTO request)
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
        public async Task<IActionResult> UpdateCertificate(int id, [FromBody] CertificateCreateDTO request)
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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            try
            {
                // Attempt to delete the certificate
                await this.certificate.DeleteCertificate(id);
                return Ok($"Certificate with ID {id} was successfully deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Certificate with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
