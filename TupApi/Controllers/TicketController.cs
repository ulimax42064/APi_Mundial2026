using Microsoft.AspNetCore.Mvc;
using TupApi.Models;
using TupApi.Services;

namespace TupApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _service;

        public TicketController(TicketService service)
        {
            _service = service;
        }

        // GET /api/ticket?email=usuario@mail.com
        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetByUsuario([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { mensaje = "El parámetro 'email' es obligatorio." });

            var tickets = await _service.ObtenerPorUsuarioAsync(email);
            return Ok(tickets);
        }

        // GET /api/ticket/partido/{partidoId}
        [HttpGet("partido/{partidoId}")]
        public async Task<ActionResult<List<Ticket>>> GetByPartido(string partidoId)
        {
            var tickets = await _service.ObtenerPorPartidoAsync(partidoId);
            return Ok(tickets);
        }

        // POST /api/ticket
        [HttpPost]
        public async Task<ActionResult<Ticket>> Post([FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creado = await _service.ComprarAsync(ticket);
            return StatusCode(201, creado);
        }

        // DELETE /api/ticket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.CancelarAsync(id);
            return NoContent();
        }
    }
}