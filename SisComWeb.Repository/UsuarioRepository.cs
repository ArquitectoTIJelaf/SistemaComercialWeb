﻿using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class UsuarioRepository
    {
        #region Métodos No Transaccionales

        public static Response<UsuarioEntity> ValidaUsuario(short CodiUsuario, string Password)
        {
            var response = new Response<UsuarioEntity>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarUsuario";
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, CodiUsuario);
                db.AddParameter("@Pws", DbType.String, ParameterDirection.Input, Password);
                var entidad = new UsuarioEntity();
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
                    response.Valor = entidad;
                    response.Mensaje = "Se encontró correctamente al usuario. ";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion
    }
}
