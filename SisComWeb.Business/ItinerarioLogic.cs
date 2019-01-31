using SisComWeb.Entity;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class ItinerarioLogic
    {
        public static Response<ItinerarioEntity> BuscaItinerarios(ItinerarioEntity entidad)
        {
            try
            {
                var response = new Response<ItinerarioEntity>(false, null, "", false);

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ItinerarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ItinerarioEntity>(false, null, Message.MsgErrExcListItinerario, false);
            }
        }
    }
}
