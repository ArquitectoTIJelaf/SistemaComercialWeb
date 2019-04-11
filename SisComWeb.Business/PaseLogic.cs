using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class PaseLogic
    {
        public static Response<int> ValidarSaldoPaseCortesia(string CodiSocio, string Mes, string Anno)
        {
            try
            {
                var validarSaldoPaseCortesia = PaseRepository.ValidarSaldoPaseCortesia(CodiSocio, Mes, Anno);

                return new Response<int>(true, validarSaldoPaseCortesia, Message.MsgCorrectoValidarSaldoPaseCortesia, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcValidarSaldoPaseCortesia, false);
            }
        }
    }
}
