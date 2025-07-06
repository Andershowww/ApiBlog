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
                await _postRepository.CadastraPostTag(postCriado.IDPost, idsTags);
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

            await _postRepository.AtualizarPost(postExistente);

            return Ok("Post atualizado com sucesso.");

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

            await _postRepository.AtualizarTagsDoPost(postExistente.IDPost, idsNovasTags);
            return Ok("Tags atualizadas com sucesso.");
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

            bool deletou = await _postRepository.DeletaPost(id);
            if (!deletou)
                return BadRequest("Ocorreu um erro ao deletar o post.");

            return Ok("Post deletado com sucesso.");
        }

        [EnableCors]
        [HttpPost("{id}/Curtir")]
        public async Task<IActionResult> Curtir(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            bool curtiu = await _postRepository.CurtirPost(id, idUsuario);
            if (!curtiu)
                return BadRequest("Ocorreu um erro ao curtir esse post, tente novamente mais tarde.");
            return Ok("Post curtido com sucesso.");
        }

        [EnableCors]
        [HttpDelete("{id}/Curtida")]
        public async Task<IActionResult> Descurtir(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");
            int idUsuario = int.Parse(userIdClaim.Value);
            bool descurtiu = await _postRepository.RemoverCurtidaPost(id, idUsuario);
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
            bool comentou = await _postRepository.Comentario(request, idUsuario);
            if (!comentou)
                return BadRequest("Ocorreu um erro ao comentar nesse post, tente novamente mais tarde.");
            return Ok("Comentário salvo com sucesso.");
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
