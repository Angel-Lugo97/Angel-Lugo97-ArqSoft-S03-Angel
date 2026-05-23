namespace CatalogoApp.Domain.Models
{
    public class Resena
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public int Calificacion { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
