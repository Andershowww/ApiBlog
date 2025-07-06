using ApiBlog.Auth;
using ApiBlog.Data;
using ApiBlog.Features.Auth.DTOs;
using ApiBlog.Features.Auth;
using ApiBlog.Post.DTO;
using ApiBlog.Post.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ApiBlog.Timeline.DTO;
using ApiBlog.Usuarios.Models;
using ApiBlog.DTO;
using System.Reflection.Metadata.Ecma335;


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

        public async Task<ResultadoAcao> CadastraPostTag(int IDPost, List<int> IdsTags)
        {
            var postTags = IdsTags.Select(tagId => new PostTag
            {
                IdPost = IDPost,
                IdTag = tagId
            }).ToList();
            try
            {
                _context.PostsTags.AddRange(postTags);
                await _context.SaveChangesAsync();
                return ResultadoAcao.Ok("Tags cadastradas com sucesso");
            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao cadastrar as tags" + ex.Message);
            }
        }

        public async Task<Models.Post> ObterPostPorID(int IDPost)
        {
            return await _context.Posts.Include(p => p.PostTags).FirstOrDefaultAsync(p => p.IDPost == IDPost);
        }

        public async Task<ResultadoAcao> AtualizarPost(Models.Post post)
        {
            try
            {
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao atualizar esse post, tente novamente mais tarde!");
            }

            return ResultadoAcao.Ok("Post Atualizado com sucesso!");
        }
        public async Task<ResultadoAcao> AtualizarTagsDoPost(int idPost, List<int> novasTags)
        {
            try
            {
                var tagsExistentes = _context.PostsTags.Where(pt => pt.IdPost == idPost);
                _context.PostsTags.RemoveRange(tagsExistentes);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao remover as tags existentes.");
            }
            try
            {
                var novasAssociacoes = novasTags.Select(tagId => new PostTag
                {
                    IdPost = idPost,
                    IdTag = tagId
                }).ToList();

                _context.PostsTags.AddRange(novasAssociacoes);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao cadastrar as tags do post!");
            }
            return ResultadoAcao.Ok("Tags atualizadas com sucesso!");
        }
        public async Task<ResultadoAcao> DeletaPost(int IDPost)
        {
            var post = _context.Posts.Where(p => p.IDPost == IDPost).FirstOrDefault();
            if (post != null)
            {
                try
                {
                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();
                    return ResultadoAcao.Ok("Post deletado com sucesso!");
                }
                catch (Exception ex)
                {
                    return ResultadoAcao.Falha("Ocorreu um erro ao deletar o post." + ex.Message);
                }
            }
            return ResultadoAcao.Falha("O post indicado não foi encontrado, revise o id e tente novamente.");
        }
        public async Task<ResultadoAcao> CurtirPost(int IDPost, int IDUsuario)
        {
            var JaCurtiu = _context.PostsCurtidos.Where(pc => pc.IdPost == IDPost && pc.IdUsuario == IDUsuario).FirstOrDefault();
            if (JaCurtiu != null)
                return ResultadoAcao.Falha("Você já curtiu esse post!");
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
                return ResultadoAcao.Ok("Post curtido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao curtir esse post!" + ex.Message);
            }
        }

        public async Task<ResultadoAcao> RemoverCurtidaPost(int IDPost, int IDUsuario)
        {
            try
            {
                var xRemoverCurtidaPost = _context.PostsCurtidos.Where(x => x.IdPost == IDPost && x.IdUsuario == IDUsuario).FirstOrDefault();
                if (xRemoverCurtidaPost != null)
                {
                    _context.Remove(xRemoverCurtidaPost);
                    await _context.SaveChangesAsync();
                    return ResultadoAcao.Ok("Você removeu a curtida com sucesso!");
                }
                else
                    return ResultadoAcao.Falha("Ocorreu um erro, a curtida já foi removida!");
            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao remover a curtida!" + ex.Message);
            }
        }

        public async Task<ResultadoAcao> Comentario(ComentarioRequest request, int IDUsuario)
        {
            try
            {
                var xNewPostComentario = new PostComentario()
                {
                    Comentario = request.Comentario,
                    DataComentario = DateTime.Now,
                    IdUsuario = IDUsuario,
                    IdPost = request.IDPost,
                    Ativo = true
                };
                _context.Add(xNewPostComentario);
                await _context.SaveChangesAsync();
                return ResultadoAcao.Ok("Comentário salvo com sucesso!");
            }
            catch (Exception ex)
            {
                return ResultadoAcao.Falha("Ocorreu um erro ao salvar seu comentário!" + ex.Message);
            }
        }

        public async Task<List<TimelinePostsReponse>> BuscarPostsUser(int IDUsuario)
        {
            return await _context.Posts.Where(p => p.IdUsuario == IDUsuario)
                 .Include(p => p.Curtidas)
                 .Include(p => p.Comentarios)
                 .Include(p => p.Usuario)
                 .Include(p => p.PostTags)
                 .ThenInclude(pt => pt.Tag)
                 .OrderByDescending(p => p.DataCriacao)
                 .Select(p => new TimelinePostsReponse
                 {
                     PostId = p.IDPost,
                     Titulo = p.Titulo,
                     Corpo = p.Corpo,
                     DataCriacao = p.DataCriacao,
                     TotalCurtidas = p.Curtidas.Count,
                     Tags = p.PostTags.Select(pt => pt.Tag.Nome).ToList(),
                     AutorId = p.IdUsuario,
                     AutorUsername = p.Usuario.Username,
                     Comentarios = p.Comentarios.Select(c => new ComentarioResponse
                     {
                         IdUsuario = c.IdUsuario,
                         NomeUsuario = c.Usuario.Username,
                         Texto = c.Comentario,
                         DataComentario = c.DataComentario
                     }).ToList()

                 }).ToListAsync();
        }
    }
}
