using SisComWeb.Entity.Peticiones.Request;
using System.Data;

namespace SisComWeb.Repository
{
    public class CambiarTPagoRepository
    {
        public static string CambiarTipoPago(CambiarTPagoRequest filtro)
        {
            string response = "";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_CambiarTipoPago";
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, filtro.IdVenta);
                db.AddParameter("@NewTipoPago", DbType.String, ParameterDirection.Input, filtro.NewTipoPago);
                db.AddParameter("@OldTipoPago", DbType.String, ParameterDirection.Input, filtro.OldTipoPago);
                db.AddParameter("@cc", DbType.Decimal, ParameterDirection.Input, filtro.Credito);
                db.AddParameter("@emp", DbType.String, ParameterDirection.Input, filtro.CodiEmpresa);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        response = Reader.GetStringValue(drlector, "NUME_CAJA");
                    }
                }
            }
            return response;
        }
    }
}
