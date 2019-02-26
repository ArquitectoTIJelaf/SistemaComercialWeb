﻿using SeguridadJelaf;
using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class UsuarioLogic
    {
        public static Response<UsuarioEntity> ValidaUsuario(short CodiUsuario, string Password)
        {
            try
            {
                var response = UsuarioRepository.ValidaUsuario(CodiUsuario);
                if (!response.Estado)
                    return response;

                Seguridad seguridad = new Seguridad();
                var desencriptaPassword = seguridad.Desencripta(response.Valor.Password, Constantes.UnaLlave);

                if (response.Valor.CodiUsuario != 0 && (Password == desencriptaPassword || Password == response.Valor.Password))
                    return new Response<UsuarioEntity>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
                else
                    return new Response<UsuarioEntity>(false, null, "Las credenciales ingresadas son inválidas.", false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(UsuarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<UsuarioEntity>(false, null, Message.MsgErrExcValidaUsuario, false);
            }
        }
    }
}
