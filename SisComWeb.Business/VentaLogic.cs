using SisComWeb.Business.ServiceFE;
using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Entity.Peticiones.Response;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SisComWeb.Business
{
    public static class VentaLogic
    {
        public static Response<string> BuscaCorrelativo(CorrelativoRequest request)
        {
            try
            {
                var response = new Response<string>(false, null, "Error: BuscaCorrelativo.", false);
                var auxBoletoCompleto = string.Empty;

                // Valida 'TerminalElectronico'
                var resValidarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(request.CodiEmpresa, request.CodiSucursal, request.CodiPuntoVenta, short.Parse(request.CodiTerminal));
                if (resValidarTerminalElectronico.Estado)
                {
                    if (string.IsNullOrEmpty(resValidarTerminalElectronico.Valor.Tipo))
                        resValidarTerminalElectronico.Valor.Tipo = "M";
                }
                else
                {
                    response.Mensaje = resValidarTerminalElectronico.Mensaje;
                    return response;
                }

                // Busca 'Correlativo'
                var resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, request.CodiDocumento, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, resValidarTerminalElectronico.Valor.Tipo);
                if (!resValidarTerminalElectronico.Estado)
                {
                    response.Mensaje = resValidarTerminalElectronico.Mensaje;
                    return response;
                }

                // Seteo 'auxBoletoCompleto'
                switch (resValidarTerminalElectronico.Valor.Tipo)
                {
                    case "M":
                        {
                            auxBoletoCompleto = "0" + resBuscarCorrelativo.Valor.SerieBoleto.ToString("D3").ToString() + "-" + (resBuscarCorrelativo.Valor.NumeBoleto + 1).ToString("D7").ToString();

                            break;
                        }
                    case "E":
                        {
                            switch (request.CodiDocumento)
                            {
                                case "17":
                                    auxBoletoCompleto = "F" + resBuscarCorrelativo.Valor.SerieBoleto.ToString("D3").ToString() + "-" + (resBuscarCorrelativo.Valor.NumeBoleto + 1).ToString("D7").ToString();
                                    break;
                                case "16":
                                    auxBoletoCompleto = "B" + resBuscarCorrelativo.Valor.SerieBoleto.ToString("D3").ToString() + "-" + (resBuscarCorrelativo.Valor.NumeBoleto + 1).ToString("D7").ToString();
                                    break;
                            }

                            break;
                        }
                }

                response.EsCorrecto = true;
                response.Valor = auxBoletoCompleto;
                response.Mensaje = "Correct : BuscaCorrelativo.";
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrExcBuscaCorrelativo, false);
            }
        }

        public static Response<string> GrabaVenta(List<VentaEntity> listado, string FlagVenta)
        {
            try
            {
                var response = new Response<string>(false, null, "Error: GrabaVenta.", false);
                string valor = string.Empty;

                foreach (var entidad in listado)
                {
                    string auxBoletoCompleto = string.Empty;
                    string auxNumeCaja = string.Empty;
                    entidad.UserWebSUNAT = "WEBPASAJES";

                    Response<int> resGrabarVentaFechaAbierta = new Response<int>(false, 0, "Error: GrabarVentaFechaAbierta.", false);
                    Response<int> resGrabarVenta = new Response<int>(false, 0, "Error: GrabarVenta.", false);

                    // RESERVA
                    if (FlagVenta == "R")
                    {
                        // Valida 'SaldoPaseCortesia'
                        var resEliminarReserva = VentaRepository.EliminarReserva(entidad.IdVenta);
                        if (!resEliminarReserva.Estado)
                        {
                            response.Mensaje = resEliminarReserva.Mensaje;
                            return response;
                        }
                    }

                    // Busca 'ProgramacionViaje'
                    var resBuscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(entidad.NroViaje, entidad.FechaProgramacion);
                    if (resBuscarProgramacionViaje.Estado)
                    {
                        if (resBuscarProgramacionViaje.Valor == 0)
                        {
                            // Genera 'CorrelativoAuxiliar'
                            var resGenerarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("TB_PROGRAMACION", "999", "", string.Empty);
                            if (resGenerarCorrelativoAuxiliar.Estado)
                                entidad.CodiProgramacion = int.Parse(resGenerarCorrelativoAuxiliar.Valor);
                            else
                            {
                                response.Mensaje = resGenerarCorrelativoAuxiliar.Mensaje;
                                return response;
                            }

                            var objProgramacion = new ProgramacionEntity
                            {
                                CodiProgramacion = entidad.CodiProgramacion,
                                CodiEmpresa = entidad.CodiEmpresa,
                                CodiSucursal = entidad.CodiSucursal,
                                CodiRuta = entidad.CodiRuta,
                                CodiBus = entidad.CodiBus,
                                FechaProgramacion = entidad.FechaProgramacion,
                                HoraProgramacion = entidad.HoraProgramacion,
                                CodiServicio = entidad.CodiServicio
                            };

                            // Graba 'Programacion'
                            var resGrabarProgramacion = VentaRepository.GrabarProgramacion(objProgramacion);
                            if (!resGrabarProgramacion.Estado)
                            {
                                response.Mensaje = resGrabarProgramacion.Mensaje;
                                return response;
                            }

                            // Graba 'ViajeProgramacion'
                            var resGrabarViajeProgramacion = VentaRepository.GrabarViajeProgramacion(entidad.NroViaje, entidad.CodiProgramacion, entidad.FechaProgramacion, entidad.CodiBus);
                            if (!resGrabarViajeProgramacion.Estado)
                            {
                                response.Mensaje = resGrabarViajeProgramacion.Mensaje;
                                return response;
                            }
                        }
                        else
                            entidad.CodiProgramacion = resBuscarProgramacionViaje.Valor;
                    }
                    else
                        return response;

                    // Valida 'TerminalElectronico'
                    var resValidarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, short.Parse(entidad.CodiTerminal));
                    if (resValidarTerminalElectronico.Estado)
                    {
                        if (string.IsNullOrEmpty(resValidarTerminalElectronico.Valor.Tipo))
                            resValidarTerminalElectronico.Valor.Tipo = "M";
                    }
                    else
                    {
                        response.Mensaje = resValidarTerminalElectronico.Mensaje;
                        return response;
                    }

                    // PASE DE CORTESÍA
                    if (FlagVenta == "7")
                    {
                        // Valida 'SaldoPaseCortesia'
                        var resValidarSaldoPaseCortesia = PaseRepository.ValidarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Año);
                        if (resValidarSaldoPaseCortesia.Estado)
                        {
                            if (resValidarSaldoPaseCortesia.Valor != 1)
                            {
                                response.EsCorrecto = true;
                                response.Valor = valor;
                                response.Mensaje = Message.MsgCorrectoGrabaPase;
                                response.Estado = true;

                                return response;
                            }
                        }
                        else
                        {
                            response.Mensaje = resValidarSaldoPaseCortesia.Mensaje;
                            return response;
                        }
                    }

                    // Seteo 'CodiDocumento'
                    if (!string.IsNullOrEmpty(entidad.RucCliente))
                    {
                        // Seteo 'AuxCodigoBF_Interno'
                        switch (FlagVenta)
                        {
                            case "V": // VENTA
                                entidad.AuxCodigoBF_Interno = "17";
                                break;
                            case "7": // PASE DE CORTESÍA
                                entidad.AuxCodigoBF_Interno = "78";
                                break;
                        }
                        // Seteo 'CodiDocumento'
                        entidad.CodiDocumento = "01"; // Factura
                    }
                    else
                    {
                        // Seteo 'AuxCodigoBF_Interno'
                        switch (FlagVenta)
                        {
                            case "V": // VENTA
                                entidad.AuxCodigoBF_Interno = "16";
                                break;
                            case "7": // PASE DE CORTESÍA
                                entidad.AuxCodigoBF_Interno = "77";
                                break;
                        }
                        // Seteo 'CodiDocumento'
                        entidad.CodiDocumento = "03"; // Boleta
                    }

                    // Busca 'Correlativo'
                    var resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, resValidarTerminalElectronico.Valor.Tipo);
                    if (resBuscarCorrelativo.Estado)
                    {
                        entidad.SerieBoleto = resBuscarCorrelativo.Valor.SerieBoleto;
                        entidad.NumeBoleto = resBuscarCorrelativo.Valor.NumeBoleto + 1;
                    }
                    else
                    {
                        response.Mensaje = resBuscarCorrelativo.Mensaje;
                        return response;
                    }

                    if (resBuscarCorrelativo.Valor.SerieBoleto == 0)
                    {
                        switch (resValidarTerminalElectronico.Valor.Tipo)
                        {
                            case "M":
                                {
                                    switch (entidad.CodiDocumento)
                                    {
                                        case "01": // Factura
                                            {
                                                if (FlagVenta == "7")
                                                {
                                                    // Seteo 'CodiBF Interno'
                                                    entidad.AuxCodigoBF_Interno = "77";
                                                    // Seteo 'CodiDocumento'
                                                    entidad.CodiDocumento = "03"; // Boleta
                                                    // Busca 'Correlativo'
                                                    resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, resValidarTerminalElectronico.Valor.Tipo);
                                                    if (!resBuscarCorrelativo.Estado)
                                                    {
                                                        response.Mensaje = resBuscarCorrelativo.Mensaje;
                                                        return response;
                                                    }
                                                }

                                                if (resBuscarCorrelativo.Valor.SerieBoleto == 0)
                                                {
                                                    // Seteo 'CodiBF Interno'
                                                    entidad.AuxCodigoBF_Interno = "16";
                                                    // Seteo 'CodiDocumento'
                                                    entidad.CodiDocumento = "03"; // Boleta

                                                    // Busca 'Correlativo'
                                                    resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, resValidarTerminalElectronico.Valor.Tipo);
                                                    if (resBuscarCorrelativo.Estado)
                                                    {
                                                        if (resBuscarCorrelativo.Valor.SerieBoleto == 0)
                                                        {
                                                            response.Mensaje = "Error: SerieBoleto igual a 0.";
                                                            return response;
                                                        }
                                                        else
                                                        {
                                                            entidad.SerieBoleto = resBuscarCorrelativo.Valor.SerieBoleto;
                                                            entidad.NumeBoleto = resBuscarCorrelativo.Valor.NumeBoleto + 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        response.Mensaje = resBuscarCorrelativo.Mensaje;
                                                        return response;
                                                    }
                                                }
                                                else
                                                {
                                                    entidad.SerieBoleto = resBuscarCorrelativo.Valor.SerieBoleto;
                                                    entidad.NumeBoleto = resBuscarCorrelativo.Valor.NumeBoleto + 1;
                                                }

                                                break;
                                            }
                                        case "03": // Boleta
                                            {
                                                if (FlagVenta == "7")
                                                {
                                                    // Seteo 'CodiBF Interno'
                                                    entidad.AuxCodigoBF_Interno = "16";
                                                    // Busca 'Correlativo'
                                                    resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, resValidarTerminalElectronico.Valor.Tipo);
                                                    if (!resBuscarCorrelativo.Estado)
                                                    {
                                                        response.Mensaje = resBuscarCorrelativo.Mensaje;
                                                        return response;
                                                    }
                                                }

                                                if (resBuscarCorrelativo.Valor.SerieBoleto == 0)
                                                {
                                                    response.Mensaje = "Error: SerieBoleto igual a 0.";
                                                    return response;
                                                }
                                                else
                                                {
                                                    entidad.SerieBoleto = resBuscarCorrelativo.Valor.SerieBoleto;
                                                    entidad.NumeBoleto = resBuscarCorrelativo.Valor.NumeBoleto + 1;
                                                }

                                                break;
                                            }
                                    }

                                    break;
                                }
                            case "E":
                                {
                                    response.Mensaje = "Error: SerieBoleto igual a 0.";
                                    return response;
                                }
                        }
                    }

                    // Seteo 'Tipo'
                    switch (resValidarTerminalElectronico.Valor.Tipo)
                    {
                        case "M":
                            {
                                entidad.Tipo = "M";

                                break;
                            }
                        case "E":
                            {
                                if (!string.IsNullOrEmpty(entidad.RucCliente))
                                    entidad.Tipo = "F";
                                else
                                    entidad.Tipo = "B";

                                break;
                            }
                    }

                    // Graba 'Venta', 'Otros' y 'FacturaciónElectrónica'
                    switch (resValidarTerminalElectronico.Valor.Tipo)
                    {
                        case "M":
                            {
                                // Graba 'Venta'
                                resGrabarVenta = VentaRepository.GrabarVenta(entidad);
                                if (resGrabarVenta.Estado)
                                {
                                    if (resGrabarVenta.Valor != -1)
                                        entidad.IdVenta = resGrabarVenta.Valor;
                                    else
                                        return response;
                                }
                                else
                                {
                                    response.Mensaje = resGrabarVenta.Mensaje;
                                    return response;
                                }

                                // Graba 'Acompañante'
                                if (!string.IsNullOrEmpty(entidad.ObjAcompañante.TipoDocumento) &&
                                    !string.IsNullOrEmpty(entidad.ObjAcompañante.NumeroDocumento))
                                {
                                    var resGrabarAcompañanteVenta = VentaRepository.GrabarAcompañanteVenta(entidad.IdVenta, entidad.ObjAcompañante);
                                    if (!resGrabarAcompañanteVenta.Estado)
                                    {
                                        response.Mensaje = resGrabarAcompañanteVenta.Mensaje;
                                        return response;
                                    }
                                }

                                break;
                            }
                        case "E":
                            {
                                SetInvoiceRequestBody bodyDocumentoSUNAT = null;

                                // Valida 'DocumentoSUNAT'
                                var resValidarDocumentoSUNAT = ValidarDocumentoSUNAT(entidad, ref bodyDocumentoSUNAT);
                                if (resValidarDocumentoSUNAT.Estado && resValidarDocumentoSUNAT != null)
                                {
                                    // Valida 'FechaAbierta'
                                    if (entidad.FechaAbierta)
                                    {
                                        var auxCodiProgramacion = entidad.CodiProgramacion;
                                        entidad.CodiProgramacion = 0;

                                        // Graba 'VentaFechaAbierta'
                                        resGrabarVentaFechaAbierta = PaseRepository.GrabarVentaFechaAbierta(entidad);
                                        if (resGrabarVentaFechaAbierta.Estado)
                                            entidad.IdVenta = resGrabarVentaFechaAbierta.Valor;
                                        else
                                        {
                                            response.Mensaje = resGrabarVentaFechaAbierta.Mensaje;
                                            return response;
                                        }

                                        entidad.CodiProgramacion = auxCodiProgramacion;
                                    }
                                    else
                                    {
                                        // Graba 'Venta'
                                        resGrabarVenta = VentaRepository.GrabarVenta(entidad);
                                        if (resGrabarVenta.Estado)
                                            entidad.IdVenta = resGrabarVenta.Valor;
                                        else
                                        {
                                            response.Mensaje = resGrabarVenta.Mensaje;
                                            return response;
                                        }
                                    }

                                    // Graba 'Acompañante'
                                    if (!string.IsNullOrEmpty(entidad.ObjAcompañante.TipoDocumento) &&
                                        !string.IsNullOrEmpty(entidad.ObjAcompañante.NumeroDocumento))
                                    {
                                        var resGrabarAcompañanteVenta = VentaRepository.GrabarAcompañanteVenta(entidad.IdVenta, entidad.ObjAcompañante);
                                        if (!resGrabarAcompañanteVenta.Estado)
                                        {
                                            response.Mensaje = resGrabarAcompañanteVenta.Mensaje;
                                            return response;
                                        }
                                    }

                                    if (resGrabarVenta.Valor > 0 || resGrabarVentaFechaAbierta.Valor > 0)
                                    {
                                        // Registra 'DocumentoSUNAT'
                                        var resRegistrarDocumentoSUNAT = RegistrarDocumentoSUNAT(bodyDocumentoSUNAT);
                                        if (!resRegistrarDocumentoSUNAT.Estado)
                                        {
                                            response.Mensaje = resRegistrarDocumentoSUNAT.MensajeError;
                                            return response;
                                        }
                                    }
                                    else
                                    {
                                        response.Mensaje = "Error: resGrabarVenta o resGrabarVentaFechaAbierta.";
                                        return response;
                                    }
                                }
                                else
                                {
                                    response.Mensaje = resValidarDocumentoSUNAT.MensajeError;
                                    return response;
                                }

                                break;
                            }
                    }

                    // Seteo 'auxBoletoCompleto'
                    auxBoletoCompleto = entidad.Tipo + entidad.SerieBoleto.ToString("D3").ToString() + "-" + entidad.NumeBoleto.ToString("D7").ToString();

                    // PASE DE CORTESÍA
                    if (FlagVenta == "7")
                    {
                        // Modifica 'SaldoPaseCortesia'
                        var resModificarSaldoPaseCortesia = PaseRepository.ModificarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Año);
                        if (!resModificarSaldoPaseCortesia.Estado)
                        {
                            response.Mensaje = resModificarSaldoPaseCortesia.Mensaje;
                            return response;
                        }

                        // Graba 'PaseSocio'
                        var resGrabarPaseSocio = PaseRepository.GrabarPaseSocio(entidad.IdVenta, entidad.CodiGerente, entidad.CodiSocio, entidad.Concepto);
                        if (!resGrabarPaseSocio.Estado)
                        {
                            response.Mensaje = resGrabarPaseSocio.Mensaje;
                            return response;
                        }
                    }

                    // Valida 'LiquidacionVentas'
                    var resValidarLiquidacionVentas = VentaRepository.ValidarLiquidacionVentas(entidad.CodiUsuario, DateTime.Now.ToString("dd/MM/yyyy"));
                    if (!resValidarLiquidacionVentas.Estado)
                    {
                        response.Mensaje = resValidarLiquidacionVentas.Mensaje;
                        return response;
                    }

                    // Actualiza 'LiquidacionVentas'
                    if (resValidarLiquidacionVentas.Valor > 0)
                    {
                        var resActualizarLiquidacionVentas = VentaRepository.ActualizarLiquidacionVentas(resValidarLiquidacionVentas.Valor, DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture));
                        if (!resActualizarLiquidacionVentas.Estado)
                        {
                            response.Mensaje = resActualizarLiquidacionVentas.Mensaje;
                            return response;
                        }
                    }
                    // Graba 'LiquidacionVentas'
                    else
                    {
                        // Genera 'CorrelativoAuxiliar'
                        var resGenerarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("Liq_Caja", "999", "", string.Empty);
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

                    // Valida 'TipoPago'
                    switch (entidad.TipoPago)
                    {
                        case "01": // Contado
                            break;
                        case "02":
                        case "03": // Tarjeta de crédito    // Múltiple pago ("02")
                            {
                                //  Genera 'CorrelativoAuxiliar'
                                var resGenerarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", entidad.CodiOficina.ToString(), entidad.CodiPuntoVenta.ToString(), string.Empty);
                                if (resGenerarCorrelativoAuxiliar.Estado)
                                {
                                    if (!string.IsNullOrEmpty(resGenerarCorrelativoAuxiliar.Valor))
                                    {
                                        // Seteo 'NumeCaja'
                                        auxNumeCaja = entidad.CodiOficina.ToString() + entidad.CodiPuntoVenta.ToString() + resGenerarCorrelativoAuxiliar.Valor;

                                        // Graba 'Caja'
                                        var objCajaEntity = new CajaEntity
                                        {
                                            NumeCaja = resGenerarCorrelativoAuxiliar.Valor,
                                            CodiEmpresa = entidad.CodiEmpresa,
                                            CodiSucursal = entidad.CodiOficina,
                                            Boleto = auxBoletoCompleto,
                                            Monto = entidad.TipoPago == "03" ? entidad.PrecioVenta : entidad.Credito,
                                            CodiUsuario = entidad.CodiUsuario,
                                            Recibe = entidad.TipoPago == "03" ? "" : "MULTIPLE PAGO PARCIAL",
                                            CodiDestino = entidad.CodiDestino.ToString(),
                                            FechaViaje = entidad.TipoPago == "03" ? entidad.FechaViaje : DateTime.Now.ToString("dd/MM/yyyy"),
                                            HoraViaje = entidad.TipoPago == "03" ? entidad.HoraViaje : "",
                                            CodiPuntoVenta = entidad.CodiPuntoVenta,
                                            IdVenta = entidad.IdVenta,
                                            Origen = entidad.TipoPago == "03" ? "VT" : "PA",
                                            Modulo = entidad.TipoPago == "03" ? "PV" : "VT",
                                            Tipo = entidad.Tipo,
                                            IdCaja = 0
                                        };

                                        var resGrabarCaja = VentaRepository.GrabarCaja(objCajaEntity);
                                        if (resGrabarCaja.Estado)
                                        {
                                            if (resGrabarCaja.Valor != 0)
                                            {
                                                // Graba 'PagoTarjetaCredito'
                                                var objTarjetaCreditoEntity = new TarjetaCreditoEntity
                                                {
                                                    IdVenta = entidad.IdVenta,
                                                    Boleto = auxBoletoCompleto,
                                                    CodiTarjetaCredito = entidad.CodiTarjetaCredito,
                                                    NumeTarjetaCredito = entidad.NumeTarjetaCredito,
                                                    Vale = auxNumeCaja,
                                                    IdCaja = resGrabarCaja.Valor,
                                                    Tipo = entidad.Tipo
                                                };
                                                var resGrabarPagoTarjetaCredito = VentaRepository.GrabarPagoTarjetaCredito(objTarjetaCreditoEntity);
                                                if (!resGrabarPagoTarjetaCredito.Estado)
                                                {
                                                    response.Mensaje = resGrabarPagoTarjetaCredito.Mensaje;
                                                    return response;
                                                }
                                            }
                                            else
                                            {
                                                response.Mensaje = "Error: resGrabarCaja.Valor -> 0.";
                                                return response;
                                            }
                                        }
                                        else
                                        {
                                            response.Mensaje = resGrabarCaja.Mensaje;
                                            return response;
                                        }
                                    }
                                    else
                                    {
                                        response.Mensaje = "Error: resGenerarCorrelativoAuxiliar.Valor -> Nulo o vacío.";
                                        return response;
                                    }
                                }
                                else
                                {
                                    response.Mensaje = resGenerarCorrelativoAuxiliar.Mensaje;
                                    return response;
                                }
                                break;
                            }
                        case "04": // Delivery
                            {
                                var resGrabarPagoDelivery = VentaRepository.GrabarPagoDelivery(entidad.IdVenta, entidad.CodiZona, entidad.Direccion, entidad.Observacion);
                                if (!resGrabarPagoDelivery.Estado)
                                {
                                    response.Mensaje = resGrabarPagoDelivery.Mensaje;
                                    return response;
                                }
                                break;
                            }
                    }

                    // Graba 'Auditoria'
                    var objAuditoriaEntity = new AuditoriaEntity
                    {
                        CodiUsuario = entidad.CodiUsuario,
                        NomUsuario = entidad.NomUsuario,
                        Tabla = "VENTA",
                        TipoMovimiento = "ADICION",
                        Boleto = auxBoletoCompleto,
                        NumeAsiento = entidad.NumeAsiento.ToString(),
                        NomOficina = entidad.NomOficina,
                        NomPuntoVenta = entidad.NomPuntoVenta,
                        Pasajero = entidad.Nombre,
                        FechaViaje = entidad.FechaViaje,
                        HoraViaje = entidad.HoraViaje,
                        NomDestino = entidad.NomDestino,
                        Precio = entidad.PrecioVenta,
                        Obs1 = string.Empty,
                        Obs2 = string.Empty,
                        Obs3 = string.Empty,
                        Obs4 = string.Empty,
                        Obs5 = string.Empty
                    };

                    var resGrabarAuditoria = VentaRepository.GrabarAuditoria(objAuditoriaEntity);
                    if (!resGrabarAuditoria.Estado)
                    {
                        response.Mensaje = resGrabarAuditoria.Mensaje;
                        return response;
                    }

                    // Añado 'auxBoletoCompleto'
                    auxBoletoCompleto = (entidad.Tipo == "M" ? "0" : entidad.Tipo) + entidad.SerieBoleto.ToString("D3").ToString() + "-" + entidad.NumeBoleto.ToString("D7").ToString();

                    valor += auxBoletoCompleto + ",";
                }

                response.EsCorrecto = true;
                response.Valor = valor;
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

        public static Response<string> GrabaReserva(List<VentaEntity> listado)
        {
            try
            {
                var response = new Response<string>(false, null, "Error: GrabaReserva.", false);
                string valor = string.Empty;

                foreach (var entidad in listado)
                {
                    string auxBoletoCompleto = string.Empty;

                    Response<int> resGrabarVenta = new Response<int>(false, 0, "Error: GrabaReserva.", false);

                    // Elimina 'Reserva'
                    var resEliminarReserva = VentaRepository.EliminarReserva(entidad.IdVenta);
                    if (resEliminarReserva.Estado)
                        // Como mandamos 'IdVenta' para 'EliminarReserva', lo volvemos a su valor por defecto.
                        entidad.IdVenta = 0;
                    else
                    {
                        response.Mensaje = resEliminarReserva.Mensaje;
                        return response;
                    }

                    // Busca 'ProgramacionViaje'
                    var resBuscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(entidad.NroViaje, entidad.FechaProgramacion);
                    if (resBuscarProgramacionViaje.Estado)
                    {
                        if (resBuscarProgramacionViaje.Valor == 0)
                        {
                            // Genera 'CorrelativoAuxiliar'
                            var resGenerarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("TB_PROGRAMACION", "999", "", string.Empty);
                            if (resGenerarCorrelativoAuxiliar.Estado)
                                entidad.CodiProgramacion = int.Parse(resGenerarCorrelativoAuxiliar.Valor);
                            else
                            {
                                response.Mensaje = resGenerarCorrelativoAuxiliar.Mensaje;
                                return response;
                            }

                            var objProgramacion = new ProgramacionEntity
                            {
                                CodiProgramacion = entidad.CodiProgramacion,
                                CodiEmpresa = entidad.CodiEmpresa,
                                CodiSucursal = entidad.CodiSucursal,
                                CodiRuta = entidad.CodiRuta,
                                CodiBus = entidad.CodiBus,
                                FechaProgramacion = entidad.FechaProgramacion,
                                HoraProgramacion = entidad.HoraProgramacion,
                                CodiServicio = entidad.CodiServicio
                            };

                            // Graba 'Programacion'
                            var resGrabarProgramacion = VentaRepository.GrabarProgramacion(objProgramacion);
                            if (!resGrabarProgramacion.Estado)
                            {
                                response.Mensaje = resGrabarProgramacion.Mensaje;
                                return response;
                            }

                            // Graba 'ViajeProgramacion'
                            var resGrabarViajeProgramacion = VentaRepository.GrabarViajeProgramacion(entidad.NroViaje, entidad.CodiProgramacion, entidad.FechaProgramacion, entidad.CodiBus);
                            if (!resGrabarViajeProgramacion.Estado)
                            {
                                response.Mensaje = resGrabarViajeProgramacion.Mensaje;
                                return response;
                            }
                        }
                        else
                            entidad.CodiProgramacion = resBuscarProgramacionViaje.Valor;
                    }
                    else
                        return response;

                    // Busca 'Correlativo'
                    var resGenerarCorrelativoAuxiliar2 = VentaRepository.GenerarCorrelativoAuxiliar("TB_RESERVAS", "999", "", string.Empty);
                    if (resGenerarCorrelativoAuxiliar2.Estado)
                    {
                        entidad.SerieBoleto = -98;
                        entidad.NumeBoleto = int.Parse(resGenerarCorrelativoAuxiliar2.Valor);
                    }
                    else
                    {
                        response.Mensaje = resGenerarCorrelativoAuxiliar2.Mensaje;
                        return response;
                    }

                    // Seteo 'Tipo'
                    entidad.Tipo = "M";

                    // Graba 'Venta'
                    resGrabarVenta = VentaRepository.GrabarVenta(entidad);
                    if (resGrabarVenta.Estado)
                    {
                        if (resGrabarVenta.Valor != -1)
                            entidad.IdVenta = resGrabarVenta.Valor;
                        else
                            return response;
                    }
                    else
                    {
                        response.Mensaje = resGrabarVenta.Mensaje;
                        return response;
                    }

                    // Añado 'auxBoletoCompleto'
                    auxBoletoCompleto = (entidad.Tipo == "M" ? "0" : entidad.Tipo) + entidad.SerieBoleto.ToString("D3").ToString() + "-" + entidad.NumeBoleto.ToString("D7").ToString();

                    valor += auxBoletoCompleto + ",";
                }

                response.EsCorrecto = true;
                response.Valor = valor;
                response.Mensaje = Message.MsgCorrectoGrabaReserva;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrExcGrabaReserva, false);
            }
        }

        public static Response<PaseCortesiaResponse> ListaBeneficiarioPase(string Codi_Socio, string año, string mes)
        {
            try
            {
                var response = new Response<PaseCortesiaResponse>(false, new PaseCortesiaResponse(), "Error: ListaBeneficiarioPase.", false);
                var ListaBeneficiario = VentaRepository.ListaBeneficiarios(Codi_Socio);
                var objBoletosCortesia = VentaRepository.ObjetoBoletosCortesia(Codi_Socio, año, mes);

                response.Valor.BoletoLibre = objBoletosCortesia.Boletos_Libres;
                response.Valor.BoletoPrecio = objBoletosCortesia.Boletos_Precio;
                response.Valor.BoletoTotal = objBoletosCortesia.Total_Boletos;
                response.Valor.ListaBeneficiarios = ListaBeneficiario;

                response.EsCorrecto = true;
                response.Valor = response.Valor;
                response.Mensaje = Message.MsgCorrectoBeneficiarioPase;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PaseCortesiaResponse>(false, null, Message.MsgErrExcBeneficiarioPase, false);
            }
        }

        public static Response<ClavesInternasResponse> ClavesInternas(int Codi_Oficina, string Password, string Codi_Tipo)
        {
            try
            {
                var response = new Response<ClavesInternasResponse>(false, new ClavesInternasResponse(), "Correcto: Clave autorizada.", false);

                var objClavesInternas = ClavesInternasRepository.ClavesInternas(Codi_Oficina, Password, Codi_Tipo);

                if (Codi_Oficina == objClavesInternas.Valor.oficina && Password.ToUpper() == objClavesInternas.Valor.pwd.ToUpper() && Codi_Tipo == objClavesInternas.Valor.cod_tipo)
                {
                    response.EsCorrecto = true;
                    response.Valor = response.Valor;
                    response.Estado = true;
                }
                else
                {
                    response.EsCorrecto = false;
                    response.Valor = response.Valor;
                    response.Mensaje = Message.MsgErrExcClaveIncorrecta;
                    response.Estado = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ClavesInternasResponse>(false, null, Message.MsgErrExcClavesInternas, false);
            }
        }

        public static Response<bool> AnularVenta(int Id_Venta, int Codi_Usuario, string CodiOficina, string CodiPuntoVenta, string tipo)
        {
            try
            {
                var response = new Response<bool>(false, false, "Error: AnularVenta.", false);
                Response<bool> anularVenta = new Response<bool>();
                Response<VentaEntity> objVenta = new Response<VentaEntity>();
                Response<string> correlativo = new Response<string>();
                Response<int> caja = new Response<int>();
                CajaEntity objCaja;
                string Table = "CAJA";

                objVenta = VentaRepository.BuscarVentaById(Id_Venta);
                correlativo = VentaRepository.GenerarCorrelativoAuxiliar(Table, CodiOficina, CodiPuntoVenta, string.Empty);

                if (!string.IsNullOrEmpty(correlativo.Valor) && correlativo.Estado == true && objVenta.Valor != null && objVenta.Estado == true)
                {
                    objCaja = new CajaEntity
                    {
                        NumeCaja = correlativo.Valor,
                        CodiEmpresa = objVenta.Valor.CodiEmpresa,
                        CodiSucursal = Convert.ToInt16(CodiOficina),
                        Boleto = Convert.ToInt32(objVenta.Valor.NUME_BOLETO).ToString("D7"),
                        Monto = objVenta.Valor.PrecioVenta,
                        CodiUsuario = Convert.ToInt16(Codi_Usuario),
                        Recibe = string.Empty,
                        CodiDestino = Convert.ToString(objVenta.Valor.Codi_ruta),
                        FechaViaje = objVenta.Valor.Fecha_Viaje.ToString("dd/MM/yyyy"),
                        HoraViaje = "VNA",
                        CodiPuntoVenta = Convert.ToInt16(CodiPuntoVenta),
                        IdVenta = Id_Venta,
                        Origen = "AB",
                        Modulo = "AP",
                        Tipo = tipo,
                        IdCaja = 0
                    };

                    caja = VentaRepository.GrabarCaja(objCaja);

                    if (caja.Valor != 0 && caja.EsCorrecto == true && caja.Estado == true)
                    {
                        anularVenta = VentaRepository.AnularVenta(Id_Venta, Codi_Usuario);
                    }

                    if (anularVenta.Estado == true && anularVenta.EsCorrecto == true)
                    {
                        response.EsCorrecto = true;
                        response.Valor = anularVenta.Valor;
                        response.Mensaje = Message.MsgCorrectoAnularVenta;
                        response.Estado = true;
                    }
                }
                else
                {
                    response.EsCorrecto = false;
                    response.Mensaje = Message.MsgErrExcAnularVenta;
                    response.Estado = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcAnularVenta, false);
            }
        }

        public static Response<VentaBeneficiarioEntity> BuscarVentaxBoleto(string Tipo, short Serie, int Numero, short CodiEmpresa)
        {
            try
            {
                var response = new Response<VentaBeneficiarioEntity>()
                {
                    EsCorrecto = true,
                    Valor = VentaRepository.BuscarVentaxBoleto(Tipo, Serie, Numero, CodiEmpresa),
                    Mensaje = Message.MsgCorrectoBuscarVentaxBoleto,
                    Estado = true
                };

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaBeneficiarioEntity>(false, null, Message.MsgErrBuscarVentaxBoleto, false);
            }
        }

        public static Response<string> PostergarVenta(PostergarVentaRequest filtro)
        {
            try
            {
                var response = new Response<string>()
                {
                    EsCorrecto = true,
                    Valor = VentaRepository.PostergarVenta(filtro.IdVenta, filtro.CodiProgramacion, filtro.NumeAsiento, filtro.CodiServicio, filtro.FechaViaje, filtro.HoraViaje),
                    Mensaje = Message.MsgCorrectoPostergarVenta,
                    Estado = true
                };

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrPostergarVenta, false);
            }
        }

        public static Response<string> ModificarVentaAFechaAbierta(int IdVenta, int CodiServicio, int CodiRuta)
        {
            try
            {
                var response = new Response<string>()
                {
                    EsCorrecto = true,
                    Valor = VentaRepository.ModificarVentaAFechaAbierta(IdVenta, CodiServicio, CodiRuta),
                    Mensaje = Message.MsgCorrectoPostergarVenta,
                    Estado = true
                };
                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrModificarVentaAFechaAbierta, false);
            }
        }

        public static Response<bool> EliminarReserva(int IdVenta)
        {
            try
            {
                var response = new Response<bool>(false, false, "Error: EliminarReserva.", false);

                // Elimina 'Reserva'
                var resEliminarReserva = VentaRepository.EliminarReserva(IdVenta);
                if (!resEliminarReserva.Estado)
                {
                    response.Mensaje = resEliminarReserva.Mensaje;
                    return response;
                }

                response.EsCorrecto = true;
                response.Valor = resEliminarReserva.Valor;
                response.Mensaje = Message.MsgCorrectoEliminarReserva;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcEliminarReserva, false);
            }
        }

        #region FACTURACIÓN ELETRÓNICA

        public static ResponseW ValidarDocumentoSUNAT(VentaEntity entidad, ref SetInvoiceRequestBody bodyDocumentoSUNAT)
        {
            try
            {
                Ws_SeeFacteSoapClient serviceFE = new Ws_SeeFacteSoapClient();
                SetInvoiceRequestBody entidadFE = new SetInvoiceRequestBody();

                // Busca 'RucEmpresa'
                var resBuscarRucEmpresa = VentaRepository.BuscarRucEmpresa(entidad.CodiEmpresa);
                if (!resBuscarRucEmpresa.Estado || string.IsNullOrEmpty(resBuscarRucEmpresa.Valor))
                {
                    return null;
                }

                Security seguridadFE = new Security
                {
                    ID = resBuscarRucEmpresa.Valor,
                    User = entidad.UserWebSUNAT
                };
                // Genera 'Seguridad'
                entidadFE.Security = seguridadFE;
                // Genera 'Persona'
                entidadFE.Persona = GenerarPersona(entidad);
                // Genera 'Cabecera'
                entidadFE.CInvoice = generarCabecera(entidad);
                // Genera 'Detalle'
                entidadFE.DetInvoice = GenerarDetalle(entidad);
                // Genera 'Documentos Relacionados'
                entidadFE.DocInvoice = new ArrayOfString();
                // Genera 'Adicionales'
                entidadFE.Aditional = GenerarAdicionales(entidad);
                entidadFE.EnvioAsync = false;
                bodyDocumentoSUNAT = entidadFE;

                return serviceFE.SetValueInvoice(bodyDocumentoSUNAT.Security, bodyDocumentoSUNAT.Persona, bodyDocumentoSUNAT.CInvoice, bodyDocumentoSUNAT.DetInvoice, bodyDocumentoSUNAT.DocInvoice, bodyDocumentoSUNAT.Aditional);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        private static ResponseW RegistrarDocumentoSUNAT(SetInvoiceRequestBody bodyDocumentoSunat)
        {
            try
            {
                Ws_SeeFacteSoapClient serviceFE = new Ws_SeeFacteSoapClient();

                ResponseW ServFactElectResponse = serviceFE.SetInvoice(bodyDocumentoSunat.Security, bodyDocumentoSunat.Persona, bodyDocumentoSunat.CInvoice, bodyDocumentoSunat.DetInvoice, bodyDocumentoSunat.DocInvoice, bodyDocumentoSunat.Aditional, false);
                return ServFactElectResponse;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }

        }

        public static string GenerarPersona(VentaEntity entidad)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[IdTipoDocIdentidad]|[NumDocIdentidad]|[RazonNombres]|[RazonComercial]|[DireccionFiscal]|[UbigeoSUNAT]|[Departamento]|[Provincia]|[Distrito]|[Urbanizacion]|[PaisCodSUNAT]");
                if (entidad.CodiDocumento == "03")
                {
                    if (entidad.TipoDocumento == "01")
                        sb = sb.Replace("[IdTipoDocIdentidad]", "1");
                    else
                        sb = sb.Replace("[IdTipoDocIdentidad]", "4");

                    // Split a 'Nombre'
                    string[] splitNombre = entidad.Nombre.Split(',');

                    sb = sb.Replace("[NumDocIdentidad]", entidad.Dni);
                    sb = sb.Replace("[RazonNombres]", (splitNombre[0].Replace("|", string.Empty) + " " +
                                                       splitNombre[1].Replace("|", string.Empty) + " " +
                                                       splitNombre[2].Replace("|", string.Empty)));
                    sb = sb.Replace("[RazonComercial]", string.Empty);
                    sb = sb.Replace("[DireccionFiscal]", string.Empty);
                }
                else
                {
                    sb = sb.Replace("[IdTipoDocIdentidad]", "6");
                    sb = sb.Replace("[NumDocIdentidad]", entidad.RucCliente);
                    sb = sb.Replace("[RazonNombres]", entidad.NomEmpresaRuc);
                    sb = sb.Replace("[RazonComercial]", string.Empty);
                    sb = sb.Replace("[DireccionFiscal]", entidad.DirEmpresaRuc);
                }

                sb = sb.Replace("[UbigeoSUNAT]", string.Empty);
                sb = sb.Replace("[Departamento]", string.Empty);
                sb = sb.Replace("[Provincia]", string.Empty);
                sb = sb.Replace("[Distrito]", string.Empty);
                sb = sb.Replace("[Urbanizacion]", string.Empty);
                sb = sb.Replace("[PaisCodSUNAT]", "PE");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static string generarCabecera(VentaEntity entidad)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[TDocumento]|[Serie]|[Numero]|[FecEmision]|[HoraEmision]|[TMoneda]|[ImporteTotalVenta]|[EnviarEmail]|[CorreoCliente]");
                sb.Append("|[TipoCambio]|[TValorOperacionGravada]|[TValorOperacionInafecta]|[TValorOperacionExo]|[PorcIgv]|[SumIgvTotal]|[SumISCTotal]");
                sb.Append("|[SumOtrosTrib]|[SumOtrosCargos]|[TDescuentos]|[ImportePercepcionN]|[ValorRefServTransp]|[NombEmbarcacionPesq]|[MatEmbarcacionPesq]");
                sb.Append("|[DTipoEspVend]|[LugarDescargar]|[FechDescarga]|[NumeroRegMTC]|[ConfigVehicular]|[PuntoOrigen]|[PuntoDestino]|[ValorReferncialPrel]");
                sb.Append("|[FechConsumo]|[TVentaGratuita]|[DescuentoGlobal]|[MontoLetras]");
                sb = sb.Replace("[TDocumento]", entidad.CodiDocumento);
                sb = sb.Replace("[Serie]", entidad.Tipo + entidad.SerieBoleto.ToString());
                sb = sb.Replace("[Numero]", entidad.NumeBoleto.ToString("0######"));
                sb = sb.Replace("[FecEmision]", DateTime.Now.ToString("dd/MM/yyyy"));
                sb = sb.Replace("[HoraEmision]", DateTime.Now.ToString("HH:mm:ss"));
                sb = sb.Replace("[TMoneda]", "PEN");
                sb = sb.Replace("[ImporteTotalVenta]", entidad.PrecioVenta.ToString("F2", CultureInfo.InvariantCulture));
                sb = sb.Replace("[EnviarEmail]", "");
                sb = sb.Replace("[CorreoCliente]", string.Empty);
                sb = sb.Replace("[TipoCambio]", string.Empty);
                sb = sb.Replace("[TValorOperacionGravada]", "0.00");
                sb = sb.Replace("[TValorOperacionInafecta]", entidad.PrecioVenta.ToString("F2", CultureInfo.InvariantCulture));
                sb = sb.Replace("[TValorOperacionExo]", "0.00");
                sb = sb.Replace("[PorcIgv]", "0.00");
                sb = sb.Replace("[SumIgvTotal]", "0.00");
                sb = sb.Replace("[SumISCTotal]", "0.00");
                sb = sb.Replace("[SumOtrosTrib]", "0.00");
                sb = sb.Replace("[SumOtrosCargos]", "0.00");
                sb = sb.Replace("[TDescuentos]", "0.00");
                sb = sb.Replace("[ImportePercepcionN]", "0.00");
                sb = sb.Replace("[ValorRefServTransp]", "0.00");
                sb = sb.Replace("[NombEmbarcacionPesq]", string.Empty);
                sb = sb.Replace("[MatEmbarcacionPesq]", string.Empty);
                sb = sb.Replace("[DTipoEspVend]", string.Empty);
                sb = sb.Replace("[LugarDescargar]", string.Empty);
                sb = sb.Replace("[FechDescarga]", string.Empty);
                sb = sb.Replace("[NumeroRegMTC]", string.Empty);
                sb = sb.Replace("[ConfigVehicular]", string.Empty);
                sb = sb.Replace("[PuntoOrigen]", string.Empty);
                sb = sb.Replace("[PuntoDestino]", string.Empty);
                sb = sb.Replace("[ValorReferncialPrel]", string.Empty);
                sb = sb.Replace("[FechConsumo]", "01/01/1900");
                sb = sb.Replace("[TVentaGratuita]", "0.00");
                sb = sb.Replace("[DescuentoGlobal]", "0.00");
                sb = sb.Replace("[MontoLetras]", DataUtility.MontoSolesALetras(entidad.PrecioVenta.ToString("F2", CultureInfo.InvariantCulture)));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static ArrayOfString GenerarDetalle(VentaEntity entidad)
        {
            try
            {
                ArrayOfString array = new ArrayOfString();
                StringBuilder sb = new StringBuilder();

                sb.Append("[ID]|[CodProdInt]|[UnidadMedida]|[CantidadItem]|[Descripcion]|[ValorUnitario]|[PrecioVenta]|[CodAFIgvxItem]|[AFIgvxItem]");
                sb.Append("|[CodAFIscxItem]|[AFIscxItem]|[AFOtroxItem]|[ValorVenta]|[VRefGratuita]|[VDescuento]");

                sb = sb.Replace("[ID]", "1");
                sb = sb.Replace("[CodProdInt]", string.Empty);
                sb = sb.Replace("[UnidadMedida]", "ZZ");
                sb = sb.Replace("[CantidadItem]", "1");
                sb = sb.Replace("[Descripcion]", entidad.DescripcionProducto);
                sb = sb.Replace("[ValorUnitario]", entidad.PrecioVenta.ToString("F2", CultureInfo.InvariantCulture));
                sb = sb.Replace("[PrecioVenta]", entidad.PrecioVenta.ToString("F2", CultureInfo.InvariantCulture));
                sb = sb.Replace("[CodAFIgvxItem]", "10");
                sb = sb.Replace("[AFIgvxItem]", "0.00");
                sb = sb.Replace("[CodAFIscxItem]", "02");
                sb = sb.Replace("[AFIscxItem]", "0.00");
                sb = sb.Replace("[AFOtroxItem]", "0.00");
                sb = sb.Replace("[ValorVenta]", entidad.PrecioVenta.ToString("F2", CultureInfo.InvariantCulture));
                sb = sb.Replace("[VRefGratuita]", "0.00");
                sb = sb.Replace("[VDescuento]", "0.00");

                array.Add(sb.ToString());

                return array;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static ArrayOfString GenerarAdicionales(VentaEntity entidad)
        {
            try
            {
                ArrayOfString array = new ArrayOfString();
                StringBuilder sb = new StringBuilder();

                sb.Append("[CodAdicional]|[Descripcion]");
                sb = sb.Replace("[CodAdicional]", "1");
                sb = sb.Replace("[Descripcion]", string.Empty);

                array.Add(sb.ToString());

                return array;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }

        }

        #endregion
    }
}
