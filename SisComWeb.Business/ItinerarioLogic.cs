﻿using SisComWeb.Entity;
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
                if (request.TodosTurnos)
                    request.Hora = string.Empty;

                // Lista Itinerarios
                var buscarItinerarios = ItinerarioRepository.BuscarItinerarios(request.CodiOrigen, request.CodiDestino, request.CodiRuta, request.Hora, request.CodiServicio);
                
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
                        buscarItinerarios[i].RazonSocial = verificaCambiosTurnoViaje.NomEmpresa;

                        buscarItinerarios[i].CodiPuntoVenta = verificaCambiosTurnoViaje.CodiPuntoVenta;
                        buscarItinerarios[i].NomPuntoVenta = verificaCambiosTurnoViaje.NomPuntoVenta;
                    }

                    // Busca 'ProgramacionViaje'
                    var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(buscarItinerarios[i].NroViaje, buscarItinerarios[i].FechaProgramacion);
                    if (buscarProgramacionViaje > 0)
                    {
                        buscarItinerarios[i].CodiProgramacion = buscarProgramacionViaje;

                        // Obtiene 'BusProgramacion'
                        obtenerBus = ItinerarioRepository.ObtenerBusProgramacion(buscarItinerarios[i].CodiProgramacion);
                        buscarItinerarios[i].CodiBus = string.IsNullOrEmpty(obtenerBus.CodiBus) ? "0000" : obtenerBus.CodiBus;
                        buscarItinerarios[i].PlanoBus = string.IsNullOrEmpty(obtenerBus.PlanBus) ? "000" : obtenerBus.PlanBus;
                        buscarItinerarios[i].CapacidadBus = string.IsNullOrEmpty(obtenerBus.NumePasajeros) ? "0" : obtenerBus.NumePasajeros;
                        buscarItinerarios[i].PlacaBus = string.IsNullOrEmpty(obtenerBus.PlacBus) ? "00-0000" : obtenerBus.PlacBus;
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
                                buscarItinerarios[i].CodiBus = string.IsNullOrEmpty(obtenerBus.CodiBus) ? "0000" : obtenerBus.CodiBus;
                                buscarItinerarios[i].PlanoBus = string.IsNullOrEmpty(obtenerBus.PlanBus) ? "000" : obtenerBus.PlanBus;
                                buscarItinerarios[i].CapacidadBus = string.IsNullOrEmpty(obtenerBus.NumePasajeros) ? "0" : obtenerBus.NumePasajeros;
                                buscarItinerarios[i].PlacaBus = string.IsNullOrEmpty(obtenerBus.PlacBus) ? "00-0000" : obtenerBus.PlacBus;
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
                    buscarItinerarios[i].ProgramacionCerrada = ItinerarioRepository.ValidarProgramacionCerrada(buscarItinerarios[i].NroViaje, buscarItinerarios[i].FechaProgramacion);

                    // Obtiene 'TotalVentas'
                    if (buscarItinerarios[i].CodiProgramacion > 0)
                        buscarItinerarios[i].AsientosVendidos = ItinerarioRepository.ObtenerTotalVentas(buscarItinerarios[i].CodiProgramacion, buscarItinerarios[i].NroViaje, buscarItinerarios[i].CodiOrigen, buscarItinerarios[i].CodiDestino);

                    // Seteo 'Color'
                    buscarItinerarios[i].Color = GetColor(buscarItinerarios[i].ProgramacionCerrada, buscarItinerarios[i].AsientosVendidos, int.Parse(buscarItinerarios[i].CapacidadBus), buscarItinerarios[i].StOpcional);

                    // Seteo 'SecondColor'
                    buscarItinerarios[i].SecondColor = GetSecondColor(buscarItinerarios[i].AsientosVendidos, int.Parse(buscarItinerarios[i].CapacidadBus), buscarItinerarios[i].StOpcional);
                }

                return new Response<List<ItinerarioEntity>>(true, buscarItinerarios, Message.MsgCorrectoBuscaItinerarios, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ItinerarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ItinerarioEntity>>(false, null, Message.MsgExcBuscaItinerarios, false);
            }
        }

        public static string GetColor(string ProgramacionCerrada, int AsientosVendidos, int CapacidadBus, string StOpcional)
        {
            var color = string.Empty;
            if (ProgramacionCerrada != "0")
            {
                color = "#169BFF"; // Azul
            }
            else
            {
                if (AsientosVendidos == 0 && StOpcional.Equals("0"))
                {
                    color = "#FFFFFF"; // Blanco
                }
                else if (AsientosVendidos > 0 && AsientosVendidos < CapacidadBus && StOpcional.Equals("0"))
                {
                    color = "#A9E36A"; // Verde
                }
                else if (AsientosVendidos > 0 && AsientosVendidos == CapacidadBus && StOpcional.Equals("0"))
                {
                    color = "#E26B67"; // Rojo
                }
                else if (AsientosVendidos == 0 && StOpcional.Equals("1"))
                {
                    color = "#F7C06E"; // Naranja
                }
                else if (CapacidadBus == 0 && StOpcional.Equals("1"))
                {
                    color = "#F7C06E"; // Naranja
                }
                else if (AsientosVendidos > 0 && AsientosVendidos < CapacidadBus && StOpcional.Equals("1"))
                {
                    color = "#F7C06E"; // Naranja y Verde
                }
                else if (AsientosVendidos > 0 && AsientosVendidos == CapacidadBus && StOpcional.Equals("1"))
                {
                    color = "#F7C06E"; // Naranja y Rojo
                }
            }
            return color;
        }

        public static string GetSecondColor(int AsientosVendidos, int CapacidadBus, string StOpcional)
        {
            var color = string.Empty;
            if (AsientosVendidos > 0 && AsientosVendidos < CapacidadBus && StOpcional.Equals("1"))
            {
                color = "#A9E36A"; // Naranja y Verde
            }
            else if (AsientosVendidos > 0 && AsientosVendidos == CapacidadBus && StOpcional.Equals("1"))
            {
                color = "#E26B67"; // Naranja y Rojo
            }
            return color;
        }
    }
}
