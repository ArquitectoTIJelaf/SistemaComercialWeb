using SisComWeb.Entity;
using System.Collections.Generic;
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
                    response.Estado = true;
                    response.Valor = entidad;
                }
            }
            return response;
        }

        #endregion
    }
}
