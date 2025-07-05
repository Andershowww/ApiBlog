using ApiBlog.Auth.Models;
using ApiBlog.Data;
using ApiBlog.Interacao.DTO;
using ApiBlog.Interacao.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.Interacao.Repository
{
    public class InteracaoRepository : IInteracaoRepository
    {

        private APIContext _context;

        public InteracaoRepository(APIContext context)
        {
            _context = context;
        }

        public async Task<bool> CurtirPost(int IDPost, int IDUsuario)
        {
            try
            {
                var xNewPostCurtido = new PostCurtido
                {
                    IdPost = IDPost,
                    IdUsuario = IDUsuario,
                    DataCurtido = DateTime.Now
                };

                _context.Add(xNewPostCurtido);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DescurtirPost(int IDPost, int IDUsuario)
        {
            try
            {
                var xNewPostCurtido = _context.PostsCurtidos.Where(x => x.IdPost == IDPost && x.IdUsuario == IDUsuario);
                _context.Remove(xNewPostCurtido);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Comentario(ComentarioRequest request, int IDUsuario)
        {
            try
            {
                var xNewPostComentario = new PostComentario()
                {
                    Comentario = request.Comentario,
                    DataComentario = DateTime.Now,
                    IdUsuario = IDUsuario,
                    IdPost = request.IDPost
                };
                _context.Add(xNewPostComentario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
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
                var xNewSeguir = _context.UsuariosSeguidos.Where(x => x.IdUsuarioSeguido == IDUsuarioSeguido && x.IdUsuario == IDUsuario);
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
