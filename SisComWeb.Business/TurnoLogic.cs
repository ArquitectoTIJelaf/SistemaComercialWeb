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
                var resBuscarTurno = TurnoRepository.BuscarTurno(request.CodiEmpresa, request.CodiPuntoVenta, request.CodiOrigen, request.CodiDestino, request.CodiSucursal, request.CodiRuta, request.CodiServicio, request.HoraViaje);
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
                    resBuscarTurno.Valor.FechaProgramacion = DateTime.Parse(request.FechaViaje).AddDays(doubleDias).ToString("dd/MM/yyyy");
                else
                    resBuscarTurno.Valor.FechaProgramacion = DateTime.Parse(request.FechaViaje).ToString("dd/MM/yyyy");

                // Verifica cambios 'TurnoViaje'
                var resVerificaCambiosTurnoViaje = ItinerarioRepository.VerificaCambiosTurnoViaje(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.FechaProgramacion);
                if (resVerificaCambiosTurnoViaje.Estado)
                {
                    if (resVerificaCambiosTurnoViaje.Valor.CodiEmpresa != 0)
                    {
                        resBuscarTurno.Valor.CodiServicio = resVerificaCambiosTurnoViaje.Valor.CodiServicio;
                        resBuscarTurno.Valor.NomServicio = resVerificaCambiosTurnoViaje.Valor.NomServicio;
                        resBuscarTurno.Valor.CodiEmpresa = resVerificaCambiosTurnoViaje.Valor.CodiEmpresa;
                    }
                }
                else
                {
                    response.Mensaje += "Error: VerificaCambiosTurnoViaje. ";
                    return response;
                }

                // Busca 'ProgramacionViaje'
                var resBuscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.FechaProgramacion);
                if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor != 0)
                {
                    resBuscarTurno.Valor.CodiProgramacion = resBuscarProgramacionViaje.Valor;

                    // Obtiene 'BusProgramacion'
                    resObtenerBus = ItinerarioRepository.ObtenerBusProgramacion(resBuscarTurno.Valor.CodiProgramacion);
                    if (resObtenerBus.Estado)
                    {
                        resBuscarTurno.Valor.CodiBus = resObtenerBus.Valor.CodiBus ?? "0000";
                        resBuscarTurno.Valor.PlanoBus = resObtenerBus.Valor.PlanBus ?? "000";
                        resBuscarTurno.Valor.CapacidadBus = resObtenerBus.Valor.NumePasajeros ?? "0";
                        resBuscarTurno.Valor.PlacaBus = resObtenerBus.Valor.PlacBus ?? "00-0000";
                    }
                    else
                    {
                        response.Mensaje += "Error: ObtenerBusProgramacion. ";
                        return response;
                    }
                }
                else if (resBuscarProgramacionViaje.Estado && resBuscarProgramacionViaje.Valor == 0)
                {
                    // Obtiene 'BusEstandar'
                    resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiSucursal, resBuscarTurno.Valor.CodiRuta, resBuscarTurno.Valor.CodiServicio, resBuscarTurno.Valor.HoraPartida);
                    if (resObtenerBus.Estado && !string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                    {
                        resBuscarTurno.Valor.CodiBus = resObtenerBus.Valor.CodiBus;
                        resBuscarTurno.Valor.PlanoBus = resObtenerBus.Valor.PlanBus;
                        resBuscarTurno.Valor.CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                        resBuscarTurno.Valor.PlacaBus = resObtenerBus.Valor.PlacBus;
                    }
                    else if (resObtenerBus.Estado && string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                    {
                        // En caso de no encontrar resultado.
                        resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiSucursal, resBuscarTurno.Valor.CodiRuta, resBuscarTurno.Valor.CodiServicio, "");
                        if (resObtenerBus.Estado && !string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                        {
                            resBuscarTurno.Valor.CodiBus = resObtenerBus.Valor.CodiBus;
                            resBuscarTurno.Valor.PlanoBus = resObtenerBus.Valor.PlanBus;
                            resBuscarTurno.Valor.CapacidadBus = resObtenerBus.Valor.NumePasajeros;
                            resBuscarTurno.Valor.PlacaBus = resObtenerBus.Valor.PlacBus;
                        }
                        else if (resObtenerBus.Estado && string.IsNullOrEmpty(resObtenerBus.Valor.CodiBus))
                        {
                            // En caso de no encontrar resultado
                            resObtenerBus = ItinerarioRepository.ObtenerBusEstandar(resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiSucursal, 0, resBuscarTurno.Valor.CodiServicio, "");
                            if (resObtenerBus.Estado)
                            {
                                resBuscarTurno.Valor.CodiBus = resObtenerBus.Valor.CodiBus ?? "0000";
                                resBuscarTurno.Valor.PlanoBus = resObtenerBus.Valor.PlanBus ?? "000";
                                resBuscarTurno.Valor.CapacidadBus = resObtenerBus.Valor.NumePasajeros ?? "0";
                                resBuscarTurno.Valor.PlacaBus = resObtenerBus.Valor.PlacBus ?? "00-0000";
                            }
                            else
                            {
                                response.Mensaje += "Error: ObtenerBusEstandar sin hora y con CodiRuta igual a 0. ";
                                return response;
                            }
                        }
                        else
                        {
                            response.Mensaje += "Error: ObtenerBusEstandar sin hora. ";
                            return response;
                        }
                    }
                    else
                    {
                        response.Mensaje += "Error: ObtenerBusEstandar con Hora. ";
                        return response;
                    }
                }
                else
                {
                    response.Mensaje += "Error: BuscarProgramacionViaje. ";
                    return response;
                }

                // Valida 'ProgramacionCerrada'
                var resValidarProgrmacionCerrada = ItinerarioRepository.ValidarProgrmacionCerrada(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.FechaProgramacion);
                if (resValidarProgrmacionCerrada.Estado)
                {
                    if (resValidarProgrmacionCerrada.Valor == 1)
                        resBuscarTurno.Valor.ProgramacionCerrada = true;
                }
                else
                {
                    response.Mensaje += "Error: ValidarProgrmacionCerrada. ";
                    return response;
                }

                // Obtiene 'TotalVentas'
                if (resBuscarTurno.Valor.CodiProgramacion != 0)
                {
                    Response<int> resObtenerTotalVentas = ItinerarioRepository.ObtenerTotalVentas(resBuscarTurno.Valor.CodiProgramacion, resBuscarTurno.Valor.CodiOrigen, resBuscarTurno.Valor.CodiDestino);
                    if (resObtenerTotalVentas.Estado)
                        resBuscarTurno.Valor.AsientosVendidos = resObtenerTotalVentas.Valor;
                    else
                    {
                        response.Mensaje += "Error: ObtenerTotalVentas. ";
                        return response;
                    }
                }

                // Lista 'PuntosEmbarque'
                Response<List<PuntoEntity>> resListarPuntosEmbarque = ItinerarioRepository.ListarPuntosEmbarque(resBuscarTurno.Valor.CodiOrigen, resBuscarTurno.Valor.CodiDestino, resBuscarTurno.Valor.CodiServicio, resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiPuntoVenta, resBuscarTurno.Valor.HoraPartida);
                if (resListarPuntosEmbarque.Estado)
                    resBuscarTurno.Valor.ListaEmbarques = resListarPuntosEmbarque.Valor;
                else
                {
                    response.Mensaje += "Error: ListarPuntosEmbarque. ";
                    return response;
                }

                // Lista 'PuntosArribo'
                Response<List<PuntoEntity>> resListarPuntosArribo = ItinerarioRepository.ListarPuntosArribo(resBuscarTurno.Valor.CodiOrigen, resBuscarTurno.Valor.CodiDestino, resBuscarTurno.Valor.CodiServicio, resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiPuntoVenta, resBuscarTurno.Valor.HoraPartida);
                if (resListarPuntosArribo.Estado)
                    resBuscarTurno.Valor.ListaArribos = resListarPuntosArribo.Valor;
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
                    resBuscarTurno.Valor.ListaPlanoBus = resMuestraPlano.Valor;
                else
                {
                    response.Mensaje += "Error: resMuestraPlano. ";
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
