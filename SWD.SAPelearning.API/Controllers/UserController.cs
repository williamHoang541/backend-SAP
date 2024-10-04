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
        [Route("api/users")]
        public async Task<IActionResult> GetAll(
            [FromQuery] string filterOn = null,
            [FromQuery] string filterQuery = null,
            [FromQuery] string sortBy = null,
            [FromQuery] bool? isAscending = null, // Change to nullable bool
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var users = await this.user.GetAllUsers(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }

        /// <summary>
        /// Login with username, password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("login-web")]
        [HttpPost]
        public async Task<IActionResult> LoginWeb(LoginDTO user)
        {
            try
            {
                var a = await this.user.LoginWeb(user);
                return Ok(a);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Login with username, password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("login-app")]
        [HttpPost]
        public async Task<IActionResult> LoginApp(LoginDTO user)
        {
            try
            {
                var a = await this.user.LoginApp(user);
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
        [HttpPut]
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
        /// update-status-is-online
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("update-status-is-online")]
        [HttpPut]
        public async Task<IActionResult> UpdateStatusIsOnline(string id)
        {
            try
            {
                var a = await this.user.UpdateIsOnlineLogout(id);
                return Ok(a);
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
        public async Task<IActionResult> Delete(RemoveUDTO id)
        {
            try
            {
                var a = await this.user.Delete(id);
                return Ok(a);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
