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
    public class ServicioPasajesLogic
    {
        public static ResListaServicio ListarTodos()
        {
            try
            {
                var response = ServicioPasajesRepository.ListarTodos();
                return new ResListaServicio(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(OficinaPasajesLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaServicio(false, null, Message.MsgErrExcListServicioPasaje);
            }
        }
    }
}
