using ApiBlog.Auth.Repository;
using ApiBlog.Features.Auth;
using ApiBlog.Features.Auth.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;


namespace ApiBlog.Auth.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUsuarioRepository _usuarioRepository;
        private TokenService _tokenService;
        private ILoginRepository _loginRepository;
        public AuthController(IUsuarioRepository usuario, TokenService token, ILoginRepository loginRepository)
        {
            _usuarioRepository = usuario;
            _tokenService = token;
            _loginRepository = loginRepository;
        }

        [EnableCors]
        [HttpPost("registro")]
        public async Task<ActionResult<AuthResponse>> Registro(RegisterRequest registroNovoUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var usuarioJaExiste = await _usuarioRepository.UsuarioExisteAsync(registroNovoUsuario.Email, registroNovoUsuario.Username);

            if (usuarioJaExiste)
                return Conflict("Usuário ou email já está em uso.");

            var usuario = await _usuarioRepository.CadastraNovoUsuario(registroNovoUsuario);
            return Ok(new AuthResponse
            {
                Token = _tokenService.GenerateToken(usuario),
                Username = usuario.Username
            });
        }

        [EnableCors]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var usuario = await _loginRepository.Entrar(login);
            if (usuario == null)
                return Unauthorized("Usuário ou senha inválidos.");

            return Ok(new AuthResponse
            {
                Token = _tokenService.GenerateToken(usuario),
                Username = usuario.Username
            });
        }
    }
}

