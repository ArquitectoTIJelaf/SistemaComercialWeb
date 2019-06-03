using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public class ClavesInternasRepository
    {
        #region Métodos No Transaccionales

        public static ClavesInternasEntity ClavesInternas(int Codi_Oficina, string Password, string Codi_Tipo)
        {
            var entidad = new ClavesInternasEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarClaveInterna";
                db.AddParameter("@Codi_Oficina", DbType.Int32, ParameterDirection.Input, Codi_Oficina);
                db.AddParameter("@Password", DbType.String, ParameterDirection.Input, Password);
                db.AddParameter("@Codi_Tipo", DbType.Int32, ParameterDirection.Input, Codi_Tipo);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Codigo = Reader.GetIntValue(drlector, "codigo");
                        entidad.Oficina = Reader.GetIntValue(drlector, "oficina");
                        entidad.Pwd = Reader.GetStringValue(drlector, "pwd");
                        entidad.CodTipo = Reader.GetStringValue(drlector, "cod_tipo");
                        break;
                    }
                }
            }

            return entidad;
        }

        #endregion
    }
}
