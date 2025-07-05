using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Features.Auth.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
