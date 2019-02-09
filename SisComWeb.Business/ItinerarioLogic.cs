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
                        resBuscarItinerarios.Valor[i].FechaProgramacion = DateTime.Parse(request.FechaViaje).AddDays(doubleDias).ToString("dd/MM/yyyy");
                    else
                        resBuscarItinerarios.Valor[i].FechaProgramacion = DateTime.Parse(request.FechaViaje).ToString("dd/MM/yyyy");

                    // Verifica cambios 'TurnoViaje'
                    var resVerificaCambiosTurnoViaje = ItinerarioRepository.VerificaCambiosTurnoViaje(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                    if (resVerificaCambiosTurnoViaje.Estado)
                    {
                        resBuscarItinerarios.Valor[i].CodiServicio = resVerificaCambiosTurnoViaje.Valor.CodiServicio;
                        resBuscarItinerarios.Valor[i].NomServicio = resVerificaCambiosTurnoViaje.Valor.NomServicio;
                        resBuscarItinerarios.Valor[i].CodiEmpresa = resVerificaCambiosTurnoViaje.Valor.CodiEmpresa;
                    }
                    else
                    {
                        response.Mensaje += "Error: VerificaCambiosTurnoViaje. ";
                        return response;
                    }

                    // Busca 'ProgramacionViaje'
                    var resBuscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                    if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor != 0)
                    {
                        // Obtiene 'BusProgramacion'
                        resObtenerBus = ItinerarioRepository.ObtenerBusProgramacion(resBuscarProgramacionViaje.Valor);
                        if (resObtenerBus.Estado)
                        {
                            resBuscarItinerarios.Valor[i].CodiBus = resObtenerBus.Valor.CodiBus;
                            resBuscarItinerarios.Valor[i].PlanoBus = resObtenerBus.Valor.PlanBus;
                            resBuscarItinerarios.Valor[i].CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                            resBuscarItinerarios.Valor[i].PlacaBus = resObtenerBus.Valor.PlacBus;
                        }
                        else
                        {
                            response.Mensaje += "Error: ObtenerBusProgramacion. ";
                            return response;
                        }
                    }
                    else if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor == 0)
                    {
                        // Valida 'SoloProgramados' : No está en el flujo, se añadió luego.
                        if (request.SoloProgramados)
                        {
                            resBuscarItinerarios.Valor.RemoveAt(i);
                            i--;
                            continue;
                        }

                        resBuscarItinerarios.Valor[i].CodiBus = "0000";
                        resBuscarItinerarios.Valor[i].PlanoBus = "000";
                        resBuscarItinerarios.Valor[i].CapacidadBus = "0";
                        resBuscarItinerarios.Valor[i].PlacaBus = "00-0000";
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
                        // Valida 'ProgramacionCerrada'
                        var resValidarProgrmacionCerrada = ItinerarioRepository.ValidarProgrmacionCerrada(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                        if (resValidarProgrmacionCerrada.Estado && resValidarProgrmacionCerrada.Valor == 1)
                        {
                            resBuscarItinerarios.Valor[i].ProgramacionCerrada = true;


                            if (resBuscarProgramacionViaje.Valor == 1)
                            {
                                // Obtiene 'TotalVentas'
                                Response<int> resObtenerTotalVentas = ItinerarioRepository.ObtenerTotalVentas(resBuscarProgramacionViaje.Valor, request.CodiOrigen, request.CodiDestino);
                                if (resObtenerTotalVentas.Estado)
                                    resBuscarItinerarios.Valor[i].AsientosVendidos = resObtenerTotalVentas.Valor;
                                else
                                {
                                    response.Mensaje += "Error: ObtenerTotalVentas. ";
                                    return response;
                                }
                            }
                        }
                        else if (resValidarProgrmacionCerrada.Estado && resValidarProgrmacionCerrada.Valor == 0)
                        {
                            // Lista 'PuntosEmbarque'
                            Response<List<PuntoEntity>> resListarPuntosEmbarque = ItinerarioRepository.ListarPuntosEmbarque(resBuscarItinerarios.Valor[i].CodiOrigen, resBuscarItinerarios.Valor[i].CodiDestino, resBuscarItinerarios.Valor[i].CodiServicio, resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiPuntoVenta, request.Hora);
                            if (resListarPuntosEmbarque.Estado)
                                resBuscarItinerarios.Valor[i].ListaEmbarques = resListarPuntosEmbarque.Valor;
                            else
                            {
                                response.Mensaje += "Error: ListarPuntosEmbarque. ";
                                return response;
                            }

                            // Lista 'PuntosArribo'
                            Response<List<PuntoEntity>> resListarPuntosArribo = ItinerarioRepository.ListarPuntosArribo(resBuscarItinerarios.Valor[i].CodiOrigen, resBuscarItinerarios.Valor[i].CodiDestino, resBuscarItinerarios.Valor[i].CodiServicio, resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiPuntoVenta, request.Hora);
                            if (resListarPuntosArribo.Estado)
                                resBuscarItinerarios.Valor[i].ListaArribos = resListarPuntosArribo.Valor;
                            else
                            {
                                response.Mensaje += "Error: ListarPuntosArribo. ";
                                return response;
                            }
                        }
                        else
                        {
                            response.Mensaje += "Error: ValidarProgrmacionCerrada. ";
                            return response;
                        }
                    }
                    else if (resValidarTurnoAdicional.Estado && resValidarTurnoAdicional.Valor == 0)
                        continue;
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
