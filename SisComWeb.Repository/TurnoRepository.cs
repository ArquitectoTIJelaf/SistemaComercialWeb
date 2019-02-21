using SisComWeb.Entity;
using SisComWeb.Utility;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class TurnoRepository
    {
        #region Métodos No Transaccionales

        public static Response<ItinerarioEntity> BuscarTurno(byte CodiEmpresa, short CodiPuntoVenta, short CodiOrigen, short CodiDestino, short CodiSucursal, short CodiRuta, short CodiServicio, string Hora)
        {
            var response = new Response<ItinerarioEntity>(false, null, "", false);
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
                var entidad = new ItinerarioEntity();
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
                        entidad.StOpcional = Reader.GetStringValue(drlector, "st_opcional");
                        entidad.CodiOrigen = Reader.GetSmallIntValue(drlector, "Codi_Origen");
                        entidad.NomOrigen = Reader.GetStringValue(drlector, "Nom_Origen");
                        entidad.CodiDestino = Reader.GetSmallIntValue(drlector, "Codi_Destino");
                        entidad.NomDestino = Reader.GetStringValue(drlector, "Nom_Destino");
                        entidad.HoraPartida = Reader.GetStringValue(drlector, "Hora_Partida");
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

        public static Response<List<PuntoEntity>> ListarPuntosEmbarque(short CodiOrigen, short CodiDestino, byte CodiServicio, byte CodiEmpresa, short CodiPuntoVenta, string Hora)
        {
            var response = new Response<List<PuntoEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarPuntosEmbarque";
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Codi_Servicio", DbType.Int16, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                var Lista = new List<PuntoEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PuntoEntity
                        {
                            CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_puntoVenta"),
                            Lugar = Reader.GetStringValue(drlector, "Embarque"),
                            Hora = Reader.GetStringValue(drlector, "Hora_Embarque")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListarPuntosEmbarque.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<PuntoEntity>> ListarPuntosArribo(short CodiOrigen, short CodiDestino, byte CodiServicio, byte CodiEmpresa, short CodiPuntoVenta, string Hora)
        {
            var response = new Response<List<PuntoEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarPuntosArribo";
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Codi_Servicio", DbType.Int16, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                var Lista = new List<PuntoEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PuntoEntity
                        {
                            CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_puntoVenta"),
                            Lugar = Reader.GetStringValue(drlector, "Arribo"),
                            Hora = Reader.GetStringValue(drlector, "Hora_Arribo")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListarPuntosArribo.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<DestinoRutaEntity>> ListaDestinosRuta(int NroViaje, short CodiSucursal)
        {
            var response = new Response<List<DestinoRutaEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaDestinosRuta";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                var Lista = new List<DestinoRutaEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new DestinoRutaEntity
                        {
                            CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal"),
                            NomOficina = Reader.GetStringValue(drlector, "Nom_Oficina"),
                            Sigla = Reader.GetStringValue(drlector, "Sigla"),
                            Color = DataUtility.ObtenerColorHexadecimal(Reader.GetStringValue(drlector, "Color"))
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListarPuntosArribo.";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion
    }
}
