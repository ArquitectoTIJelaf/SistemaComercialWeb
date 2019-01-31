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
                var response = UsuarioRepository.ValidaUsuario(CodiUsuario, Password);
                return new Response<UsuarioEntity>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(UsuarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<UsuarioEntity>(false, null, Message.MsgErrExcBusqUsuario, false);
            }
        }
    }
}
