﻿using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class BloqueoAsientoLogic
    {
        public static Response<int> BloqueoAsiento(BloqueoAsientoRequest request)
        {
            try
            {
                var response = new Response<int>(false, 0, "", false);

                // Validar 'BloqueoAsiento'
                var resValidarBloqueoAsiento = BloqueoAsientoRepository.ValidarBloqueoAsiento(request.CodiProgramacion, request.NroViaje, request.CodiOrigen, request.CodiDestino, request.NumeAsiento.ToString(), request.FechaProgramacion);
                if (resValidarBloqueoAsiento.Estado)
                    response.Mensaje += resValidarBloqueoAsiento.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscarPlanoBus. ";
                    return response;
                }

                if (resValidarBloqueoAsiento.Valor == 1)
                {
                    response.EsCorrecto = true;
                    response.Valor = 0;
                    response.Estado = true;

                    return response;
                }
                else
                {
                    // ¿Existe Programación?
                    if (request.CodiProgramacion != 0)
                    {
                        // Bloquear 'AsientoProgramacion'
                        var resBuscarPlanoBus = BloqueoAsientoRepository.BloquearAsientoProgramacion(request.CodiProgramacion, request.NumeAsiento.ToString(), decimal.Parse(request.Precio.ToString()), request.FechaProgramacion, request.CodiTerminal.ToString());
                        if (resBuscarPlanoBus.Estado)
                            response.Mensaje += resBuscarPlanoBus.Mensaje;
                        else
                        {
                            response.Mensaje += "Error: BloquearAsientoProgramacion. ";
                            return response;
                        }
                    }
                    else
                    {
                        // Bloquear 'AsientoViaje'
                        var resBloquearAsientoViaje = BloqueoAsientoRepository.BloquearAsientoViaje(request.NroViaje, request.NumeAsiento.ToString(), decimal.Parse(request.Precio.ToString()), request.FechaProgramacion, request.CodiTerminal.ToString());
                        if (resBloquearAsientoViaje.Estado)
                            response.Mensaje += resBloquearAsientoViaje.Mensaje;
                        else
                        {
                            response.Mensaje += "Error: BloquearAsientoViaje. ";
                            return response;
                        }
                    }
                }

                response.EsCorrecto = true;
                //response.Valor = resBuscarPlanoBus.Valor;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BloqueoAsientoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgErrExcBusqAsientoBloqueado, false);
            }
        }
    }
}