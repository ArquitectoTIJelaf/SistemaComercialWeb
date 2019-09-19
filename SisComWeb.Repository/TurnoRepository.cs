using SisComWeb.Entity;
using SisComWeb.Utility;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class TurnoRepository
    {
        #region Métodos No Transaccionales

        public static ItinerarioEntity BuscarTurno(TurnoRequest request)
        {
            var entidad = new ItinerarioEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarTurno";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, request.CodiEmpresa);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, request.CodiPuntoVenta);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, request.CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, request.CodiDestino);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, request.CodiSucursal);
                db.AddParameter("@Codi_Ruta", DbType.Int16, ParameterDirection.Input, request.CodiRuta);
                db.AddParameter("@Codi_Servicio", DbType.Byte, ParameterDirection.Input, request.CodiServicio);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, request.HoraViaje);

                db.AddParameter("@FechaViaje", DbType.String, ParameterDirection.Input, request.FechaViaje);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.NroViaje = Reader.GetIntValue(drlector, "NRO_VIAJE");
                        entidad.CodiEmpresa = Reader.GetByteValue(drlector, "CODI_EMPRESA");
                        entidad.RazonSocial = Reader.GetStringValue(drlector, "Razon_Social");
                        entidad.NroRuta = Reader.GetIntValue(drlector, "NRO_RUTA");
                        entidad.CodiSucursal = Reader.GetSmallIntValue(drlector, "CODI_SUCURSAL");
                        entidad.NomSucursal = Reader.GetStringValue(drlector, "Nom_Sucursal");
                        entidad.CodiRuta = Reader.GetSmallIntValue(drlector, "Codi_Ruta");
                        entidad.NomRuta = Reader.GetStringValue(drlector, "Nom_Ruta");
                        entidad.CodiServicio = Reader.GetByteValue(drlector, "CODI_SERVICIO");
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
                        entidad.DescServicio = Reader.GetStringValue(drlector, "DESC_SERVICIO");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static List<PuntoEntity> ListarPuntosEmbarque(short CodiOrigen, short CodiDestino, byte CodiServicio, byte CodiEmpresa, short CodiPuntoVenta, string Hora)
        {
            var Lista = new List<PuntoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarPuntosEmbarque";
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Codi_Servicio", DbType.Int16, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
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
                }
            }

            return Lista;
        }

        public static List<PuntoEntity> ListarPuntosArribo(short CodiOrigen, short CodiDestino, byte CodiServicio, byte CodiEmpresa, short CodiPuntoVenta, string Hora)
        {
            var Lista = new List<PuntoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarPuntosArribo";
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Codi_Servicio", DbType.Int16, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
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
                }
            }

            return Lista;
        }

        public static List<DestinoRutaEntity> ListaDestinosRuta(int NroViaje, short CodiSucursal)
        {
            var Lista = new List<DestinoRutaEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaDestinosRuta";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
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
                }
            }
            return Lista;
        }

        public static ManifiestoEntity ConsultaManifiestoProgramacion(int prog, string Suc)
        {
            var entidad = new ManifiestoEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_TB_manifiesto_programacion_consulta";
                db.AddParameter("@prog", DbType.Int32, ParameterDirection.Input, prog);
                db.AddParameter("@Suc", DbType.String, ParameterDirection.Input, Suc);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.NumeManifiesto = Reader.GetStringValue(drlector, "Nume_Manifiesto");
                        entidad.Est = Reader.GetStringValue(drlector, "est");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static string ConsultaPosCNT(string CodTab, string CodEmp)
        {
            var valor = "-1";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Tabla_Consulta_Pos_CNT";
                db.AddParameter("@COD_TAB", DbType.String, ParameterDirection.Input, CodTab);
                db.AddParameter("@COD_EMP", DbType.String, ParameterDirection.Input, CodEmp);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "NOM_TIP") ?? "0";
                        valor = string.IsNullOrEmpty(valor) ? "0" : valor;
                        break;
                    }
                }
            }

            return valor;
        }

        public static int ConsultaAnulacionPorDia(int Pv, string F)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Anulacion_por_Dia_Consulta";
                db.AddParameter("@pv", DbType.Int16, ParameterDirection.Input, Pv);
                db.AddParameter("@f", DbType.String, ParameterDirection.Input, F);

                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "cnt");
                        break;
                    }
                }
            }

            return valor;
        }

        public static short ConsultaBloqueoAsientoCantidad_Max(byte CodiEmpresa)
        {
            var valor = new short();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Consulta_Tb_BloqueAsiento_Cantidad_Max";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);

                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetSmallIntValue(drlector, "Cantidad");
                        break;
                    }
                }
            }

            return valor;
        }

        public static int BuscarNroViaje(byte CodiEmpresa, short CodiOrigenPas, short CodiOrigenBus, short CodiPuntoVentaBus, short CodiDestinoBus, string Turno, byte CodiServicio)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_programacion_Nro_Viaje";
                db.AddParameter("@CodiOrigenPas", DbType.Int16, ParameterDirection.Input, CodiOrigenPas);
                db.AddParameter("@CodiDestinoBus", DbType.Int16, ParameterDirection.Input, CodiDestinoBus);
                db.AddParameter("@CodiServicio", DbType.Byte, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Turno", DbType.String, ParameterDirection.Input, Turno);
                db.AddParameter("@CodiOrigenBus", DbType.Int16, ParameterDirection.Input, CodiOrigenBus);
                db.AddParameter("@CodiEmpresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@CodiPuntoVentaBus", DbType.Int16, ParameterDirection.Input, CodiPuntoVentaBus);

                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "NRO_VIAJE");
                        break;
                    }
                }
            }

            return valor;
        }

        #endregion

        #region Métodos Transaccionales

        public static bool EliminarReservas02(string Sucursal, int Programacion, string horaS, string fecha)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Venta_Delete_Reservas";
                db.AddParameter("@sucursal", DbType.String, ParameterDirection.Input, Sucursal);
                db.AddParameter("@programacion", DbType.Int32, ParameterDirection.Input, Programacion);
                db.AddParameter("@hora_S", DbType.String, ParameterDirection.Input, horaS);
                db.AddParameter("@fecha", DbType.String, ParameterDirection.Input, fecha);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool EliminarReservas01(int CodiProgramacion, string HoraProgramacion)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_DeleteReservas";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@hora_programacion", DbType.String, ParameterDirection.Input, HoraProgramacion);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static void ActualizarTbViajeProgramacionCantidad(int CodiProgramacion)
        {
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_tb_viaje_programacion_Update_Cnt";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);

                db.Execute();
            }
        }

        #endregion
    }
}
