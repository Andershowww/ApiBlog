using ApiBlog.Post.DTO;
using ApiBlog.Timeline.DTO;

namespace ApiBlog.Post.Repository
{
    public interface IPostRepository
    {
        Task<PostResponse> CadastrarPost(PostRequest Post, int IDUsuario);
        Task CadastraPostTag(int IDPost, List<int> ListaTags);
        Task<Models.Post> ObterPostPorID(int IDPost);
        Task AtualizarPost(Models.Post Post);
        Task AtualizarTagsDoPost(int IDPost, List<int> novasTags);
        Task<bool> DeletaPost(int IDPost);
        Task<bool> CurtirPost(int IDPost, int IDUsuario);
        Task<bool> RemoverCurtidaPost(int IDPost, int IDUsuario);
        Task<bool> Comentario(ComentarioRequest request, int IDUsuario);
        Task<List<TimelinePostsReponse>> BuscarPostsUser(int IDUsuario);
        
    }
}
