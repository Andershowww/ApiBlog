using ApiBlog.Auth.Models;
using ApiBlog.Data;
using ApiBlog.Features.Auth.DTOs;
using ApiBlog.Features.Auth;
using ApiBlog.Post.DTO;
using ApiBlog.Post.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ApiBlog.Interacao.Models;

namespace ApiBlog.Post.Repository
{
    public class PostRepository : IPostRepository
    {
        private APIContext _context;
        public PostRepository(APIContext context)
        {
            _context = context;
        }
        public async Task<PostResponse> CadastrarPost(PostRequest post, int IDUsuario)
        {
            Models.Post xNewPost = new Models.Post();
            xNewPost.DataCriacao = DateTime.Now;
            xNewPost.Corpo = post.Corpo;
            xNewPost.Titulo = post.Titulo;
            xNewPost.IdUsuario = IDUsuario;
            _context.Add(xNewPost);
            await _context.SaveChangesAsync();

            return (new PostResponse
            {
                IDPost = xNewPost.IDPost,
                Titulo = xNewPost.Titulo
            });
        }

        public async Task CadastraPostTag(int IDPost, List<int> IdsTags)
        {
            var postTags = IdsTags.Select(tagId => new PostTag
            {
                IdPost = IDPost,
                IdTag = tagId
            }).ToList();

            _context.PostsTags.AddRange(postTags);
            await _context.SaveChangesAsync();
        }

        public async Task<Models.Post> ObterPostPorID(int IDPost)
        {
            return await _context.Posts.Include(p => p.PostTags).FirstOrDefaultAsync(p => p.IDPost == IDPost);
        }

        public async Task AtualizarPost(Models.Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
        public async Task AtualizarTagsDoPost(int idPost, List<int> novasTags)
        {
            var tagsExistentes = _context.PostsTags.Where(pt => pt.IdPost == idPost);
            _context.PostsTags.RemoveRange(tagsExistentes);
            await _context.SaveChangesAsync();

            var novasAssociacoes = novasTags.Select(tagId => new PostTag
            {
                IdPost = idPost,
                IdTag = tagId
            }).ToList();

            _context.PostsTags.AddRange(novasAssociacoes);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeletaPost(int IDPost)
        {
            var post = _context.Posts.Where(p => p.IDPost == IDPost).FirstOrDefault();
            if(post!=null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
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
    }
}
