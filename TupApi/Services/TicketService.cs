using TupApi.Models;
using TupApi.Repositories;

namespace TupApi.Services
{
    public class TicketService
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IPartidoRepository _partidoRepo;

        public TicketService(ITicketRepository ticketRepo, IPartidoRepository partidoRepo)
        {
            _ticketRepo = ticketRepo;
            _partidoRepo = partidoRepo;
        }

        public async Task<List<Ticket>> ObtenerPorUsuarioAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.");
            return await _ticketRepo.GetByUsuarioAsync(email.Trim().ToLower());
        }

        public async Task<List<Ticket>> ObtenerPorPartidoAsync(string partidoId) =>
            await _ticketRepo.GetByPartidoAsync(partidoId);

        public async Task<Ticket> ComprarAsync(Ticket ticket)
        {
            var partido = await _partidoRepo.GetByIdAsync(ticket.PartidoId);
            if (partido == null)
                throw new KeyNotFoundException(
                    $"El partido con ID '{ticket.PartidoId}' no existe.");

            if (!partido.Definido)
                throw new InvalidOperationException(
                    "No se pueden comprar entradas para partidos cuyos equipos aún no están definidos.");

            if (ticket.Precio == 0)
                ticket.Precio = ticket.Sector switch
                {
                    "VIP"    => partido.Precio * 3m,
                    "Platea" => partido.Precio * 1.5m,
                    _        => partido.Precio
                };

            ticket.NombreUsuario  = ticket.NombreUsuario.Trim();
            ticket.EmailComprador = ticket.EmailComprador.Trim().ToLower();
            ticket.NumeroPartido  = partido.NumeroPartido;
            ticket.FechaCompra    = DateTime.UtcNow;

            await _ticketRepo.CreateAsync(ticket);
            return ticket;
        }

        public async Task CancelarAsync(string id)
        {
            var eliminado = await _ticketRepo.DeleteAsync(id);
            if (!eliminado)
                throw new KeyNotFoundException($"No se encontró el ticket con ID '{id}'.");
        }
    }
}