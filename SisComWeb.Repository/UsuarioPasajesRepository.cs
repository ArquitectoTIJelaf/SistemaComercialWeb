using SisComWeb.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Repository
{
    public static class UsuarioPasajesRepository
    {
        #region Métodos no transaccionales

        public static Response<UsuarioPasajesEntity> ValidaUsuario(short CodiUsuario, string Password)
        {
            var response = new Response<UsuarioPasajesEntity>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarUsuario";
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, CodiUsuario);
                db.AddParameter("@Pws", DbType.String, ParameterDirection.Input, Password);
                var entidad = new UsuarioPasajesEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.CodiUsuario = Reader.GetSmallIntValue(drlector, "Codi_Usuario");
                        entidad.CodiEmpresa = Reader.GetTinyIntValue(drlector, "Codi_Empresa");
                        entidad.CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal");
                        entidad.CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_puntoVenta");
                        entidad.Password = Reader.GetStringValue(drlector, "Pws");
                        entidad.Nivel = Reader.GetTinyIntValue(drlector, "Nivel");
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
