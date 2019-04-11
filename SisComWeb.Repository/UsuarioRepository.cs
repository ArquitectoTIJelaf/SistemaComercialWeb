using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class UsuarioRepository
    {
        #region Métodos No Transaccionales

        public static UsuarioEntity ValidaUsuario(short CodiUsuario)
        {
            var entidad = new UsuarioEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarUsuario";
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, CodiUsuario);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.CodiUsuario = Reader.GetSmallIntValue(drlector, "Codi_Usuario");
                        entidad.Login = Reader.GetStringValue(drlector, "Login");
                        entidad.CodiEmpresa = Reader.GetTinyIntValue(drlector, "Codi_Empresa");
                        entidad.CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal");
                        entidad.CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_puntoVenta");
                        entidad.Password = Reader.GetStringValue(drlector, "Pws");
                        entidad.Nivel = Reader.GetTinyIntValue(drlector, "Nivel");
                        entidad.NomSucursal = Reader.GetStringValue(drlector, "Nom_Sucursal");
                        entidad.NomPuntoVenta = Reader.GetStringValue(drlector, "Nom_PuntoVenta");
                        entidad.Terminal = Reader.GetIntValue(drlector, "Terminal");
                    }
                }
            }
            return entidad;
        }

        #endregion
    }
}
