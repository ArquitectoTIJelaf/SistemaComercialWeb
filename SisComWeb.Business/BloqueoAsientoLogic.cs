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
                var bloquearAsiento = new decimal();

                // Validar 'BloqueoAsiento'
                var validarBloqueoAsiento = BloqueoAsientoRepository.ValidarBloqueoAsiento(request.CodiProgramacion, request.NroViaje, request.CodiOrigen, request.CodiDestino, request.NumeAsiento.ToString(), request.FechaProgramacion);

                if (validarBloqueoAsiento)
                    return new Response<int>(false, int.Parse(bloquearAsiento.ToString()), Message.MsgValidaBloqueoAsiento, true);
                else
                {
                    // ¿Existe Programación?
                    if (request.CodiProgramacion > 0)
                        // Bloquear 'AsientoProgramacion'
                        bloquearAsiento = BloqueoAsientoRepository.BloquearAsientoProgramacion(request.CodiProgramacion, request.NumeAsiento.ToString(), request.Precio, request.FechaProgramacion, request.CodiTerminal.ToString());
                    else
                        // Bloquear 'AsientoViaje'
                        bloquearAsiento = BloqueoAsientoRepository.BloquearAsientoViaje(request.NroViaje, request.NumeAsiento.ToString(), request.Precio, request.FechaProgramacion, request.CodiTerminal.ToString());
                }

                if (bloquearAsiento == 0)
                    return new Response<int>(false, int.Parse(bloquearAsiento.ToString()), Message.MsgValidaCeroBloqueoAsiento, true);
                else
                    return new Response<int>(true, int.Parse(bloquearAsiento.ToString()), Message.MsgCorrectoBloqueoAsiento, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BloqueoAsientoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcBloqueoAsiento, false);
            }
        }

        public static Response<bool> LiberaAsiento(int IDS)
        {
            try
            {
                // Libera 'Asiento'
                var liberaAsiento = BloqueoAsientoRepository.LiberaAsiento(IDS);

                if (liberaAsiento)
                    return new Response<bool>(true, liberaAsiento, Message.MsgCorrectoLiberaAsiento, true);
                else
                    return new Response<bool>(false, liberaAsiento, Message.MsgValidaLiberaAsiento, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BloqueoAsientoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcLiberaAsiento, false);
            }
        }

        public static Response<bool> ActualizarAsiOcuTbBloqueoAsientos(TablaBloqueoAsientosRequest request)
        {
            try
            {
                // Volver a consultar 'TablaBloqueoAsientos' (SignalR)
                var auxCodiProgramacion = request.CodiProgramacion > 0 ? request.CodiProgramacion : request.NroViaje;
                var auxTipo = request.CodiProgramacion > 0 ? "P" : "V";

                var consultarTablaBloqueoAsientos = BloqueoAsientoRepository.ConsultarTablaBloqueoAsientos(auxCodiProgramacion, auxTipo, request.Fecha);

                if (consultarTablaBloqueoAsientos == null)
                    BloqueoAsientoRepository.ActualizarTablaBloqueoAsientos(request.CodiProgramacion.ToString(), request.NroViaje.ToString(), request.Fecha);
                // ---------------------------------------------------

                request.CodiProgramacion = auxCodiProgramacion;
                request.Tipo = auxTipo;

                // Actualizar 'AsiOcuTbBloqueoAsientos'
                var actualizarAsiOcuTbBloqueoAsientos = BloqueoAsientoRepository.ActualizarAsiOcuTbBloqueoAsientos(request);

                if (actualizarAsiOcuTbBloqueoAsientos)
                    return new Response<bool>(true, actualizarAsiOcuTbBloqueoAsientos, Message.MsgCorrectoActualizarAsiOcuTbBloqueoAsientos, true);
                else
                    return new Response<bool>(false, actualizarAsiOcuTbBloqueoAsientos, Message.MsgErrorActualizarAsiOcuTbBloqueoAsientos, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BloqueoAsientoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcActualizarAsiOcuTbBloqueoAsientos, false);
            }
        }
    }
}
