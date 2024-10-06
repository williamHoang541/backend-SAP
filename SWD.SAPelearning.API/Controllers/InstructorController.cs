using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.Instructor;

namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructor instructor;

        public InstructorController(IInstructor instructor)
        {
            this.instructor = instructor;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.instructor.GetAllInstructor();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }

        [AllowAnonymous]
        [Route("update-Instructor")]
        [HttpPost]
        public async Task<IActionResult> UpdateInstructor(string userId,  UpdateInstructorDTO updateInstructorDTO)
        {
            try
            {
                // Attempt to update the instructor
                var result = await instructor.UpdateInstructorByUserId(userId, updateInstructorDTO);

                if (!result)
                {
                    return NotFound(new { message = "Instructor or user not found" });
                }

                // Return success response if updated
                return Ok(new { message = "Instructor updated successfully" });
            }
            catch (Exception ex)
            {
                // Catch any errors and return a bad request with the exception message
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(RemoveIDTO removeIDTO)
        {
            if (removeIDTO == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                // Call the delete method on the instructor service
                var result = await this.instructor.RemoveInstructor(removeIDTO);

                if (!result)
                {
                    return NotFound(new { message = "Instructor not found." });
                }

                return Ok(new { message = "Instructor deleted successfully." });
            }
            catch (Exception ex)
            {
                // Return bad request with the exception message
                return BadRequest(ex.Message);
            }
        }

    }
}
