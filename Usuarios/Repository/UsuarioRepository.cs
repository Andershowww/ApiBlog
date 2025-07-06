using ApiBlog.Data;
using ApiBlog.Features.Auth.DTOs;
using ApiBlog.Usuarios.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Usuarios.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private APIContext _context;
        public UsuarioRepository(APIContext context)
        {
            _context = context;
        }
        public async Task<Models.Usuario> CadastraNovoUsuario(RegisterRequest registroNovoUsuario)
        {
            var hasher = new PasswordHasher<Models.Usuario>();
            Models.Usuario xNewUser = new Models.Usuario();
            xNewUser.Username = registroNovoUsuario.Username;
            xNewUser.Email = registroNovoUsuario.Email;
            xNewUser.Ativo = true;
            xNewUser.PasswordHash = hasher.HashPassword(xNewUser, registroNovoUsuario.Senha);
            xNewUser.CreatedAt = DateTime.Now;
            _context.Add(xNewUser);
            await _context.SaveChangesAsync();
            return xNewUser;
        }

        public async Task<bool> UsuarioExisteAsync(string email, string username)
        {
            return await _context.Usuarios.AnyAsync(u =>
                u.Email == email || u.Username == username);
        }
        public async Task<bool> SeguirUsuario(int IDUsuarioSeguido, int IDUsuario)
        {
            try
            {
                var xNewSeguir = new UsuarioSeguido()
                {
                    IdUsuario = IDUsuario,
                    IdUsuarioSeguido = IDUsuarioSeguido,
                    DataSeguimento = DateTime.Now,
                };
                _context.Add(xNewSeguir);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
        public async Task<bool> DeixarDeSeguirUsuario(int IDUsuarioSeguido, int IDUsuario)
        {
            try
            {
                var xNewSeguir = _context.UsuariosSeguidos.Where(x => x.IdUsuarioSeguido == IDUsuarioSeguido && x.IdUsuario == IDUsuario).FirstOrDefault();
                if (xNewSeguir != null)
                {
                    _context.Remove(xNewSeguir);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;

            }
            catch { return false; }
        }
    }
}
