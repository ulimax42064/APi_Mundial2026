using System.Security.Cryptography;
using System.Text;
using TupApi.DTOs;
using TupApi.Models;
using TupApi.Repositories;

namespace TupApi.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<Usuario> RegistrarAsync(UsuarioRegistroDto dto)
        {
            dto.Email  = dto.Email.Trim().ToLower();
            dto.Nombre = dto.Nombre.Trim();

            if (await _repo.EmailExisteAsync(dto.Email))
                throw new ArgumentException("El email ya está registrado.");

            var usuario = new Usuario
            {
                Nombre        = dto.Nombre,
                Email         = dto.Email,
                PasswordHash  = HashPassword(dto.Password),
                FechaRegistro = DateTime.UtcNow
            };

            await _repo.CreateAsync(usuario);
            return usuario;
        }

        public async Task<Usuario> LoginAsync(UsuarioLoginDto dto)
        {
            dto.Email = dto.Email.Trim().ToLower();

            var usuario = await _repo.GetByEmailAsync(dto.Email);
            if (usuario == null)
                throw new UnauthorizedAccessException("Email o contraseña incorrectos.");

            if (usuario.PasswordHash != HashPassword(dto.Password))
                throw new UnauthorizedAccessException("Email o contraseña incorrectos.");

            return usuario;
        }

        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
