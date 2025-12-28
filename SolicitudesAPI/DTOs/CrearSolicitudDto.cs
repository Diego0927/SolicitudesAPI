using System.ComponentModel.DataAnnotations;

namespace SolicitudesAPI.DTOs
{
    public class CrearSolicitudDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El tipo de solicitud es obligatorio")]
        public string TipoSolicitud { get; set; }

        public string Descripcion { get; set; }
    }
}