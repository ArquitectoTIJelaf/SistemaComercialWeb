using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ItinerarioRepository
    {
        #region Métodos No Transaccionales

        public static Response<List<ItinerarioEntity>> BuscarItinerarios(short CodiOrigen, short CodiDestino, short CodiRuta, string Hora)
        {
            var response = new Response<List<ItinerarioEntity>>(false, null, "Error: BuscarItinerarios.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarItinerarios";
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Codi_Ruta", DbType.Int16, ParameterDirection.Input, CodiRuta);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                var Lista = new List<ItinerarioEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new ItinerarioEntity
                        {
                            NroViaje = Reader.GetIntValue(drlector, "NRO_VIAJE"),
                            CodiEmpresa = Reader.GetTinyIntValue(drlector, "CODI_EMPRESA"),
                            RazonSocial = Reader.GetStringValue(drlector, "Razon_Social"),
                            NroRuta = Reader.GetIntValue(drlector, "NRO_RUTA"),
                            CodiSucursal = Reader.GetSmallIntValue(drlector, "CODI_SUCURSAL"),
                            NomSucursal = Reader.GetStringValue(drlector, "Nom_Sucursal"),
                            CodiRuta = Reader.GetTinyIntValue(drlector, "Codi_Ruta"),
                            NomRuta = Reader.GetStringValue(drlector, "Nom_Ruta"),
                            CodiServicio = Reader.GetTinyIntValue(drlector, "CODI_SERVICIO"),
                            NomServicio = Reader.GetStringValue(drlector, "Nom_Servicio"),
                            CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_PuntoVenta"),
                            NomPuntoVenta = Reader.GetStringValue(drlector, "Nom_PuntoVenta"),
                            HoraProgramacion = Reader.GetStringValue(drlector, "Hora_Programacion"),
                            StOpcional = Reader.GetStringValue(drlector, "st_opcional"),
                            CodiOrigen = Reader.GetSmallIntValue(drlector, "Codi_Origen"),
                            NomOrigen = Reader.GetStringValue(drlector, "Nom_Origen"),
                            CodiDestino = Reader.GetSmallIntValue(drlector, "Codi_Destino"),
                            NomDestino = Reader.GetStringValue(drlector, "Nom_Destino"),
                            HoraPartida = Reader.GetStringValue(drlector, "Hora_Partida"),
                            NroRutaInt = Reader.GetIntValue(drlector, "NRO_RUTA_INT"),
                            Dias = Reader.GetSmallIntValue(drlector, "DIAS")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: BuscarItinerarios.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<TurnoViajeEntity> VerificaCambiosTurnoViaje(int NroViaje, string FechaProgramacion)
        {
            var response = new Response<TurnoViajeEntity>(false, null, "Error: VerificaCambiosTurnoViaje.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_VerificaCambiosTurnoViaje";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@FechaProgramacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                var entidad = new TurnoViajeEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.CodiServicio = Reader.GetTinyIntValue(drlector, "Codi_Servicio");
                        entidad.NomServicio = Reader.GetStringValue(drlector, "Servicio");
                        entidad.CodiEmpresa = Reader.GetTinyIntValue(drlector, "Codi_Empresa");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: VerificaCambiosTurnoViaje.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<int> BuscarProgramacionViaje(int NroViaje, string FechaProgramacion)
        {
            var response = new Response<int>(false, 0, "Error: BuscarProgramacionViaje.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarProgramacionViaje";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                var valor = new int();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "codi_programacion");
                    }
                    response.EsCorrecto = true;
                    response.Valor = valor;
                    response.Mensaje = "Correcto: BuscarProgramacionViaje.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<BusEntity> ObtenerBusEstandar(byte CodiEmpresa, short CodiSucursal, short CodiRuta, short CodiServicio, string Hora)
        {
            var response = new Response<BusEntity>(false, null, "Error: ObtenerBusEstandar.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerBusEstandar";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiRuta);
                db.AddParameter("@Codi_Servicio", DbType.Int16, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                var entidad = new BusEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.CodiBus = Reader.GetStringValue(drlector, "Codi_bus");
                        entidad.PlanBus = Reader.GetStringValue(drlector, "Plan_bus");
                        entidad.NumePasajeros = Reader.GetStringValue(drlector, "Nume_Pasajeros");
                        entidad.PlacBus = Reader.GetStringValue(drlector, "Plac_bus");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: ObtenerBusEstandar.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<BusEntity> ObtenerBusProgramacion(int CodiProgramacion)
        {
            var response = new Response<BusEntity>(false, null, "Error: ObtenerBusProgramacion.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerBusProgramacion";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                var entidad = new BusEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.CodiBus = Reader.GetStringValue(drlector, "Codi_bus");
                        entidad.PlanBus = Reader.GetStringValue(drlector, "Plan_bus");
                        entidad.NumePasajeros = Reader.GetStringValue(drlector, "Nume_Pasajeros");
                        entidad.PlacBus = Reader.GetStringValue(drlector, "Plac_bus");
                        entidad.CodiChofer = Reader.GetStringValue(drlector, "Codi_Chofer");
                        entidad.NombreChofer = Reader.GetStringValue(drlector, "Nombre_Chofer");
                        entidad.CodiCopiloto = Reader.GetStringValue(drlector, "Codi_Copiloto");
                        entidad.NombreCopiloto = Reader.GetStringValue(drlector, "Nombre_Copiloto");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: ObtenerBusProgramacion.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<int> ValidarTurnoAdicional(int NroViaje, string FechaProgramacion)
        {
            var response = new Response<int>(false, 0, "Error: ValidarTurnoAdicional.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarTurnoAdicional";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                var valor = new int();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = 1;
                        break;
                    }
                    response.EsCorrecto = true;
                    response.Valor = valor;
                    response.Mensaje = "Correcto: ValidarTurnoAdicional.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<int> ValidarProgrmacionCerrada(int NroViaje, string FechaProgramacion)
        {
            var response = new Response<int>(false, 0, "Error: ValidarProgrmacionCerrada.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarProgrmacionCerrada";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                var valor = new int();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = 1;
                        break;
                    }
                    response.EsCorrecto = true;
                    response.Valor = valor;
                    response.Mensaje = "Correcto: ValidarProgrmacionCerrada.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<int> ObtenerTotalVentas(int CodiProgramacion, short CodiOrigen, short CodiDestino)
        {
            var response = new Response<int>(false, 0, "Error: ObtenerTotalVentas.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerTotalVentas";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Codi_Origen", DbType.Int32, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int32, ParameterDirection.Input, CodiDestino);
                var valor = new int();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "CantidadVenta");
                    }
                    response.EsCorrecto = true;
                    response.Valor = valor;
                    response.Mensaje = "Correcto: ObtenerTotalVentas.";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion
    }
}
