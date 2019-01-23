using SisComWeb.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Repository
{
    public static class ServicioPasajesRepository
    {
        #region Métodos No Transaccionales
        public static Response<List<ServicioPasajesEntity>> ListarTodos()
        {
            var response = new Response<List<ServicioPasajesEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarServicios";
                var Lista = new List<ServicioPasajesEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new ServicioPasajesEntity
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
