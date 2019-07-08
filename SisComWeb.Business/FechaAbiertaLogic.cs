﻿using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;

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

        public static Response<int> ValidateNumDias(string FechaVenta)
        {
            try
            {
                var CantidadPerimita = FechaAbiertaRepository.TablasPnpConsulta(Constantes.CodLimitFecha);

                var response = 0;
                var mensaje = "";

                //var DayFechaVenta = Convert.ToDateTime(FechaVenta);
                var DayFechaVenta = DateTime.ParseExact(FechaVenta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                int DiffDays = (DateTime.Now - DayFechaVenta).Days;

                if (CantidadPerimita > 0 && DiffDays >= CantidadPerimita)
                {
                    response = CantidadPerimita;
                    mensaje = string.Format("La diferencia de días es {0} y la cantidad de días permitidos es de {1}, ingrese clave de autorización", DiffDays, CantidadPerimita);
                }

                return new Response<int>(true, response, mensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcValidateNumDias, false);
            }
        }

        public static Response<int> VerificaNotaCredito(int IdVenta)
        {
            try
            {
                var Response = FechaAbiertaRepository.VerificaNotaCredito(IdVenta);
                var mensaje = (Response > 0) ? Message.MsgVerificaNotaCredito : string.Empty;
                return new Response<int>(true, Response, mensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcVerificaNotaCredito, false);
            }
        }
        
        public static Response<bool> VentaUpdatePostergacionEle(FechaAbiertaRequest filtro)
        {
            try
            {
                var Response = FechaAbiertaRepository.VentaUpdatePostergacionEle(filtro);
                FechaAbiertaRepository.VentaUpdateImpManifiesto(filtro.IdVenta);
                FechaAbiertaRepository.VentaDerivadaUpdateViaje(filtro.IdVenta, filtro.FechaViaje, filtro.HoraViaje, filtro.CodiServicio);
                FechaAbiertaRepository.VentaUpdateCnt(0, Convert.ToInt32(filtro.CodiProgramacion), filtro.Oficina, filtro.Oficina);
                return new Response<bool>(true, Response, Message.MsgCorrectoVentaUpdatePostergacionEle, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcVentaUpdatePostergacionEle, false);
            }
        }
    }
}
