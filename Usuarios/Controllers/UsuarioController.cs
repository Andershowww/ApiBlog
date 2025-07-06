using ApiBlog.Usuarios.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiBlog.Usuarios.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository= usuarioRepository;
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
            bool comentou = await _usuarioRepository.SeguirUsuario(id, idUsuario);
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
            bool comentou = await _usuarioRepository.DeixarDeSeguirUsuario(id, idUsuario);
            if (!comentou)
                return BadRequest("Ocorreu um erro ao deixar de seguir esse usuário, tente novamente mais tarde.");
            return Ok("Você parou de seguir esse usuário com sucesso.");
        }
    }
}
