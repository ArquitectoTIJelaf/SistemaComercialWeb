using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class PaseRepository
    {
        #region Métodos No Transaccionales

        public static decimal ValidarSaldoPaseCortesia(string CodiSocio, string Mes, string Anno)
        {
            var valor = new decimal();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarSaldoPaseCortesia";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Mes", DbType.String, ParameterDirection.Input, Mes);
                db.AddParameter("@Anno", DbType.String, ParameterDirection.Input, Anno);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetDecimalValue(drlector, "saldo");
                        break;
                    }
                }
            }

            return valor;
        }

        #endregion

        #region Métodos Transaccionales

        public static bool ModificarSaldoPaseCortesia(string CodiSocio, string Mes, string Anno)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarSaldoPaseCortesia";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Mes", DbType.String, ParameterDirection.Input, Mes);
                db.AddParameter("@Anno", DbType.String, ParameterDirection.Input, Anno);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool GrabarPaseSocio(int IdVenta, string CodiGerente, string CodiSocio, string Observacion)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarPaseSocio";
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@Codi_Gerente", DbType.String, ParameterDirection.Input, CodiGerente);
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Observacion", DbType.String, ParameterDirection.Input, Observacion);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        #endregion
    }
}
