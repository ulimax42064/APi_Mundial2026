using Microsoft.AspNetCore.Mvc;
using TupApi.DTOs;
using TupApi.Services;

namespace TupApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        /// <summary>Registrar un nuevo usuario.</summary>
        // POST /api/usuario/registro
        [HttpPost("registro")]
        public async Task<ActionResult<object>> Registro([FromBody] UsuarioRegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _service.RegistrarAsync(dto);
            return StatusCode(201, new
            {
                id      = usuario.Id,
                nombre  = usuario.Nombre,
                email   = usuario.Email,
                mensaje = "Usuario registrado correctamente."
            });
        }

        /// <summary>Iniciar sesión con email y contraseña.</summary>
        // POST /api/usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] UsuarioLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _service.LoginAsync(dto);
            return Ok(new
            {
                id      = usuario.Id,
                nombre  = usuario.Nombre,
                email   = usuario.Email,
                mensaje = "Login exitoso."
            });
        }
    }
}
