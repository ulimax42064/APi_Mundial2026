using System.ComponentModel.DataAnnotations;

namespace TupApi.DTOs
{
    public class PartidoUpdateDto
    {
        [StringLength(60, MinimumLength = 2)]
        public string? Equipo1 { get; set; }

        [StringLength(60, MinimumLength = 2)]
        public string? Equipo2 { get; set; }

        public string? Flags1 { get; set; }
        public string? Flags2 { get; set; }

        public string? Fecha { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string? Estadio { get; set; }

        [StringLength(100)]
        public string? Ciudad { get; set; }

        [Range(0, 10000)]
        public decimal? Precio { get; set; }

        public bool? Definido { get; set; }
    }
}