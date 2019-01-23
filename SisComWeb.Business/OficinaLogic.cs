using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class OficinaLogic
    {
        public static ResListaOficina ListarTodos()
        {
            try
            {
                var response = OficinaRepository.ListarTodos();
                return new ResListaOficina(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(OficinaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaOficina(false, null, Message.MsgErrExcListOficina);
            }
        }
    }
}
