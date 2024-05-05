using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
