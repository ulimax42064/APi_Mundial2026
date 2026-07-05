using Microsoft.AspNetCore.Mvc;
using TupApi.DTOs;
using TupApi.Models;
using TupApi.Services;

namespace TupApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidoController : ControllerBase
    {
        private readonly PartidoService _service;

        public PartidoController(PartidoService service)
        {
            _service = service;
        }

        // GET /api/partido?fase=Final&grupo=Grupo A&pagina=1&porPagina=10
        [HttpGet]
        public async Task<ActionResult<PaginadoDto<Partido>>> Get([FromQuery] PartidoFiltroDto filtro)
        {
            var resultado = await _service.ObtenerTodosAsync(filtro);
            return Ok(resultado);
        }

        // GET /api/partido/{id}  (id de MongoDB)
        [HttpGet("{id}")]
        public async Task<ActionResult<Partido>> GetById(string id)
        {
            var partido = await _service.ObtenerPorIdAsync(id);
            return Ok(partido);
        }

        // GET /api/partido/numero/{numero}  (1-104, para la app Android)
        [HttpGet("numero/{numero:int}")]
        public async Task<ActionResult<Partido>> GetByNumero(int numero)
        {
            var partido = await _service.ObtenerPorNumeroAsync(numero);
            return Ok(partido);
        }

        // GET /api/partido/stats
        [HttpGet("stats")]
        public async Task<ActionResult> Stats()
        {
            var total = await _service.ContarAsync();
            return Ok(new { total });
        }

        // POST /api/partido
        [HttpPost]
        public async Task<ActionResult<Partido>> Post([FromBody] Partido partido)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creado = await _service.CrearAsync(partido);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        // POST /api/partido/seed  (carga masiva sin borrar)
        [HttpPost("seed")]
        public async Task<ActionResult> Seed([FromBody] List<Partido> partidos)
        {
            if (partidos == null || partidos.Count == 0)
                return BadRequest(new { mensaje = "La lista no puede estar vacía." });

            var total = await _service.CargarSeedAsync(partidos, limpiarAntes: false);
            return StatusCode(201, new
            {
                insertados = total,
                mensaje = $"Seed completado. {total} partidos cargados en MongoDB."
            });
        }

        // POST /api/partido/seed/reset  (limpia y recarga)
        [HttpPost("seed/reset")]
        public async Task<ActionResult> SeedReset([FromBody] List<Partido> partidos)
        {
            if (partidos == null || partidos.Count == 0)
                return BadRequest(new { mensaje = "La lista no puede estar vacía." });

            var total = await _service.CargarSeedAsync(partidos, limpiarAntes: true);
            return StatusCode(201, new
            {
                insertados = total,
                mensaje = $"Colección reiniciada. {total} partidos cargados en MongoDB."
            });
        }

        // PUT /api/partido/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] PartidoUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.ActualizarAsync(id, dto);
            return NoContent();
        }

        // DELETE /api/partido/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }
    }
}