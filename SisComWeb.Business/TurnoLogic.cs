using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Globalization;

namespace SisComWeb.Business
{
    public class TurnoLogic
    {
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

                //// Verifica cambios 'TurnoViaje'
                //var verificaCambiosTurnoViaje = ItinerarioRepository.VerificaCambiosTurnoViaje(buscarTurno.NroViaje, buscarTurno.FechaProgramacion);
                //if (verificaCambiosTurnoViaje.CodiEmpresa > 0)
                //{
                //    buscarTurno.CodiServicio = verificaCambiosTurnoViaje.CodiServicio;
                //    buscarTurno.NomServicio = verificaCambiosTurnoViaje.NomServicio;
                //    buscarTurno.CodiEmpresa = verificaCambiosTurnoViaje.CodiEmpresa;
                //    buscarTurno.RazonSocial = verificaCambiosTurnoViaje.NomEmpresa;
                //}

                // Busca 'ProgramacionViaje'
                var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(buscarTurno.NroViaje, buscarTurno.FechaProgramacion);
                if (buscarProgramacionViaje > 0)
                {
                    buscarTurno.CodiProgramacion = buscarProgramacionViaje;

                    // Obtiene 'BusProgramacion'
                    obtenerBus = ItinerarioRepository.ObtenerBusProgramacion(buscarTurno.CodiProgramacion);
                    buscarTurno.CodiBus = obtenerBus.CodiBus ?? "0000";
                    buscarTurno.PlanoBus = obtenerBus.PlanBus ?? "000";
                    buscarTurno.CapacidadBus = obtenerBus.NumePasajeros ?? "0";
                    buscarTurno.PlacaBus = obtenerBus.PlacBus ?? "00-0000";
                    buscarTurno.CodiChofer = obtenerBus.CodiChofer ?? "00000";
                    buscarTurno.NombreChofer = obtenerBus.NombreChofer ?? "NINGUNO";
                    buscarTurno.CodiCopiloto = obtenerBus.CodiCopiloto ?? "00000";
                    buscarTurno.NombreCopiloto = obtenerBus.NombreCopiloto ?? "NINGUNO";

                    buscarTurno.Activo = obtenerBus.Activo ?? "";
                }
                else
                {
                    // Obtiene 'BusEstandar'
                    obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarTurno.CodiEmpresa, buscarTurno.CodiSucursal, buscarTurno.CodiRuta, buscarTurno.CodiServicio, buscarTurno.HoraPartida);
                    if (!string.IsNullOrEmpty(obtenerBus.CodiBus))
                    {
                        buscarTurno.CodiBus = obtenerBus.CodiBus ?? "0000";
                        buscarTurno.PlanoBus = obtenerBus.PlanBus ?? "000";
                        buscarTurno.CapacidadBus = obtenerBus.NumePasajeros ?? "0";
                        buscarTurno.PlacaBus = obtenerBus.PlacBus ?? "00-0000";
                        buscarTurno.CodiChofer = obtenerBus.CodiChofer ?? "00000";
                        buscarTurno.NombreChofer = obtenerBus.NombreChofer ?? "NINGUNO";
                        buscarTurno.CodiCopiloto = obtenerBus.CodiCopiloto ?? "00000";
                        buscarTurno.NombreCopiloto = obtenerBus.NombreCopiloto ?? "NINGUNO";
                    }
                    else
                    {
                        // En caso de no encontrar resultado.
                        obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarTurno.CodiEmpresa, buscarTurno.CodiSucursal, buscarTurno.CodiRuta, buscarTurno.CodiServicio, string.Empty);
                        if (!string.IsNullOrEmpty(obtenerBus.CodiBus))
                        {
                            buscarTurno.CodiBus = obtenerBus.CodiBus ?? "0000";
                            buscarTurno.PlanoBus = obtenerBus.PlanBus ?? "000";
                            buscarTurno.CapacidadBus = obtenerBus.NumePasajeros ?? "0";
                            buscarTurno.PlacaBus = obtenerBus.PlacBus ?? "00-0000";
                            buscarTurno.CodiChofer = obtenerBus.CodiChofer ?? "00000";
                            buscarTurno.NombreChofer = obtenerBus.NombreChofer ?? "NINGUNO";
                            buscarTurno.CodiCopiloto = obtenerBus.CodiCopiloto ?? "00000";
                            buscarTurno.NombreCopiloto = obtenerBus.NombreCopiloto ?? "NINGUNO";
                        }
                        else
                        {
                            // En caso de no encontrar resultado
                            obtenerBus = ItinerarioRepository.ObtenerBusEstandar(buscarTurno.CodiEmpresa, buscarTurno.CodiSucursal, 0, buscarTurno.CodiServicio, string.Empty);
                            buscarTurno.CodiBus = obtenerBus.CodiBus ?? "0000";
                            buscarTurno.PlanoBus = obtenerBus.PlanBus ?? "000";
                            buscarTurno.CapacidadBus = obtenerBus.NumePasajeros ?? "0";
                            buscarTurno.PlacaBus = obtenerBus.PlacBus ?? "00-0000";
                            buscarTurno.CodiChofer = obtenerBus.CodiChofer ?? "00000";
                            buscarTurno.NombreChofer = obtenerBus.NombreChofer ?? "NINGUNO";
                            buscarTurno.CodiCopiloto = obtenerBus.CodiCopiloto ?? "00000";
                            buscarTurno.NombreCopiloto = obtenerBus.NombreCopiloto ?? "NINGUNO";
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

                // Consulta 'ManifiestoProgramacion'
                var resConsultaManifiestoProgramacion = ConsultaManifiestoProgramacion(buscarTurno.CodiProgramacion, request.CodiOrigen.ToString());
                if (resConsultaManifiestoProgramacion.Estado)
                    buscarTurno.X_Estado = resConsultaManifiestoProgramacion.Valor;
                else
                    buscarTurno.X_Estado = string.Empty;

                // Valida 'ProgramacionCerrada'
                var resValidarProgramacionCerrada = ItinerarioRepository.ValidarProgramacionCerrada(buscarTurno.NroViaje, buscarTurno.FechaProgramacion);
                if (resValidarProgramacionCerrada == 1)
                    buscarTurno.ProgramacionCerrada = true;

                // Obtiene 'TotalVentas'
                if (buscarTurno.CodiProgramacion > 0)
                    buscarTurno.AsientosVendidos = ItinerarioRepository.ObtenerTotalVentas(buscarTurno.CodiProgramacion, buscarTurno.NroViaje, buscarTurno.CodiOrigen, buscarTurno.CodiDestino);

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

                // Consulta 'BloqueoAsientoCantidad_Max'
                var consultaBloqueoAsientoCantidad_Max = TurnoRepository.ConsultaBloqueoAsientoCantidad_Max(request.CodiEmpresa);
                if (consultaBloqueoAsientoCantidad_Max == 0)
                    buscarTurno.CantidadMaxBloqAsi = 10;
                else
                    buscarTurno.CantidadMaxBloqAsi = consultaBloqueoAsientoCantidad_Max;

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
                            // Actualiza tabla 'BloqueoAsientos'
                            BloqueoAsientoRepository.ActualizarTablaBloqueoAsientos(buscarTurno.CodiProgramacion.ToString(), buscarTurno.NroViaje.ToString(), buscarTurno.FechaProgramacion);

                        // Como fue 'null', vuelve a consultar tabla 'BloqueoAsientos'
                        consultarTablaBloqueoAsientos = BloqueoAsientoRepository.ConsultarTablaBloqueoAsientos(auxCodiProgramacion, auxTipo, buscarTurno.FechaProgramacion);
                    }

                    // Seteo 'buscarTurno.TablaAsientosBloqueados'
                    buscarTurno.TablaBloqueoAsientos.AsientosOcupados = consultarTablaBloqueoAsientos.AsientosOcupados;
                    buscarTurno.TablaBloqueoAsientos.AsientosLiberados = consultarTablaBloqueoAsientos.AsientosLiberados;
                    buscarTurno.TablaBloqueoAsientos.CodiOrigen = consultarTablaBloqueoAsientos.CodiOrigen;
                    buscarTurno.TablaBloqueoAsientos.CodiDestino = consultarTablaBloqueoAsientos.CodiDestino;
                }

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
    }
}
