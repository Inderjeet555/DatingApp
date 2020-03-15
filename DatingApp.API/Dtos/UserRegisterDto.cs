using System.ComponentModel.DataAnnotations;
namespace DatingApp.API.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage =  "You must specify password")]
        public string Password { get; set; }
    }
}