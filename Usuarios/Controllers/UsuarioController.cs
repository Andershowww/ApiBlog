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
            _usuarioRepository = usuarioRepository;
        }
        [EnableCors]
        [HttpPost("{id}/Seguir")]
        public async Task<IActionResult> SeguirUsuario(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            if (idUsuario == id)
                return BadRequest("Você não pode seguir sua própria conta");
            var retorno = await _usuarioRepository.SeguirUsuario(id, idUsuario);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok(retorno.Mensagem);
        }

        [EnableCors]
        [HttpDelete("{id}/Seguir")]
        public async Task<IActionResult> DeixarDeSeguir(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            if (idUsuario == id)
                return BadRequest("Essa operação só é permitida com outros usuários");
            var retorno = await _usuarioRepository.DeixarDeSeguirUsuario(id, idUsuario);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok(retorno.Mensagem);
        }
    }
}
