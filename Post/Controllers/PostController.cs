using ApiBlog.Auth.Repository;
using ApiBlog.Post.DTO;
using ApiBlog.Post.Repository;
using ApiBlog.Tag.DTO;
using ApiBlog.Tag.Repository;
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

            // Cria o post
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
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> EditarPost(int IDPost, PostUpdateRequest postAtualizado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Usuário não autenticado.");

            int idUsuario = int.Parse(userIdClaim.Value);
            var postExistente = await _postRepository.ObterPostPorID(IDPost);
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
        [HttpPut("Editar/{id}/tags")]
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
            if(!deletou)
                return BadRequest("Ocorreu um erro ao deletar o post.");

            return Ok("Post deletado com sucesso.");
        }
    }
}
