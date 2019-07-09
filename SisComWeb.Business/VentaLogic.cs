﻿using SisComWeb.Business.ServiceFE;
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

        private static readonly string TipoImprimir = ConfigurationManager.AppSettings["tipoImprimir"].ToString();
        private static readonly string TipoReimprimir = ConfigurationManager.AppSettings["tipoReimprimir"].ToString();

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
                var listaVentasRealizadas = new List<VentaRealizadaEntity>();
                var ListarPanelControl = CreditoRepository.ListarPanelControl();
                var consultaNroPoliza = new PolizaEntity()
                {
                    NroPoliza = "",
                    FechaReg = "01/01/1900",
                    FechaVen = "01/01/1900"
                };
                var buscarAgenciaEmpresa = new AgenciaEntity()
                {
                    Direccion = string.Empty,
                    Telefono1 = string.Empty,
                    Telefono2 = string.Empty
                };

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

                    // Valida 'SaldoPaseCortesia', consulta 'Contrato' y otros
                    switch (FlagVenta)
                    {
                        case "7": // PASE DE CORTESÍA
                            {
                                // Valida 'SaldoPaseCortesia'
                                var validarSaldoPaseCortesia = PaseRepository.ValidarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Anno);
                                if (validarSaldoPaseCortesia <= 0)
                                    return new Response<VentaResponse>(true, valor, Message.MsgValidaSaldoPaseCortesia, false);
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
                                            return new Response<VentaResponse>(true, valor, Message.MsgValidaConsultarContrato, false);
                                    }
                                }
                                else
                                    return new Response<VentaResponse>(true, valor, Message.MsgValidaNullConsultarContrato, false);

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
                                            return new Response<VentaResponse>(true, valor, Message.MsgErrorVerificarPrecioNormal, false);
                                    }
                                    else
                                    {
                                        var verificarContratoPasajes = CreditoRepository.VerificarContratoPasajes(entidad.RucCliente, entidad.FechaViaje, entidad.FechaViaje, entidad.CodiOficina.ToString(), entidad.CodiRuta.ToString(), entidad.CodiServicio.ToString(), entidad.IdRuc);

                                        if (verificarContratoPasajes.SaldoBoletos <= 0)
                                            return new Response<VentaResponse>(true, valor, Message.MsgErrorVerificarPrecioNormal, false);
                                    }
                                }
                            };
                            break;
                        default:
                            {
                                var objPanelPoliza = ListarPanelControl.Find(x => x.CodiPanel == "224");
                                if (objPanelPoliza != null && objPanelPoliza.Valor == "1")
                                    consultaNroPoliza = VentaRepository.ConsultaNroPoliza(entidad.CodiEmpresa, entidad.CodiBus, entidad.FechaViaje);

                                entidad.PolizaNum = consultaNroPoliza.NroPoliza;
                                entidad.PolizaFechaReg = consultaNroPoliza.FechaReg;
                                entidad.PolizaFechaVen = consultaNroPoliza.FechaVen;
                            }
                            break;
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
                            case "1": // CRÉDITO
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

                    // Busca 'AgenciaEmpresa' (E -> GenerarAdicionales, M -> También se va a necesitar.)
                    buscarAgenciaEmpresa = VentaRepository.BuscarAgenciaEmpresa(entidad.CodiEmpresa, entidad.CodiPuntoVenta);
                    entidad.EmpDirAgencia = buscarAgenciaEmpresa.Direccion;
                    entidad.EmpTelefono1 = buscarAgenciaEmpresa.Telefono1;
                    entidad.EmpTelefono2 = buscarAgenciaEmpresa.Telefono2;

                    // Graba 'Venta', 'FacturaciónElectrónica' y otros
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

                                        if (entidad.IdVenta > 0)
                                        {
                                            //Registra 'DocumentoSUNAT'
                                            var resRegistrarDocumentoSUNAT = RegistrarDocumentoSUNAT(bodyDocumentoSUNAT);

                                            if (resRegistrarDocumentoSUNAT.Estado)
                                                entidad.SignatureValue = resRegistrarDocumentoSUNAT.SignatureValue ?? string.Empty;
                                            else
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
                            Obs4 = "POSTEGADO A FECHA ABIERTA",
                            Obs5 = "TERMINAL : " + entidad.CodiTerminal
                        };
                        VentaRepository.GrabarAuditoria(objAuditoriaFechaAbierta);
                    }

                    // Seteo 'auxBoletoCompleto'
                    auxBoletoCompleto = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "7");

                    // Modifica 'SaldoPaseCortesia', actualiza 'BoletosStock' y otros
                    switch (FlagVenta)
                    {
                        case "7": // PASE DE CORTESÍA
                            {
                                // Modifica 'SaldoPaseCortesia'
                                var modificarSaldoPaseCortesia = PaseRepository.ModificarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Anno);
                                if (!modificarSaldoPaseCortesia)
                                    return new Response<VentaResponse>(false, valor, Message.MsgErrorModificarSaldoPaseCortesia, false);

                                // Graba 'PaseSocio'
                                var grabarPaseSocio = PaseRepository.GrabarPaseSocio(entidad.IdVenta, entidad.CodiGerente, entidad.CodiSocio, entidad.Concepto);
                                if (!grabarPaseSocio)
                                    return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarPaseSocio, false);
                            };
                            break;
                        case "1": // CRÉDITO
                            {
                                // Actualiza 'BoletosStock'
                                var actualizarBoletosStock = false;
                                if (!entidad.FlagPrecioNormal)
                                    actualizarBoletosStock = CreditoRepository.ActualizarBoletosStock("1", entidad.IdPrecio.ToString(), "1");
                                else
                                    actualizarBoletosStock = CreditoRepository.ActualizarBoletosStock("1", entidad.IdContrato.ToString(), "0");

                                if (!actualizarBoletosStock)
                                    return new Response<VentaResponse>(false, valor, Message.MsgErrorActualizarBoletosStock, false);
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
                            Origen= entidad.CodiSucursal,
                            Destino = entidad.CodiDestino
                        };
                        var insertarDescuentoBoleto = VentaRepository.InsertarDescuentoBoleto(descuentoBoleto);
                        if (!insertarDescuentoBoleto)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorInsertarDescuentoBoleto, false);
                    }

                    // Inserta 'DescuentoVenta'
                    if (entidad.ValidadorDescuentoControl)
                    {
                        var insertarDescuentoVenta = VentaRepository.InsertarDescuentoVenta(entidad.IdVenta, entidad.DescuentoTipoDC, entidad.ImporteDescuentoDC, entidad.ImporteDescontadoDC, entidad.AutorizadoDC);
                        if (!insertarDescuentoVenta)
                            return new Response<VentaResponse>(false, valor, Message.MsgErrorInsertarDescuentoVenta, false);
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
                        case "02": // Múltiple pago
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
                                    NumeCaja = int.Parse(generarCorrelativoAuxiliar).ToString("D7"),
                                    CodiEmpresa = entidad.CodiEmpresa,
                                    CodiSucursal = entidad.CodiOficina,
                                    Boleto = auxBoletoCompleto.Substring(1),
                                    Monto = entidad.TipoPago == "02" ? entidad.Credito : entidad.PrecioVenta,
                                    CodiUsuario = entidad.CodiUsuario,
                                    Recibe = entidad.TipoPago == "02" ? "PAGO MULTIPLE-PARCIAL" : string.Empty,
                                    CodiDestino = entidad.CodiDestino.ToString(),
                                    FechaViaje = entidad.FechaViaje,
                                    HoraViaje = entidad.HoraViaje,
                                    CodiPuntoVenta = entidad.CodiPuntoVenta,
                                    IdVenta = entidad.IdVenta,
                                    Origen = entidad.TipoPago == "02" ? "PA" : "VT",
                                    Modulo = entidad.TipoPago == "02" ? "VT" : "PV",
                                    Tipo = entidad.Tipo,

                                    NomUsuario = entidad.NomUsuario,
                                    ConcCaja = auxBoletoCompleto,
                                    TipoVale = "S",
                                    CodiBus = "",
                                    CodiChofer = "",
                                    CodiGasto = "",
                                    IndiAnulado = "F",
                                    TipoDescuento = "",
                                    TipoDoc = "XX",
                                    TipoGasto = "P",
                                    Liqui = 0M,
                                    Diferencia = 0M,
                                    Voucher = "PA",
                                    Asiento = "",
                                    Ruc = "N",

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
                        NomPuntoVenta = entidad.CodiPuntoVenta.ToString(),
                        Pasajero = entidad.Nombre,
                        FechaViaje = entidad.FechaViaje,
                        HoraViaje = entidad.HoraViaje,
                        NomDestino = entidad.NomDestino,
                        Precio = entidad.PrecioVenta,
                        Obs1 = "ID " + entidad.IdVenta + " VENTA DE PASAJES NRO BOLETO:" + auxBoletoCompleto,
                        Obs2 = "Empresa : " + entidad.CodiEmpresa.ToString("D2"),
                        Obs3 = "TERMINAL : " + entidad.CodiTerminal+ " SER. " + entidad.CodiServicio.ToString("D2"),
                        Obs4 = "PROGRAMACION" + entidad.CodiProgramacion + " ORG PAS " + entidad.CodiOrigen.ToString("D3"),
                        Obs5 = "DET BUS " + entidad.CodiRuta.ToString("D3") + " DET PAS " + entidad.CodiDestino.ToString("D3")
                    };

                    var grabarAuditoria = VentaRepository.GrabarAuditoria(objAuditoriaEntity);
                    if (!grabarAuditoria)
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorGrabarAuditoria, false);

                    // Añado 'ventaRealizada'
                    var ventaRealizada = new VentaRealizadaEntity
                    {
                        // Para la vista 'BoletosVendidos'
                        BoletoCompleto = BoletoFormatoCompleto(validarTerminalElectronico.Tipo, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "8"),
                        NumeAsiento = entidad.NumeAsiento.ToString("D2"),
                        // Para el método 'ConvertirVentaToBase64'
                        IdVenta = entidad.IdVenta,
                        BoletoTipo = entidad.Tipo,
                        BoletoSerie = entidad.SerieBoleto.ToString("D3"),
                        BoletoNum = entidad.NumeBoleto.ToString("D8"),
                        CodDocumento = entidad.CodiDocumento,
                        EmisionFecha = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
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
                        PrecioDes = DataUtility.MontoSolesALetras(DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta)),
                        NomServicio = entidad.NomServicio,
                        FechaViaje = entidad.FechaViaje,
                        EmbarqueDir = entidad.DirEmbarque,
                        EmbarqueHora = entidad.HoraEmbarque,
                        CodigoX_FE = entidad.SignatureValue,
                        CodTerminal = validarTerminalElectronico.Tipo,
                        TipImpresora = byte.Parse(validarTerminalElectronico.Imp),

                        EmpDirAgencia = entidad.EmpDirAgencia ?? string.Empty,
                        EmpTelefono1 = entidad.EmpTelefono1 ?? string.Empty,
                        EmpTelefono2 = entidad.EmpTelefono2 ?? string.Empty,
                        PolizaNum = entidad.PolizaNum,
                        PolizaFechaReg = entidad.PolizaFechaReg,
                        PolizaFechaVen = entidad.PolizaFechaVen,

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
                        var objAcompanianteRequest = new AcompanianteRequest()
                        {
                            IdVenta = entidad.IdVenta,
                            Acompaniante = entidad.ObjAcompaniante,
                            ActionType = 1
                        };
                        VentaRepository.AcompanianteVentaCRUD(objAcompanianteRequest);
                    }

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
                return new Response<bool>(false, false, Message.MsgExAcompanianteVentaCRUD, false);
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

                var objPanelCanAnuPorDia = ListarPanelControl.Find(x => x.CodiPanel == "65");

                var objVenta = VentaRepository.BuscarVentaById(request.IdVenta);
                if (objVenta.SerieBoleto == 0)
                    return new Response<byte>(false, anularVenta, Message.MsgErrorAnularVenta, true);

                // Valida 'AnularDocumentoSUNAT'
                if (request.Tipo != "M" && request.FlagVenta != "Y")
                {
                    // Anula 'DocumentoSUNAT'
                    objVenta.Tipo = request.Tipo;
                    objVenta.FechaVenta = request.FechaVenta;

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
                    if (request.FechaVenta == DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
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
                    {
                        VentaRepository.ActualizarCajaAnulacion(int.Parse(request.ValeRemoto ?? "0"));
                    }

                    // Consulta 'BoletoPorContrato'
                    var consultaBoletoPorContrato = CreditoRepository.ConsultaBoletoPorContrato(request.IdVenta);
                    if (consultaBoletoPorContrato > 0)
                    {
                        // Actualiza 'BoletosPorContrato'
                        var actualizarBoletosPorContrato = CreditoRepository.ActualizarBoletosPorContrato(consultaBoletoPorContrato.ToString());
                        if (!actualizarBoletosPorContrato)
                            return new Response<byte>(false, 0, Message.MsgErrorActualizarBoletosPorContrato, true);

                        // Actualiza 'BoletosStock'
                        var actualizarBoletosStock = CreditoRepository.ActualizarBoletosStock("0", objVenta.IdPrecio.ToString(), "1"); // Se está suponiendo que 'FlagPrecioNormal' siempre es 'false'.
                        if (!actualizarBoletosStock)
                            return new Response<byte>(false, 0, Message.MsgErrorActualizarBoletosStock, true);
                    }
                    else
                        return new Response<byte>(false, 0, Message.MsgErrorConsultaBoletoPorContrato, true);
                }

                // Genera 'CorrelativoAuxiliar'
                var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", request.CodiOficina, request.CodiPuntoVenta, string.Empty);
                if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                    return new Response<byte>(false, anularVenta, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                // Graba 'Caja'
                var objCaja = new CajaEntity
                {
                    NumeCaja = int.Parse(generarCorrelativoAuxiliar).ToString("D7"),
                    CodiEmpresa = objVenta.CodiEmpresa,
                    CodiSucursal = short.Parse(request.CodiOficina),
                    Boleto = objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7"),
                    Monto = request.PrecioVenta,
                    CodiUsuario = short.Parse(request.CodiUsuario.ToString()),
                    Recibe = string.Empty,
                    CodiDestino = request.CodiDestinoPas.TrimStart('0'),
                    FechaViaje = request.FechaViaje,
                    HoraViaje = "VNA",
                    CodiPuntoVenta = short.Parse(request.CodiPuntoVenta),
                    IdVenta = request.IdVenta,
                    Origen = "AB",
                    Modulo = "AP",
                    Tipo = request.Tipo,

                    NomUsuario = request.CodiUsuario.ToString() + request.NomUsuario,
                    ConcCaja = "AN.BOL " + objVenta.Tipo + objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7"),
                    TipoVale = "S",
                    CodiBus = "",
                    CodiChofer = "",
                    CodiGasto = "",
                    IndiAnulado = "F",
                    TipoDescuento = "",
                    TipoDoc = "XX",
                    TipoGasto = "P",
                    Liqui = 0M,
                    Diferencia = 0M,
                    Voucher = "PA",
                    Asiento = "",
                    Ruc = "",

                    IdCaja = 0
                };
                var grabarCaja = VentaRepository.GrabarCaja(objCaja);

                if (grabarCaja > 0)
                {
                    // Anula 'Venta'
                    anularVenta = VentaRepository.AnularVenta(request.IdVenta, request.CodiUsuario);
                    if (anularVenta > 0)
                    {
                        // Elimina 'Poliza'
                        VentaRepository.EliminarPoliza(request.IdVenta);

                        if (objPanelCanAnuPorDia != null && objPanelCanAnuPorDia.Valor == "1")
                        {
                            // Consulta 'AnulacionPorDia'
                            var ConsultaAnulacionPorDia =  TurnoRepository.ConsultaAnulacionPorDia(int.Parse(request.CodiPuntoVenta), DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            if (ConsultaAnulacionPorDia <= 0)
                                // Inserta 'AnulacionPorDia'
                                VentaRepository.InsertarAnulacionPorDia(DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), int.Parse(request.CodiPuntoVenta), 1); // 1 -> Porque se anula uno por uno.
                            else
                                // Actualiza 'AnulacionPorDia'
                                VentaRepository.ActualizarAnulacionPorDia(DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), int.Parse(request.CodiPuntoVenta));
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
                            var objReintegro = VentaRepository.ConsultaVentaReintegro(request.CodiEsca.Substring(1,3), request.CodiEsca.Substring(5), objVenta.CodiEmpresa.ToString(), request.CodiEsca.Substring(0,1));

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
                                if (request.CodiEsca.Substring(0, 1) != "M")
                                {
                                    // Anula 'DocumentoSUNAT'
                                    var objVentaReintegro = new VentaEntity
                                    {
                                        CodiEmpresa = objReintegro.CodiEmpresa,
                                        SerieBoleto = short.Parse(request.CodiEsca.Substring(1, 3)),
                                        NumeBoleto = int.Parse(request.CodiEsca.Substring(5)),
                                        Tipo = objReintegro.Tipo,
                                        FechaVenta = objReintegro.FechaVenta
                                    };

                                    var resAnularDocumentoSUNAT = AnularDocumentoSUNAT(objVentaReintegro);
                                    if (!resAnularDocumentoSUNAT.Estado)
                                        return new Response<byte>(false, anularVenta, resAnularDocumentoSUNAT.MensajeError, false);
                                }

                                // Genera 'CorrelativoAuxiliar'
                                var generarCorrelativoAuxiliarReintegro = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", request.CodiOficina, request.CodiPuntoVenta, string.Empty);
                                if (string.IsNullOrEmpty(generarCorrelativoAuxiliarReintegro))
                                    return new Response<byte>(false, anularVenta, Message.MsgErrorGenerarCorrelativoAuxiliarReintegro, false);

                                // Graba 'CajaReintegro'
                                var objCajaReintegro = new CajaEntity
                                {
                                    NumeCaja = int.Parse(generarCorrelativoAuxiliarReintegro).ToString("D7"),
                                    CodiEmpresa = objReintegro.CodiEmpresa,
                                    CodiSucursal = short.Parse(request.CodiOficina),
                                    Boleto = request.CodiEsca.Substring(1),
                                    Monto = objReintegro.PrecioVenta,
                                    CodiUsuario = short.Parse(request.CodiUsuario.ToString()),
                                    Recibe = "RE",
                                    CodiDestino = request.CodiDestinoPas,
                                    FechaViaje = "01/01/1900",
                                    HoraViaje = "VNA",
                                    CodiPuntoVenta = short.Parse(request.CodiPuntoVenta),
                                    IdVenta = objReintegro.IdVenta,
                                    Origen = "AR",
                                    Modulo = "PV",
                                    Tipo = request.CodiEsca.Substring(0, 1),

                                    NomUsuario = request.NomUsuario,
                                    ConcCaja = "ANUL. BOL. REINTEGRO" + request.CodiEsca,
                                    TipoVale = "S",
                                    CodiBus = "",
                                    CodiChofer = "",
                                    CodiGasto = "",
                                    IndiAnulado = "F",
                                    TipoDescuento = "RE",
                                    TipoDoc = "",
                                    TipoGasto = "P",
                                    Liqui = 0M,
                                    Diferencia = 0M,
                                    Voucher = "RE",
                                    Asiento = "",
                                    Ruc = request.IngresoManualPasajes ? "MA" : string.Empty,

                                    IdCaja = 0
                                };
                                var grabarCajaReintegro = VentaRepository.GrabarCaja(objCajaReintegro);
                            }
                        }

                        // Anulaciòn para 'Pase'
                        if (request.FlagVenta == "7")
                        {
                            // Actualiza 'BoletosPorSocio'
                            VentaRepository.ActualizarBoletosPorSocio(objVenta.PerAutoriza, DateTime.ParseExact(request.FechaVenta, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM"), Convert.ToDateTime(request.FechaVenta, CultureInfo.InvariantCulture).ToString("yyyy"));

                            // Consulta 'CajaPase'
                            var consultaCajaPase = VentaRepository.ConsultaCajaPase(objVenta.SerieBoleto.ToString("D3") + "-" + objVenta.NumeBoleto.ToString("D7"));

                            if (consultaCajaPase.IdCaja > 0)
                            {
                                // Actualiza 'BoletoPorSocioV'
                                VentaRepository.ActualizarBoletosPorSocioV(objVenta.PerAutoriza, DateTime.Now.ToString("MM", CultureInfo.InvariantCulture), DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture));
                            }
                        }

                        return new Response<byte>(true, anularVenta, Message.MsgCorrectoAnularVenta, true);
                    }
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

        public static Response<int> VerificaNC(int IdVenta)
        {
            try
            {
                var verificaNC = VentaRepository.VerificaNC(IdVenta);

                return new Response<int>(true, verificaNC, Message.MsgCorrectoVerificaNC, true);
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

                return new Response<bool>(true, consultaClaveControl, Message.MsgCorrectoConsultaClaveControl, true);
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
                var listaVentasRealizadas = new List<VentaRealizadaEntity>();

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
                else
                    return new Response<VentaResponse>(false, valor, Message.MsgErrorPostergarVenta, false);
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

                var objPanelCopia1 = ListarPanelControl.Find(x => x.CodiPanel == "197");
                var objPanelCopia2 = ListarPanelControl.Find(x => x.CodiPanel == "198");

                foreach (var entidad in Listado)
                {
                    var paginaWebEmisor = string.Empty;
                    var buscarEmpresaEmisor = VentaRepository.BuscarEmpresaEmisor(entidad.EmpCodigo);
                    var buscarDireccionPVenta = VentaRepository.BuscarAgenciaEmpresa(entidad.EmpCodigo, int.Parse(entidad.EmbarqueCod.ToString()));
                    var consultaNroPoliza = new PolizaEntity()
                    {
                        NroPoliza = "",
                        FechaReg = "01/01/1900",
                        FechaVen = "01/01/1900"
                    };
                    var objPanelPoliza = ListarPanelControl.Find(x => x.CodiPanel == "224");
                    if (objPanelPoliza != null && objPanelPoliza.Valor == "1")
                        consultaNroPoliza = VentaRepository.ConsultaNroPoliza(entidad.EmpCodigo, entidad.BusCodigo, entidad.FechaViaje);

                    // Solo para 'Terminales electrónicos'
                    if (entidad.BoletoTipo != "M")
                        paginaWebEmisor = ObtenerPaginaWebEmisor(buscarEmpresaEmisor.Ruc);

                    // Solo para 'Reimpresion'
                    if (TipoImpresion == TipoReimprimir)
                    {
                        var buscarAgenciaEmpresa = VentaRepository.BuscarAgenciaEmpresa(entidad.EmpCodigo, entidad.PVentaCodigo);
                        var resObtenerCodigoX = ObtenerCodigoX(buscarEmpresaEmisor.Ruc, entidad.BoletoTipo, short.Parse(entidad.BoletoSerie), int.Parse(entidad.BoletoNum));
                        var validarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(entidad.EmpCodigo, entidad.CajeroOficina, entidad.CajeroPVenta, short.Parse(entidad.CajeroTerminal.ToString()));
                        
                        entidad.NumeAsiento = byte.Parse(entidad.NumeAsiento).ToString("D2");
                        entidad.BoletoSerie = short.Parse(entidad.BoletoSerie).ToString("D3");
                        entidad.BoletoNum = int.Parse(entidad.BoletoNum).ToString("D8");
                        entidad.EmisionFecha = entidad.EmisionFecha;
                        entidad.EmisionHora = DateTime.ParseExact(entidad.EmisionHora, "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mmtt", CultureInfo.InvariantCulture);
                        entidad.DocTipo = TipoDocumentoHomologadoParaFE(entidad.DocTipo.ToString());
                        entidad.PrecioDes = DataUtility.MontoSolesALetras(DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioCan));
                        entidad.CodigoX_FE = resObtenerCodigoX.SignatureValue;
                        entidad.CodTerminal = validarTerminalElectronico.Tipo;
                        entidad.TipImpresora = byte.Parse(validarTerminalElectronico.Imp);
                        entidad.PolizaNum = consultaNroPoliza.NroPoliza;
                        entidad.PolizaFechaReg = consultaNroPoliza.FechaReg;
                        entidad.PolizaFechaVen = consultaNroPoliza.FechaVen;

                        entidad.EmpDirAgencia = buscarAgenciaEmpresa.Direccion;
                        entidad.EmpTelefono1 = buscarAgenciaEmpresa.Telefono1;
                        entidad.EmpTelefono2 = buscarAgenciaEmpresa.Telefono2;
                    }

                    entidad.NomTipVenta = "EFECTIVO";
                    entidad.CodX = "1";
                    entidad.EmpRuc = buscarEmpresaEmisor.Ruc;
                    entidad.EmpRazSocial = buscarEmpresaEmisor.RazonSocial;
                    entidad.EmpDireccion = buscarEmpresaEmisor.Direccion;
                    entidad.LinkPag_FE = paginaWebEmisor;
                    entidad.EmbarqueDirAgencia = buscarDireccionPVenta.Direccion;

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
                var serviceFE = new Ws_SeeFacteSoapClient();
                var entidadFE = new SetInvoiceRequestBody();
                
                // Busca 'RucEmpresa'
                var buscarEmpresaEmisor = VentaRepository.BuscarEmpresaEmisor(entidad.CodiEmpresa);

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

        public static string ObtenerPaginaWebEmisor(string Ruc)
        {
            try
            {
                var paginaWebEmisor = string.Empty;

                var serviceFE = new Ws_SeeFacteSoapClient();
                var seguridadFE = new Security
                {
                    ID = Ruc,
                    User = UserWebSUNAT
                };

                paginaWebEmisor = serviceFE.GetParametro(seguridadFE).Rempresa.PaginaWebEmisor ?? string.Empty;

                return paginaWebEmisor;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static ResponseDocument ObtenerCodigoX(string Ruc, string DocTipo, short BoletoSerie, int BoletoNum)
        {
            try
            {
                var response = new ResponseDocument();
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

                response = serviceFE.GetValidarDocument(seguridadFE, auxTDocumento, DocTipo + BoletoSerie.ToString("D3"), BoletoNum.ToString("D8"));

                return response;
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

            TipoDocumento = byte.Parse(TipoDocumento).ToString("D2");

            if (TipoDocumento == "01")
                valor = 1;
            else if (TipoDocumento == "04")
                valor = 7;
            else
                valor = 4;

            return valor;
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
                VentaRepository.ActualizaF9Elec(request.IdVenta, request.Dni, request.Nombre, request.Ruc, request.Edad, request.Telefono, request.RecoVenta, request.TipoDoc, request.Nacionalidad);
                return new Response<bool>(true, true, Message.MsgCorrectoActualizaBoletoF9, true);
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
