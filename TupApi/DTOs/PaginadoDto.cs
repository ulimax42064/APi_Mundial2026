namespace TupApi.DTOs
{
    public class PaginadoDto<T>
    {
        public int Pagina { get; set; }
        public int PorPagina { get; set; }
        public long Total { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)Total / PorPagina);
        public List<T> Datos { get; set; } = new();
    }
}