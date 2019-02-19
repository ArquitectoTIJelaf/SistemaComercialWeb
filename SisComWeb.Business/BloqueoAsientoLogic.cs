using SisComWeb.Entity;
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
                Response<decimal> resBloquearAsiento;

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
                        resBloquearAsiento = BloqueoAsientoRepository.BloquearAsientoProgramacion(request.CodiProgramacion, request.NumeAsiento.ToString(), decimal.Parse(request.Precio.ToString()), request.FechaProgramacion, request.CodiTerminal.ToString());
                        if (resBloquearAsiento.Estado)
                            response.Mensaje += resBloquearAsiento.Mensaje;
                        else
                        {
                            response.Mensaje += "Error: BloquearAsientoProgramacion. ";
                            return response;
                        }
                    }
                    else
                    {
                        // Bloquear 'AsientoViaje'
                        resBloquearAsiento = BloqueoAsientoRepository.BloquearAsientoViaje(request.NroViaje, request.NumeAsiento.ToString(), decimal.Parse(request.Precio.ToString()), request.FechaProgramacion, request.CodiTerminal.ToString());
                        if (resBloquearAsiento.Estado)
                            response.Mensaje += resBloquearAsiento.Mensaje;
                        else
                        {
                            response.Mensaje += "Error: BloquearAsientoViaje. ";
                            return response;
                        }
                    }
                }

                response.EsCorrecto = true;
                response.Valor = int.Parse(resBloquearAsiento.Valor.ToString());
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BloqueoAsientoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgErrExcBusqAsiento, false);
            }
        }

        public static Response<bool> LiberaAsiento(int IDS)
        {
            try
            {
                var response = new Response<bool>(false, false, "", false);

                // Validar 'LiberaAsiento'
                var resLiberaAsiento = BloqueoAsientoRepository.LiberaAsiento(IDS);
                if (resLiberaAsiento.Estado)
                    response.Mensaje += resLiberaAsiento.Mensaje;
                else
                {
                    response.Mensaje += "Error: LiberaAsiento. ";
                    return response;
                }

                response.EsCorrecto = true;
                response.Valor = resLiberaAsiento.Valor;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BloqueoAsientoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcBusqAsientoBloqueado, false);
            }
        }
    }
}
