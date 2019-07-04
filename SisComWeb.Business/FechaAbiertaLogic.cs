using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public class FechaAbiertaLogic
    {
        public static Response<List<FechaAbiertaEntity>> VentaConsultaF6(FechaAbiertaRequest request)
        {
            try
            {
                var result = FechaAbiertaRepository.VentaConsultaF6(request);

                return new Response<List<FechaAbiertaEntity>>(true, result, Message.MsgCorrectoVentaConsultaF6, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<FechaAbiertaEntity>>(false, null, Message.MsgExcVentaConsultaF6, false);
            }
        }

        public static Response<bool> ValidateNivelAsiento(int IdVenta, string CodiBus, string Asiento)
        {
            try
            {
                var asientoBoleto = FechaAbiertaRepository.NivelAsientoVentaDerivada(IdVenta);
                var asientoTarget = FechaAbiertaRepository.NivelDelAsiento(CodiBus, Asiento);

                var response = false;
                var mensaje = "";

                if (asientoBoleto == asientoTarget) {
                    response = true;
                } else
                {
                    response = false;
                    mensaje = "El asiento fue vendido en el nivel " + asientoBoleto;
                }

                return new Response<bool>(true, response, mensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcValidateNivelAsiento, false);
            }
        }
    }
}
