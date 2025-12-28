using Microsoft.AspNetCore.Mvc;
using SolicitudesAPI.DTOs;
using SolicitudesAPI.Services;

namespace SolicitudesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly ISolicitudService _service;

        public SolicitudesController(ISolicitudService service)
        {
            _service = service;
        }

        // POST /api/solicitudes
        [HttpPost]
        public async Task<ActionResult<SolicitudDto>> Crear([FromBody] CrearSolicitudDto dto)
        {
            try
            {
                var resultado = await _service.CrearSolicitudAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Id }, resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // GET /api/solicitudes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudDto>>> Listar(
            [FromQuery] string estado,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var solicitudes = await _service.ListarSolicitudesAsync(estado, fechaInicio, fechaFin);
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // GET /api/solicitudes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudDto>> ObtenerPorId(int id)
        {
            try
            {
                var solicitud = await _service.ObtenerDetalleAsync(id);
                return Ok(solicitud);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // PUT /api/solicitudes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarSolicitudDto dto)
        {
            try
            {
                var exito = await _service.ActualizarSolicitudAsync(id, dto);
                if (!exito)
                    return BadRequest(new { mensaje = "No se pudo actualizar la solicitud" });

                return Ok(new { mensaje = "Solicitud actualizada exitosamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // DELETE /api/solicitudes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                var exito = await _service.CancelarSolicitudAsync(id);
                if (!exito)
                    return BadRequest(new { mensaje = "No se pudo procesar la cancelación" });

                return Ok(new { mensaje = "Solicitud cancelada exitosamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }
    }
}