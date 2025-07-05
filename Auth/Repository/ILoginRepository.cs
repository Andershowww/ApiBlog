using ApiBlog.Auth.Models;
using ApiBlog.Features.Auth.DTOs;

namespace ApiBlog.Auth.Repository
{
    public interface ILoginRepository
    {
        Task<Usuario?> Entrar(LoginRequest credenciais);
    }
}
