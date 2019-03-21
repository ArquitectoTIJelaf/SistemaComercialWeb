using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public class ClavesInternasRepository
    {
        #region Métodos No Transaccionales

        public static Response<ClavesInternasEntity> ClavesInternas(int Codi_Oficina, string Password, string Codi_Tipo)
        {
            var response = new Response<ClavesInternasEntity>(false, null, "Error: ClavesInternas", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarClaveInterna";
                db.AddParameter("@Codi_Oficina", DbType.Int32, ParameterDirection.Input, Codi_Oficina);
                db.AddParameter("@Password", DbType.String, ParameterDirection.Input, Password);
                db.AddParameter("@Codi_Tipo", DbType.Int32, ParameterDirection.Input, Codi_Tipo);
                var entidad = new ClavesInternasEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.codigo = Reader.GetIntValue(drlector, "codigo");
                        entidad.oficina = Reader.GetIntValue(drlector, "oficina");
                        entidad.pwd = Reader.GetStringValue(drlector, "pwd");
                        entidad.cod_tipo = Reader.GetStringValue(drlector, "cod_tipo");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: ClavesInternas";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion
    }
}
