using ApiBlog.DTO;
using ApiBlog.Post.DTO;
using ApiBlog.Timeline.DTO;


namespace ApiBlog.Post.Repository
{
    public interface IPostRepository
    {
        Task<PostResponse> CadastrarPost(PostRequest Post, int IDUsuario);
        Task<ResultadoAcao> CadastraPostTag(int IDPost, List<int> ListaTags);
        Task<Models.Post> ObterPostPorID(int IDPost);
        Task<ResultadoAcao> AtualizarPost(Models.Post Post);
        Task<ResultadoAcao> AtualizarTagsDoPost(int IDPost, List<int> novasTags);
        Task<ResultadoAcao> DeletaPost(int IDPost);
        Task<ResultadoAcao> CurtirPost(int IDPost, int IDUsuario);
        Task<ResultadoAcao> RemoverCurtidaPost(int IDPost, int IDUsuario);
        Task<ResultadoAcao> Comentario(ComentarioRequest request, int IDUsuario);
        Task<List<TimelinePostsReponse>> BuscarPostsUser(int IDUsuario);

    }
}
