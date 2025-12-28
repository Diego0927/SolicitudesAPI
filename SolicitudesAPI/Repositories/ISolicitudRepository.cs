using SolicitudesAPI.Models;

namespace SolicitudesAPI.Repositories
{
    public interface ISolicitudRepository
    {
        Task<int> CrearAsync(Solicitud solicitud);
        Task<Solicitud> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Solicitud>> ObtenerTodasAsync(string estado, DateTime? inicio, DateTime? fin);
        Task<bool> ActualizarAsync(Solicitud solicitud);
        Task<bool> CancelarLogicoAsync(int id, DateTime fechaCancelacion);
    }
}