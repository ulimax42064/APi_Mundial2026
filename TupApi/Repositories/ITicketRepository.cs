using TupApi.Models;

namespace TupApi.Repositories
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetByUsuarioAsync(string email);
        Task<List<Ticket>> GetByPartidoAsync(string partidoId);
        Task<Ticket?> GetByIdAsync(string id);
        Task CreateAsync(Ticket ticket);
        Task<bool> DeleteAsync(string id);
    }
}