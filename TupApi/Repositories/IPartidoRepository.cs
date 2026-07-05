using TupApi.DTOs;
using TupApi.Models;

namespace TupApi.Repositories
{
    public interface IPartidoRepository
    {
        Task<(List<Partido> datos, long total)> GetAllAsync(PartidoFiltroDto filtro);
        Task<Partido?> GetByIdAsync(string id);
        Task<Partido?> GetByNumeroAsync(int numero);
        Task CreateAsync(Partido partido);
        Task CreateManyAsync(List<Partido> partidos);
        Task<bool> UpdateAsync(string id, PartidoUpdateDto dto);
        Task<bool> DeleteAsync(string id);
        Task DeleteAllAsync();
        Task<long> CountAsync();
    }
}