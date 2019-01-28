using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class UsuarioLogic
    {
        public static ResFiltroUsuario ValidaUsuario(short CodiUsuario, string Password)
        {
            try
            {
                var response = UsuarioRepository.ValidaUsuario(CodiUsuario, Password);
                return new ResFiltroUsuario(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(UsuarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroUsuario(false, null, Message.MsgErrExcBusqUsuario);
            }
        }
    }
}
