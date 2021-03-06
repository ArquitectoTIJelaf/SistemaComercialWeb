﻿using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class BloqueoAsientoRepository
    {
        #region Métodos No Transaccionales

        public static bool ValidarBloqueoAsiento(int CodiProgramacion, int NroViaje, short CodiOrigen, short CodiDestino, string NumeAsiento, string FechaProgramacion)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarBloqueoAsiento";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Nume_Asiento", DbType.String, ParameterDirection.Input, NumeAsiento);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetBooleanValue(drlector, "Validator");
                        break;
                    }
                }
            }

            return valor;
        }

        public static TablaAsientosBloqueadosEntity ConsultarTablaAsientosBloqueados(byte CodiEmpresa, short CodiSucursal, short CodiRuta, byte CodiServicio, string Turno)
        {
            var entidad = new TablaAsientosBloqueadosEntity()
            {
                Asientos = string.Empty
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_AsientosBloqueados_Consulta";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int32, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_Ruta", DbType.Int32, ParameterDirection.Input, CodiRuta);
                db.AddParameter("@Codi_Servicio", DbType.Byte, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Turno", DbType.String, ParameterDirection.Input, Turno);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Asientos = Reader.GetStringValue(drlector, "Asientos");
                        entidad.CodiOrigen = Reader.GetSmallIntValue(drlector, "cod_OrigenP");
                        entidad.CodiDestino = Reader.GetSmallIntValue(drlector, "Cod_DestinoP");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static TablaBloqueoAsientosEntity ConsultarTablaBloqueoAsientos(int CodiProgramacion, string Tipo, string FechaProgramacion)
        {
            TablaBloqueoAsientosEntity entidad = null;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_TB_bloqueo_asientos_Consulta";
                db.AddParameter("@Prog", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@T", DbType.String, ParameterDirection.Input, Tipo);
                db.AddParameter("@f", DbType.String, ParameterDirection.Input, FechaProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad = new TablaBloqueoAsientosEntity
                        {
                            CodiOrigen = Reader.GetSmallIntValue(drlector, "CODI_ORIGEN"),
                            AsientosOcupados = Reader.GetStringValue(drlector, "asientos_ocupados"),
                            CodiDestino = Reader.GetSmallIntValue(drlector, "Codi_Destino"),
                            AsientosLiberados = Reader.GetStringValue(drlector, "asientos_liberados")
                        };
                        break;
                    }
                }
            }

            return entidad;
        }

        #endregion

        #region Métodos Transaccionales

        public static decimal BloquearAsientoProgramacion(int CodiProgramacion, string NumeAsiento, decimal Costo, string FechaProgramacion, string CodiTerminal)
        {
            var valor = new decimal();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BloquearAsientobyProgramacion";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Nume_Asiento", DbType.String, ParameterDirection.Input, NumeAsiento);
                db.AddParameter("@Costo", DbType.Decimal, ParameterDirection.Input, Costo);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                db.AddParameter("@Codi_Terminal", DbType.String, ParameterDirection.Input, CodiTerminal);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetDecimalValue(drlector, "Resultado");
                        break;
                    }
                }
            }

            return valor;
        }

        public static decimal BloquearAsientoViaje(int NroViaje, string NumeAsiento, decimal Costo, string FechaProgramacion, string CodiTerminal)
        {
            var valor = new decimal();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BloquearAsientobyViaje";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Nume_Asiento", DbType.String, ParameterDirection.Input, NumeAsiento);
                db.AddParameter("@Costo", DbType.Decimal, ParameterDirection.Input, Costo);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                db.AddParameter("@Codi_Terminal", DbType.String, ParameterDirection.Input, CodiTerminal);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetDecimalValue(drlector, "Resultado");
                        break;
                    }
                }
            }

            return valor;
        }

        public static bool LiberaAsiento(int IDS)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_LiberarAsiento";
                db.AddParameter("@Ids", DbType.Int32, ParameterDirection.Input, IDS);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool InsertarTablaBloqueoAsientos(TablaBloqueoAsientosRequest request)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_bloqueo_asientos_Insert";
                db.AddParameter("@codi_programacion", DbType.Int32, ParameterDirection.Input, request.CodiProgramacion);
                db.AddParameter("@codi_origen", DbType.Int32, ParameterDirection.Input, request.CodiOrigen);
                db.AddParameter("@codi_destino", DbType.Int32, ParameterDirection.Input, request.CodiDestino);
                db.AddParameter("@asientos_ocupados", DbType.String, ParameterDirection.Input, request.AsientosOcupados);
                db.AddParameter("@asientos_liberados", DbType.String, ParameterDirection.Input, request.AsientosLiberados);
                db.AddParameter("@tipo", DbType.String, ParameterDirection.Input, request.Tipo);
                db.AddParameter("@fecha", DbType.String, ParameterDirection.Input, request.Fecha);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ActualizarTablaBloqueoAsientos(string CodiProgramcion, string NroViaje, string Fecha)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_bloqueo_asientos_update_t";
                db.AddParameter("@p", DbType.String, ParameterDirection.Input, CodiProgramcion);
                db.AddParameter("@xp", DbType.String, ParameterDirection.Input, NroViaje);
                db.AddParameter("@f", DbType.String, ParameterDirection.Input, Fecha);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ActualizarAsiOcuTbBloqueoAsientos(TablaBloqueoAsientosRequest request)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_bloqueo_asientos_Up_N_As";
                db.AddParameter("@asi1", DbType.String, ParameterDirection.Input, request.AsientosOcupados);
                db.AddParameter("@asi", DbType.String, ParameterDirection.Input, request.AsientosLiberados);
                db.AddParameter("@prog", DbType.Int32, ParameterDirection.Input, request.CodiProgramacion);
                db.AddParameter("@tip", DbType.String, ParameterDirection.Input, request.Tipo);
                db.AddParameter("@cod", DbType.Int32, ParameterDirection.Input, request.CodiOrigen);
                db.AddParameter("@des", DbType.Int32, ParameterDirection.Input, request.CodiDestino);
                db.AddParameter("@fecha", DbType.String, ParameterDirection.Input, request.Fecha);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool LiberaArregloAsientos(string Xml)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_LiberaArregloAsientos";
                db.AddParameter("@Xml", DbType.Xml, ParameterDirection.Input, Xml);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = true;
                        break;
                    }
                }
            }

            return valor;
        }

        #endregion
    }
}
