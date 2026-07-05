using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TupApi.Models
{
    public class Partido
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "El número de partido es obligatorio.")]
        [Range(1, 104, ErrorMessage = "El número de partido debe estar entre 1 y 104.")]
        public int NumeroPartido { get; set; }

        [Required(ErrorMessage = "La fase es obligatoria.")]
        [StringLength(50, MinimumLength = 3)]
        public string Fase { get; set; } = null!;

        public string? Grupo { get; set; }

        [Required(ErrorMessage = "El equipo local es obligatorio.")]
        [StringLength(60, MinimumLength = 2)]
        public string Equipo1 { get; set; } = null!;

        [Required(ErrorMessage = "El equipo visitante es obligatorio.")]
        [StringLength(60, MinimumLength = 2)]
        public string Equipo2 { get; set; } = null!;

        public string Flags1 { get; set; } = "";
        public string Flags2 { get; set; } = "";

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public string Fecha { get; set; } = null!;

        [Required(ErrorMessage = "El estadio es obligatorio.")]
        [StringLength(100, MinimumLength = 3)]
        public string Estadio { get; set; } = null!;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string Ciudad { get; set; } = null!;

        [Required(ErrorMessage = "El país es obligatorio.")]
        public string Pais { get; set; } = null!;

        [Range(0, 10000, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; } = 0;

        public bool Definido { get; set; } = true;

        public List<int>? DependeDePartidos { get; set; }
    }
}