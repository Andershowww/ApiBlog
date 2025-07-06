using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Features.Auth.DTOs
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
    }
}
