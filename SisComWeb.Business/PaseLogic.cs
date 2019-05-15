using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class PaseLogic
    {
        public static Response<decimal> ValidarSaldoPaseCortesia(string CodiSocio, string Mes, string Anno)
        {
            try
            {
                var validarSaldoPaseCortesia = PaseRepository.ValidarSaldoPaseCortesia(CodiSocio, Mes, Anno);
                if(validarSaldoPaseCortesia > 0)
                    return new Response<decimal>(true, validarSaldoPaseCortesia, Message.MsgCorrectoValidarSaldoPaseCortesia, true);
                else
                    return new Response<decimal>(false, validarSaldoPaseCortesia, Message.MsgValidaSaldoPaseCortesia, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<decimal>(false, 0, Message.MsgExcValidarSaldoPaseCortesia, false);
            }
        }
    }
}
