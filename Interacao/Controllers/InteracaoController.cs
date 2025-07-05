using ApiBlog.Auth.Models;
using ApiBlog.Interacao.DTO;
using ApiBlog.Interacao.Repository;
using ApiBlog.Post.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiBlog.Interacao.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class InteracaoController : ControllerBase
    {
        private readonly IInteracaoRepository _interacaoRepository;

        public InteracaoController(IInteracaoRepository interacaoRepository)
        {
            _interacaoRepository = interacaoRepository;
        }

        [EnableCors]
        [HttpPost("Curtir/{id}")]
        public async Task<IActionResult> Curtir(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            bool curtiu = await _interacaoRepository.CurtirPost(id, idUsuario);
            if (!curtiu)
                return BadRequest("Ocorreu um erro ao curtir esse post, tente novamente mais tarde.");
            return Ok("Post curtido com sucesso.");
        }

        [EnableCors]
        [HttpDelete("Descurtir/{id}")]
        public async Task<IActionResult> Descurtir(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            bool descurtiu = await _interacaoRepository.DescurtirPost(id, idUsuario);
            if (!descurtiu)
                return BadRequest("Ocorreu um erro ao dar dislike esse post, tente novamente mais tarde.");
            return Ok("Você deu dislike nesse post com sucesso.");
        }


        [EnableCors]
        [HttpPost("Comentario")]
        public async Task<IActionResult> Comentario(ComentarioRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            bool comentou = await _interacaoRepository.Comentario(request, idUsuario);
            if (!comentou)
                return BadRequest("Ocorreu um erro ao comentar nesse post, tente novamente mais tarde.");
            return Ok("Comentário salvo com sucesso.");
        }

        [EnableCors]
        [HttpPost("Seguir/{id}")]
        public async Task<IActionResult> SeguirUsuario(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            if (idUsuario == id)
                return BadRequest("Você não pode seguir sua própria conta");
            bool comentou = await _interacaoRepository.SeguirUsuario(id, idUsuario);
            if (!comentou)
                return BadRequest("Ocorreu um erro ao seguir esse usuário, tente novamente mais tarde.");
            return Ok("Usuário seguido com sucesso.");
        }

        [EnableCors]
        [HttpDelete("DeixarDeSeguirUsuario/{id}")]
        public async Task<IActionResult> DeixarDeSeguir(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            if (idUsuario == id)
                return BadRequest("Essa operação só é permitida com outros usuários");
            bool comentou = await _interacaoRepository.DeixarDeSeguirUsuario(id, idUsuario);
            if (!comentou)
                return BadRequest("Ocorreu um erro ao deixar de seguir esse usuário, tente novamente mais tarde.");
            return Ok("Você parou de seguir esse usuário com sucesso.");
        }

    }
}
