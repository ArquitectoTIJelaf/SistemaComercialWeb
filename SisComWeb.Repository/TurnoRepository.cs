using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class TurnoRepository
    {
        #region Métodos No Transaccionales

        public static Response<TurnoEntity> BuscarTurno(byte CodiEmpresa, short CodiPuntoVenta, short CodiOrigen, short CodiDestino, short CodiSucursal, short CodiRuta, short CodiServicio, string Hora)
        {
            var response = new Response<TurnoEntity>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarTurno";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_Ruta", DbType.Int16, ParameterDirection.Input, CodiRuta);
                db.AddParameter("@Codi_Servicio", DbType.Byte, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                var entidad = new TurnoEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.NroViaje = Reader.GetIntValue(drlector, "NRO_VIAJE");
                        entidad.CodiEmpresa = Reader.GetTinyIntValue(drlector, "CODI_EMPRESA");
                        entidad.RazonSocial = Reader.GetStringValue(drlector, "Razon_Social");
                        entidad.NroRuta = Reader.GetIntValue(drlector, "NRO_RUTA");
                        entidad.CodiSucursal = Reader.GetSmallIntValue(drlector, "CODI_SUCURSAL");
                        entidad.NomSucursal = Reader.GetStringValue(drlector, "Nom_Sucursal");
                        entidad.CodiRuta = Reader.GetSmallIntValue(drlector, "Codi_Ruta");
                        entidad.NomRuta = Reader.GetStringValue(drlector, "Nom_Ruta");
                        entidad.CodiServicio = Reader.GetTinyIntValue(drlector, "CODI_SERVICIO");
                        entidad.NomServicio = Reader.GetStringValue(drlector, "Nom_Servicio");
                        entidad.CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_PuntoVenta");
                        entidad.NomPuntoVenta = Reader.GetStringValue(drlector, "Nom_PuntoVenta");
                        entidad.HoraProgramacion = Reader.GetStringValue(drlector, "Hora_Programacion");
                        entidad.HoraPartida = Reader.GetStringValue(drlector, "Hora_Partida");
                        entidad.StOpcional = Reader.GetStringValue(drlector, "st_opcional");
                        entidad.CodiOrigen = Reader.GetSmallIntValue(drlector, "Codi_Origen");
                        entidad.NomOrigen = Reader.GetStringValue(drlector, "Nom_Origen");
                        entidad.CodiDestino = Reader.GetSmallIntValue(drlector, "Codi_Destino");
                        entidad.NomDestino = Reader.GetStringValue(drlector, "Nom_Destino");
                        entidad.NroRutaInt = Reader.GetIntValue(drlector, "NRO_RUTA_INT");
                        entidad.Dias = Reader.GetSmallIntValue(drlector, "DIAS");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: BuscarTurno. ";
                    response.Estado = true;
                }
            }
            return response;
        }
        
        #endregion
    }
}
