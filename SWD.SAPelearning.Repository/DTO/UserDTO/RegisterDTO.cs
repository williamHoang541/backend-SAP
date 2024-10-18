using System.ComponentModel.DataAnnotations;

namespace SWD.SAPelearning.Repository.DTO.UserDTO
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
