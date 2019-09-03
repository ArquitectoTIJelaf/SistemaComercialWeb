﻿using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class LiquidacionLogic
    {
        public static Response<LiquidacionEntity> ListaLiquidacion(LiquidacionRequest filtro)
        {
            try
            {
                //Borra data del temporal
                LiquidacionRepository.Borrar(filtro);
                //Puebla data con los filtros
                LiquidacionRepository.Poblar(filtro);
                //Obtiene Resultado
                var objeto = LiquidacionRepository.Data(filtro);
                objeto.Impresion = CuadreImpresora.Cuadre.WriteLiquidacion(objeto);

                return new Response<LiquidacionEntity>(true, objeto, string.Empty, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<LiquidacionEntity>(false, new LiquidacionEntity(), Message.MsgExcLiquidacion, false);
            }
        }
    }
}
