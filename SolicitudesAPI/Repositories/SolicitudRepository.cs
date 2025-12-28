using Dapper;
using SolicitudesAPI.Models;
using System.Data;
using System.Text;

namespace SolicitudesAPI.Repositories
{
    public class SolicitudRepository : ISolicitudRepository
    {
        private readonly IDbConnection _dbConnection;

        public SolicitudRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CrearAsync(Solicitud solicitud)
        {
            var sql = @"INSERT INTO SOLICITUDES (NOMBRE_USUARIO, TIPO_SOLICITUD, DESCRIPCION, ESTADO, FECHA_CREACION) 
                        VALUES (:NombreUsuario, :TipoSolicitud, :Descripcion, 'ACTIVA', SYSDATE) 
                        RETURNING ID_SOLICITUD INTO :id";

            var parameters = new DynamicParameters();
            parameters.Add("NombreUsuario", solicitud.NombreUsuario);
            parameters.Add("TipoSolicitud", solicitud.TipoSolicitud);
            parameters.Add("Descripcion", solicitud.Descripcion);
            parameters.Add("id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _dbConnection.ExecuteAsync(sql, parameters);
            return parameters.Get<int>("id");
        }

        public async Task<Solicitud> ObtenerPorIdAsync(int id)
        {
            var sql = @"SELECT ID_SOLICITUD as Id, NOMBRE_USUARIO as NombreUsuario, 
                               TIPO_SOLICITUD as TipoSolicitud, DESCRIPCION, 
                               FECHA_CREACION as FechaCreacion, ESTADO, 
                               FECHA_CANCELACION as FechaCancelacion 
                        FROM SOLICITUDES WHERE ID_SOLICITUD = :id";

            return await _dbConnection.QueryFirstOrDefaultAsync<Solicitud>(sql, new { id });
        }

        public async Task<IEnumerable<Solicitud>> ObtenerTodasAsync(string estado, DateTime? inicio, DateTime? fin)
        {
            var sql = new StringBuilder(@"SELECT ID_SOLICITUD as Id, NOMBRE_USUARIO as NombreUsuario, 
                                                 TIPO_SOLICITUD as TipoSolicitud, DESCRIPCION, 
                                                 FECHA_CREACION as FechaCreacion, ESTADO, 
                                                 FECHA_CANCELACION as FechaCancelacion 
                                          FROM SOLICITUDES WHERE 1=1");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(estado))
            {
                sql.Append(" AND ESTADO = :estado");
                parameters.Add("estado", estado);
            }
            if (inicio.HasValue)
            {
                sql.Append(" AND FECHA_CREACION >= :inicio");
                parameters.Add("inicio", inicio.Value);
            }
            if (fin.HasValue)
            {
                sql.Append(" AND FECHA_CREACION <= :fin");
                parameters.Add("fin", fin.Value);
            }

            return await _dbConnection.QueryAsync<Solicitud>(sql.ToString(), parameters);
        }

        public async Task<bool> ActualizarAsync(Solicitud solicitud)
        {
            var sql = @"UPDATE SOLICITUDES 
                        SET DESCRIPCION = :Descripcion
                        WHERE ID_SOLICITUD = :Id AND ESTADO = 'ACTIVA'";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, solicitud);
            return rowsAffected > 0;
        }

        public async Task<bool> CancelarLogicoAsync(int id, DateTime fechaCancelacion)
        {
            var sql = @"UPDATE SOLICITUDES 
                        SET ESTADO = 'CANCELADA', FECHA_CANCELACION = :fechaCancelacion 
                        WHERE ID_SOLICITUD = :id";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { id, fechaCancelacion });
            return rowsAffected > 0;
        }
    }
}
