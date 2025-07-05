using ApiBlog.Auth.Models;
using ApiBlog.Features.Auth.DTOs;

namespace ApiBlog.Auth.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario> CadastraNovoUsuario(RegisterRequest registroNovoUsuario);
        Task<bool> UsuarioExisteAsync(string email, string username);

    }
}
