using SisComWeb.Entity.Peticiones.Request;
using System.Data;

namespace SisComWeb.Repository
{
    public class PaseLoteRepository
    {        
        public static bool UpdatePostergacion(UpdatePostergacionRequest filtro)
        {
            bool Response = false;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Venta_Update_Postergacion_Ele";
                db.AddParameter("@Numero_reint", DbType.String, ParameterDirection.Input, filtro.NumeroReintegro);
                db.AddParameter("@programacion", DbType.String, ParameterDirection.Input, filtro.CodiProgramacion);
                db.AddParameter("@origen", DbType.String, ParameterDirection.Input, filtro.Origen);
                db.AddParameter("@id_Venta", DbType.Int32, ParameterDirection.Input, filtro.IdVenta);
                db.AddParameter("@asiento", DbType.String, ParameterDirection.Input, filtro.NumeAsiento);
                db.AddParameter("@ruta", DbType.String, ParameterDirection.Input, filtro.Ruta);
                db.AddParameter("@servicio", DbType.String, ParameterDirection.Input, filtro.CodiServicio);
                db.AddParameter("@TipoDoc", DbType.String, ParameterDirection.Input, filtro.TipoDoc);
                db.Execute();
                Response = true;
            }
            return Response;
        }

        public static bool UpdateProgramacion(int CodiProgramacion, int IdVenta)
        {
            bool Response = false;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_venta_Update_Prog";
                db.AddParameter("@p", DbType.String, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@id", DbType.String, ParameterDirection.Input, IdVenta);
                db.Execute();
                Response = true;
            }
            return Response;
        }
    }    
}
