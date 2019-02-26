using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public static class VentaLogic
    {
        public static Response<string> GrabaVenta(VentaEntity entidad)
        {
            try
            {
                var response = new Response<string>(false, null, Message.MsgErrExcGrabaVenta, false);

                // Valida 'TerminalElectronico'
                var resValidarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal);
                if (resValidarTerminalElectronico.Estado)
                {
                    if (!string.IsNullOrEmpty(entidad.RucCliente))
                        entidad.CodiDocumento = 17.ToString();
                    else
                        entidad.CodiDocumento = 16.ToString();
                }
                else
                {
                    response.Mensaje = resValidarTerminalElectronico.Mensaje;
                    return response;
                }

                // Valida 'TerminalElectronico'
                var resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.CodiDocumento, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, short.Parse(resValidarTerminalElectronico.Valor.Tipo));
                if (!resValidarTerminalElectronico.Estado)
                {
                    response.Mensaje = resValidarTerminalElectronico.Mensaje;
                    return response;
                }

                switch (resValidarTerminalElectronico.Valor.Tipo)
                {
                    case "E":
                        {
                            if (resBuscarCorrelativo.Valor.SerieBoleto == 0 || resBuscarCorrelativo.Valor.NumeBoleto == 0)
                            {
                                response.Mensaje = "Error: SerieBoleto o NumeBoleto nulo.";
                                return response;
                            }
                            break;
                        }
                    case "M":
                        {
                            if (entidad.CodiDocumento == 17.ToString() && (resBuscarCorrelativo.Valor.SerieBoleto == 0 || resBuscarCorrelativo.Valor.NumeBoleto == 0))
                            {
                                resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, 16.ToString(), entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, short.Parse(resValidarTerminalElectronico.Valor.Tipo));
                                if (!resValidarTerminalElectronico.Estado)
                                {
                                    if (resBuscarCorrelativo.Valor.SerieBoleto == 0 || resBuscarCorrelativo.Valor.NumeBoleto == 0)
                                    {
                                        response.Mensaje = "Error: SerieBoleto o NumeBoleto nulo.";
                                        return response;
                                    }

                                    response.Mensaje = resValidarTerminalElectronico.Mensaje;
                                    return response;
                                }
                            }
                            break;
                        }
                }

                // Graba 'Venta'
                var resGrabarVenta = VentaRepository.GrabarVenta(entidad);
                if (!resGrabarVenta.Estado)
                {
                    response.Mensaje = resGrabarVenta.Mensaje;
                    return response;
                }



                //// Graba 'Facturacion Electrónica'
                //if (resValidarTerminalElectronico.Valor.Tipo == "E")
                //{

                //}



                // Valida 'LiquidacionVentas'
                var resValidarLiquidacionVentas = VentaRepository.ValidarLiquidacionVentas(entidad.CodiUsuario, DateTime.Now.ToString("dd/MM/yyyy"));
                if (!resValidarLiquidacionVentas.Estado)
                {
                    response.Mensaje = resValidarLiquidacionVentas.Mensaje;
                    return response;
                }

                // Actualiza 'LiquidacionVentas'
                if (resValidarLiquidacionVentas.Valor != 0)
                {
                    var resActualizarLiquidacionVentas = VentaRepository.ActualizarLiquidacionVentas(resValidarLiquidacionVentas.Valor, DateTime.Now.ToString("hh:mm tt"));
                    if (!resActualizarLiquidacionVentas.Estado)
                    {
                        response.Mensaje = resActualizarLiquidacionVentas.Mensaje;
                        return response;
                    }
                }
                // Graba 'LiquidacionVentas'
                else
                {
                    // Genera 'Correlativo'
                    var resGenerarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("LIQ_CAJA", "999", string.Empty);
                    if (!resGenerarCorrelativoAuxiliar.Estado)
                    {
                        response.Mensaje = resGenerarCorrelativoAuxiliar.Mensaje;
                        return response;
                    }

                    var resGrabarLiquidacionVentas = VentaRepository.GrabarLiquidacionVentas(int.Parse(resGenerarCorrelativoAuxiliar.Valor), entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiUsuario, entidad.PrecioVenta);
                    if (!resGrabarLiquidacionVentas.Estado)
                    {
                        response.Mensaje = resGrabarLiquidacionVentas.Mensaje;
                        return response;
                    }
                }

                // Graba 'Auditoria'
                var objAuditoriaEntity = new AuditoriaEntity
                {
                    CodiUsuario = entidad.CodiUsuario,
                    NomUsuario = entidad.NomUsuario,
                    Tabla = "VENTA",
                    TipoMovimiento = "ADICION",
                    Boleto = entidad.SerieBoleto + "-" + entidad.NumeBoleto,
                    NumeAsiento = entidad.NumeAsiento.ToString(),
                    NomOficina = entidad.NomOficina,
                    NomPuntoVenta = entidad.NomPuntoVenta,
                    Pasajero = entidad.Nombre,
                    FechaViaje = entidad.FechaViaje,
                    HoraViaje = entidad.HoraViaje,
                    NomDestino = entidad.NomDestino,
                    Precio = entidad.PrecioVenta,
                    Obs1 = "",
                    Obs2 = "",
                    Obs3 = "",
                    Obs4 = "",
                    Obs5 = ""
                };

                var resGrabarAuditoria = VentaRepository.GrabarAuditoria(objAuditoriaEntity);
                if (!resGrabarAuditoria.Estado)
                {
                    response.Mensaje = resGrabarAuditoria.Mensaje;
                    return response;
                }

                response.EsCorrecto = true;
                response.Valor = entidad.SerieBoleto + "-" + entidad.NumeBoleto;
                response.Mensaje = Message.MsgCorrectoGrabaVenta;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrExcGrabaVenta, false);
            }
        }
    }
}
