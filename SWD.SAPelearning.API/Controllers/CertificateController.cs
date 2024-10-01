using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertificateDTO;
using SWD.SAPelearning.Repository.Models;

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
        [Route("get-all-certificate")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.certificate.GetAllCertificate();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Certificate>> GetCertificateById(string id)
        {
            var certificate = await this.certificate.GetCertificateById(id);
            if (certificate == null)
            {
                return NotFound("Certificate not found");
            }
            return Ok(certificate);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateCertificate([FromBody] CreateCertificateDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newCertificate = await this.certificate.CreateCertificateAsync(request);

                if (newCertificate == null)
                {
                    return BadRequest("Failed to create certificate.");
                }

                return CreatedAtAction(nameof(GetCertificateById), new { id = newCertificate.CertificateId }, newCertificate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
    }
}
