using ApiBlog.Timeline.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiBlog.Timeline.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class TimelineController : ControllerBase
    {
        private readonly ITimelineRepository _timelineRepository;

        public TimelineController(ITimelineRepository timelineRepository)
        {
            _timelineRepository = timelineRepository;
        }

        [EnableCors]
        [HttpGet("PostsUsuariosSeguidos")]
        public async Task<IActionResult> BuscarPosts([FromQuery] List<string>? tags)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            var idUsuario = int.Parse(userIdClaim.Value);
            var posts = await _timelineRepository.BuscarTimeLine(idUsuario, tags);
            if (posts==null || posts.Count<=0 )
                return BadRequest("Nenhum post encontrado, verifique se você já segue alguma conta ou revise os filtros de tag.");
            return Ok(posts);
        }
        [EnableCors]
        [HttpGet("TopPosts")]
        public async Task<IActionResult> BuscarTopPosts([FromQuery] List<string>? tags)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");

            var posts = await _timelineRepository.BuscarTopPosts24h(tags);
            if (posts == null || posts.Count <= 0)
                return BadRequest("Nenhum post encontrado, verifique se você já segue alguma conta ou revise os filtros de tag.");
            return Ok(posts);
        }
    }
}
