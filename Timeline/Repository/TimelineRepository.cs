using ApiBlog.Data;
using ApiBlog.Timeline.DTO;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Timeline.Repository
{
    public class TimelineRepository : ITimelineRepository
    {
        private APIContext _context;
        public TimelineRepository(APIContext context)
        {
            _context = context;
        }

        public async Task<List<TimelinePostsReponse>> BuscarTimeLine(int IDUsuario, List<string>? tags = null)
        {
            var idsSeguidos = _context.UsuariosSeguidos
                            .Where(u => u.IdUsuario == IDUsuario)
                            .Select(u => u.IdUsuarioSeguido)
                            .ToList();

            var postsUserSeguidos = _context.Posts
                          .Where(p => idsSeguidos.Contains(p.IdUsuario))
                          .Include(p => p.Curtidas)
                          .Include(p => p.Usuario)
                          .Include(p => p.PostTags)
                           .ThenInclude(pt => pt.Tag)
                          .Include(p => p.Comentarios)
                          .ToList();
            if (tags != null && tags.Count() > 0)
            {
                postsUserSeguidos = postsUserSeguidos
                    .Where(p => p.PostTags.Any(pt => tags.Contains(pt.Tag.Nome)))
                    .ToList();
            }

            var timeline = postsUserSeguidos.Select(p => new TimelinePostsReponse
            {
                PostId = p.IDPost,
                Titulo = p.Titulo,
                Corpo = p.Corpo,
                DataCriacao = p.DataCriacao,
                AutorId = p.IdUsuario,
                AutorUsername = p.Usuario.Username,
                Tags = p.PostTags.Select(pt => pt.Tag.Nome).ToList(),
                TotalCurtidas = p.Curtidas.Count,
                Comentarios = p.Comentarios.Select(c => new ComentarioResponse
                {
                    IdUsuario = c.IdUsuario,
                    NomeUsuario = c.Usuario.Username,
                    Texto = c.Comentario,
                    DataComentario = c.DataComentario
                }).ToList()
            }).ToList();

            return timeline;
        }
        public async Task<List<TimelinePostsReponse>> BuscarTopPosts24h(List<string>? tags = null)
        {
            var postsMaisCurtidos = _context.Posts
                          .Where(p => p.DataCriacao <= DateTime.Now && p.DataCriacao >= DateTime.Now.AddDays(-1))
                          .Include(p => p.Curtidas)
                          .Include(p => p.Usuario)
                          .Include(p => p.PostTags)
                           .ThenInclude(pt => pt.Tag)
                          .Include(p => p.Comentarios)
                          .OrderByDescending(p => p.Curtidas.Count)
                          .ToList();

            if (tags != null && tags.Count() > 0)
            {
                postsMaisCurtidos = postsMaisCurtidos
                    .Where(p => p.PostTags.Any(pt => tags.Contains(pt.Tag.Nome)))
                    .ToList();
            }
            var timeline = postsMaisCurtidos.Select(p => new TimelinePostsReponse
            {
                PostId = p.IDPost,
                Titulo = p.Titulo,
                Corpo = p.Corpo,
                DataCriacao = p.DataCriacao,
                AutorId = p.IdUsuario,
                AutorUsername = p.Usuario.Username,
                Tags = p.PostTags.Select(pt => pt.Tag.Nome).ToList(),
                TotalCurtidas = p.Curtidas.Count,
                Comentarios = p.Comentarios.Select(c => new ComentarioResponse
                {
                    IdUsuario = c.IdUsuario,
                    NomeUsuario = c.Usuario.Username,
                    Texto = c.Comentario,
                    DataComentario = c.DataComentario
                }).ToList()
            }).ToList();

            return timeline;
        }
    }
}
