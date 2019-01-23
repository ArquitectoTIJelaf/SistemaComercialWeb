using SisComWeb.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Repository
{
    public static class EmpresaPasajesRepository
    {
        #region Métodos No Transaccionales
        public static Response<List<EmpresaPasajesEntity>> ListarTodos()
        {
            var response = new Response<List<EmpresaPasajesEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarEmpresas";
                var Lista = new List<EmpresaPasajesEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new EmpresaPasajesEntity
                        {
                            CodiEmpresa = Reader.GetTinyIntValue(drlector, "Codi_Empresa"),
                            RazonSocial = Reader.GetStringValue(drlector, "Razon_Social"),
                            Ruc = Reader.GetStringValue(drlector, "Ruc"),
                            Direccion = Reader.GetStringValue(drlector, "DIRECCION")
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
