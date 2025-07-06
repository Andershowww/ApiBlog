using ApiBlog.Auth.Repository;
using ApiBlog.Post.DTO;
using ApiBlog.Post.Repository;
using ApiBlog.Tag.DTO;
using ApiBlog.Tag.Repository;
using ApiBlog.Timeline.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiBlog.Post.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
        }


        [EnableCors]
        [HttpPost]
        public async Task<IActionResult> Cadastrar(PostRequest post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");

            int idUsuario = int.Parse(userIdClaim.Value);

            var postCriado = await _postRepository.CadastrarPost(post, idUsuario);

            if (postCriado == null)
                return BadRequest("Erro ao salvar o post. Verifique os dados e tente novamente.");


            var tags = await _tagRepository.CadastraTag(post.Tags);
            if (tags != null && tags.Any())
            {
                var idsTags = tags.Select(t => t.IdTag).ToList();
                var retorno = await _postRepository.CadastraPostTag(postCriado.IDPost, idsTags);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
            }
            return Ok("Post cadastrado com sucesso.");
        }

        [EnableCors]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarPost(int id, PostUpdateRequest postAtualizado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");

            int idUsuario = int.Parse(userIdClaim.Value);
            var postExistente = await _postRepository.ObterPostPorID(id);
            if (postExistente == null)
                return NotFound("Post não encontrado.");

            if (postExistente.IdUsuario != idUsuario)
                return Forbid("Você não tem permissão para editar este post.");

            postExistente.Titulo = postAtualizado.Titulo;
            postExistente.Corpo = postAtualizado.Corpo;

            var retorno = await _postRepository.AtualizarPost(postExistente);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok(retorno.Mensagem);

        }

        [EnableCors]
        [HttpPut("Editar/{id}/Tags")]
        public async Task<IActionResult> EditarPostTags(int id, TagRequest Tags)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");

            int idUsuario = int.Parse(userIdClaim.Value);
            var postExistente = await _postRepository.ObterPostPorID(id);
            if (postExistente == null)
                return NotFound("Post não encontrado.");

            if (postExistente.IdUsuario != idUsuario)
                return Forbid("Você não tem permissão para editar este post.");

            var novasTags = await _tagRepository.CadastraTag(Tags.Nomes);
            var idsNovasTags = novasTags.Select(t => t.IdTag).ToList();

            var retorno = await _postRepository.AtualizarTagsDoPost(postExistente.IDPost, idsNovasTags);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok(retorno.Mensagem);
        }

        [EnableCors]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPost(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");

            int idUsuario = int.Parse(userIdClaim.Value);
            var postExistente = await _postRepository.ObterPostPorID(id);
            if (postExistente == null)
                return NotFound("Post não encontrado.");

            if (postExistente.IdUsuario != idUsuario)
                return Forbid("Você não tem permissão para editar este post.");

            var retorno = await _postRepository.DeletaPost(id);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);

            return Ok(retorno.Mensagem);
        }

        [EnableCors]
        [HttpPost("{id}/Curtir")]
        public async Task<IActionResult> Curtir(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            var retorno = await _postRepository.CurtirPost(id, idUsuario);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok(retorno.Mensagem);
        }

        [EnableCors]
        [HttpDelete("{id}/Curtir")]
        public async Task<IActionResult> Descurtir(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            var retorno = await _postRepository.RemoverCurtidaPost(id, idUsuario);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok(retorno.Mensagem);
        }

        [EnableCors]
        [HttpPost("Comentario")]
        public async Task<IActionResult> Comentario(ComentarioRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            var retorno = await _postRepository.Comentario(request, idUsuario);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok(retorno.Mensagem);
        }

        [EnableCors]
        [HttpGet("{idUsuario}/Posts")]
        public async Task<IActionResult> BuscarPostsPerfil(int idUsuario)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");

            var posts = await _postRepository.BuscarPostsUser(idUsuario);
            if (posts == null || !posts.Any())
                return NotFound("Esse usuário ainda não possui posts.");

            return Ok(posts);
        }
    }
}
