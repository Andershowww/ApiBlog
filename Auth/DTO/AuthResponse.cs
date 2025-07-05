namespace ApiBlog.Features.Auth.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
