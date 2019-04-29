using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SisComWeb.Business
{
    public class ItinerarioLogic
    {
        public static Response<List<ItinerarioEntity>> BuscaItinerarios(ItinerarioRequest request)
        {
            try
            {
                var obtenerBus = new BusEntity();

                // Validar 'TodosTurnos'
                if (request.TodosTurnos == true) request.Hora = string.Empty;

                // Lista Itinerarios
                var buscarItinerarios = ItinerarioRepository.BuscarItinerarios(request.CodiOrigen, request.CodiDestino, request.CodiRuta, request.Hora);

                // Recorre cada registro
                for (int i = 0; i < buscarItinerarios.Count; i++)
                {
                    // Seteo 'CodiDestino' y 'Nom_Destino' (Recordar: Antes era traído de la BD)
                    if (request.CodiDestino > 0)
                    {
                        buscarItinerarios[i].CodiDestino = request.CodiDestino;
                        buscarItinerarios[i].NomDestino = request.NomDestino;
                    }
                    else
                    {
                        buscarItinerarios[i].CodiDestino = buscarItinerarios[i].CodiRuta;
                        buscarItinerarios[i].NomDestino = buscarItinerarios[i].NomRuta;
                    }

                    // Calcula 'FechaProgramacion'
                    var doubleDias = double.Parse(buscarItinerarios[i].Dias.ToString());
                    if (buscarItinerarios[i].Dias > 0)
                        buscarItinerarios[i].FechaProgramacion = DateTime.Parse(request.FechaViaje).AddDays(-doubleDias).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    else
                        buscarItinerarios[i].FechaProgramacion = DateTime.Parse(request.FechaViaje).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    // Seteo 'FechaViaje'
                    buscarItinerarios[i].FechaViaje = request.FechaViaje;

                    // Verifica cambios 'TurnoViaje'
                    var verificaCambiosTurnoViaje = ItinerarioRepository.VerificaCambiosTurnoViaje(buscarItinerarios[i].NroViaje, buscarItinerarios[i].FechaProgramacion);
                    if (verificaCambiosTurnoViaje.CodiEmpresa > 0)
                    {
                        buscarItinerarios[i].CodiServicio = verificaCambiosTurnoViaje.CodiServicio;
                        buscarItinerarios[i].NomServicio = verificaCambiosTurnoViaje.NomServicio;
                        buscarItinerarios[i].CodiEmpresa = verificaCambiosTurnoViaje.CodiEmpresa;
                    }

                    if (buscarItinerarios[i].NroViaje == 106231)
                    {

                    }

                    // Busca 'ProgramacionViaje'
                    var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(buscarItinerarios[i].NroViaje, buscarItinerarios[i].FechaProgramacion);
                    if (buscarProgramacionViaje > 0)
                    {
                        buscarItinerarios[i].CodiProgramacion = buscarProgramacionViaje;

                        // Obtiene 'BusProgramacion'
                        obtenerBus = ItinerarioRepository.ObtenerBusProgramacion(buscarItinerarios[i].CodiProgramacion);
                        buscarItinerarios[i].CodiBus = obtenerBus.CodiBus ?? "0000";
                        buscarItinerarios[i].PlanoBus = obtenerBus.PlanBus ?? "000";
                        buscarItinerarios[i].CapacidadBus = obtenerBus.NumePasajeros ?? "0";
                        buscarItinerarios[i].PlacaBus = obtenerBus.PlacBus ?? "00-0000";
                    }
                    else
                    {
                        // Valida 'SoloProgramados': No está en el flujo, se añadió luego.
                        if (request.SoloProgramados)
                        {
                            buscarItinerarios.RemoveAt(i);
                            i--;
                            continue;
                        }

                        // Obtiene 'BusEstandar'
                        obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarItinerarios[i].CodiEmpresa, buscarItinerarios[i].CodiSucursal, buscarItinerarios[i].CodiRuta, buscarItinerarios[i].CodiServicio, buscarItinerarios[i].HoraPartida);
                        if (!string.IsNullOrEmpty(obtenerBus.CodiBus))
                        {
                            buscarItinerarios[i].CodiBus = obtenerBus.CodiBus;
                            buscarItinerarios[i].PlanoBus = obtenerBus.PlanBus;
                            buscarItinerarios[i].CapacidadBus = obtenerBus.NumePasajeros;
                            buscarItinerarios[i].PlacaBus = obtenerBus.PlacBus;
                        }
                        else
                        {
                            // En caso de no encontrar resultado
                            obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarItinerarios[i].CodiEmpresa, buscarItinerarios[i].CodiSucursal, buscarItinerarios[i].CodiRuta, buscarItinerarios[i].CodiServicio, string.Empty);
                            if (!string.IsNullOrEmpty(obtenerBus.CodiBus))
                            {
                                buscarItinerarios[i].CodiBus = obtenerBus.CodiBus;
                                buscarItinerarios[i].PlanoBus = obtenerBus.PlanBus;
                                buscarItinerarios[i].CapacidadBus = obtenerBus.NumePasajeros;
                                buscarItinerarios[i].PlacaBus = obtenerBus.PlacBus;
                            }
                            else
                            {
                                // En caso de no encontrar resultado
                                obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarItinerarios[i].CodiEmpresa, buscarItinerarios[i].CodiSucursal, 0, buscarItinerarios[i].CodiServicio, string.Empty);
                                buscarItinerarios[i].CodiBus = obtenerBus.CodiBus ?? "0000";
                                buscarItinerarios[i].PlanoBus = obtenerBus.PlanBus ?? "000";
                                buscarItinerarios[i].CapacidadBus = obtenerBus.NumePasajeros ?? "0";
                                buscarItinerarios[i].PlacaBus = obtenerBus.PlacBus ?? "00-0000";
                            }
                        }
                    }

                    // Verifica cambios 'ValidarTurnoAdicional'
                    if (buscarItinerarios[i].StOpcional == "1" && DateTime.Parse(buscarItinerarios[i].FechaProgramacion).Date >= DateTime.Now.Date)
                    {
                        var validarTurnoAdicional = ItinerarioRepository.ValidarTurnoAdicional(buscarItinerarios[i].NroViaje, buscarItinerarios[i].FechaProgramacion);
                        if (validarTurnoAdicional != 1)
                        {
                            buscarItinerarios.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }

                    // Valida 'ProgramacionCerrada'
                    var validarProgramacionCerrada = ItinerarioRepository.ValidarProgramacionCerrada(buscarItinerarios[i].NroViaje, buscarItinerarios[i].FechaProgramacion);
                    if (validarProgramacionCerrada == 1)
                        buscarItinerarios[i].ProgramacionCerrada = true;

                    // Obtiene 'TotalVentas'
                    if (buscarItinerarios[i].CodiProgramacion > 0)
                        buscarItinerarios[i].AsientosVendidos = ItinerarioRepository.ObtenerTotalVentas(buscarItinerarios[i].CodiProgramacion, buscarItinerarios[i].CodiOrigen, buscarItinerarios[i].CodiDestino);
                }

                return new Response<List<ItinerarioEntity>>(true, buscarItinerarios, Message.MsgCorrectoBuscaItinerarios, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ItinerarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ItinerarioEntity>>(false, null, Message.MsgExcBuscaItinerarios, false);
            }
        }
    }
}
