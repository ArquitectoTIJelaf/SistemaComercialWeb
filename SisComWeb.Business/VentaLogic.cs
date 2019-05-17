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
using System.Text;

namespace SisComWeb.Business
{
    public static class VentaLogic
    {
        private static readonly string UserWebSUNAT = ConfigurationManager.AppSettings["userWebSUNAT"].ToString();
        private static readonly string MotivoAnulacionFE = ConfigurationManager.AppSettings["motivoAnulacionFE"].ToString();
        private static readonly string CodiCorrelativoVentaBoleta = ConfigurationManager.AppSettings["codiCorrelativoVentaBoleta"].ToString();
        private static readonly string CodiCorrelativoVentaFactura = ConfigurationManager.AppSettings["codiCorrelativoVentaFactura"].ToString();
        private static readonly string CodiCorrelativoPaseBoleta = ConfigurationManager.AppSettings["codiCorrelativoPaseBoleta"].ToString();
        private static readonly string CodiCorrelativoPaseFactura = ConfigurationManager.AppSettings["codiCorrelativoPaseFactura"].ToString();
        private static readonly short CodiSerieReserva = short.Parse(ConfigurationManager.AppSettings["codiSerieReserva"]);

        #region BUSCAR CORRELATIVO

        public static Response<CorrelativoResponse> BuscaCorrelativo(CorrelativoRequest request)
        {
            try
            {
                var valor = new CorrelativoResponse
                {
                    CorrelativoVentaBoleta = string.Empty,
                    CorrelativoVentaFactura = string.Empty,
                    CorrelativoPaseBoleta = string.Empty,
                    CorrelativoPaseFactura = string.Empty,
                    CodiTerminalElectronico = string.Empty
                };

                var auxCorrelativos = new CorrelativoEntity[2];

                // Valida 'TerminalElectronico'
                var validarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(request.CodiEmpresa, request.CodiSucursal, request.CodiPuntoVenta, short.Parse(request.CodiTerminal));
                if (string.IsNullOrEmpty(validarTerminalElectronico.Tipo))
                    validarTerminalElectronico.Tipo = "M";

                // Seteo 'CodiTerminalElectronico'
                valor.CodiTerminalElectronico = validarTerminalElectronico.Tipo;

                switch (request.FlagVenta)
                {
                    case "7":
                        {
                            auxCorrelativos[0] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoPaseBoleta, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);
                            auxCorrelativos[1] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoPaseFactura, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);

                            if (auxCorrelativos[0].SerieBoleto != 0)
                                auxCorrelativos[0].NumeBoleto = auxCorrelativos[0].NumeBoleto + 1;
                            if (auxCorrelativos[1].SerieBoleto != 0)
                                auxCorrelativos[1].NumeBoleto = auxCorrelativos[1].NumeBoleto + 1;

                            valor.CorrelativoPaseBoleta = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoPaseBoleta, auxCorrelativos[0].SerieBoleto, auxCorrelativos[0].NumeBoleto, "3", "8");
                            valor.CorrelativoPaseFactura = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoPaseFactura, auxCorrelativos[1].SerieBoleto, auxCorrelativos[1].NumeBoleto, "3", "8");

                            switch (validarTerminalElectronico.Tipo)
                            {
                                case "E":
                                    {
                                        if (auxCorrelativos[0].SerieBoleto == 0)
                                            return new Response<CorrelativoResponse>(false, valor, Message.MsgErrorSerieBoleto, true);
                                    };
                                    break;
                            }
                        };
                        break;
                    default:
                        {
                            auxCorrelativos[0] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoVentaBoleta, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);
                            auxCorrelativos[1] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoVentaFactura, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);

                            if (auxCorrelativos[0].SerieBoleto == 0)
                                return new Response<CorrelativoResponse>(false, valor, Message.MsgErrorSerieBoleto, false);

                            if (auxCorrelativos[0].SerieBoleto != 0)
                                auxCorrelativos[0].NumeBoleto = auxCorrelativos[0].NumeBoleto + 1;
                            if (auxCorrelativos[1].SerieBoleto != 0)
                                auxCorrelativos[1].NumeBoleto = auxCorrelativos[1].NumeBoleto + 1;

                            valor.CorrelativoVentaBoleta = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoVentaBoleta, auxCorrelativos[0].SerieBoleto, auxCorrelativos[0].NumeBoleto, "3", "8");
                            valor.CorrelativoVentaFactura = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoVentaFactura, auxCorrelativos[1].SerieBoleto, auxCorrelativos[1].NumeBoleto, "3", "8");
                        };
                        break;
                };

                return new Response<CorrelativoResponse>(true, valor, Message.MsgCorrectoBuscaCorrelativo, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<CorrelativoResponse>(false, null, Message.MsgExcBuscaCorrelativo, false);
            }
        }

        #endregion

        #region GRABAR VENTA

        public static Response<VentaResponse> GrabaVenta(List<VentaEntity> Listado, string FlagVenta)
        {
            try
            {
                var valor = new VentaResponse();
                var listaVentasRealizadas = new List<VentaRealizada>();

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
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

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
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarProgramacion, false);

                        // Graba 'ViajeProgramacion'
                        var grabarViajeProgramacion = VentaRepository.GrabarViajeProgramacion(entidad.NroViaje, entidad.CodiProgramacion, entidad.FechaProgramacion, entidad.CodiBus);
                        if (!grabarViajeProgramacion)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarViajeProgramacion, false);
                    }
                    else
                        entidad.CodiProgramacion = buscarProgramacionViaje;

                    // Seteo 'valor.CodiProgramacion'
                    valor.CodiProgramacion = entidad.CodiProgramacion;

                    // Valida 'TerminalElectronico'
                    var validarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, short.Parse(entidad.CodiTerminal));
                    if (string.IsNullOrEmpty(validarTerminalElectronico.Tipo))
                        validarTerminalElectronico.Tipo = "M";

                    // PASE DE CORTESÍA
                    if (FlagVenta == "7")
                    {
                        // Valida 'SaldoPaseCortesia'
                        var validarSaldoPaseCortesia = PaseRepository.ValidarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Anno);
                        if (validarSaldoPaseCortesia <= 0)
                            return new Response<VentaResponse>(true, valor, Message.MsgValidaSaldoPaseCortesia, false);
                    }

                    // RESERVA
                    if (entidad.FlagVenta == "R")
                    {
                        // Elimina 'Reserva'
                        var eliminarReserva = VentaRepository.EliminarReserva(entidad.IdVenta);
                        if (eliminarReserva <= 0)
                            return new Response<VentaResponse>(true, valor, Message.MsgErrorEliminarReserva, false);
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
                                entidad.AuxCodigoBF_Interno = CodiCorrelativoVentaFactura;
                                break;
                            case "7": // PASE DE CORTESÍA
                                entidad.AuxCodigoBF_Interno = CodiCorrelativoPaseFactura;
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
                                entidad.AuxCodigoBF_Interno = CodiCorrelativoVentaBoleta;
                                break;
                            case "7": // PASE DE CORTESÍA
                                entidad.AuxCodigoBF_Interno = CodiCorrelativoPaseBoleta;
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
                                                    entidad.AuxCodigoBF_Interno = CodiCorrelativoPaseBoleta;
                                                    // Seteo 'CodiDocumento'
                                                    entidad.CodiDocumento = "03"; // Boleta
                                                    // Busca 'Correlativo'
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, validarTerminalElectronico.Tipo);
                                                }

                                                if (buscarCorrelativo.SerieBoleto == 0)
                                                {
                                                    // Seteo 'CodiBF Interno'
                                                    entidad.AuxCodigoBF_Interno = CodiCorrelativoVentaBoleta;
                                                    // Seteo 'CodiDocumento'
                                                    entidad.CodiDocumento = "03"; // Boleta

                                                    // Busca 'Correlativo'
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, validarTerminalElectronico.Tipo);
                                                    if (buscarCorrelativo.SerieBoleto == 0)
                                                        return new Response<VentaResponse>(false, valor, Message.MsgErrorSerieBoleto, false);
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
                                                    entidad.AuxCodigoBF_Interno = CodiCorrelativoVentaBoleta;
                                                    // Busca 'Correlativo'
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, validarTerminalElectronico.Tipo);
                                                    if (buscarCorrelativo.SerieBoleto == 0)
                                                        return new Response<VentaResponse>(false, valor, Message.MsgErrorSerieBoleto, false);
                                                }

                                                if (buscarCorrelativo.SerieBoleto == 0)
                                                    return new Response<VentaResponse>(false, valor, Message.MsgErrorSerieBoleto, false);
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
                                return new Response<VentaResponse>(false, valor, Message.MsgErrorSerieBoleto, false);
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

                    // Siempre '+ 1' al 'NumeBoleto'
                    entidad.NumeBoleto = entidad.NumeBoleto + 1;

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
                                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarVentaFechaAbierta, false);

                                    entidad.IdVenta = grabarVentaFechaAbierta;
                                    entidad.CodiProgramacion = auxCodiProgramacion;
                                }
                                else
                                {
                                    // Graba 'Venta'
                                    var grabarVenta = VentaRepository.GrabarVenta(entidad);
                                    if (grabarVenta <= 0)
                                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabaVenta, false);

                                    entidad.IdVenta = grabarVenta;
                                }

                                // Graba 'Acompañante'
                                if (!string.IsNullOrEmpty(entidad.ObjAcompaniante.TipoDocumento) && !string.IsNullOrEmpty(entidad.ObjAcompaniante.NumeroDocumento))
                                {
                                    var grabarAcompanianteVenta = VentaRepository.GrabarAcompanianteVenta(entidad.IdVenta, entidad.ObjAcompaniante);
                                    if (!grabarAcompanianteVenta)
                                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarAcompanianteVenta, false);
                                }
                            };
                            break;
                        case "E":
                            {
                                SetInvoiceRequestBody bodyDocumentoSUNAT = null;

                                // Valida 'DocumentoSUNAT'
                                var resValidarDocumentoSUNAT = ValidarDocumentoSUNAT(entidad, ref bodyDocumentoSUNAT);

                                if (resValidarDocumentoSUNAT != null && resValidarDocumentoSUNAT.Estado)
                                {
                                    if (resValidarDocumentoSUNAT.Estado)
                                    {
                                        // Valida 'FechaAbierta'
                                        if (entidad.FechaAbierta)
                                        {
                                            var auxCodiProgramacion = entidad.CodiProgramacion;
                                            entidad.CodiProgramacion = 0;

                                            // Graba 'VentaFechaAbierta'
                                            var grabarVentaFechaAbierta = PaseRepository.GrabarVentaFechaAbierta(entidad);
                                            if (grabarVentaFechaAbierta <= 0)
                                                return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarVentaFechaAbierta, false);

                                            entidad.IdVenta = grabarVentaFechaAbierta;
                                            entidad.CodiProgramacion = auxCodiProgramacion;
                                        }
                                        else
                                        {
                                            // Graba 'Venta'
                                            var grabarVenta = VentaRepository.GrabarVenta(entidad);
                                            if (grabarVenta <= 0)
                                                return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabaVenta, false);

                                            entidad.IdVenta = grabarVenta;
                                        }

                                        // Graba 'Acompañante'
                                        if (!string.IsNullOrEmpty(entidad.ObjAcompaniante.TipoDocumento) && !string.IsNullOrEmpty(entidad.ObjAcompaniante.NumeroDocumento))
                                        {
                                            var grabarAcompanianteVenta = VentaRepository.GrabarAcompanianteVenta(entidad.IdVenta, entidad.ObjAcompaniante);
                                            if (!grabarAcompanianteVenta)
                                                return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarAcompanianteVenta, false);
                                        }

                                        if (entidad.IdVenta > 0)
                                        {
                                            // Registra 'DocumentoSUNAT'
                                            var resRegistrarDocumentoSUNAT = RegistrarDocumentoSUNAT(bodyDocumentoSUNAT);
                                            if (!resRegistrarDocumentoSUNAT.Estado)
                                                return new Response<VentaResponse>(false, valor, resRegistrarDocumentoSUNAT.MensajeError, false);
                                        }
                                        else
                                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabaVenta, false);
                                    }
                                    else
                                        return new Response<VentaResponse>(false, valor, resValidarDocumentoSUNAT.MensajeError, false);
                                }
                                else
                                    return new Response<VentaResponse>(false, valor, resValidarDocumentoSUNAT.MensajeError, false);
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
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorModificarSaldoPaseCortesia, false);

                        // Graba 'PaseSocio'
                        var grabarPaseSocio = PaseRepository.GrabarPaseSocio(entidad.IdVenta, entidad.CodiGerente, entidad.CodiSocio, entidad.Concepto);
                        if (!grabarPaseSocio)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarPaseSocio, false);
                    }

                    // Valida 'LiquidacionVentas'
                    var validarLiquidacionVentas = VentaRepository.ValidarLiquidacionVentas(entidad.CodiUsuario, DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

                    // Actualiza 'LiquidacionVentas'
                    if (validarLiquidacionVentas > 0)
                    {
                        var actualizarLiquidacionVentas = VentaRepository.ActualizarLiquidacionVentas(validarLiquidacionVentas, DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture));
                        if (!actualizarLiquidacionVentas)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorActualizarLiquidacionVentas, false);
                    }
                    
                    // Graba 'LiquidacionVentas'
                    else
                    {
                        int auxCorrelativoAuxiliar = 0;

                        // Genera 'CorrelativoAuxiliar'
                        var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("LIQ_CAJA", "999", string.Empty, string.Empty);
                        if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                        auxCorrelativoAuxiliar = int.Parse(generarCorrelativoAuxiliar) + 1;

                        // Graba 'LiquidacionVentas'
                        var grabarLiquidacionVentas = VentaRepository.GrabarLiquidacionVentas(auxCorrelativoAuxiliar, entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiUsuario, entidad.PrecioVenta);
                        if (!grabarLiquidacionVentas)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarLiquidacionVentas, false);
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
                                    return new Response<VentaResponse>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

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
                                    HoraViaje = entidad.TipoPago == "03" ? entidad.HoraViaje : string.Empty,
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
                                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarPagoTarjetaCredito, false);
                                }
                                else
                                    return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarCaja, false);
                            };
                            break;
                        case "04": // Delivery
                            var grabarPagoDelivery = VentaRepository.GrabarPagoDelivery(entidad.IdVenta, entidad.CodiZona, entidad.Direccion, entidad.Observacion);
                            if (!grabarPagoDelivery)
                                return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarPagoDelivery, false);
                            break;
                    };

                    // Graba 'Auditoria'
                    var objAuditoriaEntity = new AuditoriaEntity
                    {
                        CodiUsuario = entidad.CodiUsuario,
                        NomUsuario = entidad.NomUsuario,
                        Tabla = "VENTA",
                        TipoMovimiento = "ADICION",
                        Boleto = auxBoletoCompleto.Substring(1),
                        NumeAsiento = entidad.NumeAsiento.ToString("D2"),
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
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarAuditoria, false);

                    // Para la tabla 'tb_impresion'
                    
                    var buscarEmpresaEmisor = VentaRepository.BuscarEmpresaEmisor(entidad.CodiEmpresa);
                    var buscarAgenciaEmpresa = VentaRepository.BuscarAgenciaEmpresa(entidad.CodiEmpresa, entidad.CodiPuntoVenta);

                    // ----------------------------

                    // Añado 'ventaRealizada'
                    var ventaRealizada = new VentaRealizada
                    {
                        // Para la vista 'BoletosVendidos'
                        NumeAsiento = entidad.NumeAsiento.ToString("D2"),
                        BoletoCompleto = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "8"),
                        // Para la tabla 'tb_impresion'
                        IdVenta = entidad.IdVenta,
                        NomTipVenta = "EFECTIVO",
                        BoletoTipo = entidad.Tipo,
                        BoletoSerie = entidad.SerieBoleto.ToString("D3"),
                        BoletoNum = entidad.NumeBoleto.ToString("D8"),
                        EmpRuc = buscarEmpresaEmisor.Ruc,
                        EmpRazSocial = buscarEmpresaEmisor.RazonSocial,
                        EmpDireccion = buscarEmpresaEmisor.Direccion,
                        EmpDirAgencia = buscarAgenciaEmpresa.Direccion,
                        EmpTelefono1 = buscarAgenciaEmpresa.Telefono1,
                        EmpTelefono2 = buscarAgenciaEmpresa.Telefono2,
                        CodDocumento = entidad.CodiDocumento,
                        EmisionFecha = entidad.FechaVenta,
                        EmisionHora = DateTime.Now.ToString("hh:mmtt", CultureInfo.InvariantCulture),
                        CajeroCod = entidad.CodiUsuario,
                        CajeroNom = entidad.NomUsuario,
                        PasNombreCom = entidad.SplitNombre[0] + " " + entidad.SplitNombre[1] + " " + entidad.SplitNombre[2],
                        PasRuc = entidad.RucCliente,
                        PasRazSocial = entidad.NomEmpresaRuc,
                        PasDireccion = entidad.DirEmpresaRuc,
                        NomOriPas = entidad.NomOrigen,
                        NomDesPas = entidad.NomDestino,
                        DocTipo = TipoDocumentoHomologadoParaFE(entidad.TipoDocumento),
                        DocNumero = entidad.Dni,
                        PrecioCan = entidad.PrecioVenta,
                        PrecioDes = DataUtility.MontoSolesALetras(entidad.PrecioVenta.ToString("F2", CultureInfo.InvariantCulture)),
                        NomServicio = entidad.NomServicio,
                        FechaViaje = entidad.FechaViaje,
                        EmbarqueDir = entidad.DirEmbarque,
                        EmbarqueHora = entidad.HoraEmbarque,

                        CodigoX_FE = string.Empty,
                        LinkPag_FE = string.Empty,
                        CodPoliza = string.Empty,

                        CodTerminal = validarTerminalElectronico.Tipo,
                        TipImpresora = byte.Parse(validarTerminalElectronico.Imp),

                        CodX = "1"
                    };
                    listaVentasRealizadas.Add(ventaRealizada);
                }

                // Seteo 'valor.ListaVentasRealizadas'
                valor.ListaVentasRealizadas = listaVentasRealizadas;

                return new Response<VentaResponse>(true, valor, Message.MsgCorrectoGrabaVenta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcGrabaVenta, false);
            }
        }

        #endregion

        #region GRABAR RESERVA

        public static Response<VentaResponse> GrabaReserva(List<VentaEntity> Listado)
        {
            try
            {
                var valor = new VentaResponse();
                var listaVentasRealizadas = new List<VentaRealizada>();

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
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

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
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarProgramacion, false);

                        // Graba 'ViajeProgramacion'
                        var grabarViajeProgramacion = VentaRepository.GrabarViajeProgramacion(entidad.NroViaje, entidad.CodiProgramacion, entidad.FechaProgramacion, entidad.CodiBus);
                        if (!grabarViajeProgramacion)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarViajeProgramacion, false);
                    }
                    else
                        entidad.CodiProgramacion = buscarProgramacionViaje;

                    // Seteo 'valor.CodiProgramacion'
                    valor.CodiProgramacion = entidad.CodiProgramacion;

                    // Busca 'Correlativo'
                    var generarCorrelativoAuxiliar2 = VentaRepository.GenerarCorrelativoAuxiliar("TB_RESERVAS", "999", string.Empty, string.Empty);
                    if (string.IsNullOrEmpty(generarCorrelativoAuxiliar2))
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                    entidad.SerieBoleto = CodiSerieReserva;
                    entidad.NumeBoleto = int.Parse(generarCorrelativoAuxiliar2) + 1;

                    // Seteo 'Tipo' (Reserva siempre es 'M')
                    entidad.Tipo = "M";

                    // Graba 'Venta'
                    var grabarVenta = VentaRepository.GrabarVenta(entidad);
                    if (grabarVenta > 0)
                        entidad.IdVenta = grabarVenta;
                    else
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabaVenta, false);

                    // Graba 'Acompañante'
                    if (!string.IsNullOrEmpty(entidad.ObjAcompaniante.TipoDocumento) && !string.IsNullOrEmpty(entidad.ObjAcompaniante.NumeroDocumento))
                    {
                        var grabarAcompanianteVenta = VentaRepository.GrabarAcompanianteVenta(entidad.IdVenta, entidad.ObjAcompaniante);
                        if (!grabarAcompanianteVenta)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarAcompanianteVenta, false);
                    }

                    // Seteo 'auxBoletoCompleto'
                    auxBoletoCompleto = BoletoFormatoCompleto("M", string.Empty, entidad.SerieBoleto, entidad.NumeBoleto, "3", "8");

                    // Añado 'ventaRealizada'
                    var ventaRealizada = new VentaRealizada
                    {
                        NumeAsiento = entidad.NumeAsiento.ToString("D2"),
                        BoletoCompleto = auxBoletoCompleto
                    };
                    listaVentasRealizadas.Add(ventaRealizada);
                }

                // Seteo 'valor'
                valor.ListaVentasRealizadas = listaVentasRealizadas;

                return new Response<VentaResponse>(true, valor, Message.MsgCorrectoGrabaReserva, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcGrabaReserva, false);
            }
        }

        #endregion

        #region ELIMINAR RESERVA

        public static Response<byte> EliminarReserva(int IdVenta)
        {
            try
            {
                // Elimina 'Reserva'
                var eliminarReserva = VentaRepository.EliminarReserva(IdVenta);
                if (eliminarReserva > 0)
                    return new Response<byte>(true, eliminarReserva, Message.MsgCorrectoEliminarReserva, true);
                else
                    return new Response<byte>(false, eliminarReserva, Message.MsgErrorEliminarReserva, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<byte>(false, 0, Message.MsgExcEliminarReserva, false);
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

        public static Response<byte> AnularVenta(int IdVenta, int CodiUsuario, string CodiOficina, string CodiPuntoVenta, string Tipo)
        {
            try
            {
                var anularVenta = new byte();

                var objVenta = VentaRepository.BuscarVentaById(IdVenta);

                if (objVenta.SerieBoleto == 0)
                    return new Response<byte>(false, anularVenta, Message.MsgErrorAnularVenta, true);

                // Valida 'AnularDocumentoSUNAT'
                if (objVenta.Tipo != "M")
                {
                    // Anula 'DocumentoSUNAT'
                    var resAnularDocumentoSUNAT = AnularDocumentoSUNAT(objVenta);
                    if (!resAnularDocumentoSUNAT.Estado)
                        return new Response<byte>(false, anularVenta, resAnularDocumentoSUNAT.MensajeError, false);
                }

                var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", CodiOficina, CodiPuntoVenta, string.Empty);
                if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                    return new Response<byte>(false, anularVenta, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                CajaEntity objCaja = new CajaEntity
                {
                    NumeCaja = generarCorrelativoAuxiliar,
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
                    anularVenta = VentaRepository.AnularVenta(IdVenta, CodiUsuario);
                    if (anularVenta > 0)
                        return new Response<byte>(true, anularVenta, Message.MsgCorrectoAnularVenta, true);
                    else
                        return new Response<byte>(false, anularVenta, Message.MsgErrorAnularVenta, true);
                }
                else
                    return new Response<byte>(false, 0, Message.MsgErrorGrabarCaja, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<byte>(false, 0, Message.MsgExcAnularVenta, false);
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
                    return new Response<VentaBeneficiarioEntity>(true, buscarVentaxBoleto, Message.MsgValidaBuscarVentaxBoleto, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaBeneficiarioEntity>(false, null, Message.MsgExcBuscarVentaxBoleto, false);
            }
        }

        #endregion

        #region POSTERGAR VENTA

        public static Response<VentaResponse> PostergarVenta(PostergarVentaRequest filtro)
        {
            try
            {
                var valor = new VentaResponse();
                var listaVentasRealizadas = new List<VentaRealizada>();

                var postergarVenta = new bool();

                // Busca 'ProgramacionViaje'
                var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(filtro.NroViaje, filtro.FechaProgramacion);
                if (buscarProgramacionViaje == 0)
                {
                    // Genera 'CorrelativoAuxiliar'
                    var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("TB_PROGRAMACION", "999", string.Empty, string.Empty);
                    if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

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
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarProgramacion, false);

                    // Graba 'ViajeProgramacion'
                    var grabarViajeProgramacion = VentaRepository.GrabarViajeProgramacion(filtro.NroViaje, filtro.CodiProgramacion, filtro.FechaProgramacion, filtro.CodiBus);
                    if (!grabarViajeProgramacion)
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarViajeProgramacion, false);
                }
                else
                    filtro.CodiProgramacion = buscarProgramacionViaje;

                // Seteo 'valor.CodiProgramacion'
                valor.CodiProgramacion = filtro.CodiProgramacion;

                postergarVenta = VentaRepository.PostergarVenta(filtro.IdVenta, filtro.CodiProgramacion, filtro.NumeAsiento, filtro.CodiServicio, filtro.FechaViaje, filtro.HoraViaje);
                if (postergarVenta)
                {
                    // Añado 'ventaRealizada'
                    var ventaRealizada = new VentaRealizada
                    {
                        NumeAsiento = filtro.NumeAsiento.ToString("D2"),
                        BoletoCompleto = string.Empty
                    };
                    listaVentasRealizadas.Add(ventaRealizada);

                    // Seteo 'valor.ListaVentasRealizadas'
                    valor.ListaVentasRealizadas = listaVentasRealizadas;

                    return new Response<VentaResponse>(true, valor, Message.MsgCorrectoPostergarVenta, true);
                }
                else
                    return new Response<VentaResponse>(false, valor, Message.MsgErrorPostergarVenta, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcPostergarVenta, false);
            }
        }

        #endregion

        #region MODIFICAR VENTA FECHA ABIERTA

        public static Response<byte> ModificarVentaAFechaAbierta(int IdVenta, int CodiServicio, int CodiRuta)
        {
            try
            {
                var modificarVentaAFechaAbierta = VentaRepository.ModificarVentaAFechaAbierta(IdVenta, CodiServicio, CodiRuta);
                if (modificarVentaAFechaAbierta > 0)
                    return new Response<byte>(true, modificarVentaAFechaAbierta, Message.MsgCorrectoModificarVentaAFechaAbierta, true);
                else
                    return new Response<byte>(false, modificarVentaAFechaAbierta, Message.MsgErrorModificarVentaAFechaAbierta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<byte>(false, 0, Message.MsgExcModificarVentaAFechaAbierta, false);
            }
        }

        #endregion

        #region INSERTAR IMPRESIÓN

        public static Response<List<int>> InsertarImpresion(List<VentaRealizada> Listado)
        {
            try
            {
                var valor = new List<int>();

                foreach (var entidad in Listado)
                {
                    // Inserta Impresion
                    var insertarImpresion = VentaRepository.InsertarImpresion(entidad);
                    if (!insertarImpresion)
                        return new Response<List<int>>(false, valor, Message.MsgErrorInsertarImpresion, false);

                    valor.Add(entidad.IdVenta);
                }

                return new Response<List<int>>(true, valor, Message.MsgCorrectoInsertarImpresion, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<int>>(false, null, Message.MsgExcInsertarImpresion, false);
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
                var buscarEmpresaEmisor = VentaRepository.BuscarEmpresaEmisor(entidad.CodiEmpresa);
                if (string.IsNullOrEmpty(buscarEmpresaEmisor.Ruc))
                    return null;

                var seguridadFE = new Security
                {
                    ID = buscarEmpresaEmisor.Ruc,
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
                var buscarEmpresaEmisor = VentaRepository.BuscarEmpresaEmisor(entidad.CodiEmpresa);
                if (string.IsNullOrEmpty(buscarEmpresaEmisor.Ruc))
                    return null;

                var seguridadFE = new Security
                {
                    ID = buscarEmpresaEmisor.Ruc,
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
                    var auxIdTipoDocIdentidad = TipoDocumentoHomologadoParaFE(entidad.TipoDocumento);

                    sb = sb.Replace("[IdTipoDocIdentidad]", auxIdTipoDocIdentidad.ToString());
                    sb = sb.Replace("[NumDocIdentidad]", entidad.Dni);
                    sb = sb.Replace("[RazonNombres]", (entidad.SplitNombre[0].Replace("|", string.Empty) + " " +
                                                      entidad.SplitNombre[1].Replace("|", string.Empty) + " " +
                                                      entidad.SplitNombre[2].Replace("|", string.Empty)));
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
                sb = sb.Replace("[Serie]", entidad.Tipo + entidad.SerieBoleto.ToString("D3"));
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
                    {
                        if (serieBoleto == CodiSerieReserva)
                            boletoCompleto = "0" + serieBoleto.ToString("D" + formatoSerieBol).Substring(1) + "-" + numeroBoleto.ToString("D" + formatoNumeroBol);
                        else
                            boletoCompleto = "0" + serieBoleto.ToString("D" + formatoSerieBol) + "-" + numeroBoleto.ToString("D" + formatoNumeroBol);
                    };
                    break;
                case "E":
                    {
                        if (codiDocumento == CodiCorrelativoVentaFactura || codiDocumento == CodiCorrelativoPaseFactura)
                            boletoCompleto = "F" + serieBoleto.ToString("D" + formatoSerieBol) + "-" + numeroBoleto.ToString("D" + formatoNumeroBol);

                        else if (codiDocumento == CodiCorrelativoVentaBoleta || codiDocumento == CodiCorrelativoPaseBoleta)
                            boletoCompleto = "B" + serieBoleto.ToString("D" + formatoSerieBol) + "-" + numeroBoleto.ToString("D" + formatoNumeroBol);
                    };
                    break;
            };

            return boletoCompleto;
        }

        public static byte TipoDocumentoHomologadoParaFE(string TipoDocumento)
        {
            var valor = new byte();

            if (TipoDocumento == "01")
                valor = 1;
            else if (TipoDocumento == "04")
                valor = 7;
            else
                valor = 4;

            return valor;
        }

        #endregion
    }
}
