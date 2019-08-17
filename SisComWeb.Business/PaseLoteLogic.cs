using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class PaseLoteLogic
    {
        public static Response<bool> UpdatePostergacion(UpdatePostergacionRequest request)
        {
            try
            {
                var res = PaseLoteRepository.UpdatePostergacion(request);
                FechaAbiertaRepository.VentaDerivadaUpdateViaje(request.IdVenta, request.FechaViaje, request.HoraViaje, request.CodiServicio);
                return new Response<bool>(true, res, string.Empty, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaIgv, false);
            }
        }
        
        public static Response<bool> UpdateProgramacion(int CodiProgramacion, int IdVenta)
        {
            try
            {
                var res = PaseLoteRepository.UpdateProgramacion(CodiProgramacion, IdVenta);
                return new Response<bool>(true, res, string.Empty, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaIgv, false);
            }
        }
    }
}
