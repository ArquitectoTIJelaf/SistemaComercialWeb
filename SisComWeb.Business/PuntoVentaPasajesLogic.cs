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
    public class PuntoVentaPasajesLogic
    {
        public static ResListaPuntoVenta ListarTodos()
        {
            try
            {
                var response = PuntoVentaPasajesRepository.ListarTodos();
                return new ResListaPuntoVenta(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PuntoVentaPasajesLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaPuntoVenta(false, null, Message.MsgErrExcListPuntoVentaPasaje);
            }
        }
    }
}
