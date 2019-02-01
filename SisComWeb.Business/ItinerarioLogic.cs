using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public class ItinerarioLogic
    {
        public static Response<List<ItinerarioEntity>> BuscaItinerarios(ItinerarioRequest request)
        {
            try
            {
                var response = new Response<List<ItinerarioEntity>>(false, null, "", false);

                Response<BusEntity> resObtenerBus;
                Response<int> resObtenerTotalVentas;
                Response<List<PuntoEntity>> resListarPuntosEmbarque;
                Response<List<PuntoEntity>> resListarPuntosArribo;

                // Lista Itinerarios
                var resBuscarItinerarios = ItinerarioRepository.BuscarItinerarios(request.CodiOrigen, request.CodiDestino, request.CodiRuta, request.Hora);
                if (resBuscarItinerarios.Estado)
                    response.Mensaje += resBuscarItinerarios.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscarItinerarios. ";
                    return response;
                }

                // Recorre cada registro
                foreach (var iti in resBuscarItinerarios.Valor)
                {
                    // Calcula 'FechaProgramacion'
                    var doubleDias = double.Parse(iti.Dias.ToString());
                    if (iti.Dias > 0)
                        iti.FechaProgramacion = DateTime.Parse(request.FechaViaje).AddDays(doubleDias).ToString();
                    else
                        iti.FechaProgramacion = DateTime.Parse(request.FechaViaje).ToString();

                    // Verifica cambios 'TurnoViaje'
                    var resVerificaCambiosTurnoViaje = ItinerarioRepository.VerificaCambiosTurnoViaje(iti.NroViaje, iti.FechaProgramacion);
                    if (resVerificaCambiosTurnoViaje.Estado && !string.IsNullOrEmpty(resVerificaCambiosTurnoViaje.Valor.NomServicio))
                    {
                        iti.CodiServicio = resVerificaCambiosTurnoViaje.Valor.CodiServicio;
                        iti.NomServicio = resVerificaCambiosTurnoViaje.Valor.NomServicio;
                        iti.CodiEmpresa = resVerificaCambiosTurnoViaje.Valor.CodiEmpresa;

                        response.Mensaje += resVerificaCambiosTurnoViaje.Mensaje;
                    }
                    else if (resVerificaCambiosTurnoViaje.Estado && string.IsNullOrEmpty(resVerificaCambiosTurnoViaje.Valor.NomServicio))
                        response.Mensaje += "Mensaje: VerificaCambiosTurnoViaje: CodiServicio nulo o vacío, no se pudo actualizar desde 'TurnoViaje'. ";
                    else
                    {
                        response.Mensaje += "Error: VerificaCambiosTurnoViaje. ";
                        return response;
                    }

                    // Busca 'ProgramacionViaje'
                    var resBuscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(iti.NroViaje, iti.FechaProgramacion);
                    if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor != 0)
                    {
                        response.Mensaje += resBuscarProgramacionViaje.Mensaje;

                        // Obtiene 'BusProgramacion'
                        resObtenerBus = ItinerarioRepository.ObtenerBusProgramacion(resBuscarProgramacionViaje.Valor);
                        if (resObtenerBus.Estado)
                            response.Mensaje += resObtenerBus.Mensaje;
                        else
                        {
                            response.Mensaje += "Error: ObtenerBusProgramacion. ";
                            return response;
                        }
                    }
                    else if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor == 0)
                    {
                        response.Mensaje += "Mensaje: resBuscarProgramacionViaje.Valor: es cero. ";

                        // Obtiene 'BusEstandar'
                        resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(iti.CodiEmpresa, iti.CodiSucursal, iti.CodiRuta, iti.CodiServicio, request.Hora);
                        if (resObtenerBus.Estado)
                            response.Mensaje += resObtenerBus.Mensaje;
                        else
                        {
                            response.Mensaje += "Error: ObtenerBusEstandar. ";
                            return response;
                        }
                    }
                    else
                    {
                        response.Mensaje += "Error: BuscarProgramacionViaje. ";
                        return response;
                    }

                    // Valida 'TurnoAdicional'
                    var resValidarTurnoAdicional = ItinerarioRepository.ValidarTurnoAdicional(iti.NroViaje, iti.FechaProgramacion);
                    if (resValidarTurnoAdicional.Estado && resValidarTurnoAdicional.Valor == 1)
                    {
                        response.Mensaje += resValidarTurnoAdicional.Mensaje;

                        // Valida 'ViajeCalendario'
                        var resValidarViajeCalendario = ItinerarioRepository.ValidarViajeCalendario(iti.NroViaje, iti.FechaProgramacion);
                        if (resValidarViajeCalendario.Estado)
                        {
                            response.Mensaje += resValidarViajeCalendario.Mensaje;

                            if (resValidarViajeCalendario.Valor == 1)
                                break;
                            else
                            {
                                // Obtiene 'TotalVentas'
                                resObtenerTotalVentas = ItinerarioRepository.ObtenerTotalVentas(resBuscarProgramacionViaje.Valor);
                                if (resObtenerTotalVentas.Estado)
                                    response.Mensaje += resObtenerTotalVentas.Mensaje;
                                else
                                {
                                    response.Mensaje += "Error: ObtenerTotalVentas. ";
                                    return response;
                                }

                                // Lista 'TotalVentas'
                                resListarPuntosEmbarque = ItinerarioRepository.ListarPuntosEmbarque(iti.CodiOrigen, iti.CodiDestino, iti.CodiServicio, iti.CodiEmpresa, iti.CodiPuntoVenta, request.Hora);
                                if (resListarPuntosEmbarque.Estado)
                                    response.Mensaje += resListarPuntosEmbarque.Mensaje;
                                else
                                {
                                    response.Mensaje += "Error: ListarPuntosEmbarque. ";
                                    return response;
                                }

                                // Lista 'PuntosArribo'
                                resListarPuntosArribo = ItinerarioRepository.ListarPuntosArribo(iti.CodiOrigen, iti.CodiDestino, iti.CodiServicio, iti.CodiEmpresa, iti.CodiPuntoVenta, request.Hora);
                                if (resListarPuntosArribo.Estado)
                                    response.Mensaje += resListarPuntosArribo.Mensaje;
                                else
                                {
                                    response.Mensaje += "Error: ListarPuntosArribo. ";
                                    return response;
                                }
                            }
                        }
                        else
                        {
                            response.Mensaje += "Error: ValidarViajeCalendario. ";
                            return response;
                        }
                    }
                    else if (resValidarTurnoAdicional.Estado && resValidarTurnoAdicional.Valor == 0)
                    {
                        response.Mensaje += "Mensaje: resValidarTurnoAdicional: Valor es cero. ";

                        break;
                    }
                    else
                    {
                        response.Mensaje += "Error: ValidarTurnoAdicional. ";
                        return response;
                    }
                }

                response.EsCorrecto = true;
                response.Valor = resBuscarItinerarios.Valor;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ItinerarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ItinerarioEntity>>(false, null, Message.MsgErrExcListItinerario, false);
            }
        }
    }
}
