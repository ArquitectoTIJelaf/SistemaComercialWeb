using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class TurnoLogic
    {
        public static Response<ItinerarioEntity> MuestraTurno(TurnoRequest request)
        {
            try
            {
                var response = new Response<ItinerarioEntity>(false, null, "Error: MuestraTurno.", false);

                Response<BusEntity> resObtenerBus;

                // Lista Itinerarios
                var resBuscarTurno = TurnoRepository.BuscarTurno(request.CodiEmpresa, request.CodiPuntoVenta, request.CodiOrigen, request.CodiDestino, request.CodiSucursal, request.CodiRuta, request.CodiServicio, request.HoraViaje);
                if (!resBuscarTurno.Estado)
                {
                    response.Mensaje = resBuscarTurno.Mensaje;
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
                    response.Mensaje = resVerificaCambiosTurnoViaje.Mensaje;
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
                        resBuscarTurno.Valor.CodiChofer = resObtenerBus.Valor.CodiChofer ?? "00000";
                        resBuscarTurno.Valor.NombreChofer = resObtenerBus.Valor.NombreChofer ?? "NINGUNO";
                        resBuscarTurno.Valor.CodiCopiloto = resObtenerBus.Valor.CodiCopiloto ?? "00000";
                        resBuscarTurno.Valor.NombreCopiloto = resObtenerBus.Valor.NombreCopiloto ?? "NINGUNO";
                    }
                    else
                    {
                        response.Mensaje = resObtenerBus.Mensaje;
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
                        resBuscarTurno.Valor.CodiChofer = resObtenerBus.Valor.CodiChofer ?? "00000";
                        resBuscarTurno.Valor.NombreChofer = resObtenerBus.Valor.NombreChofer ?? "NINGUNO";
                        resBuscarTurno.Valor.CodiCopiloto = resObtenerBus.Valor.CodiCopiloto ?? "00000";
                        resBuscarTurno.Valor.NombreCopiloto = resObtenerBus.Valor.NombreCopiloto ?? "NINGUNO";
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
                            resBuscarTurno.Valor.CodiChofer = resObtenerBus.Valor.CodiChofer ?? "00000";
                            resBuscarTurno.Valor.NombreChofer = resObtenerBus.Valor.NombreChofer ?? "NINGUNO";
                            resBuscarTurno.Valor.CodiCopiloto = resObtenerBus.Valor.CodiCopiloto ?? "00000";
                            resBuscarTurno.Valor.NombreCopiloto = resObtenerBus.Valor.NombreCopiloto ?? "NINGUNO";
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
                                resBuscarTurno.Valor.CodiChofer = resObtenerBus.Valor.CodiChofer ?? "00000";
                                resBuscarTurno.Valor.NombreChofer = resObtenerBus.Valor.NombreChofer ?? "NINGUNO";
                                resBuscarTurno.Valor.CodiCopiloto = resObtenerBus.Valor.CodiCopiloto ?? "00000";
                                resBuscarTurno.Valor.NombreCopiloto = resObtenerBus.Valor.NombreCopiloto ?? "NINGUNO";
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
                var resValidarProgrmacionCerrada = ItinerarioRepository.ValidarProgrmacionCerrada(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.FechaProgramacion);
                if (resValidarProgrmacionCerrada.Estado)
                {
                    if (resValidarProgrmacionCerrada.Valor == 1)
                        resBuscarTurno.Valor.ProgramacionCerrada = true;
                }
                else
                {
                    response.Mensaje = resValidarProgrmacionCerrada.Mensaje;
                    return response;
                }

                // Obtiene 'TotalVentas'
                if (resBuscarTurno.Valor.CodiProgramacion != 0)
                {
                    var resObtenerTotalVentas = ItinerarioRepository.ObtenerTotalVentas(resBuscarTurno.Valor.CodiProgramacion, resBuscarTurno.Valor.CodiOrigen, resBuscarTurno.Valor.CodiDestino);
                    if (resObtenerTotalVentas.Estado)
                        resBuscarTurno.Valor.AsientosVendidos = resObtenerTotalVentas.Valor;
                    else
                    {
                        response.Mensaje = resObtenerTotalVentas.Mensaje;
                        return response;
                    }
                }

                // Lista 'PuntosEmbarque'
                var resListarPuntosEmbarque = TurnoRepository.ListarPuntosEmbarque(resBuscarTurno.Valor.CodiOrigen, resBuscarTurno.Valor.CodiDestino, resBuscarTurno.Valor.CodiServicio, resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiPuntoVenta, resBuscarTurno.Valor.HoraPartida);
                if (resListarPuntosEmbarque.Estado)
                {
                    if (resListarPuntosEmbarque.Valor.Count == 0)
                    {
                        var objPuntoEntity = new PuntoEntity
                        {
                            CodiPuntoVenta = resBuscarTurno.Valor.CodiOrigen,
                            Lugar = resBuscarTurno.Valor.NomOrigen,
                            Hora = resBuscarTurno.Valor.HoraPartida
                        };
                        resListarPuntosEmbarque.Valor.Add(objPuntoEntity);

                    }
                    resBuscarTurno.Valor.ListaEmbarques = resListarPuntosEmbarque.Valor;
                }
                else
                {
                    response.Mensaje = resListarPuntosEmbarque.Mensaje;
                    return response;
                }

                // Lista 'PuntosArribo'
                var resListarPuntosArribo = TurnoRepository.ListarPuntosArribo(resBuscarTurno.Valor.CodiOrigen, resBuscarTurno.Valor.CodiDestino, resBuscarTurno.Valor.CodiServicio, resBuscarTurno.Valor.CodiEmpresa, resBuscarTurno.Valor.CodiPuntoVenta, resBuscarTurno.Valor.HoraPartida);
                if (resListarPuntosArribo.Estado)
                    resBuscarTurno.Valor.ListaArribos = resListarPuntosArribo.Valor;
                else
                {
                    response.Mensaje = resListarPuntosArribo.Mensaje;
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
                var resMuestraPlano = PlanoLogic.MuestraPlano(requestPlano);
                if (resMuestraPlano.Estado)
                    resBuscarTurno.Valor.ListaPlanoBus = resMuestraPlano.Valor;
                else
                {
                    response.Mensaje = resMuestraPlano.Mensaje;
                    return response;
                }

                // Lista 'DestinosRuta'
                var resListarDestinosRuta = TurnoRepository.ListaDestinosRuta(resBuscarTurno.Valor.NroViaje, resBuscarTurno.Valor.CodiSucursal);
                if (resListarDestinosRuta.Estado)
                    resBuscarTurno.Valor.ListaDestinosRuta = resListarDestinosRuta.Valor;
                else
                {
                    response.Mensaje = resListarDestinosRuta.Mensaje;
                    return response;
                }

                response.EsCorrecto = true;
                response.Valor = resBuscarTurno.Valor;
                response.Mensaje = Message.MsgCorrectoMuestraTurno;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(TurnoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ItinerarioEntity>(false, null, Message.MsgErrExcMuestraTurno, false);
            }
        }
    }
}
