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
        private static readonly string UserWebSUNAT = ConfigurationManager.AppSettings["userWebSUNAT"];
        private static readonly string MotivoAnulacionFE = ConfigurationManager.AppSettings["motivoAnulacionFE"];
        private static readonly string CodiCorrelativoVentaBoleta = ConfigurationManager.AppSettings["codiCorrelativoVentaBoleta"];
        private static readonly string CodiCorrelativoVentaFactura = ConfigurationManager.AppSettings["codiCorrelativoVentaFactura"];
        private static readonly string CodiCorrelativoPaseBoleta = ConfigurationManager.AppSettings["codiCorrelativoPaseBoleta"];
        private static readonly string CodiCorrelativoPaseFactura = ConfigurationManager.AppSettings["codiCorrelativoPaseFactura"];
        private static readonly string CodiCorrelativoCredito = ConfigurationManager.AppSettings["codiCorrelativoCredito"];
        private static readonly short CodiSerieReserva = short.Parse(ConfigurationManager.AppSettings["codiSerieReserva"]);
        private static readonly string TipoImprimir = ConfigurationManager.AppSettings["tipoImprimir"];
        private static readonly string TipoReimprimir = ConfigurationManager.AppSettings["tipoReimprimir"];

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
                    CorrelativoCredito = string.Empty,
                    TipoTerminalElectronico = string.Empty
                };

                var auxCorrelativos = new CorrelativoEntity[3];

                // Valida 'TerminalElectronico'
                var validarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(request.CodiEmpresa, request.CodiSucursal, request.CodiPuntoVenta, short.Parse(request.CodiTerminal));

                // Seteo 'TerminalElectronico'
                valor.TipoTerminalElectronico = validarTerminalElectronico.Tipo;
                valor.TipoImpresora = validarTerminalElectronico.Imp;

                switch (request.FlagVenta)
                {
                    case "7":
                        {
                            auxCorrelativos[0] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoPaseBoleta, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);
                            // Validación
                            switch (validarTerminalElectronico.Tipo)
                            {
                                case "E":
                                    {
                                        if (auxCorrelativos[0].SerieBoleto == 0)
                                            return new Response<CorrelativoResponse>(false, valor, Message.MsgErrorSerieBoleto, true);
                                    };
                                    break;
                            }
                            // ----------
                            auxCorrelativos[1] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoPaseFactura, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);

                            if (auxCorrelativos[0].SerieBoleto != 0)
                                auxCorrelativos[0].NumeBoleto = auxCorrelativos[0].NumeBoleto + 1;
                            if (auxCorrelativos[1].SerieBoleto != 0)
                                auxCorrelativos[1].NumeBoleto = auxCorrelativos[1].NumeBoleto + 1;

                            valor.CorrelativoPaseBoleta = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoPaseBoleta, auxCorrelativos[0].SerieBoleto, auxCorrelativos[0].NumeBoleto, "3", "8");
                            valor.CorrelativoPaseFactura = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoPaseFactura, auxCorrelativos[1].SerieBoleto, auxCorrelativos[1].NumeBoleto, "3", "8");
                        };
                        break;
                    default:
                        {
                            auxCorrelativos[0] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoVentaBoleta, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);
                            // Validación
                            if (auxCorrelativos[0].SerieBoleto == 0)
                                return new Response<CorrelativoResponse>(false, valor, Message.MsgErrorSerieBoleto, false);
                            // ----------
                            auxCorrelativos[1] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoVentaFactura, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);

                            if (auxCorrelativos[0].SerieBoleto != 0)
                                auxCorrelativos[0].NumeBoleto = auxCorrelativos[0].NumeBoleto + 1;
                            if (auxCorrelativos[1].SerieBoleto != 0)
                                auxCorrelativos[1].NumeBoleto = auxCorrelativos[1].NumeBoleto + 1;

                            valor.CorrelativoVentaBoleta = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoVentaBoleta, auxCorrelativos[0].SerieBoleto, auxCorrelativos[0].NumeBoleto, "3", "8");
                            valor.CorrelativoVentaFactura = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoVentaFactura, auxCorrelativos[1].SerieBoleto, auxCorrelativos[1].NumeBoleto, "3", "8");

                            // Correlativo '20'
                            switch (validarTerminalElectronico.Tipo)
                            {
                                case "M":
                                    {
                                        auxCorrelativos[2] = VentaRepository.BuscarCorrelativo(request.CodiEmpresa, CodiCorrelativoCredito, request.CodiSucursal, request.CodiPuntoVenta, request.CodiTerminal, validarTerminalElectronico.Tipo);

                                        if (auxCorrelativos[2].SerieBoleto != 0)
                                            auxCorrelativos[2].NumeBoleto = auxCorrelativos[2].NumeBoleto + 1;

                                        valor.CorrelativoCredito = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, CodiCorrelativoCredito, auxCorrelativos[2].SerieBoleto, auxCorrelativos[2].NumeBoleto, "3", "8");
                                    };
                                    break;
                            }
                            // ----------------
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
                var listaVentasRealizadas = new List<VentaRealizadaEntity>();
                var ListarPanelControl = CreditoRepository.ListarPanelControl();

                foreach (var entidad in Listado)
                {
                    string auxBoletoCompleto = string.Empty;
                    entidad.UserWebSUNAT = UserWebSUNAT;

                    var consultaNroPoliza = new PolizaEntity()
                    {
                        NroPoliza = string.Empty,
                        FechaReg = "01/01/1900",
                        FechaVen = "01/01/1900"
                    };
                    var buscarAgenciaEmpresa = new AgenciaEntity()
                    {
                        Direccion = string.Empty,
                        Telefono1 = string.Empty,
                        Telefono2 = string.Empty
                    };
                    var buscarCorrelativo = new CorrelativoEntity();

                    // Verifica 'CodiProgramacion'
                    var objProgramacion = new ProgramacionEntity()
                    {
                        NroViaje = entidad.NroViaje,
                        FechaProgramacion = entidad.FechaProgramacion,
                        CodiProgramacion = entidad.CodiProgramacion,
                        CodiEmpresa = entidad.CodiEmpresa,
                        CodiSucursal = entidad.CodiSucursal,
                        CodiRuta = entidad.CodiRuta,
                        CodiBus = entidad.CodiBus,
                        HoraProgramacion = entidad.HoraProgramacion,
                        CodiServicio = entidad.CodiServicio,

                        CodiUsuario = entidad.CodiUsuario.ToString(),
                        NomUsuario = entidad.NomUsuario,
                        CodiPuntoVenta = entidad.CodiPuntoVenta.ToString(),
                        Terminal = entidad.CodiTerminal,
                        CodiOrigen = entidad.CodiOrigen.ToString(),
                        CodiDestino = entidad.CodiDestino.ToString(),
                        NomOrigen = entidad.NomOrigen
                    };
                    var verificaCodiProgramacion = VerificaCodiProgramacion(objProgramacion);
                    if (verificaCodiProgramacion == 0)
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorVerificaCodiProgramacion, false);
                    else
                        entidad.CodiProgramacion = verificaCodiProgramacion;

                    // Seteo 'valor.CodiProgramacion'
                    valor.CodiProgramacion = entidad.CodiProgramacion;

                    // Valida 'SaldoPaseCortesia', consulta 'Contrato' y otros
                    switch (FlagVenta)
                    {
                        case "7": // PASE DE CORTESÍA
                            {
                                // Valida 'SaldoPaseCortesia'
                                var validarSaldoPaseCortesia = PaseRepository.ValidarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Anno);
                                if (validarSaldoPaseCortesia <= 0)
                                    return new Response<VentaResponse>(false, valor, Message.MsgValidaSaldoPaseCortesia, true);
                            };
                            break;
                        case "1": // CRÉDITO
                            {
                                // Consulta 'Contrato'
                                ContratoEntity consultarContrato = CreditoRepository.ConsultarContrato(entidad.IdContrato);
                                if (consultarContrato != null)
                                {
                                    if (consultarContrato.Marcador == "1")
                                    {
                                        if (consultarContrato.Saldo < entidad.PrecioVenta)
                                            return new Response<VentaResponse>(false, valor, Message.MsgValidaConsultarContrato, true);
                                    }
                                }
                                else
                                    return new Response<VentaResponse>(false, valor, Message.MsgValidaNullConsultarContrato, true);

                                // Agrega validación de Precio que está en la vista.
                                var validatorPrecioValor = false;

                                var objPanelPrecioValor = ListarPanelControl.Find(x => x.CodiPanel == "121");
                                if (objPanelPrecioValor != null && objPanelPrecioValor.Valor == "1")
                                    validatorPrecioValor = true;

                                if (!validatorPrecioValor)
                                {
                                    // Verifica 'PrecioNormal'
                                    var verificarPrecioNormal = CreditoRepository.VerificarPrecioNormal(entidad.IdContrato);
                                    if (verificarPrecioNormal.IdNormal != -1)
                                    {
                                        if (verificarPrecioNormal.Saldo <= 0)
                                            return new Response<VentaResponse>(false, valor, Message.MsgErrorVerificarPrecioNormal, true);
                                    }
                                    else
                                    {
                                        var verificarContratoPasajes = CreditoRepository.VerificarContratoPasajes(entidad.RucCliente, entidad.FechaViaje, entidad.FechaViaje, entidad.CodiOrigen.ToString(), entidad.CodiDestino.ToString(), entidad.CodiServicio.ToString(), entidad.IdRuc);

                                        if (verificarContratoPasajes.SaldoBoletos <= 0)
                                            return new Response<VentaResponse>(false, valor, Message.MsgErrorVerificarPrecioNormal, true);
                                    }
                                }
                            };
                            break;
                    }

                    // Consulta 'NroPoliza'
                    var objPanelPoliza = ListarPanelControl.Find(x => x.CodiPanel == "224");
                    if (objPanelPoliza != null && objPanelPoliza.Valor == "1")
                    {
                        consultaNroPoliza = VentaRepository.ConsultaNroPoliza(entidad.CodiEmpresa, entidad.CodiBus, entidad.FechaViaje);
                        if (string.IsNullOrEmpty(consultaNroPoliza.NroPoliza))
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorConsultaNroPoliza, true);
                    }

                    entidad.PolizaNum = consultaNroPoliza.NroPoliza;
                    entidad.PolizaFechaReg = consultaNroPoliza.FechaReg;
                    entidad.PolizaFechaVen = consultaNroPoliza.FechaVen;

                    // RESERVA
                    if (entidad.FlagVenta == "R")
                    {
                        // Elimina 'Reservas' por escala
                        TurnoRepository.EliminarReservas02(entidad.CodiOrigen.ToString(), entidad.CodiProgramacion, entidad.HoraEscala, entidad.FechaViaje);
                        TurnoRepository.EliminarReservas01(entidad.CodiProgramacion, entidad.HoraEscala);

                        // Elimina 'Reserva'
                        var eliminarReserva = VentaRepository.EliminarReserva(entidad.IdVenta);
                        if (eliminarReserva <= 0)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorEliminarReserva, true);
                        // Como mandamos 'IdVenta' para 'EliminarReserva', lo volvemos a su valor por defecto.
                        entidad.IdVenta = 0;
                        // Cuando 'confirmasReserva' se venderá como una 'Venta'.
                        entidad.FlagVenta = "V";
                    }

                    // Seteo 'CodiDocumento'
                    if (!string.IsNullOrEmpty(entidad.RucCliente))
                    {
                        // Seteo 'AuxCodigoBF_Interno'
                        switch (FlagVenta)
                        {
                            case "V": // VENTA
                            case "1": // CRÉDITO
                                {
                                    // Correlativo '20'
                                    switch (entidad.CodiTerminal)
                                    {
                                        case "M":
                                            {
                                                var objPanelCorrelativoCredito = ListarPanelControl.Find(x => x.CodiPanel == "105");
                                                if (objPanelCorrelativoCredito != null && objPanelCorrelativoCredito.Valor == "1")
                                                {
                                                    if (entidad.FlagVenta != "1")
                                                    {
                                                        var objPanelCorrelativoCredito02 = ListarPanelControl.Find(x => x.CodiPanel == "145");
                                                        if (objPanelCorrelativoCredito02 != null && objPanelCorrelativoCredito02.Valor == "1")
                                                        {
                                                            entidad.AuxCodigoBF_Interno = CodiCorrelativoCredito;

                                                            // Busca 'Correlativo'
                                                            buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.TipoTerminalElectronico);
                                                            entidad.SerieBoleto = buscarCorrelativo.SerieBoleto;
                                                            entidad.NumeBoleto = buscarCorrelativo.NumeBoleto;
                                                        }
                                                    }
                                                }
                                            };
                                            break;
                                    };

                                    // Para continuar el flujo normal
                                    if (buscarCorrelativo.SerieBoleto == 0)
                                        entidad.AuxCodigoBF_Interno = CodiCorrelativoVentaFactura;
                                };
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
                            case "1": // CRÉDITO
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
                    if (entidad.AuxCodigoBF_Interno != CodiCorrelativoCredito)
                    {
                        buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.TipoTerminalElectronico);
                        entidad.SerieBoleto = buscarCorrelativo.SerieBoleto;
                        entidad.NumeBoleto = buscarCorrelativo.NumeBoleto;
                    }

                    if (buscarCorrelativo.SerieBoleto == 0)
                    {
                        switch (entidad.TipoTerminalElectronico)
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
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.TipoTerminalElectronico);
                                                }

                                                if (buscarCorrelativo.SerieBoleto == 0)
                                                {
                                                    // Seteo 'CodiBF Interno'
                                                    entidad.AuxCodigoBF_Interno = CodiCorrelativoVentaBoleta;
                                                    // Seteo 'CodiDocumento'
                                                    entidad.CodiDocumento = "03"; // Boleta

                                                    // Busca 'Correlativo'
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.TipoTerminalElectronico);
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
                                                    buscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.TipoTerminalElectronico);
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
                    switch (entidad.TipoTerminalElectronico)
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

                    // Busca 'AgenciaEmpresa' (E -> GenerarAdicionales, M -> También se va a necesitar.)
                    buscarAgenciaEmpresa = VentaRepository.BuscarAgenciaEmpresa(entidad.CodiEmpresa, entidad.CodiPuntoVenta);
                    entidad.EmpDirAgencia = buscarAgenciaEmpresa.Direccion;
                    entidad.EmpTelefono1 = buscarAgenciaEmpresa.Telefono1;
                    entidad.EmpTelefono2 = buscarAgenciaEmpresa.Telefono2;

                    // Graba 'Venta', 'FacturaciónElectrónica' y otros
                    SetInvoiceRequestBody bodyDocumentoSUNAT = null;

                    // Valida 'DocumentoSUNAT'
                    var resValidarDocumentoSUNAT = ValidarDocumentoSUNAT(entidad, ref bodyDocumentoSUNAT);

                    if (resValidarDocumentoSUNAT != null)
                    {
                        if (resValidarDocumentoSUNAT.Estado)
                        {
                            // Valida 'FechaAbierta'
                            if (entidad.FechaAbierta)
                            {
                                var auxCodiProgramacion = entidad.CodiProgramacion;
                                entidad.CodiProgramacion = 0;

                                // Graba 'VentaFechaAbierta'
                                var grabarVentaFechaAbierta = VentaRepository.GrabarVenta(entidad);
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

                            if (entidad.IdVenta > 0)
                            {
                                if (entidad.TipoTerminalElectronico == "E" && entidad.ElectronicoEmpresa == "1")
                                {
                                    //Registra 'DocumentoSUNAT'
                                    var resRegistrarDocumentoSUNAT = RegistrarDocumentoSUNAT(bodyDocumentoSUNAT);

                                    if (resRegistrarDocumentoSUNAT.Estado)
                                        entidad.SignatureValue = resRegistrarDocumentoSUNAT.SignatureValue ?? string.Empty;
                                    else
                                        return new Response<VentaResponse>(false, valor, resRegistrarDocumentoSUNAT.MensajeError, false);
                                }
                            }
                            else
                                return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabaVenta, false);
                        }
                        else
                            return new Response<VentaResponse>(false, valor, resValidarDocumentoSUNAT.MensajeError, false);
                    }
                    else
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorWebServiceFacturacionElectronica, false);

                    // Graba 'Acompañante'
                    if (!string.IsNullOrEmpty(entidad.ObjAcompaniante.TipoDocumento) && !string.IsNullOrEmpty(entidad.ObjAcompaniante.NumeroDocumento))
                    {
                        var objAcompanianteRequest = new AcompanianteRequest()
                        {
                            IdVenta = entidad.IdVenta,
                            Acompaniante = entidad.ObjAcompaniante,
                            ActionType = 1
                        };
                        VentaRepository.AcompanianteVentaCRUD(objAcompanianteRequest);
                    }

                    // Seteo 'auxBoletoCompleto'
                    auxBoletoCompleto = BoletoFormatoCompleto(entidad.TipoTerminalElectronico, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "7");

                    // Modifica 'SaldoPaseCortesia', actualiza 'BoletosStock' y otros
                    switch (FlagVenta)
                    {
                        case "7": // PASE DE CORTESÍA
                            {
                                // Modifica 'SaldoPaseCortesia'
                                PaseRepository.ModificarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Anno);

                                // Graba 'PaseSocio'
                                PaseRepository.GrabarPaseSocio(entidad.IdVenta, entidad.CodiGerente, entidad.CodiSocio, entidad.Concepto);
                            };
                            break;
                        case "1": // CRÉDITO
                            {
                                // Actualiza 'BoletosStock'
                                if (!entidad.FlagPrecioNormal)
                                    CreditoRepository.ActualizarBoletosStock("1", entidad.IdPrecio.ToString(), "1");
                                else
                                    CreditoRepository.ActualizarBoletosStock("1", entidad.IdContrato.ToString(), "0");
                            };
                            break;
                    }

                    // Inserta 'DescuentoBoleto'
                    if (entidad.ValidadorDescuento)
                    {
                        var descuentoBoleto = new DescuentoBoletoEntity()
                        {
                            Usuario = entidad.CodiUsuario,
                            Oficina = entidad.CodiOficina,
                            Motivo = entidad.ObservacionDescuento,
                            Boleto = auxBoletoCompleto.Substring(1),
                            ImpTeorico = entidad.PrecioNormal,
                            ImpReal = entidad.PrecioVenta,
                            Servicio = entidad.CodiServicio,
                            Origen = entidad.CodiSucursal,
                            Destino = entidad.CodiDestino
                        };
                        VentaRepository.InsertarDescuentoBoleto(descuentoBoleto);
                    }

                    // Inserta 'DescuentoVenta'
                    if (entidad.ValidadorDescuentoControl)
                        VentaRepository.InsertarDescuentoVenta(entidad.IdVenta, entidad.DescuentoTipoDC, entidad.ImporteDescuentoDC, entidad.ImporteDescontadoDC, entidad.AutorizadoDC);

                    // Valida 'LiquidacionVentas'
                    var validarLiquidacionVentas = VentaRepository.ValidarLiquidacionVentas(entidad.CodiUsuario, DataUtility.ObtenerFechaDelSistema());

                    // Actualiza 'LiquidacionVentas'
                    if (validarLiquidacionVentas > 0)
                        VentaRepository.ActualizarLiquidacionVentas(validarLiquidacionVentas, DataUtility.Obtener12HorasDelSistema());

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
                        VentaRepository.GrabarLiquidacionVentas(auxCorrelativoAuxiliar, entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiUsuario, entidad.PrecioVenta);
                    }

                    // Valida 'TipoPago'
                    switch (entidad.TipoPago)
                    {
                        case "01": // Contado
                            break;
                        case "02": // Múltiple pago
                        case "03": // Tarjeta de crédito    
                            {
                                //  Genera 'CorrelativoAuxiliar'
                                var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", entidad.CodiOficina.ToString(), entidad.CodiPuntoVenta.ToString(), string.Empty);

                                // Graba 'Caja'
                                var objCajaEntity = new CajaEntity
                                {
                                    NumeCaja = generarCorrelativoAuxiliar.PadLeft(7, '0'),
                                    CodiEmpresa = entidad.CodiEmpresa,
                                    CodiSucursal = entidad.CodiOficina,
                                    FechaCaja = DataUtility.ObtenerFechaDelSistema(),
                                    TipoVale = "S",
                                    Boleto = auxBoletoCompleto.Substring(1),
                                    NomUsuario = entidad.NomUsuario,
                                    CodiBus = string.Empty,
                                    CodiChofer = string.Empty,
                                    CodiGasto = string.Empty,
                                    ConcCaja = auxBoletoCompleto,
                                    Monto = entidad.TipoPago == "03" ? entidad.PrecioVenta : entidad.Credito,
                                    CodiUsuario = entidad.CodiUsuario,
                                    IndiAnulado = "F",
                                    TipoDescuento = string.Empty,
                                    TipoDoc = "XX",
                                    TipoGasto = "P",
                                    Liqui = 0M,
                                    Diferencia = 0M,
                                    Recibe = entidad.TipoPago == "03" ? (entidad.FlagVenta == "Y" || entidad.FlagVenta == "I" ? "REMOTO" : string.Empty) : "PAGO MULTIPLE-PARCIAL",
                                    CodiDestino = entidad.CodiDestino.ToString(),
                                    FechaViaje = entidad.FechaViaje,
                                    HoraViaje = entidad.HoraViaje,
                                    CodiPuntoVenta = entidad.CodiPuntoVenta,
                                    Voucher = "PA",
                                    Asiento = string.Empty,
                                    Ruc = entidad.IngresoManualPasajes ? "M" : "N",
                                    IdVenta = entidad.IdVenta,
                                    Origen = "VT",
                                    Modulo = entidad.TipoPago == "03" ? "PV" : "PA",
                                    Tipo = entidad.Tipo,

                                    IdCaja = 0
                                };

                                var grabarCaja = VentaRepository.GrabarCaja(objCajaEntity);

                                // Seteo 'NumeCaja'
                                var auxNumeCaja = entidad.CodiOficina.ToString("D3") + entidad.CodiPuntoVenta.ToString("D3") + generarCorrelativoAuxiliar.PadLeft(7, '0');

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
                                VentaRepository.GrabarPagoTarjetaCredito(objTarjetaCreditoEntity);
                            };
                            break;
                        case "04": // Delivery
                            VentaRepository.GrabarPagoDelivery(entidad.IdVenta, entidad.CodiZona, entidad.Direccion, entidad.Observacion);
                            break;
                    };

                    // Graba 'AuditoriaFechaAbierta'
                    if (entidad.FechaAbierta)
                    {
                        var objAuditoriaFechaAbierta = new AuditoriaEntity
                        {
                            CodiUsuario = entidad.CodiUsuario,
                            NomUsuario = entidad.NomUsuario,
                            Tabla = "VENTA",
                            TipoMovimiento = "POSTERGACION DE PASAJES",
                            Boleto = auxBoletoCompleto.Substring(1),
                            NumeAsiento = entidad.NumeAsiento.ToString("D2"),
                            NomOficina = entidad.NomOficina,
                            NomPuntoVenta = entidad.CodiPuntoVenta.ToString(),
                            Pasajero = entidad.Nombre,
                            FechaViaje = entidad.FechaViaje,
                            HoraViaje = entidad.HoraViaje,
                            NomDestino = entidad.NomDestino,
                            Precio = entidad.PrecioVenta,
                            Obs1 = string.Empty,
                            Obs2 = string.Empty,
                            Obs3 = string.Empty,
                            Obs4 = "POSTERGADO A FECHA ABIERTA",
                            Obs5 = "TERMINAL : " + entidad.CodiTerminal
                        };
                        VentaRepository.GrabarAuditoria(objAuditoriaFechaAbierta);
                    }

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
                        NomPuntoVenta = entidad.CodiPuntoVenta.ToString(),
                        Pasajero = entidad.Nombre,
                        FechaViaje = entidad.FechaViaje,
                        HoraViaje = entidad.HoraViaje,
                        NomDestino = entidad.NomDestino,
                        Precio = entidad.PrecioVenta,
                        Obs1 = "ID " + entidad.IdVenta + " VENTA DE PASAJES",
                        Obs2 = "Empresa : " + entidad.CodiEmpresa.ToString("D2"),
                        Obs3 = "TERMINAL : " + entidad.CodiTerminal + " SER. " + entidad.CodiServicio.ToString("D2"),
                        Obs4 = "PROGRAMACION" + entidad.CodiProgramacion + " ORG PAS " + entidad.CodiOrigen.ToString("D3"),
                        Obs5 = "DET BUS " + entidad.CodiRuta.ToString("D3") + " DET PAS " + entidad.CodiDestino.ToString("D3")
                    };
                    VentaRepository.GrabarAuditoria(objAuditoriaEntity);

                    // Añado 'ventaRealizada'
                    var ventaRealizada = new VentaRealizadaEntity
                    {
                        // Para la vista 'BoletosVendidos'
                        BoletoCompleto = BoletoFormatoCompleto(entidad.TipoTerminalElectronico, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "8"),
                        NumeAsiento = entidad.NumeAsiento.ToString("D2"),
                        // Para el método 'ConvertirVentaToBase64'
                        IdVenta = entidad.IdVenta,
                        BoletoTipo = entidad.Tipo,
                        BoletoSerie = entidad.SerieBoleto.ToString("D3"),
                        BoletoNum = entidad.NumeBoleto.ToString("D8"),
                        CodDocumento = entidad.CodiDocumento,
                        EmisionFecha = DataUtility.ObtenerFechaDelSistema(),
                        EmisionHora = DataUtility.Obtener12HorasDelSistema(),
                        CajeroCod = entidad.CodiUsuario,
                        CajeroNom = entidad.NomUsuario,
                        PasNombreCom = entidad.SplitNombre[0] + " " + entidad.SplitNombre[1] + " " + entidad.SplitNombre[2],
                        PasRuc = entidad.RucCliente,
                        PasRazSocial = entidad.NomEmpresaRuc,
                        PasDireccion = entidad.DirEmpresaRuc,
                        NomOriPas = entidad.NomOrigen,
                        NomDesPas = entidad.NomDestino,
                        DocTipo = TipoDocumentoHomologado(entidad.TipoDocumento),
                        DocNumero = entidad.Dni,
                        PrecioCan = entidad.PrecioVenta,
                        PrecioDes = DataUtility.MontoSolesALetras(DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta)),
                        NomServicio = entidad.NomServicio,
                        FechaViaje = entidad.FechaViaje,
                        EmbarqueDir = entidad.DirEmbarque,
                        EmbarqueHora = entidad.HoraEmbarque,
                        CodigoX_FE = entidad.SignatureValue ?? string.Empty,
                        TipoTerminalElectronico = entidad.TipoTerminalElectronico,
                        TipoImpresora = entidad.TipoImpresora,
                        EmpDirAgencia = entidad.EmpDirAgencia ?? string.Empty,
                        EmpTelefono1 = entidad.EmpTelefono1 ?? string.Empty,
                        EmpTelefono2 = entidad.EmpTelefono2 ?? string.Empty,
                        PolizaNum = entidad.PolizaNum,
                        PolizaFechaReg = entidad.PolizaFechaReg,
                        PolizaFechaVen = entidad.PolizaFechaVen,

                        EmpRuc = entidad.RucEmpresa,
                        EmpRazSocial = entidad.NomEmpresa,
                        EmpDireccion = entidad.DireccionEmpresa,
                        EmpElectronico = entidad.ElectronicoEmpresa,

                        // Parámetros extras
                        EmpCodigo = entidad.CodiEmpresa,
                        PVentaCodigo = entidad.CodiPuntoVenta,
                        BusCodigo = entidad.CodiBus,
                        EmbarqueCod = entidad.CodiEmbarque
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
                var listaVentasRealizadas = new List<VentaRealizadaEntity>();

                foreach (var entidad in Listado)
                {
                    string auxBoletoCompleto = string.Empty;

                    // Si 'TipoDoc' es raya, generamos 'Documento'
                    if (entidad.TipoDocumento == "00")
                    {
                        var verificaClientesP = VentaRepository.VerificaClientesP(string.Empty);
                        if (!string.IsNullOrEmpty(verificaClientesP))
                            entidad.Dni = verificaClientesP;
                        else
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorVerificaClientesP, false);
                    }

                    // Verifica 'CodiProgramacion'
                    var objProgramacion = new ProgramacionEntity()
                    {
                        NroViaje = entidad.NroViaje,
                        FechaProgramacion = entidad.FechaProgramacion,
                        CodiProgramacion = entidad.CodiProgramacion,
                        CodiEmpresa = entidad.CodiEmpresa,
                        CodiSucursal = entidad.CodiSucursal,
                        CodiRuta = entidad.CodiRuta,
                        CodiBus = entidad.CodiBus,
                        HoraProgramacion = entidad.HoraProgramacion,
                        CodiServicio = entidad.CodiServicio,

                        CodiUsuario = entidad.CodiUsuario.ToString(),
                        NomUsuario = entidad.NomUsuario,
                        CodiPuntoVenta = entidad.CodiPuntoVenta.ToString(),
                        Terminal = entidad.CodiTerminal,
                        CodiOrigen = entidad.CodiOrigen.ToString(),
                        CodiDestino = entidad.CodiDestino.ToString(),
                        NomOrigen = entidad.NomOrigen
                    };
                    var verificaCodiProgramacion = VerificaCodiProgramacion(objProgramacion);
                    if (verificaCodiProgramacion <= 0)
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorVerificaCodiProgramacion, false);
                    else
                        entidad.CodiProgramacion = verificaCodiProgramacion;

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

                    // Graba 'Auditoria'
                    var objAuditoriaEntity = new AuditoriaEntity
                    {
                        CodiUsuario = entidad.CodiUsuario,
                        NomUsuario = entidad.NomUsuario,
                        Tabla = "RESERVACION",
                        TipoMovimiento = "RESERVA DE PASAJES",
                        Boleto = entidad.SerieBoleto.ToString() + "-" + entidad.NumeBoleto.ToString("D7"),
                        NumeAsiento = entidad.NumeAsiento.ToString("D2"),
                        NomOficina = entidad.NomOficina,
                        NomPuntoVenta = entidad.NomPuntoVenta,
                        Pasajero = entidad.Nombre,
                        FechaViaje = entidad.FechaViaje,
                        HoraViaje = entidad.HoraViaje,
                        NomDestino = entidad.NomDestino,
                        Precio = entidad.PrecioVenta,
                        Obs1 = "RESERVACION DE PASAJES",
                        Obs2 = "RESERVACION AUTOMATICA",
                        Obs3 = "hora_c=" + entidad.HoraReservacion,
                        Obs4 = "Fecha_c=" + entidad.HoraReservacion,
                        Obs5 = "TERMINAL : " + entidad.CodiTerminal
                    };
                    VentaRepository.GrabarAuditoria(objAuditoriaEntity);

                    // Graba 'Venta'
                    var grabarVenta = VentaRepository.GrabarVenta(entidad);
                    if (grabarVenta > 0)
                        entidad.IdVenta = grabarVenta;
                    else
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabaVenta, false);

                    // Graba 'Acompañante'
                    if (!string.IsNullOrEmpty(entidad.ObjAcompaniante.TipoDocumento) && !string.IsNullOrEmpty(entidad.ObjAcompaniante.NumeroDocumento))
                    {
                        var objAcompanianteRequest = new AcompanianteRequest()
                        {
                            IdVenta = entidad.IdVenta,
                            Acompaniante = entidad.ObjAcompaniante,
                            ActionType = 1
                        };
                        VentaRepository.AcompanianteVentaCRUD(objAcompanianteRequest);
                    }

                    VentaRepository.InsertarReservacionHoraFecha(entidad.IdVenta, entidad.SerieBoleto.ToString() + "-" + entidad.NumeBoleto.ToString("D7"), entidad.FechaReservacion, entidad.HoraReservacion, entidad.Nombre, entidad.CodiUsuario, entidad.CodiPuntoVenta);

                    // Seteo 'auxBoletoCompleto'
                    auxBoletoCompleto = BoletoFormatoCompleto("M", string.Empty, entidad.SerieBoleto, entidad.NumeBoleto, "3", "8");

                    // Añado 'ventaRealizada'
                    var ventaRealizada = new VentaRealizadaEntity
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

        public static Response<string> VerificaClaveReserva(int CodiUsr, string Password)
        {
            try
            {
                var verificaClaveReserva = VentaRepository.VerificaClaveReserva(CodiUsr);

                if (verificaClaveReserva == Password)
                    return new Response<string>(true, verificaClaveReserva, Message.MsgCorrectoVerificaClaveReserva, true);
                else
                    return new Response<string>(false, verificaClaveReserva, Message.MsgErrorVerificaClaveReserva, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaClaveReserva, false);
            }
        }

        public static Response<string> VerificaClaveTbClaveRe(int CodiUsr)
        {
            try
            {
                var verificaClaveTbClaveRe = VentaRepository.VerificaClaveTbClaveRe(CodiUsr);

                return new Response<string>(true, verificaClaveTbClaveRe, Message.MsgCorrectoVerificaClaveReserva, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaClaveReserva, false);
            }
        }

        public static Response<string> VerificaHoraConfirmacion(int Origen, int Destino)
        {
            try
            {
                var verificaHoraConfirmacion = VentaRepository.VerificaHoraConfirmacion(Origen, Destino);

                return new Response<string>(true, verificaHoraConfirmacion, Message.MsgCorrectoVerificaHoraConfirmacion, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaHoraConfirmacion, false);
            }
        }

        public static Response<ReservacionEntity> ObtenerTiempoReserva()
        {
            try
            {
                var obtenerTiempoReserva = VentaRepository.ObtenerTiempoReserva();

                return new Response<ReservacionEntity>(true, obtenerTiempoReserva, Message.MsgCorrectoObtenerTiempoReserva, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReservacionEntity>(false, null, Message.MsgExcObtenerTiempoReserva, false);
            }
        }

        #endregion

        #region ELIMINAR RESERVA

        public static Response<byte> EliminarReserva(CancelarReservaRequest request)
        {
            try
            {
                // Elimina 'Reserva'
                var eliminarReserva = VentaRepository.EliminarReserva(request.IdVenta);
                if (eliminarReserva > 0)
                {
                    // Graba 'Auditoria'
                    var objAuditoriaEntity = new AuditoriaEntity
                    {
                        CodiUsuario = request.CodiUsuario,
                        NomUsuario = request.NomUsuario,
                        Tabla = "RESERVACION",
                        TipoMovimiento = "ANULACION DE RESERVA",
                        Boleto = request.Boleto.Substring(4),
                        NumeAsiento = request.NumeAsiento.ToString("D2"),
                        NomOficina = request.NomOficina,
                        NomPuntoVenta = request.NomPuntoVenta,
                        Pasajero = request.NomPasajero,
                        FechaViaje = request.FechaViaje,
                        HoraViaje = request.HoraViaje,
                        NomDestino = request.NomDestinoPas,
                        Precio = request.PrecioVenta,
                        Obs1 = "ANULACION DE RESERVACION",
                        Obs2 = string.Empty,
                        Obs3 = string.Empty,
                        Obs4 = string.Empty,
                        Obs5 = "TER. : " + request.Terminal.ToString("D3")
                    };
                    VentaRepository.GrabarAuditoria(objAuditoriaEntity);

                    return new Response<byte>(true, eliminarReserva, Message.MsgCorrectoEliminarReserva, true);
                }
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

        #region CRUD ACOMPANIANTE

        public static Response<bool> AcompanianteVentaCRUD(AcompanianteRequest request)
        {
            try
            {
                var acompanianteVentaCRUD = VentaRepository.AcompanianteVentaCRUD(request);

                return new Response<bool>(true, acompanianteVentaCRUD, Message.MsgCorrectoAcompanianteVentaCRUD, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcAcompanianteVentaCRUD, false);
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
                    return new Response<bool>(false, false, Message.MsgValidaClavesInternas, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcClavesInternas, false);
            }
        }

        #endregion

        #region ANULAR VENTA

        public static Response<byte> AnularVenta(AnularVentaRequest request)
        {
            try
            {
                var anularVenta = new byte();
                var ListarPanelControl = CreditoRepository.ListarPanelControl();

                var objVenta = VentaRepository.BuscarVentaById(request.IdVenta);
                if (objVenta.SerieBoleto == 0)
                    return new Response<byte>(false, anularVenta, Message.MsgErrorAnularVenta, true);

                // Valida 'AnularDocumentoSUNAT'
                if (request.Tipo != "M" && request.FlagVenta != "Y" && request.ElectronicoEmpresa == "1")
                {
                    // Anula 'DocumentoSUNAT'
                    objVenta.Tipo = request.Tipo;
                    objVenta.FechaVenta = request.FechaVenta;
                    objVenta.RucEmpresa = request.RucEmpresa;

                    var resAnularDocumentoSUNAT = AnularDocumentoSUNAT(objVenta);
                    if (!resAnularDocumentoSUNAT.Estado)
                        return new Response<byte>(false, anularVenta, resAnularDocumentoSUNAT.MensajeError, false);
                }

                if (request.FlagVenta == "8" || request.FlagVenta == "7")
                    // Actualiza 'VentaPromokmt'
                    VentaRepository.ActualizarVentaPromokmt(request.IdVenta);

                // Verifica 'VentaPromokmt'
                if (VentaRepository.VerificaVentaPromoKmt(request.IdVenta))
                    // Elimina 'VentaPromokmt'
                    VentaRepository.EliminarVentaPromokmt(request.IdVenta);

                if (request.TipoPago == "03")
                {
                    if (request.FechaVenta == DataUtility.ObtenerFechaDelSistema())
                    {
                        // Consulta 'PagoTarjetaVenta'
                        var consultaPagoTarjetaVenta = VentaRepository.ConsultaPagoTarjetaVenta(request.IdVenta);
                        // Actualiza 'CajaAnulacion'
                        VentaRepository.ActualizarCajaAnulacion(consultaPagoTarjetaVenta);
                    }
                }

                if (request.FlagVenta == "1")
                {
                    if (request.CodiUsuarioBoleto == request.CodiUsuario)
                        VentaRepository.ActualizarCajaAnulacion(int.Parse(string.IsNullOrEmpty(request.ValeRemoto) ? "0" : request.ValeRemoto));

                    // Consulta 'BoletoPorContrato'
                    var consultaBoletoPorContrato = CreditoRepository.ConsultaBoletoPorContrato(request.IdVenta);
                    if (consultaBoletoPorContrato > 0)
                    {
                        // Actualiza 'BoletosPorContrato'
                        CreditoRepository.ActualizarBoletosPorContrato(consultaBoletoPorContrato.ToString());

                        // Actualiza 'BoletosStock'
                        CreditoRepository.ActualizarBoletosStock("0", objVenta.IdPrecio.ToString(), "1"); // Se está suponiendo que 'FlagPrecioNormal' siempre es 'false'.
                    }
                    else
                        return new Response<byte>(false, 0, Message.MsgErrorConsultaBoletoPorContrato, true);
                }

                // Anula 'Venta'
                anularVenta = VentaRepository.AnularVenta(request.IdVenta, request.CodiUsuario);
                if (anularVenta > 0)
                {
                    // Genera 'CorrelativoAuxiliar'
                    var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", request.CodiOficina, request.CodiPuntoVenta, string.Empty);
                    if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                        return new Response<byte>(false, anularVenta, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                    // Graba 'Caja'
                    var objCaja = new CajaEntity
                    {
                        NumeCaja = generarCorrelativoAuxiliar.PadLeft(7, '0'),
                        CodiEmpresa = objVenta.CodiEmpresa,
                        CodiSucursal = short.Parse(request.CodiOficina),
                        FechaCaja = DataUtility.ObtenerFechaDelSistema(),
                        TipoVale = "S",
                        Boleto = objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7"),
                        NomUsuario = request.CodiUsuario.ToString() + " " + request.NomUsuario,
                        CodiBus = string.Empty,
                        CodiChofer = string.Empty,
                        CodiGasto = string.Empty,
                        ConcCaja = string.Empty, /*Va cambiar*/
                        Monto = request.PrecioVenta,
                        CodiUsuario = short.Parse(request.CodiUsuario.ToString()),
                        IndiAnulado = "F",
                        TipoDescuento = string.Empty, /*Va cambiar*/
                        TipoDoc = "16",
                        TipoGasto = "P",
                        Liqui = 0M,
                        Diferencia = 0M,
                        Recibe = string.Empty, /*Va cambiar*/
                        CodiDestino = request.TipoPago,
                        FechaViaje = request.FechaVenta,
                        HoraViaje = string.Empty, /*Va cambiar*/
                        CodiPuntoVenta = short.Parse(request.CodiPuntoVenta),
                        Voucher = string.Empty, /*Va cambiar*/
                        Asiento = string.Empty, /*Va cambiar*/
                        Ruc = request.IngresoManualPasajes ? "MA" : string.Empty,
                        IdVenta = request.IdVenta,
                        Origen = string.Empty, /*Va cambiar*/
                        Modulo = "AP",
                        Tipo = request.Tipo,

                        IdCaja = 0
                    };
                    if (request.FlagVenta == "Y" && (request.CodiUsuarioBoleto != request.CodiUsuario || DataUtility.ObtenerFechaDelSistema() != request.FechaVenta))
                    {
                        objCaja.ConcCaja = "ANUL.VALE x VTA REMOTA" + CondicionAnul(request.ValeRemoto, request.NomOrigenPas, request.NomDestinoPas);
                        objCaja.TipoDescuento = "0";
                        objCaja.Recibe = request.FlagVenta == "Y" || request.FlagVenta == "I" ? "REMOTO" : string.Empty;
                        objCaja.HoraViaje = "VRA";
                        objCaja.Voucher = request.ValeRemoto;
                        objCaja.Asiento = "PA";
                        objCaja.Origen = "YO";
                    }
                    else if (request.FlagVenta == "Y")
                    {
                        objCaja.ConcCaja = "ANUL.VALE x VTA REMOTA" + CondicionAnul(request.ValeRemoto, request.NomOrigenPas, request.NomDestinoPas);
                        objCaja.TipoDescuento = "0";
                        objCaja.Recibe = request.FlagVenta == "Y" || request.FlagVenta == "I" ? "REMOTO" : string.Empty;
                        objCaja.HoraViaje = "VRA";
                        objCaja.Voucher = request.ValeRemoto;
                        objCaja.Asiento = "PA";
                        objCaja.Origen = "AY";
                    }
                    else if (request.FlagVenta == "1" && (request.CodiUsuarioBoleto != request.CodiUsuario || DataUtility.ObtenerFechaDelSistema() != request.FechaVenta))
                    {
                        objCaja.ConcCaja = "AN.BOL " + request.Tipo + objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7");
                        objCaja.TipoDescuento = "VC";
                        objCaja.Recibe = request.CodiUsuarioBoleto != request.CodiUsuario ? "XVC" : string.Empty;
                        objCaja.HoraViaje = "VCA";
                        objCaja.Voucher = request.ValeRemoto;
                        objCaja.Asiento = "PA";
                        objCaja.Origen = "CR";
                    }
                    else if (request.FlagVenta == "1")
                    {
                        objCaja.ConcCaja = "AN.BOL " + request.Tipo + objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7");
                        objCaja.TipoDescuento = "VC";
                        objCaja.Recibe = request.CodiUsuarioBoleto != request.CodiUsuario ? "XVC" : string.Empty;
                        objCaja.HoraViaje = "VCA";
                        objCaja.Voucher = request.ValeRemoto;
                        objCaja.Asiento = "PA";
                        objCaja.Origen = "AC";
                    }
                    else if (request.FlagVenta == "I" && (request.CodiUsuarioBoleto != request.CodiUsuario || DataUtility.ObtenerFechaDelSistema() != request.FechaVenta))
                    {
                        objCaja.ConcCaja = "AN.BOL " + request.Tipo + objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7");
                        objCaja.TipoDescuento = " ";
                        objCaja.Recibe = request.FlagVenta == "Y" || request.FlagVenta == "I" ? "REMOTO" : string.Empty;
                        objCaja.HoraViaje = "VNA";
                        objCaja.Voucher = "PA";
                        objCaja.Asiento = string.Empty;
                        objCaja.Origen = "RC";
                    }
                    else if (request.FlagVenta == "I")
                    {
                        objCaja.ConcCaja = "AN.BOL " + request.Tipo + objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7");
                        objCaja.TipoDescuento = " ";
                        objCaja.Recibe = request.FlagVenta == "Y" || request.FlagVenta == "I" ? "REMOTO" : string.Empty;
                        objCaja.HoraViaje = "VNA";
                        objCaja.Voucher = "PA";
                        objCaja.Asiento = string.Empty;
                        objCaja.Origen = "RR";
                    }
                    else if (request.CodiUsuarioBoleto != request.CodiUsuario || DataUtility.ObtenerFechaDelSistema() != request.FechaVenta)
                    {
                        objCaja.ConcCaja = "AN.BOL " + request.Tipo + objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7");
                        objCaja.TipoDescuento = " ";
                        objCaja.Recibe = request.FlagVenta == "Y" || request.FlagVenta == "I" ? "REMOTO" : string.Empty;
                        objCaja.HoraViaje = "VNA";
                        objCaja.Voucher = "PA";
                        objCaja.Asiento = string.Empty;
                        objCaja.Origen = "BO";
                    }
                    else
                    {
                        objCaja.ConcCaja = "AN.BOL " + request.Tipo + objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7");
                        objCaja.TipoDescuento = " ";
                        objCaja.Recibe = request.FlagVenta == "Y" || request.FlagVenta == "I" ? "REMOTO" : string.Empty;
                        objCaja.HoraViaje = "VNA";
                        objCaja.Voucher = "PA";
                        objCaja.Asiento = string.Empty;
                        objCaja.Origen = "AB";
                    }
                    VentaRepository.GrabarCaja(objCaja);

                    // Elimina 'Poliza'
                    VentaRepository.EliminarPoliza(request.IdVenta);

                    var objPanelCanAnuPorDia = ListarPanelControl.Find(x => x.CodiPanel == "65");
                    if (objPanelCanAnuPorDia != null && objPanelCanAnuPorDia.Valor == "1")
                    {
                        // Consulta 'AnulacionPorDia'
                        var ConsultaAnulacionPorDia = TurnoRepository.ConsultaAnulacionPorDia(int.Parse(request.CodiPuntoVenta), DataUtility.ObtenerFechaDelSistema());
                        if (ConsultaAnulacionPorDia <= 0)
                            // Inserta 'AnulacionPorDia'
                            VentaRepository.InsertarAnulacionPorDia(DataUtility.ObtenerFechaDelSistema(), int.Parse(request.CodiPuntoVenta), 1); // 1 -> Porque se anula uno por uno.
                        else
                            // Actualiza 'AnulacionPorDia'
                            VentaRepository.ActualizarAnulacionPorDia(DataUtility.ObtenerFechaDelSistema(), int.Parse(request.CodiPuntoVenta));
                    }

                    // Graba 'Auditoria'
                    var objAuditoriaEntity = new AuditoriaEntity
                    {
                        CodiUsuario = short.Parse(request.CodiUsuario.ToString()),
                        NomUsuario = request.NomUsuario,
                        Tabla = "VENTA",
                        TipoMovimiento = "ANULACION",
                        Boleto = objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7"),
                        NumeAsiento = request.NumeAsiento.ToString("D2"),
                        NomOficina = request.NomOficina,
                        NomPuntoVenta = request.CodiPuntoVenta.ToString(),
                        Pasajero = request.NomPasajero,
                        FechaViaje = request.FechaViaje,
                        HoraViaje = request.HoraViaje,
                        NomDestino = request.NomDestinoPas,
                        Precio = request.PrecioVenta,
                        Obs1 = "ANULACION DE BOLETO",
                        Obs2 = "TERMINAL" + request.Terminal.ToString("D3"),
                        Obs3 = string.Empty,
                        Obs4 = string.Empty,
                        Obs5 = string.Empty
                    };
                    VentaRepository.GrabarAuditoria(objAuditoriaEntity);

                    // Anulación de su respectivo 'Reintegro'
                    if (!string.IsNullOrEmpty(request.CodiEsca))
                    {
                        // Consulta 'VentaReintegro'
                        var objReintegro = VentaRepository.ConsultaVentaReintegro(request.CodiEsca.Substring(1, 3), request.CodiEsca.Substring(5), objVenta.CodiEmpresa.ToString(), request.CodiEsca.Substring(0, 1));

                        if (objReintegro.IdVenta > 0)
                        {
                            if (objReintegro.TipoPago == "03")
                            {
                                // Consulta 'PagoTarjetaVenta'
                                var consultaPagoTarjetaVenta = VentaRepository.ConsultaPagoTarjetaVenta(objReintegro.IdVenta);

                                // Actualiza 'CajaAnulacion'
                                VentaRepository.ActualizarCajaAnulacion(consultaPagoTarjetaVenta);
                            }

                            // Valida 'AnularDocumentoSUNAT'
                            if (request.CodiEsca.Substring(0, 1) != "M" && request.ElectronicoEmpresa == "1")
                            {
                                // Anula 'DocumentoSUNAT'
                                var objVentaReintegro = new VentaEntity
                                {
                                    CodiEmpresa = objReintegro.CodiEmpresa,
                                    SerieBoleto = short.Parse(request.CodiEsca.Substring(1, 3)),
                                    NumeBoleto = int.Parse(request.CodiEsca.Substring(5)),
                                    Tipo = objReintegro.Tipo,
                                    FechaVenta = objReintegro.FechaVenta,
                                    RucEmpresa = request.RucEmpresa // Recordar: el boleto original y su reintegro siempre pertenecen a una misma Empresa.
                                };

                                var resAnularDocumentoSUNAT = AnularDocumentoSUNAT(objVentaReintegro);
                                if (!resAnularDocumentoSUNAT.Estado)
                                    return new Response<byte>(false, anularVenta, resAnularDocumentoSUNAT.MensajeError, false);
                            }

                            // Anula 'VentaReintegro'
                            var anularVentaReintegro = VentaRepository.AnularVenta(objReintegro.IdVenta, request.CodiUsuario);

                            if (anularVentaReintegro > 0)
                            {
                                // Genera 'CorrelativoAuxiliar'
                                var generarCorrelativoAuxiliarReintegro = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", request.CodiOficina, request.CodiPuntoVenta, string.Empty);
                                if (string.IsNullOrEmpty(generarCorrelativoAuxiliarReintegro))
                                    return new Response<byte>(false, anularVenta, Message.MsgErrorGenerarCorrelativoAuxiliarReintegro, false);

                                // Graba 'CajaReintegro'
                                var objCajaReintegro = new CajaEntity
                                {
                                    NumeCaja = generarCorrelativoAuxiliarReintegro.PadLeft(7, '0'),
                                    CodiEmpresa = objReintegro.CodiEmpresa,
                                    CodiSucursal = short.Parse(request.CodiOficina),
                                    FechaCaja = DataUtility.ObtenerFechaDelSistema(),
                                    TipoVale = "S",
                                    Boleto = request.CodiEsca.Substring(1),
                                    NomUsuario = request.NomUsuario,
                                    CodiBus = string.Empty,
                                    CodiChofer = string.Empty,
                                    CodiGasto = string.Empty,
                                    ConcCaja = "ANUL. BOL. REINTEGRO " + request.CodiEsca,
                                    Monto = objReintegro.PrecioVenta,
                                    CodiUsuario = short.Parse(request.CodiUsuario.ToString()),
                                    IndiAnulado = "F",
                                    TipoDescuento = "RE",
                                    TipoDoc = "16",
                                    TipoGasto = "P",
                                    Liqui = 0M,
                                    Diferencia = 0M,
                                    Recibe = "RE",
                                    CodiDestino = short.Parse(request.CodiDestinoPas).ToString("D3"),
                                    FechaViaje = "01/01/1900",
                                    HoraViaje = "VNA",
                                    CodiPuntoVenta = short.Parse(request.CodiPuntoVenta),
                                    Voucher = "RE",
                                    Asiento = string.Empty,
                                    Ruc = request.IngresoManualPasajes ? "MA" : string.Empty,
                                    IdVenta = objReintegro.IdVenta,
                                    Origen = "AR",
                                    Modulo = "PV",
                                    Tipo = request.CodiEsca.Substring(0, 1),

                                    IdCaja = 0
                                };
                                VentaRepository.GrabarCaja(objCajaReintegro);
                            }
                        }
                    }

                    // Anulación para 'Pase'
                    if (request.FlagVenta == "7")
                    {
                        // Actualiza 'BoletosPorSocio'
                        VentaRepository.ActualizarBoletosPorSocio(objVenta.PerAutoriza, DateTime.ParseExact(request.FechaVenta, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM"), DateTime.ParseExact(request.FechaVenta, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy"));

                        // Consulta 'CajaPase'
                        var consultaCajaPase = VentaRepository.ConsultaCajaPase(objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7"));

                        if (consultaCajaPase.IdCaja > 0)
                        {
                            // Actualiza 'BoletoPorSocioV'
                            VentaRepository.ActualizarBoletosPorSocioV(objVenta.PerAutoriza, DataUtility.ObtenerMesDelSistema(), DataUtility.ObtenerAñoDelSistema());
                        }
                    }

                    return new Response<byte>(true, anularVenta, Message.MsgCorrectoAnularVenta, true);
                }
                else
                    return new Response<byte>(false, anularVenta, Message.MsgErrorAnularVenta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<byte>(false, 0, Message.MsgExcAnularVenta, false);
            }
        }

        public static Response<int> VerificaNC(int IdVenta)
        {
            try
            {
                var verificaNC = VentaRepository.VerificaNC(IdVenta);
                var mensaje = string.Empty;

                if (int.Parse(verificaNC.id) > 0)
                    mensaje = "El boleto está sujeto a Nota de Crédito " + "(N.C.: " + verificaNC.label + ").";
                else
                    mensaje = Message.MsgCorrectoVerificaNC;

                return new Response<int>(true, Convert.ToInt32(verificaNC.id), mensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcVerificaNC, false);
            }
        }

        public static Response<decimal> ConsultaControlTiempo(string tipo)
        {
            try
            {
                var consultaControlTiempo = VentaRepository.ConsultaControlTiempo(tipo);

                return new Response<decimal>(true, consultaControlTiempo, Message.MsgCorrectoConsultaControlTiempo, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<decimal>(false, 0, Message.MsgExcConsultaControlTiempo, false);
            }
        }

        public static Response<string> ConsultaPanelNiveles(int codigo, int Nivel)
        {
            try
            {
                var consultaPanelNiveles = VentaRepository.ConsultaPanelNiveles(codigo, Nivel);

                return new Response<string>(true, consultaPanelNiveles, Message.MsgCorrectoConsultaPanelNiveles, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaPanelNiveles, false);
            }
        }

        public static Response<int> VerificaLiquidacionComiDet(int IdVenta)
        {
            try
            {
                var verificaLiquidacionComiDet = VentaRepository.VerificaLiquidacionComiDet(IdVenta);

                return new Response<int>(true, verificaLiquidacionComiDet, Message.MsgCorrectoVerificaLiquidacionComiDet, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcVerificaLiquidacionComiDet, false);
            }
        }

        public static Response<string> VerificaLiquidacionComi(int CodiProgramacion, int Pvta)
        {
            try
            {
                var verificaLiquidacionComi = VentaRepository.VerificaLiquidacionComi(CodiProgramacion, Pvta);

                return new Response<string>(true, verificaLiquidacionComi, Message.MsgCorrectoVerificaLiquidacionComi, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaLiquidacionComi, false);
            }
        }

        public static Response<bool> ConsultaVentaIdaV(int IdVenta)
        {
            try
            {
                var consultaVentaIdaV = VentaRepository.ConsultaVentaIdaV(IdVenta);

                return new Response<bool>(true, consultaVentaIdaV, Message.MsgCorrectoConsultaVentaIdaV, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaVentaIdaV, false);
            }
        }

        public static Response<bool> GrabarAuditoria(AuditoriaEntity entidad)
        {
            try
            {
                var grabarAuditoria = VentaRepository.GrabarAuditoria(entidad);

                return new Response<bool>(true, grabarAuditoria, Message.MsgCorrectoGrabarAuditoria, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcGrabarAuditoria, false);
            }
        }

        public static Response<bool> ConsultaClaveAnuRei(int CodiUsuario, string Clave)
        {
            try
            {
                var consultaClaveAnuRei = VentaRepository.ConsultaClaveAnuRei(CodiUsuario, Clave);

                return new Response<bool>(true, consultaClaveAnuRei, Message.MsgCorrectoConsultaClaveAnuRei, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaClaveAnuRei, false);
            }
        }

        public static Response<bool> ConsultaClaveControl(short Usuario, string Pwd)
        {
            try
            {
                var consultaClaveControl = VentaRepository.ConsultaClaveControl(Usuario, Pwd);
                var mensaje = (consultaClaveControl) ? Message.MsgCorrectoConsultaClaveControl : Message.MsgValidaClavesInternas;
                return new Response<bool>(true, consultaClaveControl, mensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaClaveControl, false);
            }
        }

        public static Response<bool> InsertarUsuarioPorVenta(string Usuario, string Accion, decimal IdVenta, string Motivo)
        {
            try
            {
                var insertarUsuarioPorVenta = VentaRepository.InsertarUsuarioPorVenta(Usuario, Accion, IdVenta, Motivo);

                return new Response<bool>(true, insertarUsuarioPorVenta, Message.MsgCorrectoInsertarUsuarioPorVenta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcInsertarUsuarioPorVenta, false);
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
                    return new Response<VentaBeneficiarioEntity>(false, buscarVentaxBoleto, Message.MsgValidaBuscarVentaxBoleto, true);
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
                var listaVentasRealizadas = new List<VentaRealizadaEntity>();

                // Verifica 'CodiProgramacion'
                var objProgramacion = new ProgramacionEntity()
                {
                    NroViaje = filtro.NroViaje,
                    FechaProgramacion = filtro.FechaProgramacion,
                    CodiProgramacion = filtro.CodiProgramacion,
                    CodiEmpresa = filtro.CodiEmpresa,
                    CodiSucursal = filtro.CodiSucursal,
                    CodiRuta = filtro.CodiRuta,
                    CodiBus = filtro.CodiBus,
                    HoraProgramacion = filtro.HoraProgramacion,
                    CodiServicio = byte.Parse(filtro.CodiServicio.ToString()),

                    CodiUsuario = filtro.CodiUsuario.ToString(),
                    NomUsuario = filtro.NomUsuario,
                    CodiPuntoVenta = filtro.CodiPuntoVenta.ToString(),
                    Terminal = filtro.CodiTerminal,
                    CodiOrigen = filtro.CodiOrigen.ToString(),
                    CodiDestino = filtro.CodiDestino.ToString(),
                    NomOrigen = filtro.NomOrigen
                };
                var verificaCodiProgramacion = VerificaCodiProgramacion(objProgramacion);
                if (verificaCodiProgramacion == 0)
                    return new Response<VentaResponse>(false, valor, Message.MsgErrorVerificaCodiProgramacion, false);
                else
                    filtro.CodiProgramacion = verificaCodiProgramacion;

                // Seteo 'valor.CodiProgramacion'
                valor.CodiProgramacion = filtro.CodiProgramacion;

                var objPostergacion = new FechaAbiertaRequest()
                {
                    CodiEsca = filtro.CodiEsca,
                    CodiProgramacion = filtro.CodiProgramacion, // DEL PLANO FINAL
                    CodiOrigen = filtro.CodiOrigenBoleto, // DEL BOLETO
                    IdVenta = filtro.IdVenta,
                    NumeAsiento = filtro.NumeAsiento.ToString("D2"), // DEL PLANO FINAL
                    CodiRuta = filtro.CodiRutaBoleto.ToString(), // DEL BOLETO
                    CodiServicio = filtro.CodiServicio.ToString(), // DEL PLANO FINAL
                    Tipo = "" // No es utilizado en: Usp_Tb_Venta_Update_Postergacion_Ele
                };
                FechaAbiertaRepository.VentaUpdatePostergacionEle(objPostergacion);
                FechaAbiertaRepository.VentaDerivadaUpdateViaje(filtro.IdVenta, filtro.FechaViaje, filtro.HoraViaje, filtro.CodiServicio.ToString());
                FechaAbiertaRepository.VentaUpdateCnt(filtro.CodiProgramacionBoleto, filtro.CodiProgramacion, int.Parse(filtro.CodiOrigenBoleto), int.Parse(filtro.CodiOrigenBoleto));
                if(filtro.CodiProgramacion > 0)
                    FechaAbiertaRepository.VentaUpdateCnt(filtro.CodiProgramacion, filtro.CodiProgramacion, int.Parse(filtro.CodiOrigenBoleto), int.Parse(filtro.CodiOrigenBoleto));
                FechaAbiertaRepository.VentaUpdateImpManifiesto(filtro.IdVenta);
                VentaRepository.InsertarBoletosPostergados(filtro.CodiEmpresaUsuario, filtro.BoletoCompleto.Substring(1), "1", filtro.CodiUsuario, filtro.CodiSucursalUsuario, filtro.CodiPuntoVenta.ToString(), DataUtility.ObtenerFechaDelSistema(), filtro.BoletoCompleto.Substring(0, 1));

                // Graba 'Auditoria'
                var objAuditoria = new AuditoriaEntity
                {
                    CodiUsuario = filtro.CodiUsuario,
                    NomUsuario = filtro.NomUsuario,
                    Tabla = "VENTA",
                    TipoMovimiento = "POSTERGACION DE PASAJES",
                    Boleto = filtro.BoletoCompleto.Substring(1),
                    NumeAsiento = filtro.NumeAsiento.ToString("D2"),
                    NomOficina = filtro.NomSucursalUsuario,
                    NomPuntoVenta = filtro.CodiPuntoVenta.ToString(),
                    Pasajero = filtro.NomPasajero,
                    FechaViaje = filtro.FechaViajeBoleto,
                    HoraViaje = filtro.HoraViajeBoleto,
                    NomDestino = filtro.NomDestinoBoleto, // DEL BOLETO
                    Precio = filtro.PrecioVenta, // DEL BOLETO
                    Obs1 = "TERMINAL : " + filtro.CodiTerminal,
                    Obs2 = string.Empty,
                    Obs3 = string.Empty,
                    Obs4 = filtro.CodiProgramacion <= 0 ? "POSTERGADO A FECHA ABIERTA" : string.Empty,
                    Obs5 = string.Empty
                };
                VentaRepository.GrabarAuditoria(objAuditoria);

                // Añado 'ventaRealizada'
                var ventaRealizada = new VentaRealizadaEntity
                {
                    NumeAsiento = filtro.NumeAsiento.ToString("D2"),
                    BoletoCompleto = string.Empty
                };
                listaVentasRealizadas.Add(ventaRealizada);

                // Seteo 'valor.ListaVentasRealizadas'
                valor.ListaVentasRealizadas = listaVentasRealizadas;

                return new Response<VentaResponse>(true, valor, Message.MsgCorrectoPostergarVenta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcPostergarVenta, false);
            }
        }

        public static Response<string> ConsultaPos(string CodTab, string CodEmp)
        {
            try
            {
                var consultaPos = VentaRepository.ConsultaPos(CodTab, CodEmp);

                return new Response<string>(true, consultaPos, Message.MsgCorrectoConsultaPos, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaPos, false);
            }
        }

        public static Response<int> ConsultaSumaBoletosPostergados(string Tipo, string Numero, string Emp)
        {
            try
            {
                var consultaSumaBoletosPostergados = VentaRepository.ConsultaSumaBoletosPostergados(Tipo, Numero, Emp);

                return new Response<int>(true, consultaSumaBoletosPostergados, Message.MsgCorrectoConsultaSumaBoletosPostergados, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcConsultaSumaBoletosPostergados, false);
            }
        }

        #endregion

        #region MODIFICAR VENTA FECHA ABIERTA

        public static Response<bool> ModificarVentaAFechaAbierta(VentaToFechaAbiertaRequest request)
        {
            try
            {
                var objFechaAbierta = new FechaAbiertaRequest()
                {
                    CodiEsca = request.CodiEsca,
                    CodiProgramacion = 0,
                    CodiOrigen = request.CodiOrigen,
                    IdVenta = request.IdVenta,
                    NumeAsiento = request.NumeAsiento.ToString("D2"),
                    CodiRuta = request.CodiRuta.ToString(),
                    CodiServicio = request.CodiServicio.ToString(),
                    Tipo = "" // No es utilizado en: Usp_Tb_Venta_Update_Postergacion_Ele
                };
                var modificarVentaAFechaAbierta = FechaAbiertaRepository.VentaUpdatePostergacionEle(objFechaAbierta);

                FechaAbiertaRepository.VentaDerivadaUpdateViaje(request.IdVenta, request.FechaViaje, request.HoraViaje, request.CodiServicio.ToString());
                FechaAbiertaRepository.VentaUpdateCnt(request.CodiProgramacion, 0, int.Parse(request.CodiOrigen), 0);
                FechaAbiertaRepository.VentaUpdateImpManifiesto(request.IdVenta);

                // Graba 'AuditoriaFechaAbierta'
                var objAuditoriaFechaAbierta = new AuditoriaEntity
                {
                    CodiUsuario = request.CodiUsuario,
                    NomUsuario = request.NomUsuario,
                    Tabla = "VENTA",
                    TipoMovimiento = "POSTERGACION DE PASAJES",
                    Boleto = request.BoletoCompleto, // Verificar
                    NumeAsiento = request.NumeAsiento.ToString("D2"),
                    NomOficina = request.NomOficina,
                    NomPuntoVenta = request.CodiPuntoVenta.ToString(),
                    Pasajero = request.Pasajero,
                    FechaViaje = request.FechaViaje,
                    HoraViaje = request.HoraViaje,
                    NomDestino = request.NomDestino,
                    Precio = request.PrecioVenta,
                    Obs1 = string.Empty,
                    Obs2 = string.Empty,
                    Obs3 = string.Empty,
                    Obs4 = "POSTERGADO A FECHA ABIERTA",
                    Obs5 = "TERMINAL : " + request.CodiTerminal
                };
                VentaRepository.GrabarAuditoria(objAuditoriaFechaAbierta);

                return new Response<bool>(true, modificarVentaAFechaAbierta, Message.MsgCorrectoModificarVentaAFechaAbierta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcModificarVentaAFechaAbierta, false);
            }
        }

        #endregion

        #region MANIFIESTO

        public static Response<string> VerificaManifiestoPorPVenta(int CodiProgramacion, short Pvta)
        {
            try
            {
                var verificaManifiestoPorPVenta = VentaRepository.VerificaManifiestoPorPVenta(CodiProgramacion, Pvta);

                return new Response<string>(true, verificaManifiestoPorPVenta, Message.MsgCorrectoVerificaManifiestoPorPVenta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaManifiestoPorPVenta, false);
            }
        }

        public static Response<string> ConsultaConfigManifiestoPorHora(short CodiEmpresa, short CodiSucursal, short CodiPuntoVenta)
        {
            try
            {
                var consultaConfigManifiestoPorHora = VentaRepository.ConsultaConfigManifiestoPorHora(CodiEmpresa, CodiSucursal, CodiPuntoVenta);

                if (consultaConfigManifiestoPorHora == "0")
                {
                    consultaConfigManifiestoPorHora = VentaRepository.ConsultaConfigManifiestoPorHora(CodiEmpresa, CodiSucursal, 0);

                    if (consultaConfigManifiestoPorHora == "0")
                        consultaConfigManifiestoPorHora = VentaRepository.ConsultaConfigManifiestoPorHora(CodiEmpresa, 0, 0);
                }

                return new Response<string>(true, consultaConfigManifiestoPorHora, Message.MsgCorrectoConsultaConfigManifiestoPorHora, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaConfigManifiestoPorHora, false);
            }
        }

        #endregion

        #region IMPRESIÓN

        public static Response<List<ImpresionEntity>> ConvertirVentaToBase64(List<VentaRealizadaEntity> Listado, string TipoImpresion)
        {
            try
            {
                var listaImpresiones = new List<ImpresionEntity>();
                var ListarPanelControl = CreditoRepository.ListarPanelControl();
                var consultaNroPoliza = new PolizaEntity()
                {
                    NroPoliza = string.Empty,
                    FechaReg = "01/01/1900",
                    FechaVen = "01/01/1900"
                };

                var objPanelCopia1 = ListarPanelControl.Find(x => x.CodiPanel == "197");
                var objPanelCopia2 = ListarPanelControl.Find(x => x.CodiPanel == "198");

                foreach (var entidad in Listado)
                {
                    var obtenerParametroRempresa = new Rempresa();

                    // Solo para 'Terminales electrónicos'
                    if (entidad.BoletoTipo != "M" && entidad.EmpElectronico == "1")
                    {
                        obtenerParametroRempresa = ObtenerParametroRempresa(entidad.EmpRuc);
                        if (obtenerParametroRempresa == null)
                            return new Response<List<ImpresionEntity>>(false, listaImpresiones, Message.MsgErrorWebServiceFacturacionElectronica, true);
                    }

                    // Solo para 'Reimpresion'
                    if (TipoImpresion == TipoReimprimir)
                    {
                        var auxBoletoCompleto = entidad.BoletoTipo + entidad.BoletoSerie + "-" + entidad.BoletoNum;
                        var obtenerPrecioReimpresion = new decimal();

                        var obtenerCodigoX = string.Empty;
                        if (entidad.BoletoTipo != "M")
                        {
                            if (entidad.ValidateCaja)
                            {
                                obtenerPrecioReimpresion = VentaRepository.ObtenerPrecioReimpresion();

                                if (obtenerPrecioReimpresion <= 0)
                                    return new Response<List<ImpresionEntity>>(false, listaImpresiones, Message.MsgErrorObtenerPrecioReimpresion, true);

                                //  Genera 'CorrelativoAuxiliar'
                                var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", entidad.UsuarioCodOficina.ToString(), entidad.UsuarioCodPVenta.ToString(), string.Empty);
                                if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                                    return new Response<List<ImpresionEntity>>(false, listaImpresiones, Message.MsgErrorGenerarCorrelativoAuxiliar, true);

                                // Graba 'Caja'
                                var objCajaEntity = new CajaEntity
                                {
                                    NumeCaja = generarCorrelativoAuxiliar.PadLeft(7, '0'),
                                    CodiEmpresa = entidad.EmpCodigo,
                                    CodiSucursal = entidad.UsuarioCodOficina,
                                    FechaCaja = DataUtility.ObtenerFechaDelSistema(),
                                    TipoVale = "I",
                                    Boleto = auxBoletoCompleto,
                                    NomUsuario = "REIMPRESION",
                                    CodiBus = string.Empty,
                                    CodiChofer = string.Empty,
                                    CodiGasto = "L",
                                    ConcCaja = "REIMPRESION POR EL BOLETO : " + auxBoletoCompleto,
                                    Monto = obtenerPrecioReimpresion,
                                    CodiUsuario = entidad.UsuarioCodigo,
                                    IndiAnulado = "F",
                                    TipoDescuento = string.Empty,
                                    TipoDoc = entidad.PasRuc.Length == 11 ? "17" : "16",
                                    TipoGasto = "11",
                                    Liqui = 0M,
                                    Diferencia = 0M,
                                    Recibe = string.Empty,
                                    CodiDestino = string.Empty,
                                    FechaViaje = entidad.FechaViaje,
                                    HoraViaje = entidad.HoraViaje, // TRAER
                                    CodiPuntoVenta = entidad.UsuarioCodPVenta,
                                    Voucher = "PA",
                                    Asiento = entidad.NumeAsiento,
                                    Ruc = entidad.PasRuc,
                                    IdVenta = entidad.IdVenta,
                                    Origen = "CA",
                                    Modulo = "PA",
                                    Tipo = entidad.BoletoTipo,

                                    IdCaja = 0
                                };
                                VentaRepository.GrabarCaja(objCajaEntity);
                            }

                            if (entidad.EmpElectronico == "1")
                            {
                                obtenerCodigoX = ObtenerCodigoX(entidad.EmpRuc, entidad.BoletoTipo, short.Parse(entidad.BoletoSerie), int.Parse(entidad.BoletoNum));
                                if (obtenerCodigoX == null)
                                    return new Response<List<ImpresionEntity>>(false, listaImpresiones, Message.MsgErrorWebServiceFacturacionElectronica, true);
                            }
                        }

                        var buscarAgenciaEmpresa = VentaRepository.BuscarAgenciaEmpresa(entidad.EmpCodigo, entidad.PVentaCodigo);

                        var objPanelPoliza = ListarPanelControl.Find(x => x.CodiPanel == "224");
                        if (objPanelPoliza != null && objPanelPoliza.Valor == "1")
                        {
                            consultaNroPoliza = VentaRepository.ConsultaNroPoliza(entidad.EmpCodigo, entidad.BusCodigo, entidad.FechaViaje);
                            if (string.IsNullOrEmpty(consultaNroPoliza.NroPoliza))
                                return new Response<List<ImpresionEntity>>(false, listaImpresiones, Message.MsgErrorConsultaNroPoliza, true);
                        }

                        entidad.NumeAsiento = entidad.NumeAsiento.PadLeft(2, '0');
                        entidad.BoletoSerie = entidad.BoletoSerie.PadLeft(3, '0');
                        entidad.BoletoNum = entidad.BoletoNum.PadLeft(8, '0');
                        entidad.EmisionFecha = entidad.EmisionFecha;
                        entidad.EmisionHora = DateTime.ParseExact(entidad.EmisionHora, "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mmtt", CultureInfo.InvariantCulture);
                        entidad.DocTipo = TipoDocumentoHomologado(entidad.DocTipo.ToString("D2"));
                        entidad.PrecioDes = DataUtility.MontoSolesALetras(DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioCan));
                        entidad.CodigoX_FE = obtenerCodigoX;
                        entidad.PolizaNum = consultaNroPoliza.NroPoliza;
                        entidad.PolizaFechaReg = consultaNroPoliza.FechaReg;
                        entidad.PolizaFechaVen = consultaNroPoliza.FechaVen;
                        entidad.EmpDirAgencia = buscarAgenciaEmpresa.Direccion;
                        entidad.EmpTelefono1 = buscarAgenciaEmpresa.Telefono1;
                        entidad.EmpTelefono2 = buscarAgenciaEmpresa.Telefono2;

                        var objAuditoria = new AuditoriaEntity
                        {
                            CodiUsuario = entidad.UsuarioCodigo,
                            NomUsuario = entidad.UsuarioNombre,
                            Tabla = "VENTA",
                            TipoMovimiento = "REIMPRESION",
                            Boleto = auxBoletoCompleto.Substring(1),
                            NumeAsiento = entidad.NumeAsiento,
                            NomOficina = entidad.UsuarioNomOficina,
                            NomPuntoVenta = entidad.UsuarioCodPVenta.ToString(),
                            Pasajero = entidad.PasNombreCom,
                            FechaViaje = entidad.FechaViaje,
                            HoraViaje = entidad.HoraViaje,
                            NomDestino = entidad.NomDesPas,
                            Precio = entidad.PrecioCan,
                            Obs1 = string.Empty,
                            Obs2 = string.Empty,
                            Obs3 = "TERMINAL : " + entidad.UsuarioCodTerminal,
                            Obs4 = "REIMPRESION",
                            Obs5 = string.Empty
                        };
                        VentaRepository.GrabarAuditoria(objAuditoria);
                    }

                    entidad.NomTipVenta = "EFECTIVO";
                    entidad.LinkPag_FE = obtenerParametroRempresa.PaginaWebEmisor ?? string.Empty;
                    entidad.ResAut_FE = obtenerParametroRempresa.ResolucionAutorizado ?? string.Empty;

                    var original = CuadreImpresora.Cuadre.WriteText(entidad, TipoImpresion);

                    var copia1 = string.Empty;
                    if (objPanelCopia1 != null && objPanelCopia1.Valor == "1")
                        copia1 = CuadreImpresora.Cuadre.WriteTextCopy(entidad, TipoImpresion);

                    var copia2 = string.Empty;
                    if (objPanelCopia2 != null && objPanelCopia2.Valor == "1")
                        copia2 = CuadreImpresora.Cuadre.WriteTextCopy(entidad, TipoImpresion);

                    var documentos = new ImpresionEntity()
                    {
                        Original = original,
                        Copia1 = copia1,
                        Copia2 = copia2
                    };

                    listaImpresiones.Add(documentos);
                }

                return new Response<List<ImpresionEntity>>(true, listaImpresiones, Message.MsgCorrectoConvertirVentaToBase64, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ImpresionEntity>>(false, null, Message.MsgExcConvertirVentaToBase64, false);
            }
        }

        #endregion

        #region FACTURACIÓN ELETRÓNICA

        public static ResponseW ValidarDocumentoSUNAT(VentaEntity entidad, ref SetInvoiceRequestBody bodyDocumentoSUNAT)
        {
            try
            {
                // Valida "ElectronicoEmpresa"
                if (entidad.TipoTerminalElectronico != "E" || entidad.ElectronicoEmpresa != "1")
                {
                    var tmpResponseW = new ResponseW { Estado = true };
                    return tmpResponseW;
                }
                // ---------------------------

                var serviceFE = new Ws_SeeFacteSoapClient();
                var entidadFE = new SetInvoiceRequestBody();

                var seguridadFE = new Security
                {
                    ID = entidad.RucEmpresa,
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

        public static ResponseW RegistrarDocumentoSUNAT(SetInvoiceRequestBody bodyDocumentoSunat)
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

                var seguridadFE = new Security
                {
                    ID = entidad.RucEmpresa,
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
                                "|" + DataUtility.ObtenerFechaDelSistema();

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
                    var auxIdTipoDocIdentidad = TipoDocumentoHomologado(entidad.TipoDocumento);

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
                sb = sb.Replace("[FecEmision]", DataUtility.ObtenerFechaDelSistema());
                sb = sb.Replace("[HoraEmision]", DataUtility.Obtener24HorasConSegDelSistema());
                sb = sb.Replace("[TMoneda]", "PEN");
                sb = sb.Replace("[ImporteTotalVenta]", DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta));
                sb = sb.Replace("[EnviarEmail]", string.Empty);
                sb = sb.Replace("[CorreoCliente]", string.Empty);
                sb = sb.Replace("[TipoCambio]", string.Empty);
                sb = sb.Replace("[TValorOperacionGravada]", "0.00");
                sb = sb.Replace("[TValorOperacionInafecta]", DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta));
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
                sb = sb.Replace("[MontoLetras]", DataUtility.MontoSolesALetras(DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta)));
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
                sb = sb.Replace("[ValorUnitario]", DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta));
                sb = sb.Replace("[PrecioVenta]", DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta));
                sb = sb.Replace("[CodAFIgvxItem]", "10");
                sb = sb.Replace("[AFIgvxItem]", "0.00");
                sb = sb.Replace("[CodAFIscxItem]", "02");
                sb = sb.Replace("[AFIscxItem]", "0.00");
                sb = sb.Replace("[AFOtroxItem]", "0.00");
                sb = sb.Replace("[ValorVenta]", DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta));
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

                sb.Append("1|[CondicionPago]|2|[SucDir]|4|[NombreUsuario]|5|[IdVenta]|16|[NombreSucursal]|21|[PolizaNro]");
                sb = sb.Replace("[CondicionPago]", entidad.FlagVenta == "1" ? "02" : "01");
                sb = sb.Replace("[SucDir]", entidad.EmpDirAgencia);
                sb = sb.Replace("[NombreUsuario]", entidad.CodiUsuario.ToString());
                sb = sb.Replace("[IdVenta]", entidad.IdVenta.ToString());
                sb = sb.Replace("[NombreSucursal]", entidad.CodiOficina.ToString());
                sb = sb.Replace("[PolizaNro]", entidad.PolizaNum);

                array.Add(sb.ToString());

                return array;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static Rempresa ObtenerParametroRempresa(string Ruc)
        {
            try
            {
                var Rempresa = new Rempresa();

                var serviceFE = new Ws_SeeFacteSoapClient();
                var seguridadFE = new Security
                {
                    ID = Ruc,
                    User = UserWebSUNAT
                };
                
                 Rempresa = serviceFE.GetParametro(seguridadFE).Rempresa; // ResolucionAutorizado // Paginaweb

                return Rempresa;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static string ObtenerCodigoX(string Ruc, string DocTipo, short BoletoSerie, int BoletoNum)
        {
            try
            {
                var CodigoX = string.Empty;
                var auxTDocumento = string.Empty;

                var serviceFE = new Ws_SeeFacteSoapClient();
                var seguridadFE = new Security
                {
                    ID = Ruc,
                    User = UserWebSUNAT
                };

                // Seteo 'auxTDocumento'
                switch (DocTipo)
                {
                    case "F":
                        auxTDocumento = "01";
                        break;
                    case "B":
                        auxTDocumento = "03";
                        break;
                };

                CodigoX = serviceFE.GetValidarDocument(seguridadFE, auxTDocumento, DocTipo + BoletoSerie.ToString("D3"), BoletoNum.ToString("D8")).SignatureValue ?? string.Empty;

                return CodigoX;
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

        public static byte TipoDocumentoHomologado(string TipoDocumento)
        {
            var valor = new byte();

            if (TipoDocumento == "01")
                valor = 1;
            else if (TipoDocumento == "03")
                valor = 4;
            else if (TipoDocumento == "07")
                valor = 7;
            else
                valor = 0;

            return valor;
        }

        public static string CondicionAnul(string ValeRemoto, string NomOrigenPas, string NomDestinoPas)
        {
            var value = string.Empty;

            if (!string.IsNullOrEmpty(ValeRemoto))
                value = "Val. : " + ValeRemoto + " : " + NomOrigenPas.Substring(0, 3) + "-" + NomDestinoPas.Substring(0, 3);
            else
                value = NomOrigenPas.Substring(0, 3) + "-" + NomDestinoPas.Substring(0, 3);

            return value;
        }

        public static int VerificaCodiProgramacion(ProgramacionEntity entidad)
        {
            var value = new int();

            // Busca 'ProgramacionViaje'
            var buscarProgramacionViaje = ItinerarioRepository.BuscarProgramacionViaje(entidad.NroViaje, entidad.FechaProgramacion);
            if (buscarProgramacionViaje == 0)
            {
                // Genera 'CorrelativoAuxiliar'
                var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("TB_PROGRAMACION", "999", string.Empty, string.Empty);
                if (!string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                {
                    value = int.Parse(generarCorrelativoAuxiliar) + 1;

                    var objProgramacion = new ProgramacionEntity
                    {
                        CodiProgramacion = value,
                        CodiEmpresa = entidad.CodiEmpresa,
                        CodiSucursal = entidad.CodiSucursal,
                        CodiRuta = entidad.CodiRuta,
                        CodiBus = entidad.CodiBus,
                        FechaProgramacion = entidad.FechaProgramacion,
                        HoraProgramacion = entidad.HoraProgramacion,
                        CodiServicio = entidad.CodiServicio
                    };

                    // Graba 'Programacion'
                    VentaRepository.GrabarProgramacion(objProgramacion);

                    // Graba 'ViajeProgramacion'
                    VentaRepository.GrabarViajeProgramacion(entidad.NroViaje, value, entidad.FechaProgramacion, entidad.CodiBus);

                    // Graba 'AuditoriaProg'
                    var objAuditoriaProg = new AuditoriaEntity
                    {
                        CodiUsuario = short.Parse(entidad.CodiUsuario),
                        NomUsuario = entidad.NomUsuario,
                        Tabla = "PROGRAMACION",
                        TipoMovimiento = "ADICION",
                        Boleto = value.ToString(),
                        NumeAsiento = "0",
                        NomOficina = entidad.NomOrigen,
                        NomPuntoVenta = entidad.CodiPuntoVenta.PadLeft(3, '0'),
                        Pasajero = string.Empty,
                        FechaViaje = entidad.FechaProgramacion,
                        HoraViaje = entidad.HoraProgramacion,
                        NomDestino = entidad.CodiDestino.PadLeft(3, '0'),
                        Precio = 0,
                        Obs1 = "CREACION DE PROGRAMACION",
                        Obs2 = "TERMINAL : " + entidad.Terminal.PadLeft(3, '0'),
                        Obs3 = string.Join("-", new string[] { entidad.CodiOrigen.PadLeft(3, '0'), entidad.CodiDestino.PadLeft(3, '0'), entidad.CodiServicio.ToString().PadLeft(2, '0') }),
                        Obs4 = "FECHA PROG " + entidad.FechaProgramacion + " " + entidad.HoraProgramacion,
                        Obs5 = " NRO VIAJE " + entidad.NroViaje
                    };
                    ManifiestoRepository.GrabarAuditoriaProg(objAuditoriaProg);
                }
            }
            else
                value = buscarProgramacionViaje;

            return value;
        }

        #endregion

        #region BUSCAR

        public static Response<BuscaEntity> BuscaBoletoF9(int Serie, int Numero, string Tipo, int CodEmpresa)
        {
            try
            {
                var valor = VentaRepository.ConsultaF9Elec(Serie, Numero, Tipo, CodEmpresa);

                // Busca 'Empresa'
                if (!string.IsNullOrEmpty(valor.RucCliente))
                {
                    var buscarEmpresa = ClientePasajeRepository.BuscarEmpresa(valor.RucCliente);
                    valor.RazonSocial = buscarEmpresa.RazonSocial;
                    valor.DireccionFiscal = buscarEmpresa.Direccion;
                }
                else
                {
                    valor.RazonSocial = string.Empty;
                    valor.DireccionFiscal = string.Empty;
                }

                if (valor.IdVenta == 0)
                {
                    return new Response<BuscaEntity>(false, null, Message.MsgErrorBuscaBoletoF9, true);
                }
                return new Response<BuscaEntity>(true, valor, Message.MsgCorrectoBuscaBoletoF9, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<BuscaEntity>(false, null, Message.MsgExcBuscaBoletoF9, false);
            }
        }

        public static Response<bool> ActualizaBoletoF9(BoletoF9Request request)
        {
            try
            {
                var res = VentaRepository.ActualizaF9Elec(request.IdVenta, request.Dni, request.Nombre, request.Ruc, request.Edad, request.Telefono, request.RecoVenta, request.TipoDoc, request.Nacionalidad);

                if (res)
                {
                    var objAuditoria = new AuditoriaEntity
                    {
                        CodiUsuario = Convert.ToInt16(request.CodiUsuario),
                        NomUsuario = request.NombUsuario,
                        Tabla = "VENTA",
                        TipoMovimiento = "MODIFICACION F9",
                        Boleto = request.Boleto,
                        NumeAsiento = request.NumAsiento.PadLeft(2, '0'),
                        NomOficina = request.NomSucursal,
                        NomPuntoVenta = request.CodiPuntoVenta.PadLeft(3, '0'),
                        Pasajero = request.Nombre,
                        FechaViaje = request.FechaViaje,
                        HoraViaje = request.HoraViaje,
                        NomDestino = request.NombDestino,
                        Precio = request.Precio,
                        Obs1 = "",
                        Obs2 = "",
                        Obs3 = "TERMINAL: " + request.Terminal,
                        Obs4 = "MODIFICACION DE BOLETO F9",
                        Obs5 = ""
                    };
                    //Graba Auditoria
                    VentaRepository.GrabarAuditoria(objAuditoria);
                }

                return new Response<bool>(true, res, Message.MsgCorrectoActualizaBoletoF9, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExActualizaBoletoF9, false);
            }
        }
        #endregion
    }
}
