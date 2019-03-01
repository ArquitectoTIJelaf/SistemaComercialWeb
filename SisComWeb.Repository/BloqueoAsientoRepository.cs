using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class BloqueoAsientoRepository
    {
        #region Métodos No Transaccionales

        public static Response<int> ValidarBloqueoAsiento(int CodiProgramacion, int NroViaje, short CodiOrigen, short CodiDestino, string NumeAsiento, string FechaProgramacion)
        {
            var response = new Response<int>(false, 0, "Error: ValidarBloqueoAsiento.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarBloqueoAsiento";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Nume_Asiento", DbType.String, ParameterDirection.Input, NumeAsiento);
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
                    response.Mensaje = "Correcto: ValidarBloqueoAsiento.";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion

        #region Métodos Transaccionales

        public static Response<decimal> BloquearAsientoProgramacion(int CodiProgramacion, string NumeAsiento, decimal Costo, string FechaProgramacion, string CodiTerminal)
        {
            var response = new Response<decimal>(false, 0, "Error: BloquearAsientoProgramacion.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BloquearAsientobyProgramacion";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Nume_Asiento", DbType.String, ParameterDirection.Input, NumeAsiento);
                db.AddParameter("@Costo", DbType.Decimal, ParameterDirection.Input, Costo);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                db.AddParameter("@Codi_Terminal", DbType.String, ParameterDirection.Input, CodiTerminal);
                var valor = new decimal();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetDecimalValue(drlector, "Resultado");
                        break;
                    }
                    response.EsCorrecto = true;
                    response.Valor = valor;
                    response.Mensaje = "Correcto: BloquearAsientoProgramacion.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<decimal> BloquearAsientoViaje(int NroViaje, string NumeAsiento, decimal Costo, string FechaProgramacion, string CodiTerminal)
        {
            var response = new Response<decimal>(false, 0, "Error: BloquearAsientoViaje.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BloquearAsientobyViaje";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Nume_Asiento", DbType.String, ParameterDirection.Input, NumeAsiento);
                db.AddParameter("@Costo", DbType.Decimal, ParameterDirection.Input, Costo);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                db.AddParameter("@Codi_Terminal", DbType.String, ParameterDirection.Input, CodiTerminal);
                var valor = new decimal();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetDecimalValue(drlector, "Resultado");
                        break;
                    }
                    response.EsCorrecto = true;
                    response.Valor = valor;
                    response.Mensaje = "Correcto: BloquearAsientoViaje.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<bool> LiberaAsiento(int IDS)
        {
            var response = new Response<bool>(false, false, "Error: LiberaAsiento.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_LiberarAsiento";
                db.AddParameter("@Ids", DbType.Int32, ParameterDirection.Input, IDS);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: LiberaAsiento.";
                response.Estado = true;
            }
            return response;
        }

        #endregion
    }
}
