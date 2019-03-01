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
                var response = new Response<List<ItinerarioEntity>>(false, null, "Error: BuscaItinerarios.", false);
                Response<BusEntity> resObtenerBus;

                // Validar 'TodosTurnos'
                if (request.TodosTurnos == true) request.Hora = "";

                // Lista Itinerarios
                var resBuscarItinerarios = ItinerarioRepository.BuscarItinerarios(request.CodiOrigen, request.CodiDestino, request.CodiRuta, request.Hora);
                if (!resBuscarItinerarios.Estado)
                {
                    response.Mensaje = resBuscarItinerarios.Mensaje;
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
                        if (resVerificaCambiosTurnoViaje.Valor.CodiEmpresa > 0)
                        {
                            resBuscarItinerarios.Valor[i].CodiServicio = resVerificaCambiosTurnoViaje.Valor.CodiServicio;
                            resBuscarItinerarios.Valor[i].NomServicio = resVerificaCambiosTurnoViaje.Valor.NomServicio;
                            resBuscarItinerarios.Valor[i].CodiEmpresa = resVerificaCambiosTurnoViaje.Valor.CodiEmpresa;
                        }
                    }
                    else
                    {
                        response.Mensaje = resVerificaCambiosTurnoViaje.Mensaje;
                        return response;
                    }

                    // Busca 'ProgramacionViaje'
                    var resBuscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                    if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor > 0)
                    {
                        resBuscarItinerarios.Valor[i].CodiProgramacion = resBuscarProgramacionViaje.Valor;

                        // Obtiene 'BusProgramacion'
                        resObtenerBus = ItinerarioRepository.ObtenerBusProgramacion(resBuscarItinerarios.Valor[i].CodiProgramacion);
                        if (resObtenerBus.Estado)
                        {
                            resBuscarItinerarios.Valor[i].CodiBus = resObtenerBus.Valor.CodiBus ?? "0000";
                            resBuscarItinerarios.Valor[i].PlanoBus = resObtenerBus.Valor.PlanBus ?? "000";
                            resBuscarItinerarios.Valor[i].CapacidadBus = resObtenerBus.Valor.NumePasajeros ?? "0";
                            resBuscarItinerarios.Valor[i].PlacaBus = resObtenerBus.Valor.PlacBus ?? "00-0000";
                        }
                        else
                        {
                            response.Mensaje = resObtenerBus.Mensaje;
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

                        // Obtiene 'BusEstandar'
                        resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiSucursal, resBuscarItinerarios.Valor[i].CodiRuta, resBuscarItinerarios.Valor[i].CodiServicio, resBuscarItinerarios.Valor[i].HoraPartida);
                        if (resObtenerBus.Estado && !string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                        {
                            resBuscarItinerarios.Valor[i].CodiBus = resObtenerBus.Valor.CodiBus;
                            resBuscarItinerarios.Valor[i].PlanoBus = resObtenerBus.Valor.PlanBus;
                            resBuscarItinerarios.Valor[i].CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                            resBuscarItinerarios.Valor[i].PlacaBus = resObtenerBus.Valor.PlacBus;
                        }
                        else if (resObtenerBus.Estado && string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                        {
                            // En caso de no encontrar resultado
                            resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiSucursal, resBuscarItinerarios.Valor[i].CodiRuta, resBuscarItinerarios.Valor[i].CodiServicio, "");
                            if (resObtenerBus.Estado && !string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                            {
                                resBuscarItinerarios.Valor[i].CodiBus = resObtenerBus.Valor.CodiBus;
                                resBuscarItinerarios.Valor[i].PlanoBus = resObtenerBus.Valor.PlanBus;
                                resBuscarItinerarios.Valor[i].CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                                resBuscarItinerarios.Valor[i].PlacaBus = resObtenerBus.Valor.PlacBus;
                            }
                            else if (resObtenerBus.Estado && string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                            {
                                // En caso de no encontrar resultado
                                resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarItinerarios.Valor[i].CodiEmpresa, resBuscarItinerarios.Valor[i].CodiSucursal, 0, resBuscarItinerarios.Valor[i].CodiServicio, "");
                                if (resObtenerBus.Estado)
                                {
                                    resBuscarItinerarios.Valor[i].CodiBus = resObtenerBus.Valor.CodiBus ?? "0000";
                                    resBuscarItinerarios.Valor[i].PlanoBus = resObtenerBus.Valor.PlanBus ?? "000";
                                    resBuscarItinerarios.Valor[i].CapacidadBus = resObtenerBus.Valor.NumePasajeros ?? "0";
                                    resBuscarItinerarios.Valor[i].PlacaBus = resObtenerBus.Valor.PlacBus ?? "00-0000";
                                }
                                else
                                {
                                    response.Mensaje = resObtenerBus.Mensaje;
                                    return response;
                                }
                            }
                            else
                            {
                                response.Mensaje = resObtenerBus.Mensaje;
                                return response;
                            }
                        }
                        else
                        {
                            response.Mensaje = resObtenerBus.Mensaje;
                            return response;
                        }
                    }
                    else
                    {
                        response.Mensaje = resBuscarProgramacionViaje.Mensaje;
                        return response;
                    }

                    // Valida 'ProgramacionCerrada'
                    var resValidarProgrmacionCerrada = ItinerarioRepository.ValidarProgrmacionCerrada(resBuscarItinerarios.Valor[i].NroViaje, resBuscarItinerarios.Valor[i].FechaProgramacion);
                    if (resValidarProgrmacionCerrada.Estado)
                    {
                        if (resValidarProgrmacionCerrada.Valor == 1)
                            resBuscarItinerarios.Valor[i].ProgramacionCerrada = true;
                    }
                    else
                    {
                        response.Mensaje = resValidarProgrmacionCerrada.Mensaje;
                        return response;
                    }

                    // Obtiene 'TotalVentas'
                    if (resBuscarItinerarios.Valor[i].CodiProgramacion > 0)
                    {
                        var resObtenerTotalVentas = ItinerarioRepository.ObtenerTotalVentas(resBuscarItinerarios.Valor[i].CodiProgramacion, resBuscarItinerarios.Valor[i].CodiOrigen, resBuscarItinerarios.Valor[i].CodiDestino);
                        if (resObtenerTotalVentas.Estado)
                            resBuscarItinerarios.Valor[i].AsientosVendidos = resObtenerTotalVentas.Valor;
                        else
                        {
                            response.Mensaje = resObtenerTotalVentas.Mensaje;
                            return response;
                        }
                    }
                }

                response.EsCorrecto = true;
                response.Valor = resBuscarItinerarios.Valor;
                response.Mensaje = Message.MsgCorrectoBuscaItinerarios;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ItinerarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ItinerarioEntity>>(false, null, Message.MsgErrExcBuscaItinerarios, false);
            }
        }
    }
}
