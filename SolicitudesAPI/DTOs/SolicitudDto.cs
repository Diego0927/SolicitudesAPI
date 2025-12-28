namespace SolicitudesAPI.DTOs
{
    public class SolicitudDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoSolicitud { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}