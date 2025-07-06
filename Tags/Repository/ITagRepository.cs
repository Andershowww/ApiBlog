using ApiBlog.Tag.DTO;

namespace ApiBlog.Tag.Repository
{
    public interface ITagRepository
    {
        Task<List<Models.Tag>> CadastraTag(List<string> tags);
        Task<List<Models.Tag>> BuscarTagsPorNome(string? termo);

    }
}
