using ApiBlog.Interacao.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.Interacao.Repository
{
    public interface IInteracaoRepository
    {
        Task<bool> CurtirPost(int IDPost, int IDUsuario);
        Task<bool> DescurtirPost(int IDPost, int IDUsuario);
        Task<bool> Comentario(ComentarioRequest request, int IDUsuario);
        Task<bool> SeguirUsuario(int IDUsuarioSeguido, int IDUsuario);
        Task<bool> DeixarDeSeguirUsuario(int IDUsuarioSeguido, int IDUsuario);

    }
}
