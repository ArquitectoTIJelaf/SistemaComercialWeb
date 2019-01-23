using SisComWeb.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Repository
{
    public static class OficinaPasajesRepository
    {
        #region Métodos No Transaccionales
        public static Response<List<OficinaPasajesEntity>> ListarTodos()
        {
            var response = new Response<List<OficinaPasajesEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarSucursales";
                var Lista = new List<OficinaPasajesEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new OficinaPasajesEntity
                        {
                            CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal"),
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
