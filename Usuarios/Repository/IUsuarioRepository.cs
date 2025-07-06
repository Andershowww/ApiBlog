using ApiBlog.DTO;
using ApiBlog.Features.Auth.DTOs;
using ApiBlog.Usuarios.Models;
namespace ApiBlog.Usuarios.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario> CadastraNovoUsuario(RegisterRequest registroNovoUsuario);
        Task<bool> UsuarioExisteAsync(string email, string username);
        Task<ResultadoAcao> SeguirUsuario(int IDUsuarioSeguido, int IDUsuario);
        Task<ResultadoAcao> DeixarDeSeguirUsuario(int IDUsuarioSeguido, int IDUsuario);
      
    }
}
