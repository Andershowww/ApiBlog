using ApiBlog.Auth.Models;
using ApiBlog.Data;
using ApiBlog.Features.Auth.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Auth.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private APIContext _context;
        public UsuarioRepository(APIContext context)
        {
            _context = context;
        }
        public async Task<Usuario> CadastraNovoUsuario(RegisterRequest registroNovoUsuario)
        {
            var hasher = new PasswordHasher<Usuario>();
            Usuario xNewUser= new Usuario();
            xNewUser.Username=registroNovoUsuario.Username;
            xNewUser.Email= registroNovoUsuario.Email;
            xNewUser.Ativo = true;
            xNewUser.PasswordHash= hasher.HashPassword(xNewUser, registroNovoUsuario.Senha);
            xNewUser.CreatedAt= DateTime.Now;
            _context.Add(xNewUser);
            await _context.SaveChangesAsync();
            return xNewUser;
        }

        public async Task<bool> UsuarioExisteAsync(string email, string username)
        {
            return await _context.Usuarios.AnyAsync(u =>
                u.Email == email || u.Username == username);
        }
    }
}
