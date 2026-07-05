namespace TupApi.DTOs
{
    public class PartidoFiltroDto
    {
        public string? Fase { get; set; }
        public string? Grupo { get; set; }
        public string? Equipo { get; set; }
        public bool? Definido { get; set; }

        private int _pagina = 1;
        public int Pagina
        {
            get => _pagina;
            set => _pagina = value < 1 ? 1 : value;
        }

        private int _porPagina = 20;
        public int PorPagina
        {
            get => _porPagina;
            set => _porPagina = (value < 1 || value > 200) ? 20 : value;
        }
    }
}