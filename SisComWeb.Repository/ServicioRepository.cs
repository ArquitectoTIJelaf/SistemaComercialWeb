using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ServicioRepository
    {
        #region Métodos No Transaccionales

        public static Response<List<ServicioEntity>> ListarTodos()
        {
            var response = new Response<List<ServicioEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarServicios";
                var Lista = new List<ServicioEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new ServicioEntity
                        {
                            CodiServicio = Reader.GetTinyIntValue(drlector, "Codi_Servicio"),
                            Descripcion = Reader.GetStringValue(drlector, "Descripcion")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Estado = true;
                    response.Valor = Lista;
                }
            }
            return response;
        }

        #endregion
    }
}
