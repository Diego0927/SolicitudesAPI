using SolicitudesAPI.DTOs;

namespace SolicitudesAPI.Services
{
    public interface ISolicitudService
    {
        Task<SolicitudDto> CrearSolicitudAsync(CrearSolicitudDto dto);
        Task<IEnumerable<SolicitudDto>> ListarSolicitudesAsync(string estado, DateTime? inicio, DateTime? fin);
        Task<SolicitudDto> ObtenerDetalleAsync(int id);
        Task<bool> ActualizarSolicitudAsync(int id, ActualizarSolicitudDto dto);
        Task<bool> CancelarSolicitudAsync(int id);
    }
}