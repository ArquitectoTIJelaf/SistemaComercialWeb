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
    public static class OficinaPasajesLogic
    {
        public static ResListaOficina ListarTodos()
        {
            try
            {
                var response = OficinaPasajesRepository.ListarTodos();
                return new ResListaOficina(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(OficinaPasajesLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaOficina(false, null, Message.MsgErrExcListOficinaPasaje);
            }
        }
    }
}
