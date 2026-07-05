using TupApi.DTOs;
using TupApi.Models;
using TupApi.Repositories;

namespace TupApi.Services
{
    public class PartidoService
    {
        private readonly IPartidoRepository _repo;

        public PartidoService(IPartidoRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaginadoDto<Partido>> ObtenerTodosAsync(PartidoFiltroDto filtro)
        {
            var (datos, total) = await _repo.GetAllAsync(filtro);
            return new PaginadoDto<Partido>
            {
                Pagina = filtro.Pagina,
                PorPagina = filtro.PorPagina,
                Total = total,
                Datos = datos
            };
        }

        public async Task<Partido> ObtenerPorIdAsync(string id)
        {
            var partido = await _repo.GetByIdAsync(id);
            if (partido == null)
                throw new KeyNotFoundException($"No se encontró el partido con ID '{id}'.");
            return partido;
        }

        public async Task<Partido> ObtenerPorNumeroAsync(int numero)
        {
            if (numero < 1 || numero > 104)
                throw new ArgumentOutOfRangeException(nameof(numero),
                    "El número de partido debe estar entre 1 y 104.");

            var partido = await _repo.GetByNumeroAsync(numero);
            if (partido == null)
                throw new KeyNotFoundException($"No se encontró el partido número {numero}.");
            return partido;
        }

        public async Task<Partido> CrearAsync(Partido partido)
        {
            partido.Equipo1 = partido.Equipo1.Trim();
            partido.Equipo2 = partido.Equipo2.Trim();
            partido.Estadio = partido.Estadio.Trim();
            partido.Ciudad  = partido.Ciudad.Trim();
            partido.Pais    = partido.Pais.Trim();
            partido.Fase    = partido.Fase.Trim();

            await _repo.CreateAsync(partido);
            return partido;
        }

        public async Task<int> CargarSeedAsync(List<Partido> partidos, bool limpiarAntes)
        {
            if (partidos == null || partidos.Count == 0)
                throw new ArgumentException("La lista de partidos no puede estar vacía.");

            if (limpiarAntes)
                await _repo.DeleteAllAsync();

            await _repo.CreateManyAsync(partidos);
            return partidos.Count;
        }

        public async Task ActualizarAsync(string id, PartidoUpdateDto dto)
        {
            var existe = await _repo.GetByIdAsync(id);
            if (existe == null)
                throw new KeyNotFoundException($"No se encontró el partido con ID '{id}'.");

            if (dto.Equipo1 != null) dto.Equipo1 = dto.Equipo1.Trim();
            if (dto.Equipo2 != null) dto.Equipo2 = dto.Equipo2.Trim();
            if (dto.Estadio != null) dto.Estadio = dto.Estadio.Trim();

            var actualizado = await _repo.UpdateAsync(id, dto);
            if (!actualizado)
                throw new InvalidOperationException("No se pudo actualizar el partido.");
        }

        public async Task EliminarAsync(string id)
        {
            var eliminado = await _repo.DeleteAsync(id);
            if (!eliminado)
                throw new KeyNotFoundException($"No se encontró el partido con ID '{id}'.");
        }

        public async Task<long> ContarAsync() => await _repo.CountAsync();
    }
}