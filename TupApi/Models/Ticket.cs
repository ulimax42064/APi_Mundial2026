using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TupApi.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "El ID del partido es obligatorio.")]
        public string PartidoId { get; set; } = null!;

        [Range(1, 104)]
        public int NumeroPartido { get; set; }

        [Required(ErrorMessage = "El nombre del comprador es obligatorio.")]
        [StringLength(100, MinimumLength = 2)]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(150)]
        public string EmailComprador { get; set; } = null!;

        [Required(ErrorMessage = "El sector es obligatorio.")]
        [RegularExpression("^(VIP|General|Platea)$",
            ErrorMessage = "El sector debe ser VIP, General o Platea.")]
        public string Sector { get; set; } = null!;

        [Range(0, 10000)]
        public decimal Precio { get; set; }

        public DateTime FechaCompra { get; set; }
    }
}