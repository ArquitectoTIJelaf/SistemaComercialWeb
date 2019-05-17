using SeguridadJelaf;
using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class CreditoLogic
    {
        //public static Response<bool> ListaClientes()
        //{
        //    try
        //    {
        //        //var validaUsuario = UsuarioRepository.ValidaUsuario(CodiUsuario);

        //        //Seguridad seguridad = new Seguridad();
        //        //var desencriptaPassword = seguridad.Desencripta(validaUsuario.Password, Constantes.UnaLlave);

        //        //if (validaUsuario.CodiUsuario > 0 && (Password == desencriptaPassword || Password == validaUsuario.Password))
        //        //    return new Response<UsuarioEntity>(true, validaUsuario, Message.MsgCorrectoValidaUsuario, true);
        //        //else
        //        //    return new Response<UsuarioEntity>(false, validaUsuario, Message.MsgErrorValidaUsuario, false);

        //        return new Response<bool>(true, true, Message.MsgExcValidaUsuario, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        return new Response<bool>(false, false, Message.MsgExcValidaUsuario, false);
        //    }
        //}
    }
}
