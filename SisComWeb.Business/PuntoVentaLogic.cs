using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class PuntoVentaLogic
    {
        public static ResListaPuntoVenta ListarTodos(string Codi_Sucursal)
        {
            try
            {
                var response = PuntoVentaRepository.ListarTodos(Convert.ToInt16(Codi_Sucursal));
                return new ResListaPuntoVenta(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PuntoVentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaPuntoVenta(false, null, Message.MsgErrExcListPuntoVenta, false);
            }
        }
    }
}
