using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class ServicioLogic
    {
        public static ResListaServicio ListarTodos()
        {
            try
            {
                var response = ServicioRepository.ListarTodos();
                return new ResListaServicio(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(OficinaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaServicio(false, null, Message.MsgErrExcListServicio);
            }
        }
    }
}
