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
                for (int i = 0; i < resBuscarItinerarios.Valor.Count; i++)
                {
                    // Calcula 'FechaProgramacion'
                    var doubleDias = double.Parse(resBuscarItinerarios.Valor[i].Dias.ToString());
                    if (resBuscarItinerarios.Valor[i].Dias > 0)
                        resBuscarItinerarios.Valor[i].FechaProgramacion = DateTime.Parse(request.FechaViaje).AddDays(doubleDias).ToString();
                    else
                        resBuscarItinerarios.Valor[i].FechaProgramacion = DateTime.Parse(request.FechaViaje).ToString();

                    // Verifica cambios 'TurnoViaje'
                    var resVerificaCambiosTurnoViaje = ItinerarioRepository.VerificaCambiosTurnoViaje(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                    if (resVerificaCambiosTurnoViaje.Estado && !string.IsNullOrEmpty(resVerificaCambiosTurnoViaje.Valor.NomServicio))
                    {
                        resBuscarItinerarios.Valor[i].CodiServicio = resVerificaCambiosTurnoViaje.Valor.CodiServicio;
                        resBuscarItinerarios.Valor[i].NomServicio = resVerificaCambiosTurnoViaje.Valor.NomServicio;
                        resBuscarItinerarios.Valor[i].CodiEmpresa = resVerificaCambiosTurnoViaje.Valor.CodiEmpresa;

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
                    var resBuscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                    if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor != 0)
                    {
                        response.Mensaje += resBuscarProgramacionViaje.Mensaje;
                        // Obtiene 'BusProgramacion'
                        resObtenerBus = ItinerarioRepository.ObtenerBusProgramacion(resBuscarProgramacionViaje.Valor);
                        if (resObtenerBus.Estado)
                        {
                            resBuscarItinerarios.Valor[i].CodiBus = resObtenerBus.Valor.CodiBus;
                            resBuscarItinerarios.Valor[i].PlanoBus = resObtenerBus.Valor.PlanBus;
                            resBuscarItinerarios.Valor[i].CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                            resBuscarItinerarios.Valor[i].PlacaBus = resObtenerBus.Valor.PlacBus;

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


                        // Valida 'SoloProgramados'
                        if (request.SoloProgramados)
                        {
                            resBuscarItinerarios.Valor.RemoveAt(i);
                            i--;
                            continue;
                        }


                        // Obtiene 'BusEstandar'
                        resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiSucursal, resBuscarItinerarios.Valor[i].CodiRuta, resBuscarItinerarios.Valor[i].CodiServicio, request.Hora);
                        if (resObtenerBus.Estado)
                        {
                            // En caso de no encontrar resultado.
                            if (!string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                                response.Mensaje += resObtenerBus.Mensaje;
                            else
                            {
                                resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiSucursal, resBuscarItinerarios.Valor[i].CodiRuta, resBuscarItinerarios.Valor[i].CodiServicio, "");
                                if (resObtenerBus.Estado)
                                {
                                    resBuscarItinerarios.Valor[i].CodiBus = resObtenerBus.Valor.CodiBus;
                                    resBuscarItinerarios.Valor[i].PlanoBus = resObtenerBus.Valor.PlanBus;
                                    resBuscarItinerarios.Valor[i].CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                                    resBuscarItinerarios.Valor[i].PlacaBus = resObtenerBus.Valor.PlacBus;

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
                    var resValidarTurnoAdicional = ItinerarioRepository.ValidarTurnoAdicional(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                    if (resValidarTurnoAdicional.Estado && resValidarTurnoAdicional.Valor == 1)
                    {
                        response.Mensaje += resValidarTurnoAdicional.Mensaje;

                        // Valida 'ViajeCalendario'
                        var resValidarViajeCalendario = ItinerarioRepository.ValidarViajeCalendario(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                        if (resValidarViajeCalendario.Estado)
                        {
                            response.Mensaje += resValidarViajeCalendario.Mensaje;

                            if (resValidarViajeCalendario.Valor == 1)
                                continue;
                            else
                            {
                                // Obtiene 'TotalVentas'
                                Response<int> resObtenerTotalVentas = ItinerarioRepository.ObtenerTotalVentas(resBuscarProgramacionViaje.Valor);
                                if (resObtenerTotalVentas.Estado)
                                {
                                    resBuscarItinerarios.Valor[i].AsientosVendidos = resObtenerTotalVentas.Valor;

                                    response.Mensaje += resObtenerTotalVentas.Mensaje;
                                }
                                else
                                {
                                    response.Mensaje += "Error: ObtenerTotalVentas. ";
                                    return response;
                                }

                                // Lista 'PuntosEmbarque'
                                Response<List<PuntoEntity>> resListarPuntosEmbarque = ItinerarioRepository.ListarPuntosEmbarque(resBuscarItinerarios.Valor[i].CodiOrigen, resBuscarItinerarios.Valor[i].CodiDestino, resBuscarItinerarios.Valor[i].CodiServicio, resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiPuntoVenta, request.Hora);
                                if (resListarPuntosEmbarque.Estado)
                                {
                                    resBuscarItinerarios.Valor[i].ListaEmbarques = resListarPuntosEmbarque.Valor;

                                    response.Mensaje += resListarPuntosEmbarque.Mensaje;
                                }
                                else
                                {
                                    response.Mensaje += "Error: ListarPuntosEmbarque. ";
                                    return response;
                                }

                                // Lista 'PuntosArribo'
                                Response<List<PuntoEntity>> resListarPuntosArribo = ItinerarioRepository.ListarPuntosArribo(resBuscarItinerarios.Valor[i].CodiOrigen, resBuscarItinerarios.Valor[i].CodiDestino, resBuscarItinerarios.Valor[i].CodiServicio, resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiPuntoVenta, request.Hora);
                                if (resListarPuntosArribo.Estado)
                                {
                                    resBuscarItinerarios.Valor[i].ListaArribos = resListarPuntosArribo.Valor;

                                    response.Mensaje += resListarPuntosArribo.Mensaje;
                                }
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

                        continue;
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
