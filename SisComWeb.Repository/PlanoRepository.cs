using SisComWeb.Entity;
using SisComWeb.Utility;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class PlanoRepository
    {
        #region Métodos No Transaccionales

        public static Response<List<PlanoEntity>> BuscarPlanoBus(string CodiPlano)
        {
            var response = new Response<List<PlanoEntity>>(false, null, "Error: BuscarPlanoBus.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarPlanoBus";
                db.AddParameter("@Codi_Plano", DbType.String, ParameterDirection.Input, CodiPlano);
                var Lista = new List<PlanoEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PlanoEntity
                        {
                            Codigo = Reader.GetStringValue(drlector, "Codigo"),
                            Tipo = Reader.GetStringValue(drlector, "Tipo"),
                            Indice = Reader.GetIntValue(drlector, "Indice")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: BuscarPlanoBus.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<string> ObtenerNivelAsiento(string CodiBus, int NumeAsiento)
        {
            var response = new Response<string>(false, null, "Error: ObtenerNivelAsiento.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerNivelAsiento";
                db.AddParameter("@Codi_Bus", DbType.String, ParameterDirection.Input, CodiBus);
                db.AddParameter("@Nume_Asiento", DbType.Int32, ParameterDirection.Input, NumeAsiento);
                var valor = string.Empty;
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "Nivel");
                    }
                    response.EsCorrecto = true;
                    response.Valor = valor;
                    response.Mensaje = "Correcto: ObtenerNivelAsiento.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<PlanoEntity> ObtenerPrecioAsiento(short CodiOrigen, short CodiDestino, string Hora, string Fecha, short CodiServicio, byte CodiEmpresa, string Nivel)
        {
            var response = new Response<PlanoEntity>(false, null, "Error: ObtenerPrecioAsiento.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerPrecioAsiento";
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                db.AddParameter("@Fecha", DbType.String, ParameterDirection.Input, Fecha);
                db.AddParameter("@Codi_Servicio", DbType.String, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Codi_Empresa", DbType.String, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Nivel", DbType.String, ParameterDirection.Input, Nivel);
                var entidad = new PlanoEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.PrecioNormal = Reader.GetRealValue(drlector, "Precio_Nor");
                        entidad.PrecioMinimo = Reader.GetRealValue(drlector, "Precio_Min");
                        entidad.PrecioMaximo = Reader.GetRealValue(drlector, "Precio_Max");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: ObtenerPrecioAsiento.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<PlanoEntity>> ListarAsientosOcupados(int CodiProgramacion, string FechaProgramacion, int NroViajem, short CodiOrigen, short CodiDestino)
        {
            var response = new Response<List<PlanoEntity>>(false, null, "Error: ListarAsientosOcupados.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarAsientosOcupados";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViajem);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                var Lista = new List<PlanoEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PlanoEntity
                        {
                            NumeAsiento = Reader.GetTinyIntValue(drlector, "NUME_ASIENTO"),
                            TipoDocumento = Reader.GetStringValue(drlector, "Tipo_Documento"),
                            NumeroDocumento = Reader.GetStringValue(drlector, "Numero_Documento"),
                            RucContacto = Reader.GetStringValue(drlector, "Ruc_Contacto"),
                            FechaViaje = Reader.GetStringValue(drlector, "Fecha_Viaje"),
                            FechaVenta = Reader.GetStringValue(drlector, "Fecha_Venta"),
                            Nacionalidad = Reader.GetStringValue(drlector, "Nacionalidad"),
                            PrecioVenta = Reader.GetRealValue(drlector, "Precio_Venta"),
                            RecogeEn = Reader.GetStringValue(drlector, "Recoge_En"),
                            Color = DataUtility.ObtenerColorHexadecimal(Reader.GetStringValue(drlector, "Color")),
                            FlagVenta = Reader.GetStringValue(drlector, "FLAG_VENTA"),
                            Sigla = Reader.GetStringValue(drlector, "Sigla")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListarAsientosOcupados.";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion
    }
}
