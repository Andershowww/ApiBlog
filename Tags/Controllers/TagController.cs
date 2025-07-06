using ApiBlog.Tag.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.Tag.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }


        [EnableCors]
        [HttpGet("sugestoes")]
        public async Task<IActionResult> ListarSugestoes([FromQuery] string? busca)
        {
            var sugestoes = await _tagRepository.BuscarTagsPorNome(busca);
            return Ok(sugestoes);
        }

    }
}
