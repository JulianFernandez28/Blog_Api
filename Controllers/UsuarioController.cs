using API_BLOG.Models;
using API_BLOG.Models.Dtos.Login;
using API_BLOG.Models.Dtos.Register;
using API_BLOG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;

namespace API_BLOG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private APIResponse  _response;
        public UsuarioController(IUsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
            _response = new();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto modelo)
        {
            var loginresponse = await _usuarioRepo.Login(modelo);
            if (loginresponse.Usuario == null || string.IsNullOrEmpty(loginresponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("UserName o Password son Incorrectos");
                return BadRequest(_response);
            }

            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Resultado = loginresponse;
            return Ok(_response);
        }
        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequestDto modelo)
        {
            bool IsUsuario = _usuarioRepo.isUserUnique(modelo.Email);

            if (!IsUsuario)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages.Add("Usuario ya existe");
                return BadRequest(_response);
            }

            RegisterResponseDto response = await _usuarioRepo.Register(modelo);
            if (response.Usuario == null || string.IsNullOrEmpty(response.Usuario.Id))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                _response.ErrorMessages = response.ErrorMessages;
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsExitoso = true;
            return Ok(_response);
        }
    }
}
