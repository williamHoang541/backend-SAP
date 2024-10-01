using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPelearning_bakend.DTO.UserDTO;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.UserDTO;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser user;

        public UserController(IUser user)
        {
            this.user = user;
        }

        [HttpGet]
        [Route("get-all-user")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.user.GetAllUsers();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }

        /// <summary>
        /// Login with username, password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            try
            {
                var a = await this.user.Login(user);
                return Ok(a);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// register for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("registration")]
        [HttpPost]
        public async Task<IActionResult> Registration(RegisterDTO user)
        {
            try
            {
                var a = await this.user.Registration(user);
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// create-instructor
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("create-instructor")]
        [HttpPost]
        public async Task<IActionResult> CreateInstructor(CreateUserInstructorDTO user)
        {
            try
            {
                var a = await this.user.CreateInstructor(user);
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// update-student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("update-student")]
        [HttpPatch]
        public async Task<IActionResult> UpdateStudent(string id, UpdateUserStudentDTO user)
        {
            try
            {
                var a = await this.user.UpdateStudent(id,user);
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// search-by-name
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("search-by-name")]
        [HttpGet]
        public async Task<IActionResult> SearchByName(string user)
        {
            try
            {
                var a = await this.user.SearchByName(user);
                return Ok(a);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// get-by-id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("get-by-id")]
        [HttpPost]
        public async Task<IActionResult> GetUserById(SearchUserIdDTO id)
        {
            try
            {
                var user = await this.user.getUserByID(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// update-status-is-online
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("update-status-is-online")]
        [HttpPatch]
        public async Task<IActionResult> UpdateStatusIsOnline(string id)
        {
            try
            {
                var a = await this.user.UpdateStatusIsOnline(id);
                return Ok(a);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("get-all-instructors-by-id-{prefix}")]
        [HttpGet]
        public async Task<IActionResult> GetInstructorsByPrefix(string prefix)
        {
            try
            {
                // Fetch instructors based on the prefix
                var instructors = await this.user.GetInstructorsByPrefix(prefix);

                // Check if any instructors were found
                if (instructors == null || instructors.Count == 0)
                {
                    return NotFound("No instructors found with the specified prefix.");
                }

                return Ok(instructors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("get-all-student-by-id-{prefix}")]
        [HttpGet]
        public async Task<IActionResult> GetStudentsByPrefix(string prefix)
        {
            try
            {
                // Fetch students based on the prefix
                var students = await this.user.GetStudentsByPrefix(prefix);

                // Check if any students were found
                if (students == null || students.Count == 0)
                {
                    return NotFound("No students found with the specified prefix.");
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(RemoveUDTO user)
        {
            try
            {
                var a = await this.user.Delete(user);
                return Ok(a);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
