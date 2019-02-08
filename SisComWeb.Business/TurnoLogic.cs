using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public class TurnoLogic
    {
        public static Response<ItinerarioEntity> MuestraTurno(TurnoRequest request)
        {
            try
            {
                var response = new Response<ItinerarioEntity>(false, null, "", false);

                Response<BusEntity> resObtenerBus;

                // Lista Itinerarios
                var resBuscarTurno = TurnoRepository.BuscarTurno(request.CodiEmpresa, request.CodiPuntoVenta, request.CodiOrigen, request.CodiDestino, request.CodiSucusal, request.CodiRuta, request.CodiServicio, request.HoraViaje);
                if (resBuscarTurno.Estado)
                    response.Mensaje += resBuscarTurno.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscarTurno. ";
                    return response;
                }

                // Calcula 'FechaProgramacion'
                var doubleDias = double.Parse(resBuscarTurno.Valor.Dias.ToString());
                if (resBuscarTurno.Valor.Dias > 0)
                    resBuscarTurno.Valor.FechaProgramacion = DateTime.Parse(request.FechaViaje).AddDays(doubleDias).ToString();
                else
                    resBuscarTurno.Valor.FechaProgramacion = DateTime.Parse(request.FechaViaje).ToString();

                // Verifica cambios 'TurnoViaje'
                var resVerificaCambiosTurnoViaje = ItinerarioRepository.VerificaCambiosTurnoViaje(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.FechaProgramacion);
                if (resVerificaCambiosTurnoViaje.Estado && !string.IsNullOrEmpty(resVerificaCambiosTurnoViaje.Valor.NomServicio))
                {
                    resBuscarTurno.Valor.CodiServicio = resVerificaCambiosTurnoViaje.Valor.CodiServicio;
                    resBuscarTurno.Valor.NomServicio = resVerificaCambiosTurnoViaje.Valor.NomServicio;
                    resBuscarTurno.Valor.CodiEmpresa = resVerificaCambiosTurnoViaje.Valor.CodiEmpresa;

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
                var resBuscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.FechaProgramacion);
                if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor != 0)
                {
                    response.Mensaje += resBuscarProgramacionViaje.Mensaje;
                    // Obtiene 'BusProgramacion'
                    resObtenerBus = ItinerarioRepository.ObtenerBusProgramacion(resBuscarProgramacionViaje.Valor);
                    if (resObtenerBus.Estado)
                    {
                        resBuscarTurno.Valor.CodiBus = resObtenerBus.Valor.CodiBus;
                        resBuscarTurno.Valor.PlanoBus = resObtenerBus.Valor.PlanBus;
                        resBuscarTurno.Valor.CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                        resBuscarTurno.Valor.PlacaBus = resObtenerBus.Valor.PlacBus;

                        response.Mensaje += resObtenerBus.Mensaje;
                    }
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
                    resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiSucursal, resBuscarTurno.Valor.CodiRuta, resBuscarTurno.Valor.CodiServicio, request.HoraViaje);
                    if (resObtenerBus.Estado)
                    {
                        // En caso de no encontrar resultado.
                        if (!string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                            response.Mensaje += resObtenerBus.Mensaje;
                        else
                        {
                            resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiSucursal, resBuscarTurno.Valor.CodiRuta, resBuscarTurno.Valor.CodiServicio, "");
                            if (resObtenerBus.Estado)
                            {
                                resBuscarTurno.Valor.CodiBus = resObtenerBus.Valor.CodiBus;
                                resBuscarTurno.Valor.PlanoBus = resObtenerBus.Valor.PlanBus;
                                resBuscarTurno.Valor.CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                                resBuscarTurno.Valor.PlacaBus = resObtenerBus.Valor.PlacBus;

                                response.Mensaje += resObtenerBus.Mensaje;
                            }
                            else
                            {
                                response.Mensaje += "Error: ObtenerBusEstandar. ";
                                return response;
                            }
                        }
                    }
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
                var resValidarTurnoAdicional = ItinerarioRepository.ValidarTurnoAdicional(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.FechaProgramacion);
                if (resValidarTurnoAdicional.Estado && resValidarTurnoAdicional.Valor == 1)
                {
                    response.Mensaje += resValidarTurnoAdicional.Mensaje;

                    // Valida 'ViajeCalendario'
                    var resValidarViajeCalendario = ItinerarioRepository.ValidarViajeCalendario(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.FechaProgramacion);
                    if (resValidarViajeCalendario.Estado)
                    {
                        response.Mensaje += resValidarViajeCalendario.Mensaje;

                        if (resValidarViajeCalendario.Valor == 1)
                        {
                            response.Mensaje += "Mensaje: ValidarViajeCalendario: Valor es 1. ";
                            return response;
                        }
                        else
                        {
                            // Obtiene 'TotalVentas'
                            Response<int> resObtenerTotalVentas = ItinerarioRepository.ObtenerTotalVentas(resBuscarProgramacionViaje.Valor);
                            if (resObtenerTotalVentas.Estado)
                            {
                                resBuscarTurno.Valor.AsientosVendidos = resObtenerTotalVentas.Valor;

                                response.Mensaje += resObtenerTotalVentas.Mensaje;
                            }
                                
                            else
                            {
                                response.Mensaje += "Error: ObtenerTotalVentas. ";
                                return response;
                            }

                            // Lista 'PuntosEmbarque'
                            Response<List<PuntoEntity>> resListarPuntosEmbarque = ItinerarioRepository.ListarPuntosEmbarque(resBuscarTurno.Valor.CodiOrigen, resBuscarTurno.Valor.CodiDestino, resBuscarTurno.Valor.CodiServicio, resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiPuntoVenta, request.HoraViaje);
                            if (resListarPuntosEmbarque.Estado)
                            {
                                resBuscarTurno.Valor.ListaEmbarques = resListarPuntosEmbarque.Valor;

                                response.Mensaje += resListarPuntosEmbarque.Mensaje;
                            }
                            else
                            {
                                response.Mensaje += "Error: ListarPuntosEmbarque. ";
                                return response;
                            }

                            // Lista 'PuntosArribo'
                            Response<List<PuntoEntity>> resListarPuntosArribo = ItinerarioRepository.ListarPuntosArribo(resBuscarTurno.Valor.CodiOrigen, resBuscarTurno.Valor.CodiDestino, resBuscarTurno.Valor.CodiServicio, resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiPuntoVenta, request.HoraViaje);
                            if (resListarPuntosArribo.Estado)
                            {
                                resBuscarTurno.Valor.ListaArribos = resListarPuntosArribo.Valor;

                                response.Mensaje += resListarPuntosArribo.Mensaje;
                            }
                            else
                            {
                                response.Mensaje += "Error: ListarPuntosArribo. ";
                                return response;
                            }

                            // Lista 'PlanoBus'
                            PlanoRequest requestPlano = new PlanoRequest
                            {
                                PlanoBus = resBuscarTurno.Valor.PlanoBus,
                                CodiProgramacion = resBuscarTurno.Valor.CodiProgramacion,
                                CodiOrigen = resBuscarTurno.Valor.CodiOrigen,
                                CodiDestino = resBuscarTurno.Valor.CodiDestino,
                                CodiBus = resBuscarTurno.Valor.CodiBus,
                                HoraViaje = request.HoraViaje,
                                FechaViaje = request.FechaViaje,
                                CodiServicio = resBuscarTurno.Valor.CodiServicio,
                                CodiEmpresa = resBuscarTurno.Valor.CodiEmpresa,
                                FechaProgramacion = resBuscarTurno.Valor.FechaProgramacion,
                                NroViaje = resBuscarTurno.Valor.NroViaje
                            };

                            Response<List<PlanoEntity>> resMuestraPlano = PlanoLogic.MuestraPlano(requestPlano);
                            if (resMuestraPlano.Estado)
                            {
                                resBuscarTurno.Valor.ListaPlanoBus = resMuestraPlano.Valor;

                                response.Mensaje += resMuestraPlano.Mensaje;
                            }
                            else
                            {
                                response.Mensaje += "Error: resMuestraPlano. ";
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
                    return response;
                }
                else
                {
                    response.Mensaje += "Error: ValidarTurnoAdicional. ";
                    return response;
                }

                response.EsCorrecto = true;
                response.Valor = resBuscarTurno.Valor;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(TurnoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ItinerarioEntity>(false, null, Message.MsgErrExcBusqTurno, false);
            }
        }
    }
}
