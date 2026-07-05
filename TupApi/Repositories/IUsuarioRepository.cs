using TupApi.Models;

namespace TupApi.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> EmailExisteAsync(string email);
        Task CreateAsync(Usuario usuario);
    }
}
