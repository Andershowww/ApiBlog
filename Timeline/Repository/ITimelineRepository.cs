using ApiBlog.Post.Repository;
using ApiBlog.Timeline.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.Timeline.Repository
{
    public interface ITimelineRepository
    {

        Task<List<TimelinePostsReponse>> BuscarTimeLine(int IDUsuario, List<string>? tag = null);
        Task<List<TimelinePostsReponse>> BuscarTopPosts24h();

    }
}
