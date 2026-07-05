using System.ComponentModel.DataAnnotations;

namespace TupApi.DTOs
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(30)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; } = null!;
    }

    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(30, MinimumLength = 2)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(30)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; } = null!;
    }
}
