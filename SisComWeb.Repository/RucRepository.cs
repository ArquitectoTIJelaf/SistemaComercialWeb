using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class RucRepository
    {
        #region Métodos No Transaccionales

        public static Response<RucEntity> BuscarEmpresa(string RucCliente)
        {
            var response = new Response<RucEntity>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, RucCliente);
                var entidad = new RucEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.RucCliente = Reader.GetStringValue(drlector, "Ruc_Cliente");
                        entidad.RazonSocial = Reader.GetStringValue(drlector, "Razon_Social");
                        entidad.Direccion = Reader.GetStringValue(drlector, "Direccion");
                        entidad.Telefono = Reader.GetStringValue(drlector, "Telefono");
                    }

                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Se encontró correctamente la empresa. ";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion

        #region Métodos Transaccionales

        public static Response<bool> GrabarEmpresa(RucEntity entidad)
        {
            var response = new Response<bool>(false, false, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, entidad.RucCliente);
                db.AddParameter("@Razon_Social", DbType.String, ParameterDirection.Input, entidad.RazonSocial);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, entidad.Direccion);
                db.AddParameter("@Telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Se registró correctamente la empresa. ";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> ModificarEmpresa(RucEntity entidad)
        {
            var response = new Response<bool>(false, false, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, entidad.RucCliente);
                db.AddParameter("@Razon_Social", DbType.String, ParameterDirection.Input, entidad.RazonSocial);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, entidad.Direccion);
                db.AddParameter("@Telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Se modificó correctamente la empresa. ";
                response.Estado = true;
            }

            return response;
        }

        #endregion

    }
}
