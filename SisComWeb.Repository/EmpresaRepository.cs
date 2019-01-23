using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class EmpresaRepository
    {
        #region Métodos No Transaccionales
        public static Response<List<EmpresaEntity>> ListarTodos()
        {
            var response = new Response<List<EmpresaEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarEmpresas";
                var Lista = new List<EmpresaEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new EmpresaEntity
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
