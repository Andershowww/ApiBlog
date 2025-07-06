using ApiBlog.Data;
using ApiBlog.DTO;
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
        public async Task<ResultadoAcao> SeguirUsuario(int IDUsuarioSeguido, int IDUsuario)
        {
            var RelacaoJaExiste = _context.UsuariosSeguidos.Where(us => us.IdUsuarioSeguido == IDUsuarioSeguido && us.IdUsuario == IDUsuario).FirstOrDefault();
            if (RelacaoJaExiste != null)
                return ResultadoAcao.Falha("Usuário já seguido!");
            try
            {
                var xNewSeguir = new UsuarioSeguido()
                {
                    IdUsuario = IDUsuario,
                    IdUsuarioSeguido = IDUsuarioSeguido,
                    Data = DateTime.Now,
                };
                _context.Add(xNewSeguir);
                await _context.SaveChangesAsync();
                return ResultadoAcao.Ok("Usuário seguido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao seguir o usuário! " + ex.Message);
            }
        }
        public async Task<ResultadoAcao> DeixarDeSeguirUsuario(int IDUsuarioSeguido, int IDUsuario)
        {
            try
            {
                var xNewSeguir = _context.UsuariosSeguidos.Where(x => x.IdUsuarioSeguido == IDUsuarioSeguido && x.IdUsuario == IDUsuario).FirstOrDefault();
                if (xNewSeguir != null)
                {
                    _context.Remove(xNewSeguir);
                    await _context.SaveChangesAsync();
                    return ResultadoAcao.Falha("Você parou de seguir esse usuário com sucesso!");
                }
                else
                    return ResultadoAcao.Falha("Você já deixou de seguir esse usuário!");

            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao tentar deixar de seguir esse usuário!" + ex.Message);
            }
        }
    }
}
