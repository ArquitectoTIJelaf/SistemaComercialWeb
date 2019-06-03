using System.Data;

namespace SisComWeb.Repository
{
    public static class BloqueoAsientoRepository
    {
        #region Métodos No Transaccionales

        public static int ValidarBloqueoAsiento(int CodiProgramacion, int NroViaje, short CodiOrigen, short CodiDestino, string NumeAsiento, string FechaProgramacion)
        {
            var valor = new int();

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
                        valor = 1;
                        break;
                    }
                }
            }

            return valor;
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

        #endregion
    }
}
