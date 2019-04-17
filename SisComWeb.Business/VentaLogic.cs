using SisComWeb.Business.ServiceFE;
using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Entity.Peticiones.Response;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;

namespace SisComWeb.Business
{
    public static class VentaLogic
    {
        private static readonly string UserWebSUNAT = ConfigurationManager.AppSettings["userWebSUNAT"].ToString();
        private static readonly string MotivoAnulacionFE = ConfigurationManager.AppSettings["motivoAnulacionFE"].ToString();

        #region BUSCAR CORRELATIVO

        public static Response<string> BuscaCorrelativo(CorrelativoRequest request)
        {
            try
            {
                var valor = string.Empty;

                // Valida 'TerminalElectronico'
                var validarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(request.CodiEmpresa, request.CodiSucursal, request.CodiPuntoVenta, short.Parse(request.CodiTerminal));
                if (string.IsNullOrEmpty(validarTerminalElectronico.Tipo))
                    validarTerminalElectronico.Tipo = "M";

                var buscarCorrelativo = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, request.CodiDocumento, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);

                if (buscarCorrelativo.SerieBoleto == 0)
                    return new Response<string>(false, valor, Message.MsgErrorSerieBoleto, false);

                // Aumento para el método 'BuscarCorrelativo'
                buscarCorrelativo.NumeBoleto = buscarCorrelativo.NumeBoleto + AumentoDelCorrelativo(validarTerminalElectronico.Tipo, request.CodiDocumento);

                valor = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, request.CodiDocumento, buscarCorrelativo.SerieBoleto, buscarCorrelativo.NumeBoleto, "3", "8");

                return new Response<string>(true, valor, Message.MsgCorrectoBuscaCorrelativo, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcBuscaCorrelativo, false);
            }
        }

        #endregion

        #region GRABAR VENTA

        public static Response<string> GrabaVenta(List<VentaEntity> Listado, string FlagVenta)
        {
            try
            {
                var valor = string.Empty;
                var listaFilesPDF = new List<byte[]>();

                foreach (var entidad in Listado)
                {
                    string auxBoletoCompleto = string.Empty;
                    string auxNumeCaja = string.Empty;
                    entidad.UserWebSUNAT = UserWebSUNAT;

                    // Busca 'ProgramacionViaje'
                    var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(entidad.NroViaje, entidad.FechaProgramacion);
                    if (buscarProgramacionViaje == 0)
                    {
                        // Genera 'CorrelativoAuxiliar'
                        var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("TB_PROGRAMACION", "999", string.Empty, string.Empty);
                        if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                            return new Response<string>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                        entidad.CodiProgramacion = int.Parse(generarCorrelativoAuxiliar) + 1;

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
                        var grabarProgramacion = VentaRepository.GrabarProgramacion(objProgramacion);
                        if (!grabarProgramacion)
                            return new Response<string>(false, valor, Message.MsgErrorGrabarProgramacion, false);

                        // Graba 'ViajeProgramacion'
                        var grabarViajeProgramacion = VentaRepository.GrabarViajeProgramacion(entidad.NroViaje, entidad.CodiProgramacion, entidad.FechaProgramacion, entidad.CodiBus);
                        if (!grabarViajeProgramacion)
                            return new Response<string>(false, valor, Message.MsgErrorGrabarViajeProgramacion, false);
                    }
                    else
                        entidad.CodiProgramacion = buscarProgramacionViaje;

                    // Valida 'TerminalElectronico'
                    var validarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, short.Parse(entidad.CodiTerminal));
                    if (string.IsNullOrEmpty(validarTerminalElectronico.Tipo))
                        validarTerminalElectronico.Tipo = "M";

                    // PASE DE CORTESÍA
                    if (FlagVenta == "7")
                    {
                        // Valida 'SaldoPaseCortesia'
                        var validarSaldoPaseCortesia = PaseRepository.ValidarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Anno);
                        if (validarSaldoPaseCortesia != 1)
                            return new Response<string>(false, valor, Message.MsgValidaSaldoPaseCortesia, false);
                    }

                    // RESERVA
                    if (entidad.FlagVenta == "R")
                    {
                        // Elimina 'Reserva'
                        var eliminarReserva = VentaRepository.EliminarReserva(entidad.IdVenta);
                        if (!eliminarReserva)
                            return new Response<string>(false, valor, Message.MsgErrorEliminarReserva, false);
                        // Como mandamos 'IdVenta' para 'EliminarReserva', lo volvemos a su valor por defecto.
                        entidad.IdVenta = 0;
                        // Cuando 'confirmasReserva', por ahora se venderá como una 'Venta'.
                        entidad.FlagVenta = "V";
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
                        };
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
                        };
                        // Seteo 'CodiDocumento'
                        entidad.CodiDocumento = "03"; // Boleta
                    }

                    // Busca 'Correlativo'
                    var buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, validarTerminalElectronico.Tipo);
                    entidad.SerieBoleto = buscarCorrelativo.SerieBoleto;
                    entidad.NumeBoleto = buscarCorrelativo.NumeBoleto;

                    if (buscarCorrelativo.SerieBoleto == 0)
                    {
                        switch (validarTerminalElectronico.Tipo)
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
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, validarTerminalElectronico.Tipo);
                                                    if (buscarCorrelativo.SerieBoleto == 0)
                                                        return new Response<string>(false, valor, Message.MsgErrorSerieBoleto, false);
                                                }

                                                if (buscarCorrelativo.SerieBoleto == 0)
                                                {
                                                    // Seteo 'CodiBF Interno'
                                                    entidad.AuxCodigoBF_Interno = "16";
                                                    // Seteo 'CodiDocumento'
                                                    entidad.CodiDocumento = "03"; // Boleta

                                                    // Busca 'Correlativo'
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, validarTerminalElectronico.Tipo);
                                                    if (buscarCorrelativo.SerieBoleto == 0)
                                                        return new Response<string>(false, valor, Message.MsgErrorSerieBoleto, false);
                                                    else
                                                    {
                                                        entidad.SerieBoleto = buscarCorrelativo.SerieBoleto;
                                                        entidad.NumeBoleto = buscarCorrelativo.NumeBoleto;
                                                    }
                                                }
                                                else
                                                {
                                                    entidad.SerieBoleto = buscarCorrelativo.SerieBoleto;
                                                    entidad.NumeBoleto = buscarCorrelativo.NumeBoleto;
                                                }
                                            };
                                            break;
                                        case "03": // Boleta
                                            {
                                                if (FlagVenta == "7")
                                                {
                                                    // Seteo 'CodiBF Interno'
                                                    entidad.AuxCodigoBF_Interno = "16";
                                                    // Busca 'Correlativo'
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, validarTerminalElectronico.Tipo);
                                                    if (buscarCorrelativo.SerieBoleto == 0)
                                                        return new Response<string>(false, valor, Message.MsgErrorSerieBoleto, false);
                                                }

                                                if (buscarCorrelativo.SerieBoleto == 0)
                                                    return new Response<string>(false, valor, Message.MsgErrorSerieBoleto, false);
                                                else
                                                {
                                                    entidad.SerieBoleto = buscarCorrelativo.SerieBoleto;
                                                    entidad.NumeBoleto = buscarCorrelativo.NumeBoleto;
                                                }
                                            };
                                            break;
                                    };
                                };
                                break;
                            case "E":
                                return new Response<string>(false, valor, Message.MsgErrorSerieBoleto, false);
                        };
                    }

                    // Seteo 'Tipo'
                    switch (validarTerminalElectronico.Tipo)
                    {
                        case "M":
                            entidad.Tipo = "M";
                            break;
                        case "E":
                            {
                                if (!string.IsNullOrEmpty(entidad.RucCliente))
                                    entidad.Tipo = "F";
                                else
                                    entidad.Tipo = "B";
                            };
                            break;
                    };

                    // Aumento para el método 'BuscarCorrelativo'
                    entidad.NumeBoleto = entidad.NumeBoleto + AumentoDelCorrelativo(validarTerminalElectronico.Tipo, entidad.AuxCodigoBF_Interno);

                    // Graba 'Venta', 'Otros' y 'FacturaciónElectrónica'
                    switch (validarTerminalElectronico.Tipo)
                    {
                        case "M":
                            {
                                // Valida 'FechaAbierta'
                                if (entidad.FechaAbierta)
                                {
                                    var auxCodiProgramacion = entidad.CodiProgramacion;
                                    entidad.CodiProgramacion = 0;

                                    // Graba 'VentaFechaAbierta'
                                    var grabarVentaFechaAbierta = PaseRepository.GrabarVentaFechaAbierta(entidad);
                                    if (grabarVentaFechaAbierta <= 0)
                                        return new Response<string>(false, valor, Message.MsgErrorGrabarVentaFechaAbierta, false);

                                    entidad.IdVenta = grabarVentaFechaAbierta;
                                    entidad.CodiProgramacion = auxCodiProgramacion;
                                }
                                else
                                {
                                    // Graba 'Venta'
                                    var grabarVenta = VentaRepository.GrabarVenta(entidad);
                                    if (grabarVenta <= 0)
                                        return new Response<string>(false, valor, Message.MsgErrorGrabaVenta, false);

                                    entidad.IdVenta = grabarVenta;
                                }

                                // Graba 'Acompañante'
                                if (!string.IsNullOrEmpty(entidad.ObjAcompaniante.TipoDocumento) && !string.IsNullOrEmpty(entidad.ObjAcompaniante.NumeroDocumento))
                                {
                                    var grabarAcompanianteVenta = VentaRepository.GrabarAcompanianteVenta(entidad.IdVenta, entidad.ObjAcompaniante);
                                    if (!grabarAcompanianteVenta)
                                        return new Response<string>(false, valor, Message.MsgErrorGrabarAcompanianteVenta, false);
                                }
                            };
                            break;
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
                                        var grabarVentaFechaAbierta = PaseRepository.GrabarVentaFechaAbierta(entidad);
                                        if (grabarVentaFechaAbierta <= 0)
                                            return new Response<string>(false, valor, Message.MsgErrorGrabarVentaFechaAbierta, false);

                                        entidad.IdVenta = grabarVentaFechaAbierta;
                                        entidad.CodiProgramacion = auxCodiProgramacion;
                                    }
                                    else
                                    {
                                        // Graba 'Venta'
                                        var grabarVenta = VentaRepository.GrabarVenta(entidad);
                                        if (grabarVenta <= 0)
                                            return new Response<string>(false, valor, Message.MsgErrorGrabaVenta, false);

                                        entidad.IdVenta = grabarVenta;
                                    }

                                    // Graba 'Acompañante'
                                    if (!string.IsNullOrEmpty(entidad.ObjAcompaniante.TipoDocumento) && !string.IsNullOrEmpty(entidad.ObjAcompaniante.NumeroDocumento))
                                    {
                                        var grabarAcompanianteVenta = VentaRepository.GrabarAcompanianteVenta(entidad.IdVenta, entidad.ObjAcompaniante);
                                        if (!grabarAcompanianteVenta)
                                            return new Response<string>(false, valor, Message.MsgErrorGrabarAcompanianteVenta, false);
                                    }

                                    if (entidad.IdVenta > 0)
                                    {
                                        // Registra 'DocumentoSUNAT'
                                        var resRegistrarDocumentoSUNAT = RegistrarDocumentoSUNAT(bodyDocumentoSUNAT);
                                        if (resRegistrarDocumentoSUNAT.Estado)
                                        {
                                            // Guarda PDFs
                                            if (!string.IsNullOrEmpty(resRegistrarDocumentoSUNAT.PdfValue))
                                            {
                                                string pdfValue = resRegistrarDocumentoSUNAT.PdfValue;
                                                var filePDF = Convert.FromBase64String(pdfValue);
                                                listaFilesPDF.Add(filePDF);
                                            }
                                            else
                                                return new Response<string>(false, valor, Message.MsgErrorPdfValue, false);
                                        }
                                        else
                                            return new Response<string>(false, valor, resRegistrarDocumentoSUNAT.MensajeError, false);
                                    }
                                    else
                                        return new Response<string>(false, valor, Message.MsgErrorGrabaVenta, false);
                                }
                                else
                                    return new Response<string>(false, valor, resValidarDocumentoSUNAT.MensajeError, false);
                            };
                            break;
                    };

                    // Seteo 'auxBoletoCompleto'
                    auxBoletoCompleto = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "7");

                    // PASE DE CORTESÍA
                    if (FlagVenta == "7")
                    {
                        // Modifica 'SaldoPaseCortesia'
                        var modificarSaldoPaseCortesia = PaseRepository.ModificarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Anno);
                        if (!modificarSaldoPaseCortesia)
                            return new Response<string>(false, valor, Message.MsgErrorModificarSaldoPaseCortesia, false);

                        // Graba 'PaseSocio'
                        var grabarPaseSocio = PaseRepository.GrabarPaseSocio(entidad.IdVenta, entidad.CodiGerente, entidad.CodiSocio, entidad.Concepto);
                        if (!grabarPaseSocio)
                            return new Response<string>(false, valor, Message.MsgErrorGrabarPaseSocio, false);
                    }

                    // Valida 'LiquidacionVentas'
                    var validarLiquidacionVentas = VentaRepository.ValidarLiquidacionVentas(entidad.CodiUsuario, DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

                    // Actualiza 'LiquidacionVentas'
                    if (validarLiquidacionVentas > 0)
                    {
                        var actualizarLiquidacionVentas = VentaRepository.ActualizarLiquidacionVentas(validarLiquidacionVentas, DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture));
                        if (!actualizarLiquidacionVentas)
                            return new Response<string>(false, valor, Message.MsgErrorActualizarLiquidacionVentas, false);
                    }

                    // Graba 'LiquidacionVentas'
                    else
                    {
                        int auxCorrelativoAuxiliar = 0;

                        // Genera 'CorrelativoAuxiliar'
                        var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("LIQ_CAJA", "999", string.Empty, string.Empty);
                        if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                            return new Response<string>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                        auxCorrelativoAuxiliar = int.Parse(generarCorrelativoAuxiliar) + 1;

                        var grabarLiquidacionVentas = VentaRepository.GrabarLiquidacionVentas(auxCorrelativoAuxiliar, entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiUsuario, entidad.PrecioVenta);
                        if (!grabarLiquidacionVentas)
                            return new Response<string>(false, valor, Message.MsgErrorGrabarLiquidacionVentas, false);
                    }

                    // Valida 'TipoPago'
                    switch (entidad.TipoPago)
                    {
                        case "01": // Contado
                            break;
                        case "02": // Múltiple pago ("02")
                        case "03": // Tarjeta de crédito    
                            {
                                //  Genera 'CorrelativoAuxiliar'
                                var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", entidad.CodiOficina.ToString(), entidad.CodiPuntoVenta.ToString(), string.Empty);
                                if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                                    return new Response<string>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                                // Seteo 'NumeCaja'
                                auxNumeCaja = entidad.CodiOficina.ToString() + entidad.CodiPuntoVenta.ToString() + generarCorrelativoAuxiliar;

                                // Graba 'Caja'
                                var objCajaEntity = new CajaEntity
                                {
                                    NumeCaja = generarCorrelativoAuxiliar,
                                    CodiEmpresa = entidad.CodiEmpresa,
                                    CodiSucursal = entidad.CodiOficina,
                                    Boleto = auxBoletoCompleto.Substring(1),
                                    Monto = entidad.TipoPago == "03" ? entidad.PrecioVenta : entidad.Credito,
                                    CodiUsuario = entidad.CodiUsuario,
                                    Recibe = entidad.TipoPago == "03" ? string.Empty : "MULTIPLE PAGO PARCIAL",
                                    CodiDestino = entidad.CodiDestino.ToString(),
                                    FechaViaje = entidad.TipoPago == "03" ? entidad.FechaViaje : DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    HoraViaje = entidad.TipoPago == "03" ? entidad.HoraViaje.Replace(" ", "") : string.Empty,
                                    CodiPuntoVenta = entidad.CodiPuntoVenta,
                                    IdVenta = entidad.IdVenta,
                                    Origen = entidad.TipoPago == "03" ? "VT" : "PA",
                                    Modulo = entidad.TipoPago == "03" ? "PV" : "VT",
                                    Tipo = entidad.Tipo,
                                    IdCaja = 0
                                };

                                var grabarCaja = VentaRepository.GrabarCaja(objCajaEntity);
                                if (grabarCaja > 0)
                                {
                                    // Graba 'PagoTarjetaCredito'
                                    var objTarjetaCreditoEntity = new TarjetaCreditoEntity
                                    {
                                        IdVenta = entidad.IdVenta,
                                        Boleto = auxBoletoCompleto.Substring(1),
                                        CodiTarjetaCredito = entidad.CodiTarjetaCredito,
                                        NumeTarjetaCredito = entidad.NumeTarjetaCredito,
                                        Vale = auxNumeCaja,
                                        IdCaja = grabarCaja,
                                        Tipo = entidad.Tipo
                                    };
                                    var grabarPagoTarjetaCredito = VentaRepository.GrabarPagoTarjetaCredito(objTarjetaCreditoEntity);
                                    if (!grabarPagoTarjetaCredito)
                                        return new Response<string>(false, valor, Message.MsgErrorGrabarPagoTarjetaCredito, false);
                                }
                                else
                                    return new Response<string>(false, valor, Message.MsgErrorGrabarCaja, false);
                            };
                            break;
                        case "04": // Delivery
                            var grabarPagoDelivery = VentaRepository.GrabarPagoDelivery(entidad.IdVenta, entidad.CodiZona, entidad.Direccion, entidad.Observacion);
                            if (!grabarPagoDelivery)
                                return new Response<string>(false, valor, Message.MsgErrorGrabarPagoDelivery, false);
                            break;
                    };

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

                    var grabarAuditoria = VentaRepository.GrabarAuditoria(objAuditoriaEntity);
                    if (!grabarAuditoria)
                        return new Response<string>(false, valor, Message.MsgErrorGrabarAuditoria, false);

                    // Añado 'auxBoletoCompleto'
                    auxBoletoCompleto = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "7");

                    valor += auxBoletoCompleto + ",";
                }

                // Imprime PDFs
                //foreach (var filePDF in listaFilesPDF)
                //{

                //}

                return new Response<string>(true, valor, Message.MsgCorrectoGrabaVenta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcGrabaVenta, false);
            }
        }

        #endregion

        #region GRABAR RESERVA

        public static Response<string> GrabaReserva(List<VentaEntity> Listado)
        {
            try
            {
                string valor = string.Empty;

                foreach (var entidad in Listado)
                {
                    string auxBoletoCompleto = string.Empty;

                    // Busca 'ProgramacionViaje'
                    var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(entidad.NroViaje, entidad.FechaProgramacion);
                    if (buscarProgramacionViaje == 0)
                    {
                        // Genera 'CorrelativoAuxiliar'
                        var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("TB_PROGRAMACION", "999", string.Empty, string.Empty);
                        if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                            return new Response<string>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                        entidad.CodiProgramacion = int.Parse(generarCorrelativoAuxiliar) + 1;

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
                        var grabarProgramacion = VentaRepository.GrabarProgramacion(objProgramacion);
                        if (!grabarProgramacion)
                            return new Response<string>(false, valor, Message.MsgErrorGrabarProgramacion, false);

                        // Graba 'ViajeProgramacion'
                        var grabarViajeProgramacion = VentaRepository.GrabarViajeProgramacion(entidad.NroViaje, entidad.CodiProgramacion, entidad.FechaProgramacion, entidad.CodiBus);
                        if (!grabarViajeProgramacion)
                            return new Response<string>(false, valor, Message.MsgErrorGrabarViajeProgramacion, false);
                    }
                    else
                        entidad.CodiProgramacion = buscarProgramacionViaje;

                    // Busca 'Correlativo'
                    var generarCorrelativoAuxiliar2 = VentaRepository.GenerarCorrelativoAuxiliar("TB_RESERVAS", "999", string.Empty, string.Empty);
                    if (string.IsNullOrEmpty(generarCorrelativoAuxiliar2))
                        return new Response<string>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                    entidad.SerieBoleto = -98;
                    entidad.NumeBoleto = int.Parse(generarCorrelativoAuxiliar2) + 1;

                    // Seteo 'Tipo' (Reserva siempre es 'M')
                    entidad.Tipo = "M";

                    // Graba 'Venta'
                    var grabarVenta = VentaRepository.GrabarVenta(entidad);
                    if (grabarVenta > 0)
                        entidad.IdVenta = grabarVenta;
                    else
                        return new Response<string>(false, valor, Message.MsgErrorGrabaVenta, false);

                    // Graba 'Acompañante'
                    if (!string.IsNullOrEmpty(entidad.ObjAcompaniante.TipoDocumento) && !string.IsNullOrEmpty(entidad.ObjAcompaniante.NumeroDocumento))
                    {
                        var grabarAcompanianteVenta = VentaRepository.GrabarAcompanianteVenta(entidad.IdVenta, entidad.ObjAcompaniante);
                        if (!grabarAcompanianteVenta)
                            return new Response<string>(false, valor, Message.MsgErrorGrabarAcompanianteVenta, false);
                    }

                    // Añado 'auxBoletoCompleto'
                    auxBoletoCompleto = BoletoFormatoCompleto("M", string.Empty, entidad.SerieBoleto, entidad.NumeBoleto, "3", "8");

                    valor += auxBoletoCompleto + ",";
                }

                return new Response<string>(true, valor, Message.MsgCorrectoGrabaReserva, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcGrabaReserva, false);
            }
        }

        #endregion

        #region ELIMINAR RESERVA

        public static Response<bool> EliminarReserva(int IdVenta)
        {
            try
            {
                // Elimina 'Reserva'
                var eliminarReserva = VentaRepository.EliminarReserva(IdVenta);
                if (eliminarReserva)
                    return new Response<bool>(true, eliminarReserva, Message.MsgCorrectoEliminarReserva, true);
                else
                    return new Response<bool>(false, eliminarReserva, Message.MsgErrorEliminarReserva, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcEliminarReserva, false);
            }
        }

        #endregion

        #region LISTA BENEFICIARIO PASE 

        public static Response<PaseCortesiaResponse> ListaBeneficiarioPase(string CodiSocio, string Anno, string Mes)
        {
            try
            {
                var entidad = new PaseCortesiaResponse();

                var objBoletosCortesia = VentaRepository.ObjetoBoletosCortesia(CodiSocio, Anno, Mes);

                entidad.BoletoLibre = objBoletosCortesia.BoletosLibres;
                entidad.BoletoPrecio = objBoletosCortesia.BoletosPrecio;
                entidad.BoletoTotal = objBoletosCortesia.TotalBoletos;
                entidad.ListaBeneficiarios = VentaRepository.ListaBeneficiarios(CodiSocio);

                return new Response<PaseCortesiaResponse>(true, entidad, Message.MsgCorrectoBeneficiarioPase, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PaseCortesiaResponse>(false, null, Message.MsgExcListaBeneficiarioPase, false);
            }
        }

        #endregion

        #region CLAVES INTERNAS

        public static Response<bool> ClavesInternas(int CodiOficina, string Password, string CodiTipo)
        {
            try
            {
                var clavesInternas = ClavesInternasRepository.ClavesInternas(CodiOficina, Password, CodiTipo);

                if (CodiOficina == clavesInternas.Oficina && Password.ToUpper() == clavesInternas.Pwd.ToUpper() && CodiTipo == clavesInternas.CodTipo)
                    return new Response<bool>(true, true, Message.MsgCorrectoClavesInternas, true);
                else
                    return new Response<bool>(false, false, Message.MsgValidaClavesInternas, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcClavesInternas, false);
            }
        }

        #endregion

        #region ANULAR VENTA

        public static Response<bool> AnularVenta(int IdVenta, int CodiUsuario, string CodiOficina, string CodiPuntoVenta, string Tipo)
        {
            try
            {
                var objVenta = VentaRepository.BuscarVentaById(IdVenta);

                if (objVenta.SerieBoleto == 0)
                    return new Response<bool>(false, false, Message.MsgErrorBuscarVentaById, false);

                // Valida 'AnularDocumentoSUNAT'
                if (objVenta.Tipo != "M")
                {
                    // Anula 'DocumentoSUNAT'
                    var resAnularDocumentoSUNAT = AnularDocumentoSUNAT(objVenta);
                    if (!resAnularDocumentoSUNAT.Estado)
                        return new Response<bool>(false, false, resAnularDocumentoSUNAT.MensajeError, false);
                }

                var correlativo = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", CodiOficina, CodiPuntoVenta, string.Empty);
                if (string.IsNullOrEmpty(correlativo))
                    return new Response<bool>(false, false, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                CajaEntity objCaja = new CajaEntity
                {
                    NumeCaja = correlativo,
                    CodiEmpresa = objVenta.CodiEmpresa,
                    CodiSucursal = short.Parse(CodiOficina),
                    Boleto = objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7"),
                    Monto = objVenta.PrecioVenta,
                    CodiUsuario = short.Parse(CodiUsuario.ToString()),
                    Recibe = string.Empty,
                    CodiDestino = objVenta.CodiRuta.ToString(),
                    FechaViaje = objVenta.FechaViaje,
                    HoraViaje = "VNA",
                    CodiPuntoVenta = short.Parse(CodiPuntoVenta),
                    IdVenta = IdVenta,
                    Origen = "AB",
                    Modulo = "AP",
                    Tipo = Tipo,
                    IdCaja = 0
                };
                var caja = VentaRepository.GrabarCaja(objCaja);

                if (caja > 0)
                {
                    var anularVenta = VentaRepository.AnularVenta(IdVenta, CodiUsuario);
                    if (anularVenta)
                        return new Response<bool>(true, anularVenta, Message.MsgCorrectoAnularVenta, true);
                    else
                        return new Response<bool>(false, anularVenta, Message.MsgErrorAnularVenta, false);
                }
                else
                    return new Response<bool>(false, false, Message.MsgErrorGrabarCaja, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcAnularVenta, false);
            }
        }

        #endregion

        #region BUSCAR VENTA POR BOLETO

        public static Response<VentaBeneficiarioEntity> BuscarVentaxBoleto(string Tipo, short Serie, int Numero, short CodiEmpresa)
        {
            try
            {
                var buscarVentaxBoleto = VentaRepository.BuscarVentaxBoleto(Tipo, Serie, Numero, CodiEmpresa);

                if (buscarVentaxBoleto.IdVenta > 0)
                    return new Response<VentaBeneficiarioEntity>(true, buscarVentaxBoleto, Message.MsgCorrectoBuscarVentaxBoleto, true);
                else
                    return new Response<VentaBeneficiarioEntity>(false, buscarVentaxBoleto, Message.MsgValidaBuscarVentaxBoleto, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaBeneficiarioEntity>(false, null, Message.MsgExcBuscarVentaxBoleto, false);
            }
        }

        #endregion

        #region POSTERGAR VENTA

        public static Response<bool> PostergarVenta(PostergarVentaRequest filtro)
        {
            try
            {
                var valor = new bool();

                // Busca 'ProgramacionViaje'
                var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(filtro.NroViaje, filtro.FechaProgramacion);
                if (buscarProgramacionViaje == 0)
                {
                    // Genera 'CorrelativoAuxiliar'
                    var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("TB_PROGRAMACION", "999", string.Empty, string.Empty);
                    if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                        return new Response<bool>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                    filtro.CodiProgramacion = int.Parse(generarCorrelativoAuxiliar) + 1;

                    var objProgramacion = new ProgramacionEntity
                    {
                        CodiProgramacion = filtro.CodiProgramacion,
                        CodiEmpresa = filtro.CodiEmpresa,
                        CodiSucursal = filtro.CodiSucursal,
                        CodiRuta = filtro.CodiRuta,
                        CodiBus = filtro.CodiBus,
                        FechaProgramacion = filtro.FechaProgramacion,
                        HoraProgramacion = filtro.HoraProgramacion,
                        CodiServicio = byte.Parse(filtro.CodiServicio.ToString())
                    };

                    // Graba 'Programacion'
                    var grabarProgramacion = VentaRepository.GrabarProgramacion(objProgramacion);
                    if (!grabarProgramacion)
                        return new Response<bool>(false, valor, Message.MsgErrorGrabarProgramacion, false);

                    // Graba 'ViajeProgramacion'
                    var grabarViajeProgramacion = VentaRepository.GrabarViajeProgramacion(filtro.NroViaje, filtro.CodiProgramacion, filtro.FechaProgramacion, filtro.CodiBus);
                    if (!grabarViajeProgramacion)
                        return new Response<bool>(false, valor, Message.MsgErrorGrabarViajeProgramacion, false);
                }
                else
                    filtro.CodiProgramacion = buscarProgramacionViaje;

                valor = VentaRepository.PostergarVenta(filtro.IdVenta, filtro.CodiProgramacion, filtro.NumeAsiento, filtro.CodiServicio, filtro.FechaViaje, filtro.HoraViaje);
                if (valor)
                    return new Response<bool>(true, valor, Message.MsgCorrectoPostergarVenta, true);
                else
                    return new Response<bool>(false, valor, Message.MsgErrorPostergarVenta, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcPostergarVenta, false);
            }
        }

        #endregion

        #region MODIFICAR VENTA FECHA ABIERTA

        public static Response<bool> ModificarVentaAFechaAbierta(int IdVenta, int CodiServicio, int CodiRuta)
        {
            try
            {
                var modificarVentaAFechaAbierta = VentaRepository.ModificarVentaAFechaAbierta(IdVenta, CodiServicio, CodiRuta);
                if (modificarVentaAFechaAbierta)
                    return new Response<bool>(true, modificarVentaAFechaAbierta, Message.MsgCorrectoPostergarVenta, true);
                else
                    return new Response<bool>(false, modificarVentaAFechaAbierta, Message.MsgErrorModificarVentaAFechaAbierta, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcModificarVentaAFechaAbierta, false);
            }
        }

        #endregion

        #region FACTURACIÓN ELETRÓNICA

        public static ResponseW ValidarDocumentoSUNAT(VentaEntity entidad, ref SetInvoiceRequestBody bodyDocumentoSUNAT)
        {
            try
            {
                var serviceFE = new Ws_SeeFacteSoapClient();
                var entidadFE = new SetInvoiceRequestBody();

                // Busca 'RucEmpresa'
                var buscarRucEmpresa = VentaRepository.BuscarRucEmpresa(entidad.CodiEmpresa);
                if (string.IsNullOrEmpty(buscarRucEmpresa))
                    return null;

                var seguridadFE = new Security
                {
                    ID = buscarRucEmpresa,
                    User = entidad.UserWebSUNAT
                };

                // Genera 'Seguridad'
                entidadFE.Security = seguridadFE;
                // Genera 'Persona'
                entidadFE.Persona = GenerarPersona(entidad);
                // Genera 'Cabecera'
                entidadFE.CInvoice = GenerarCabecera(entidad);
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
                var serviceFE = new Ws_SeeFacteSoapClient();

                ResponseW ServFactElectResponse = serviceFE.SetInvoice(bodyDocumentoSunat.Security, bodyDocumentoSunat.Persona, bodyDocumentoSunat.CInvoice, bodyDocumentoSunat.DetInvoice, bodyDocumentoSunat.DocInvoice, bodyDocumentoSunat.Aditional, false);
                return ServFactElectResponse;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }

        }

        public static ResponseW AnularDocumentoSUNAT(VentaEntity entidad)
        {
            try
            {
                var serviceFE = new Ws_SeeFacteSoapClient();
                var auxTDocumento = string.Empty;

                // Busca 'RucEmpresa'
                var buscarRucEmpresa = VentaRepository.BuscarRucEmpresa(entidad.CodiEmpresa);
                if (string.IsNullOrEmpty(buscarRucEmpresa))
                    return null;

                var seguridadFE = new Security
                {
                    ID = buscarRucEmpresa,
                    User = UserWebSUNAT
                };

                // Seteo 'auxTDocumento'
                switch (entidad.Tipo)
                {
                    case "F":
                        auxTDocumento = "01";
                        break;
                    case "B":
                        auxTDocumento = "03";
                        break;
                };

                var documento = auxTDocumento +
                                "|" + entidad.Tipo + entidad.SerieBoleto.ToString("D3") +
                                "|" + entidad.NumeBoleto.ToString("D8") +
                                "|" + MotivoAnulacionFE +
                                "|" + entidad.FechaVenta +
                                "|" + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                ResponseW ServFactElectResponse = serviceFE.SetVoidedDocument(seguridadFE, documento);
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
                var sb = new StringBuilder();
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

        public static string GenerarCabecera(VentaEntity entidad)
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append("[TDocumento]|[Serie]|[Numero]|[FecEmision]|[HoraEmision]|[TMoneda]|[ImporteTotalVenta]|[EnviarEmail]|[CorreoCliente]");
                sb.Append("|[TipoCambio]|[TValorOperacionGravada]|[TValorOperacionInafecta]|[TValorOperacionExo]|[PorcIgv]|[SumIgvTotal]|[SumISCTotal]");
                sb.Append("|[SumOtrosTrib]|[SumOtrosCargos]|[TDescuentos]|[ImportePercepcionN]|[ValorRefServTransp]|[NombEmbarcacionPesq]|[MatEmbarcacionPesq]");
                sb.Append("|[DTipoEspVend]|[LugarDescargar]|[FechDescarga]|[NumeroRegMTC]|[ConfigVehicular]|[PuntoOrigen]|[PuntoDestino]|[ValorReferncialPrel]");
                sb.Append("|[FechConsumo]|[TVentaGratuita]|[DescuentoGlobal]|[MontoLetras]");
                sb = sb.Replace("[TDocumento]", entidad.CodiDocumento);
                sb = sb.Replace("[Serie]", entidad.Tipo + entidad.SerieBoleto.ToString());
                sb = sb.Replace("[Numero]", entidad.NumeBoleto.ToString("D7"));
                sb = sb.Replace("[FecEmision]", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                sb = sb.Replace("[HoraEmision]", DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
                sb = sb.Replace("[TMoneda]", "PEN");
                sb = sb.Replace("[ImporteTotalVenta]", entidad.PrecioVenta.ToString("F2", CultureInfo.InvariantCulture));
                sb = sb.Replace("[EnviarEmail]", string.Empty);
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
                var array = new ArrayOfString();
                var sb = new StringBuilder();

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
                var array = new ArrayOfString();
                var sb = new StringBuilder();

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

        #region REUTILIZABLE

        public static string BoletoFormatoCompleto(string tipo, string codiDocumento, short serieBoleto, int numeroBoleto, string formatoSerieBol, string formatoNumeroBol)
        {
            var boletoCompleto = string.Empty;

            switch (tipo)
            {
                case "M":
                    boletoCompleto = "0" + serieBoleto.ToString("D" + formatoSerieBol) + "-" + (numeroBoleto).ToString("D" + formatoNumeroBol);
                    break;
                case "E":
                    {
                        switch (codiDocumento)
                        {
                            case "17":
                            case "78":
                                boletoCompleto = "F" + serieBoleto.ToString("D" + formatoSerieBol) + "-" + (numeroBoleto).ToString("D" + formatoNumeroBol);
                                break;
                            case "16":
                            case "77":
                                boletoCompleto = "B" + serieBoleto.ToString("D" + formatoSerieBol) + "-" + (numeroBoleto).ToString("D" + formatoNumeroBol);
                                break;
                        };
                    };
                    break;
            };

            return boletoCompleto;
        }

        public static byte AumentoDelCorrelativo(string tipo, string auxCodigoBF_Interno)
        {
            byte auxAumento = 0;

            // Nota:
            // Cuando vuelva a fallar el 'correlativo' entonces habilitar o deshabilitar el 'case X' correspondiente.
            // No volver a modificarlo hasta un nuevo fallo para evitar saltos en el 'correlativo'.

            switch (tipo)
            {
                case "M":
                    {
                        switch (auxCodigoBF_Interno)
                        {
                            case "17":
                                //case "16": // No necesita.
                                //case "78": // No hay data: se supone que no necesita.
                                //case "77": // No hay data: se supone que no necesita.
                                auxAumento = 1;
                                break;
                        };
                    }
                    break;
                case "E":
                    {
                        switch (auxCodigoBF_Interno)
                        {
                            case "17":
                            case "16":
                            //case "78": // No hay data: se supone que no necesita.
                            case "77":
                                auxAumento = 1;
                                break;
                        };
                    };
                    break;
            };

            return auxAumento;
        }

        public static void ImprimirPDF()
        {

        }

        #endregion
    }
}
