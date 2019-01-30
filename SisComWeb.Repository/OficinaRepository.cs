using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class OficinaRepository
    {
        #region Métodos No Transaccionales

        public static Response<List<OficinaEntity>> ListarTodos()
        {
            var response = new Response<List<OficinaEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarSucursales";
                var Lista = new List<OficinaEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new OficinaEntity
                        {
                            CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal"),
                            Descripcion = Reader.GetStringValue(drlector, "Descripcion")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Se encontró correctamente las oficinas. ";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion
    }
}
