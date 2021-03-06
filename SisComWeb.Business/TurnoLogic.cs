﻿using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace SisComWeb.Business
{
    public class TurnoLogic
    {
        private static readonly short DefaultCantMaxBloqAsi = short.Parse(ConfigurationManager.AppSettings["defaultCantMaxBloqAsi"]);


        public static Response<ItinerarioEntity> MuestraTurno(TurnoRequest request)
        {
            try
            {
                var obtenerBus = new BusEntity();

                // Busca Turno
                var buscarTurno = TurnoRepository.BuscarTurno(request);

                // Calcula 'FechaProgramacion'
                var doubleDias = double.Parse(buscarTurno.Dias.ToString());
                if (buscarTurno.Dias > 0)
                    buscarTurno.FechaProgramacion = DateTime.Parse(request.FechaViaje).AddDays(-doubleDias).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                else
                    buscarTurno.FechaProgramacion = DateTime.Parse(request.FechaViaje).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Busca 'ProgramacionViaje'
                var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(buscarTurno.NroViaje, buscarTurno.FechaProgramacion);
                if (buscarProgramacionViaje > 0)
                {
                    buscarTurno.CodiProgramacion = buscarProgramacionViaje;

                    // Obtiene 'BusProgramacion'
                    obtenerBus = ItinerarioRepository.ObtenerBusProgramacion(buscarTurno.CodiProgramacion);
                    buscarTurno.CodiBus = string.IsNullOrEmpty(obtenerBus.CodiBus) ? "0000" : obtenerBus.CodiBus;
                    buscarTurno.PlanoBus = string.IsNullOrEmpty(obtenerBus.PlanBus) ? "000" : obtenerBus.PlanBus;
                    buscarTurno.CapacidadBus = string.IsNullOrEmpty(obtenerBus.NumePasajeros) ? "0" : obtenerBus.NumePasajeros;
                    buscarTurno.PlacaBus = string.IsNullOrEmpty(obtenerBus.PlacBus) ? "00-0000" : obtenerBus.PlacBus;
                    buscarTurno.CodiChofer = string.IsNullOrEmpty(obtenerBus.CodiChofer) ? "00000" : obtenerBus.CodiChofer;
                    buscarTurno.NombreChofer = string.IsNullOrEmpty(obtenerBus.NombreChofer) ? "NINGUNO" : obtenerBus.NombreChofer;
                    buscarTurno.CodiCopiloto = string.IsNullOrEmpty(obtenerBus.CodiCopiloto) ? "00000" : obtenerBus.CodiCopiloto;
                    buscarTurno.NombreCopiloto = string.IsNullOrEmpty(obtenerBus.NombreCopiloto) ? "NINGUNO" : obtenerBus.NombreCopiloto;

                    buscarTurno.Activo = string.IsNullOrEmpty(obtenerBus.Activo) ? string.Empty : obtenerBus.Activo;
                }
                else
                {
                    // Obtiene 'BusEstandar'
                    obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarTurno.CodiEmpresa, buscarTurno.CodiSucursal, buscarTurno.CodiRuta, buscarTurno.CodiServicio, buscarTurno.HoraPartida);
                    if (!string.IsNullOrEmpty(obtenerBus.CodiBus))
                    {
                        buscarTurno.CodiBus = string.IsNullOrEmpty(obtenerBus.CodiBus) ? "0000" : obtenerBus.CodiBus;
                        buscarTurno.PlanoBus = string.IsNullOrEmpty(obtenerBus.PlanBus) ? "000" : obtenerBus.PlanBus;
                        buscarTurno.CapacidadBus = string.IsNullOrEmpty(obtenerBus.NumePasajeros) ? "0" : obtenerBus.NumePasajeros;
                        buscarTurno.PlacaBus = string.IsNullOrEmpty(obtenerBus.PlacBus) ? "00-0000" : obtenerBus.PlacBus;
                        buscarTurno.CodiChofer = string.IsNullOrEmpty(obtenerBus.CodiChofer) ? "00000" : obtenerBus.CodiChofer;
                        buscarTurno.NombreChofer = string.IsNullOrEmpty(obtenerBus.NombreChofer) ? "NINGUNO" : obtenerBus.NombreChofer;
                        buscarTurno.CodiCopiloto = string.IsNullOrEmpty(obtenerBus.CodiCopiloto) ? "00000" : obtenerBus.CodiCopiloto;
                        buscarTurno.NombreCopiloto = string.IsNullOrEmpty(obtenerBus.NombreCopiloto) ? "NINGUNO" : obtenerBus.NombreCopiloto;
                    }
                    else
                    {
                        // En caso de no encontrar resultado.
                        obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarTurno.CodiEmpresa, buscarTurno.CodiSucursal, buscarTurno.CodiRuta, buscarTurno.CodiServicio, string.Empty);
                        if (!string.IsNullOrEmpty(obtenerBus.CodiBus))
                        {
                            buscarTurno.CodiBus = string.IsNullOrEmpty(obtenerBus.CodiBus) ? "0000" : obtenerBus.CodiBus;
                            buscarTurno.PlanoBus = string.IsNullOrEmpty(obtenerBus.PlanBus) ? "000" : obtenerBus.PlanBus;
                            buscarTurno.CapacidadBus = string.IsNullOrEmpty(obtenerBus.NumePasajeros) ? "0" : obtenerBus.NumePasajeros;
                            buscarTurno.PlacaBus = string.IsNullOrEmpty(obtenerBus.PlacBus) ? "00-0000" : obtenerBus.PlacBus;
                            buscarTurno.CodiChofer = string.IsNullOrEmpty(obtenerBus.CodiChofer) ? "00000" : obtenerBus.CodiChofer;
                            buscarTurno.NombreChofer = string.IsNullOrEmpty(obtenerBus.NombreChofer) ? "NINGUNO" : obtenerBus.NombreChofer;
                            buscarTurno.CodiCopiloto = string.IsNullOrEmpty(obtenerBus.CodiCopiloto) ? "00000" : obtenerBus.CodiCopiloto;
                            buscarTurno.NombreCopiloto = string.IsNullOrEmpty(obtenerBus.NombreCopiloto) ? "NINGUNO" : obtenerBus.NombreCopiloto;
                        }
                        else
                        {
                            // En caso de no encontrar resultado
                            obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarTurno.CodiEmpresa, buscarTurno.CodiSucursal, 0, buscarTurno.CodiServicio, string.Empty);
                            buscarTurno.CodiBus = string.IsNullOrEmpty(obtenerBus.CodiBus) ? "0000" : obtenerBus.CodiBus;
                            buscarTurno.PlanoBus = string.IsNullOrEmpty(obtenerBus.PlanBus) ? "000" : obtenerBus.PlanBus;
                            buscarTurno.CapacidadBus = string.IsNullOrEmpty(obtenerBus.NumePasajeros) ? "0" : obtenerBus.NumePasajeros;
                            buscarTurno.PlacaBus = string.IsNullOrEmpty(obtenerBus.PlacBus) ? "00-0000" : obtenerBus.PlacBus;
                            buscarTurno.CodiChofer = string.IsNullOrEmpty(obtenerBus.CodiChofer) ? "00000" : obtenerBus.CodiChofer;
                            buscarTurno.NombreChofer = string.IsNullOrEmpty(obtenerBus.NombreChofer) ? "NINGUNO" : obtenerBus.NombreChofer;
                            buscarTurno.CodiCopiloto = string.IsNullOrEmpty(obtenerBus.CodiCopiloto) ? "00000" : obtenerBus.CodiCopiloto;
                            buscarTurno.NombreCopiloto = string.IsNullOrEmpty(obtenerBus.NombreCopiloto) ? "NINGUNO" : obtenerBus.NombreCopiloto;
                        }
                    }
                }

                // Verifica cambios 'ValidarTurnoAdicional'
                if (buscarTurno.StOpcional == "1" && DateTime.Parse(buscarTurno.FechaProgramacion).Date >= DateTime.Now.Date)
                {
                    var validarTurnoAdicional = ItinerarioRepository.ValidarTurnoAdicional(buscarTurno.NroViaje, buscarTurno.FechaProgramacion);
                    if (validarTurnoAdicional != 1)
                        return new Response<ItinerarioEntity>(false, buscarTurno, Message.MsgErrorValidarTurnoAdicional, true);
                }

                // Valida 'ProgramacionCerrada'
                buscarTurno.ProgramacionCerrada = ItinerarioRepository.ValidarProgramacionCerrada(buscarTurno.NroViaje, buscarTurno.FechaProgramacion);
                if (buscarTurno.ProgramacionCerrada != "0")
                {
                    var auxMsgErrorValidarProgramacionCerrada = string.Empty;
                    switch (buscarTurno.ProgramacionCerrada)
                    {
                        case "1":
                            auxMsgErrorValidarProgramacionCerrada = Message.MsgErrorValidarProgramacionCerrada_1;
                            break;
                        case "2":
                            auxMsgErrorValidarProgramacionCerrada = Message.MsgErrorValidarProgramacionCerrada_2;
                            break;
                    };

                    return new Response<ItinerarioEntity>(false, buscarTurno, auxMsgErrorValidarProgramacionCerrada, true);
                }

                // Obtiene 'TotalVentas'
                if (buscarTurno.CodiProgramacion > 0)
                    buscarTurno.AsientosVendidos = ItinerarioRepository.ObtenerTotalVentas(buscarTurno.CodiProgramacion, buscarTurno.NroViaje, buscarTurno.CodiOrigen, buscarTurno.CodiDestino);

                // Seteo 'Color'
                buscarTurno.Color = ItinerarioLogic.GetColor(buscarTurno.ProgramacionCerrada, buscarTurno.AsientosVendidos, int.Parse(buscarTurno.CapacidadBus), buscarTurno.StOpcional);

                // Seteo 'SecondColor'
                buscarTurno.SecondColor = ItinerarioLogic.GetSecondColor(buscarTurno.AsientosVendidos, int.Parse(buscarTurno.CapacidadBus), buscarTurno.StOpcional);

                // Consulta 'ManifiestoProgramacion'
                var consultaManifiestoProgramacion = ConsultaManifiestoProgramacion(buscarTurno.CodiProgramacion, request.CodiOrigen.ToString());
                if (consultaManifiestoProgramacion.Estado)
                    buscarTurno.X_Estado = consultaManifiestoProgramacion.Valor;
                else
                    buscarTurno.X_Estado = string.Empty;

                // Lista 'PuntosEmbarque'
                var listarPuntosEmbarque = TurnoRepository.ListarPuntosEmbarque(buscarTurno.CodiOrigen, buscarTurno.CodiDestino, buscarTurno.CodiServicio, buscarTurno.CodiEmpresa, buscarTurno.CodiPuntoVenta, buscarTurno.HoraPartida);
                if (listarPuntosEmbarque.Count == 0)
                {
                    var objPuntoEntity = new PuntoEntity
                    {
                        CodiPuntoVenta = buscarTurno.CodiOrigen,
                        Lugar = buscarTurno.NomOrigen,
                        Hora = buscarTurno.HoraPartida
                    };
                    listarPuntosEmbarque.Add(objPuntoEntity);
                }
                buscarTurno.ListaEmbarques = listarPuntosEmbarque;

                // Lista 'PuntosArribo'
                buscarTurno.ListaArribos = TurnoRepository.ListarPuntosArribo(buscarTurno.CodiOrigen, buscarTurno.CodiDestino, buscarTurno.CodiServicio, buscarTurno.CodiEmpresa, buscarTurno.CodiPuntoVenta, buscarTurno.HoraPartida);

                // Lista 'DestinosRuta'
                buscarTurno.ListaDestinosRuta = TurnoRepository.ListaDestinosRuta(buscarTurno.NroViaje, buscarTurno.CodiSucursal);

                // Lista 'ResumenProgramacion'
                buscarTurno.ListaResumenProgramacion = TurnoRepository.ListaResumenProgramacion(buscarTurno.CodiProgramacion, request.CodiSucursalUsuario);
                // 'ResumenProgramacion' Capacidad del Bus
                buscarTurno.ListaResumenProgramacion.CAP = buscarTurno.CapacidadBus;
                // 'ResumenProgramacion' Libres
                buscarTurno.ListaResumenProgramacion.LBR = Convert.ToString(Convert.ToDecimal(buscarTurno.ListaResumenProgramacion.CAP) - (Convert.ToDecimal(buscarTurno.ListaResumenProgramacion.VTT)
                    + Convert.ToDecimal(buscarTurno.ListaResumenProgramacion.PAS) + Convert.ToDecimal(buscarTurno.ListaResumenProgramacion.RET) + Convert.ToDecimal(buscarTurno.ListaResumenProgramacion.RVA)));
                // 'ResumenProgramacion' Total
                buscarTurno.ListaResumenProgramacion.TOT = Convert.ToString(Convert.ToDecimal(buscarTurno.ListaResumenProgramacion.CAP) - Convert.ToDecimal(buscarTurno.ListaResumenProgramacion.LBR)
                    - Convert.ToDecimal(buscarTurno.ListaResumenProgramacion.RVA));

                // Elimina 'Reservas' por escala
                if (buscarTurno.CodiProgramacion > 0)
                {
                    var auxHora = string.Empty;

                    var objPuntoEmbarque = buscarTurno.ListaEmbarques.Find(x => x.CodiPuntoVenta == request.CodiPvUsuario);
                    if (objPuntoEmbarque != null)
                        auxHora = objPuntoEmbarque.Hora;
                    else
                        auxHora = buscarTurno.HoraPartida;

                    TurnoRepository.EliminarReservas02(buscarTurno.CodiOrigen.ToString(), buscarTurno.CodiProgramacion, auxHora, buscarTurno.FechaViaje);
                    TurnoRepository.EliminarReservas01(buscarTurno.CodiProgramacion, auxHora);
                }

                // Consulta 'BloqueoAsientoCantidad_Max'
                var consultaBloqueoAsientoCantidad_Max = TurnoRepository.ConsultaBloqueoAsientoCantidad_Max(request.CodiEmpresa);
                if (consultaBloqueoAsientoCantidad_Max == 0)
                    buscarTurno.CantidadMaxBloqAsi = DefaultCantMaxBloqAsi;
                else
                    buscarTurno.CantidadMaxBloqAsi = consultaBloqueoAsientoCantidad_Max;

                // Consulta tabla 'AsientosBloqueados'
                var consultarTablaAsientosBloqueados = BloqueoAsientoRepository.ConsultarTablaAsientosBloqueados(buscarTurno.CodiEmpresa, buscarTurno.CodiSucursal, buscarTurno.CodiRuta, buscarTurno.CodiServicio, buscarTurno.HoraPartida);

                buscarTurno.TablaBloqueoAsientos = new TablaBloqueoAsientosEntity()
                {
                    AsientosOcupados = string.Empty,
                    AsientosLiberados = string.Empty
                };

                // Consulta tabla 'BloqueoAsientos'
                if (!string.IsNullOrEmpty(consultarTablaAsientosBloqueados.Asientos))
                {
                    var auxCodiProgramacion = buscarTurno.CodiProgramacion > 0 ? buscarTurno.CodiProgramacion : buscarTurno.NroViaje;
                    var auxTipo = buscarTurno.CodiProgramacion > 0 ? "P" : "V";

                    var consultarTablaBloqueoAsientos = BloqueoAsientoRepository.ConsultarTablaBloqueoAsientos(auxCodiProgramacion, auxTipo, buscarTurno.FechaProgramacion);

                    if (consultarTablaBloqueoAsientos == null)
                    {
                        if (buscarTurno.CodiProgramacion <= 0)
                        {
                            // Inserta tabla 'BloqueoAsientos'
                            var requestTBA = new TablaBloqueoAsientosRequest()
                            {
                                CodiProgramacion = auxCodiProgramacion,
                                CodiOrigen = request.CodiOrigen,
                                CodiDestino = request.CodiDestino,
                                AsientosOcupados = consultarTablaAsientosBloqueados.Asientos,
                                AsientosLiberados = string.Empty,
                                Tipo = auxTipo,
                                Fecha = buscarTurno.FechaProgramacion
                            };
                            BloqueoAsientoRepository.InsertarTablaBloqueoAsientos(requestTBA);
                        }
                        else
                        {
                            // Volvemos a consultar tabla 'BloqueoAsientos'
                            consultarTablaBloqueoAsientos = BloqueoAsientoRepository.ConsultarTablaBloqueoAsientos(buscarTurno.NroViaje, "V", buscarTurno.FechaProgramacion);

                            if (consultarTablaBloqueoAsientos == null)
                            {
                                // Inserta tabla 'BloqueoAsientos'
                                var requestTBA = new TablaBloqueoAsientosRequest()
                                {
                                    CodiProgramacion = auxCodiProgramacion,
                                    CodiOrigen = request.CodiOrigen,
                                    CodiDestino = request.CodiDestino,
                                    AsientosOcupados = consultarTablaAsientosBloqueados.Asientos,
                                    AsientosLiberados = string.Empty,
                                    Tipo = auxTipo,
                                    Fecha = buscarTurno.FechaProgramacion
                                };
                                BloqueoAsientoRepository.InsertarTablaBloqueoAsientos(requestTBA);
                            }
                            else
                                // Actualiza tabla 'BloqueoAsientos'
                                BloqueoAsientoRepository.ActualizarTablaBloqueoAsientos(buscarTurno.CodiProgramacion.ToString(), buscarTurno.NroViaje.ToString(), buscarTurno.FechaProgramacion);
                        }

                        // Como fue 'null', vuelve a consultar tabla 'BloqueoAsientos'
                        consultarTablaBloqueoAsientos = BloqueoAsientoRepository.ConsultarTablaBloqueoAsientos(auxCodiProgramacion, auxTipo, buscarTurno.FechaProgramacion);
                    }

                    // Seteo 'buscarTurno.TablaAsientosBloqueados'
                    buscarTurno.TablaBloqueoAsientos.AsientosOcupados = consultarTablaBloqueoAsientos.AsientosOcupados;
                    buscarTurno.TablaBloqueoAsientos.AsientosLiberados = consultarTablaBloqueoAsientos.AsientosLiberados;
                    buscarTurno.TablaBloqueoAsientos.CodiOrigen = consultarTablaBloqueoAsientos.CodiOrigen;
                    buscarTurno.TablaBloqueoAsientos.CodiDestino = consultarTablaBloqueoAsientos.CodiDestino;
                }

                // Actualiza 'TbViajeProgramacionCantidad'
                if (buscarTurno.CodiProgramacion > 0)
                    TurnoRepository.ActualizarTbViajeProgramacionCantidad(buscarTurno.CodiProgramacion);

                // Lista 'PlanoBus'
                PlanoRequest requestPlano = new PlanoRequest
                {
                    PlanoBus = buscarTurno.PlanoBus,
                    CodiProgramacion = buscarTurno.CodiProgramacion,
                    CodiOrigen = buscarTurno.CodiOrigen,
                    CodiDestino = buscarTurno.CodiDestino,
                    CodiBus = buscarTurno.CodiBus,
                    HoraViaje = request.HoraViaje,
                    FechaViaje = request.FechaViaje,
                    CodiServicio = buscarTurno.CodiServicio,
                    CodiEmpresa = buscarTurno.CodiEmpresa,
                    FechaProgramacion = buscarTurno.FechaProgramacion,
                    NroViaje = buscarTurno.NroViaje,
                    CodiSucursal = buscarTurno.CodiSucursal,
                    CodiRuta = buscarTurno.CodiRuta
                };
                var resMuestraPlano = PlanoLogic.MuestraPlano(requestPlano);
                if (resMuestraPlano.Estado)
                {
                    if (resMuestraPlano.EsCorrecto)
                        buscarTurno.ListaPlanoBus = resMuestraPlano.Valor;
                    else
                        return new Response<ItinerarioEntity>(false, buscarTurno, resMuestraPlano.Mensaje, true);
                }
                else
                    return new Response<ItinerarioEntity>(false, buscarTurno, resMuestraPlano.Mensaje, false);

                // Seteo de 'Cantidad' por 'DestinosRuta'
                foreach (var destinoRuta in buscarTurno.ListaDestinosRuta)
                {
                    destinoRuta.Cantidad = resMuestraPlano.Valor.Count(x => x.CodiDestino == destinoRuta.CodiSucursal && x.CodiOrigen == request.CodiSucursalUsuario && (x.FlagVenta != "X" && x.FlagVenta != "R" && x.FlagVenta != "O"));
                }

                return new Response<ItinerarioEntity>(true, buscarTurno, Message.MsgCorrectoMuestraTurno, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(TurnoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ItinerarioEntity>(false, null, Message.MsgExcMuestraTurno, false);
            }
        }

        public static Response<string> ConsultaManifiestoProgramacion(int Prog, string Suc)
        {
            try
            {
                var valor = string.Empty;

                var consultaManifiestoProgramacion = TurnoRepository.ConsultaManifiestoProgramacion(Prog, Suc);
                if (!string.IsNullOrEmpty(consultaManifiestoProgramacion.NumeManifiesto)
                    && !string.IsNullOrEmpty(consultaManifiestoProgramacion.Est)
                    && consultaManifiestoProgramacion.Est != "0")

                    valor = "X";

                return new Response<string>(true, valor, Message.MsgCorrectoConsultaManifiestoProgramacion, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaManifiestoProgramacion, false);
            }
        }

        public static Response<bool> ObtenerStAnulacion(string CodTab, int Pv, string F)
        {
            try
            {
                var valor = new bool();
                var ListarPanelControl = CreditoRepository.ListarPanelControl();

                var objPanelCanAnuPorDia = ListarPanelControl.Find(x => x.CodiPanel == "65");
                if (objPanelCanAnuPorDia != null && objPanelCanAnuPorDia.Valor == "1")
                {
                    var consultaPosCNT = TurnoRepository.ConsultaPosCNT(CodTab, Pv.ToString()); // CodEmp -> Usuario.CodiPuntoVenta
                    if (int.Parse(consultaPosCNT) == -1) // Si no existe en la tabla, dejar anular.
                        valor = true;
                    else
                    {
                        var consultaAnulacionPorDia = TurnoRepository.ConsultaAnulacionPorDia(Pv, F);
                        if (int.Parse(consultaPosCNT) > consultaAnulacionPorDia)
                            valor = true;
                    }
                }
                else
                    valor = true;

                return new Response<bool>(true, valor, Message.MsgCorrectoObtenerStAnulacion, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcObtenerStAnulacion, false);
            }
        }

        public static Response<List<DestinoRutaEntity>> GetNewListaDestinosPas(byte CodiEmpresa, short CodiOrigenPas, short CodiOrigenBus, short CodiPuntoVentaBus, short CodiDestinoBus, string Turno, byte CodiServicio, int NroViaje)
        {
            try
            {
                var ListaDestinosRuta = new List<DestinoRutaEntity>();
                var buscarNroViaje = TurnoRepository.BuscarNroViaje(CodiEmpresa, CodiOrigenPas, CodiOrigenBus, CodiPuntoVentaBus, CodiDestinoBus, Turno, CodiServicio);

                if (buscarNroViaje > 0)
                {
                    ListaDestinosRuta = TurnoRepository.ListaDestinosRuta(buscarNroViaje, CodiOrigenPas);

                    if (ListaDestinosRuta.Count > 0)
                        return new Response<List<DestinoRutaEntity>>(true, ListaDestinosRuta, Message.MsgCorrectoGetNewListaDestinosPas, true);
                    else
                        return new Response<List<DestinoRutaEntity>>(false, ListaDestinosRuta, Message.MsgErrorGetNewListaDestinosPas, true);
                }
                else
                    return new Response<List<DestinoRutaEntity>>(false, ListaDestinosRuta, Message.MsgErrorGetNewListaDestinosPas, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<DestinoRutaEntity>>(false, null, Message.MsgExcGetNewListaDestinosPas, false);
            }
        }
    }
}
