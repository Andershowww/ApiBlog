using ApiBlog.Features.Auth.DTOs;
using ApiBlog.Usuarios.Models;

namespace ApiBlog.Auth.Repository
{
    public interface ILoginRepository
    {
        Task<Usuario?> Entrar(LoginRequest credenciais);
    }
}
