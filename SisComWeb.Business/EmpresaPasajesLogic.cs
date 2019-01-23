using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Business
{
    public class EmpresaPasajesLogic
    {
        public static ResListaEmpresa ListarTodos()
        {
            try
            {
                var response = EmpresaPasajesRepository.ListarTodos();
                return new ResListaEmpresa(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(EmpresaPasajesLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaEmpresa(false, null, Message.MsgErrExcListEmpresaPasaje);
            }
        }
    }
}
