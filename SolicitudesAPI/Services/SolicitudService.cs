using SolicitudesAPI.DTOs;
using SolicitudesAPI.Models;
using SolicitudesAPI.Repositories;

namespace SolicitudesAPI.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly ISolicitudRepository _repository;

        public SolicitudService(ISolicitudRepository repository)
        {
            _repository = repository;
        }

        public async Task<SolicitudDto> CrearSolicitudAsync(CrearSolicitudDto dto)
        {
            var solicitud = new Solicitud
            {
                NombreUsuario = dto.NombreUsuario,
                TipoSolicitud = dto.TipoSolicitud,
                Descripcion = dto.Descripcion,
                Estado = "ACTIVA",
                FechaCreacion = DateTime.Now
            };

            var id = await _repository.CrearAsync(solicitud);
            solicitud.Id = id;

            return MapearADto(solicitud);
        }

        public async Task<IEnumerable<SolicitudDto>> ListarSolicitudesAsync(string estado, DateTime? inicio, DateTime? fin)
        {
            var solicitudes = await _repository.ObtenerTodasAsync(estado, inicio, fin);
            return solicitudes.Select(MapearADto);
        }

        public async Task<SolicitudDto> ObtenerDetalleAsync(int id)
        {
            var solicitud = await _repository.ObtenerPorIdAsync(id);
            if (solicitud == null)
            {
                throw new KeyNotFoundException($"La solicitud con ID {id} no existe.");
            }

            return MapearADto(solicitud);
        }

        public async Task<bool> ActualizarSolicitudAsync(int id, ActualizarSolicitudDto dto)
        {
            var solicitudExistente = await _repository.ObtenerPorIdAsync(id);
            if (solicitudExistente == null)
            {
                throw new KeyNotFoundException($"La solicitud con ID {id} no existe.");
            }

            if (solicitudExistente.Estado != "ACTIVA")
            {
                throw new InvalidOperationException("Solo se pueden actualizar solicitudes activas");
            }

            solicitudExistente.Descripcion = dto.Descripcion;
            return await _repository.ActualizarAsync(solicitudExistente);
        }

        public async Task<bool> CancelarSolicitudAsync(int id)
        {
            var solicitudExistente = await _repository.ObtenerPorIdAsync(id);
            if (solicitudExistente == null)
            {
                throw new KeyNotFoundException($"La solicitud con ID {id} no existe.");
            }

            if (solicitudExistente.Estado == "CANCELADA")
            {
                throw new InvalidOperationException("La solicitud ya se encuentra cancelada");
            }

            return await _repository.CancelarLogicoAsync(id, DateTime.Now);
        }

        private SolicitudDto MapearADto(Solicitud solicitud)
        {
            return new SolicitudDto
            {
                Id = solicitud.Id,
                NombreUsuario = solicitud.NombreUsuario,
                TipoSolicitud = solicitud.TipoSolicitud,
                Descripcion = solicitud.Descripcion,
                Estado = solicitud.Estado,
                FechaCreacion = solicitud.FechaCreacion
            };
        }
    }
}
