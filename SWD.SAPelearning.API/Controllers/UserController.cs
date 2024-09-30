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
        [HttpPost]
        public async Task<IActionResult> UpdateStudent(string id, UpdateUserStudent user)
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
        /// delete user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(RemoveDTO user)
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
