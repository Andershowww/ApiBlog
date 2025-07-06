using ApiBlog.Data;
using ApiBlog.Features.Auth.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Auth.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private APIContext _context;
        public LoginRepository(APIContext context)
        {
            _context = context;
        }
        public async Task<Usuario?> Entrar(LoginRequest credenciais)
        {
            var hasher = new PasswordHasher<Usuario>();
            var usuario = await _context.Usuarios
         .FirstOrDefaultAsync(u => u.Email == credenciais.Email && u.Ativo);

            if (usuario == null)
                return null;
            var resultado = hasher.VerifyHashedPassword(
              usuario,
              usuario.PasswordHash,
              credenciais.Senha
                 );

            if (resultado == PasswordVerificationResult.Success)
            {
                return usuario;
            }
            return null;
        }
    }
}
