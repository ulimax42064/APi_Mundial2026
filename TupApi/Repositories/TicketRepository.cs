using MongoDB.Driver;
using Microsoft.Extensions.Options;
using TupApi.Config;
using TupApi.Models;

namespace TupApi.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _col;

        public TicketRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _col = db.GetCollection<Ticket>("Tickets");
        }

        public async Task<List<Ticket>> GetByUsuarioAsync(string email) =>
            await _col.Find(t => t.EmailComprador == email).ToListAsync();

        public async Task<List<Ticket>> GetByPartidoAsync(string partidoId) =>
            await _col.Find(t => t.PartidoId == partidoId).ToListAsync();

        public async Task<Ticket?> GetByIdAsync(string id) =>
            await _col.Find(t => t.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Ticket ticket) =>
            await _col.InsertOneAsync(ticket);

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _col.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }
    }
}