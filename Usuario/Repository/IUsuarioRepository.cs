using ApiBlog.Features.Auth.DTOs;

namespace ApiBlog.Usuario.Repository
{
    public interface IUsuarioRepository
    {
        Task<Models.Usuario> CadastraNovoUsuario(RegisterRequest registroNovoUsuario);
        Task<bool> UsuarioExisteAsync(string email, string username);
        Task<bool> SeguirUsuario(int IDUsuarioSeguido, int IDUsuario);
        Task<bool> DeixarDeSeguirUsuario(int IDUsuarioSeguido, int IDUsuario);
    }
}
