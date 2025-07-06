using ApiBlog.Features.Auth.DTOs;
using ApiBlog.Usuarios.Models;
namespace ApiBlog.Usuarios.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario> CadastraNovoUsuario(RegisterRequest registroNovoUsuario);
        Task<bool> UsuarioExisteAsync(string email, string username);
        Task<bool> SeguirUsuario(int IDUsuarioSeguido, int IDUsuario);
        Task<bool> DeixarDeSeguirUsuario(int IDUsuarioSeguido, int IDUsuario);
    }
}
