using MongoDB.Driver;
using Microsoft.Extensions.Options;
using TupApi.Config;
using TupApi.Models;

namespace TupApi.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _col;

        public UsuarioRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _col = db.GetCollection<Usuario>("Usuarios");
        }

        public async Task<Usuario?> GetByEmailAsync(string email) =>
            await _col.Find(u => u.Email == email.ToLower()).FirstOrDefaultAsync();

        public async Task<bool> EmailExisteAsync(string email) =>
            await _col.Find(u => u.Email == email.ToLower()).AnyAsync();

        public async Task CreateAsync(Usuario usuario) =>
            await _col.InsertOneAsync(usuario);
    }
}
