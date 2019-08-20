using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ItinerarioRepository
    {
        #region Métodos No Transaccionales

        public static List<ItinerarioEntity> BuscarItinerarios(short CodiOrigen, short CodiDestino, short CodiRuta, string Hora, short Servicio)
        {
            var Lista = new List<ItinerarioEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarItinerarios";
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Codi_Ruta", DbType.Int16, ParameterDirection.Input, CodiRuta);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                db.AddParameter("@Codi_Servicio", DbType.Int16, ParameterDirection.Input, Servicio);
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
                }
            }

            return Lista;
        }

        public static TurnoViajeEntity VerificaCambiosTurnoViaje(int NroViaje, string FechaProgramacion)
        {
            var entidad = new TurnoViajeEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_VerificaCambiosTurnoViaje";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@FechaProgramacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.CodiServicio = Reader.GetTinyIntValue(drlector, "Codi_Servicio");
                        entidad.NomServicio = Reader.GetStringValue(drlector, "Servicio");
                        entidad.CodiEmpresa = Reader.GetTinyIntValue(drlector, "Codi_Empresa");
                        entidad.NomEmpresa = Reader.GetStringValue(drlector, "Empresa");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static int BuscarProgramacionViaje(int NroViaje, string FechaProgramacion)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarProgramacionViaje";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "codi_programacion");
                        break;
                    }
                }
            }

            return valor;
        }

        public static BusEntity ObtenerBusEstandar(byte CodiEmpresa, short CodiSucursal, short CodiRuta, short CodiServicio, string Hora)
        {
            var entidad = new BusEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerBusEstandar";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiRuta);
                db.AddParameter("@Codi_Servicio", DbType.Int16, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.CodiBus = Reader.GetStringValue(drlector, "Codi_bus");
                        entidad.PlanBus = Reader.GetStringValue(drlector, "Plan_bus");
                        entidad.NumePasajeros = Reader.GetStringValue(drlector, "Nume_Pasajeros");
                        entidad.PlacBus = Reader.GetStringValue(drlector, "Plac_bus");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static BusEntity ObtenerBusProgramacion(int CodiProgramacion)
        {
            var entidad = new BusEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerBusProgramacion";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                
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
                        entidad.Activo = Reader.GetStringValue(drlector, "Activo");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static int ValidarTurnoAdicional(int NroViaje, string FechaProgramacion)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarTurnoAdicional";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = 1;
                        break;
                    }
                }
            }

            return valor;
        }

        public static int ValidarProgramacionCerrada(int NroViaje, string FechaProgramacion)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarProgrmacionCerrada";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = 1;
                        break;
                    }
                }
            }

            return valor;
        }

        public static int ObtenerTotalVentas(int CodiProgramacion, int NroViaje, short CodiOrigen, short CodiDestino)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerTotalVentas";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "CantidadVenta");
                        break;
                    }
                }
            }

            return valor;
        }

        #endregion
    }
}
