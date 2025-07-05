using ApiBlog.Post.DTO;

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
    }
}
