using SeguridadJelaf;
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
                var validaUsuario = UsuarioRepository.ValidaUsuario(CodiUsuario);

                Seguridad seguridad = new Seguridad();
                var desencriptaPassword = seguridad.Desencripta(validaUsuario.Password, Constantes.UnaLlave);

                if (validaUsuario.CodiUsuario > 0 && (Password == desencriptaPassword || Password == validaUsuario.Password))
                    return new Response<UsuarioEntity>(true, validaUsuario, Message.MsgCorrectoValidaUsuario, true);
                else
                    return new Response<UsuarioEntity>(false, validaUsuario, Message.MsgErrorValidaUsuario, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(UsuarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<UsuarioEntity>(false, null, Message.MsgExcValidaUsuario, false);
            }
        }
    }
}
