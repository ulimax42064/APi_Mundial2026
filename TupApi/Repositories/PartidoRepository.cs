using MongoDB.Driver;
using Microsoft.Extensions.Options;
using TupApi.Config;
using TupApi.DTOs;
using TupApi.Models;

namespace TupApi.Repositories
{
    public class PartidoRepository : IPartidoRepository
    {
        private readonly IMongoCollection<Partido> _col;

        public PartidoRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _col = db.GetCollection<Partido>("Partidos");
        }

        public async Task<(List<Partido> datos, long total)> GetAllAsync(PartidoFiltroDto filtro)
        {
            var builder = Builders<Partido>.Filter;
            var filtros = new List<FilterDefinition<Partido>>();

            if (!string.IsNullOrWhiteSpace(filtro.Fase))
                filtros.Add(builder.Regex(p => p.Fase,
                    new MongoDB.Bson.BsonRegularExpression($"^{filtro.Fase}$", "i")));

            if (!string.IsNullOrWhiteSpace(filtro.Grupo))
                filtros.Add(builder.Regex(p => p.Grupo,
                    new MongoDB.Bson.BsonRegularExpression(filtro.Grupo, "i")));

            if (!string.IsNullOrWhiteSpace(filtro.Equipo))
            {
                var regex = new MongoDB.Bson.BsonRegularExpression(filtro.Equipo, "i");
                filtros.Add(builder.Or(
                    builder.Regex(p => p.Equipo1, regex),
                    builder.Regex(p => p.Equipo2, regex)
                ));
            }

            if (filtro.Definido.HasValue)
                filtros.Add(builder.Eq(p => p.Definido, filtro.Definido.Value));

            var filtroFinal = filtros.Count > 0 ? builder.And(filtros) : builder.Empty;

            var total = await _col.CountDocumentsAsync(filtroFinal);

            var skip = (filtro.Pagina - 1) * filtro.PorPagina;
            var datos = await _col
                .Find(filtroFinal)
                .SortBy(p => p.NumeroPartido)
                .Skip(skip)
                .Limit(filtro.PorPagina)
                .ToListAsync();

            return (datos, total);
        }

        public async Task<Partido?> GetByIdAsync(string id) =>
            await _col.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<Partido?> GetByNumeroAsync(int numero) =>
            await _col.Find(p => p.NumeroPartido == numero).FirstOrDefaultAsync();

        public async Task CreateAsync(Partido partido) =>
            await _col.InsertOneAsync(partido);

        public async Task CreateManyAsync(List<Partido> partidos) =>
            await _col.InsertManyAsync(partidos);

        public async Task<bool> UpdateAsync(string id, PartidoUpdateDto dto)
        {
            var updates = new List<UpdateDefinition<Partido>>();
            var upd = Builders<Partido>.Update;

            if (dto.Equipo1 != null)   updates.Add(upd.Set(p => p.Equipo1, dto.Equipo1.Trim()));
            if (dto.Equipo2 != null)   updates.Add(upd.Set(p => p.Equipo2, dto.Equipo2.Trim()));
            if (dto.Flags1 != null)    updates.Add(upd.Set(p => p.Flags1, dto.Flags1));
            if (dto.Flags2 != null)    updates.Add(upd.Set(p => p.Flags2, dto.Flags2));
            if (dto.Fecha != null)     updates.Add(upd.Set(p => p.Fecha, dto.Fecha));
            if (dto.Estadio != null)   updates.Add(upd.Set(p => p.Estadio, dto.Estadio.Trim()));
            if (dto.Ciudad != null)    updates.Add(upd.Set(p => p.Ciudad, dto.Ciudad.Trim()));
            if (dto.Precio.HasValue)   updates.Add(upd.Set(p => p.Precio, dto.Precio.Value));
            if (dto.Definido.HasValue) updates.Add(upd.Set(p => p.Definido, dto.Definido.Value));

            if (updates.Count == 0) return false;

            var result = await _col.UpdateOneAsync(
                p => p.Id == id,
                upd.Combine(updates)
            );
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _col.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task DeleteAllAsync() =>
            await _col.DeleteManyAsync(_ => true);

        public async Task<long> CountAsync() =>
            await _col.CountDocumentsAsync(_ => true);
    }
}