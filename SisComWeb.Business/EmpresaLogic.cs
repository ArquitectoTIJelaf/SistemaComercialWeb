using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class EmpresaLogic
    {
        public static ResListaEmpresa ListarTodos()
        {
            try
            {
                var response = EmpresaRepository.ListarTodos();
                return new ResListaEmpresa(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(EmpresaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaEmpresa(false, null, Message.MsgErrExcListEmpresa, false);
            }
        }
    }
}
