using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.EnrollmentDTO;
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Services;

namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollment enrollment;

        public EnrollmentController(IEnrollment enrollment)
        {
            this.enrollment = enrollment;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllEnrollments([FromQuery] GetAllDTO getAllDTO)
        {
            try
            {
                var enrollments = await this.enrollment.GetAllEnrollmentsAsync(getAllDTO);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging library)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> EnrollUser([FromBody] EnrollmentCreateDTO enrollmentCreateDTO)
        {
            try
            {
                // Validate the input request
                if (enrollmentCreateDTO == null)
                    return BadRequest("Enrollment data cannot be null.");

                if (string.IsNullOrEmpty(enrollmentCreateDTO.UserId))
                    return BadRequest("User ID is required.");

                if (enrollmentCreateDTO.CourseId <= 0)
                    return BadRequest("Valid Course ID is required.");

                if (enrollmentCreateDTO.EnrollmentPrice <= 0)
                    return BadRequest("Valid Enrollment Price is required.");

                // Call the service to create the enrollment
                var enrollmentDTO = await this.enrollment.CreateEnrollmentAsync(
                    enrollmentCreateDTO.UserId,
                    enrollmentCreateDTO.CourseId,
                    enrollmentCreateDTO.EnrollmentPrice
                );

                // Return the created enrollment
                return CreatedAtAction(nameof(GetEnrollmentById), new { id = enrollmentDTO.Id }, enrollmentDTO);
            }
            catch (InvalidOperationException ex)
            {
                // Return specific error messages for invalid operations
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEnrollmentById(int id)
        {
            try
            {
                var enrollment = await this.enrollment.GetEnrollmentByIdAsync(id);

                if (enrollment == null)
                {
                    return NotFound(new { Message = $"Enrollment with ID {id} not found." });
                }

                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("confirm/{enrollmentId:int}")]
        public async Task<IActionResult> ConfirmEnrollment(int enrollmentId)
        {
            try
            {
                bool isConfirmed = await this.enrollment.ConfirmEnrollmentAsync(enrollmentId);
                if (!isConfirmed)
                {
                    return NotFound(new { Message = $"Enrollment with ID {enrollmentId} not found or could not be confirmed." });
                }

                return Ok(new { Message = $"Enrollment with ID {enrollmentId} confirmed successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("complete/{enrollmentId}")]
        public async Task<IActionResult> MarkEnrollmentAsCompleted(int enrollmentId, string userId)
        {
            try
            {
                var result = await this.enrollment.MarkEnrollmentAsConfirmedAsync(enrollmentId, userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("cancel/{enrollmentId:int}")]
        public async Task<IActionResult> CancelEnrollment(int enrollmentId)
        {
            try
            {
                bool isCanceled = await this.enrollment.CancelEnrollmentAsync(enrollmentId);

                return Ok(new { Message = $"Enrollment with ID {enrollmentId} canceled successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
