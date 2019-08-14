using SisComWeb.Business.ServiceFE;
using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Entity.Peticiones.Response;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SisComWeb.Business
{
    public class ReintegroLogic
    {
        private static readonly string UserWebSUNAT = ConfigurationManager.AppSettings["userWebSUNAT"].ToString();
        private static readonly string CodiCorrelativoVentaBoleta = ConfigurationManager.AppSettings["codiCorrelativoVentaBoleta"].ToString();
        private static readonly string CodiCorrelativoVentaFactura = ConfigurationManager.AppSettings["codiCorrelativoVentaFactura"].ToString();
        private static readonly string CodiCorrelativoPaseBoleta = ConfigurationManager.AppSettings["codiCorrelativoPaseBoleta"].ToString();
        private static readonly string CodiCorrelativoPaseFactura = ConfigurationManager.AppSettings["codiCorrelativoPaseFactura"].ToString();
        private static readonly string CodiCorrelativoCredito = ConfigurationManager.AppSettings["codiCorrelativoCredito"].ToString();

        public static Response<ReintegroEntity> VentaConsultaF12(ReintegroRequest request)
        {
            try
            {
                var valor = ReintegroRepository.VentaConsultaF12(request);
                //Datos adicionales: 'FechaNacimiento'
                var clientePasaje = ClientePasajeRepository.BuscaPasajero(valor.TipoDocumento, valor.Dni);
                valor.FechaNac = clientePasaje.FechaNacimiento;

                if (valor.IdVenta == 0)
                {
                    return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12NoExiste, true);
                }
                //Setea Razón Social y Dirección con el RUC
                if (!string.IsNullOrEmpty(valor.RucCliente))
                {
                    var buscarEmpresa = ClientePasajeRepository.BuscarEmpresa(valor.RucCliente);
                    valor.RazonSocial = buscarEmpresa.RazonSocial;
                    valor.Direccion = buscarEmpresa.Direccion;
                }
                else
                {
                    valor.RazonSocial = string.Empty;
                    valor.Direccion = string.Empty;
                }

                // Busca 'AgenciaEmpresa' (E -> GenerarAdicionales, M -> También se va a necesitar.)
                var buscarAgenciaEmpresa = new AgenciaEntity();
                buscarAgenciaEmpresa = VentaRepository.BuscarAgenciaEmpresa(valor.CodiEmpresa, valor.CodiPuntoVenta);
                valor.DirEmbarque = buscarAgenciaEmpresa.Direccion;

                //Verifica si el boleto está en Fecha Abierta
                if (valor.CodiProgramacion != 0)
                {
                    var programacion = ReintegroRepository.DatosProgramacion(valor.CodiProgramacion);
                    //Verifica si no tiene Programación, caso contrario setea Ruta y Servicio
                    if (programacion.CodiRuta != 0 && programacion.CodiServicio != 00)
                    {
                        valor.CodiRuta = programacion.CodiRuta;
                        valor.CodiServicio = programacion.CodiServicio;
                        valor.CodiBus = programacion.CodiBus;
                    }
                    else
                    {
                        valor.CodiError = 2;
                        return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12SinProgramacion, true);
                    }
                    //Verfica si tiene Nota de Crédito
                    var notaCredito = VentaRepository.VerificaNC(valor.IdVenta);
                    if (Convert.ToInt32(notaCredito.id) > 0)
                    {
                        valor.CodiError = 3;
                        return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12NotaCredito + " " + notaCredito.label, true);
                    }
                    //Verfica si esta como Reintegro
                    if (valor.FlagVenta == "O")
                    {
                        valor.CodiError = 4;
                        return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12EsReintegro, true);
                    }
                    //Verfica si ya tiene adjunto un Reintegro
                    if (valor.CodiEsca != "")
                    {
                        valor.CodiError = 5;
                        return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12TieneReintegro, true);
                    }
                }
                else
                {
                    valor.CodiError = 1;
                    return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12EsFechaAbierta, true);
                }
                return new Response<ReintegroEntity>(true, valor, Message.MsgCorrectoVentaConsultaF12, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReintegroEntity>(false, null, Message.MsgExcVentaConsultaF12, false);
            }
        }

        public static Response<List<SelectReintegroEntity>> ListaOpcionesModificacion()
        {
            try
            {
                var lista = ReintegroRepository.ListaOpcionesModificacion();
                return new Response<List<SelectReintegroEntity>>(true, lista, Message.MsgCorrectoListaOpcionesModificacion, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<SelectReintegroEntity>>(false, null, Message.MsgExcListaOpcionesModificacion, false);
            }
        }

        public static Response<bool> ValidaExDni(string documento)
        {
            try
            {
                var res = ReintegroRepository.ValidaExDni(documento);
                var mensaje = (res) ? Message.MsgValidaExDni : string.Empty;
                return new Response<bool>(true, res, mensaje, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcValidaExDni, false);
            }
        }

        public static Response<VentaResponse> SaveReintegro(ReintegroVentaRequest filtro)
        {
            var valor = new VentaResponse();

            try
            {
                var ListarPanelControl = CreditoRepository.ListarPanelControl();
                var listaVentasRealizadas = new List<VentaRealizadaEntity>();

                SetInvoiceRequestBody bodyDocumentoSUNAT = null;

                // Modifica Empresa por Panel 223
                var objModificaEmpresa = ListarPanelControl.Find(x => x.CodiPanel == "223");
                if (objModificaEmpresa != null && objModificaEmpresa.Valor == "1")
                {
                    var NuevoCodiEmpresa = ReintegroRepository.ConsultaEmpresaPVentaYServicio(Convert.ToInt32(filtro.Punto_Venta), Convert.ToInt32(filtro.servicio));
                    filtro.Codi_Empresa__ = (NuevoCodiEmpresa == 0) ? filtro.Codi_Empresa : Convert.ToString(NuevoCodiEmpresa);
                }

                var entidad = new VentaEntity()
                {
                    CodiEmpresa = byte.Parse(filtro.Codi_Empresa__), //verificar
                    UserWebSUNAT = UserWebSUNAT,
                    TipoDocumento = filtro.tipo_doc,
                    RucCliente = filtro.NIT_CLIENTE,
                    NomEmpresaRuc = filtro.NomEmpresaRuc,
                    DirEmpresaRuc = filtro.DirEmpresaRuc,
                    Tipo = filtro.Tipo,
                    SerieBoleto = short.Parse(filtro.Serie),
                    NumeBoleto = int.Parse(filtro.nume_boleto.PadLeft(8, '0')),
                    PrecioVenta = Convert.ToDecimal(filtro.PRECIO_VENTA),
                    NomDestino = filtro.NombDestino,
                    NomServicio = filtro.NomServicio,
                    NumeAsiento = byte.Parse(filtro.NUMERO_ASIENTO),
                    Dni = filtro.Dni,
                    FechaViaje = filtro.Fecha_viaje,
                    HoraViaje = filtro.HORA_V,
                    FlagVenta = filtro.FLAG_VENTA,
                    CodiUsuario = short.Parse(filtro.Clav_Usuario),
                    CodiOficina = short.Parse(filtro.CODI_SUCURSAL),
                    CodiPuntoVenta = short.Parse(filtro.Pventa__),
                    CodiTerminal = filtro.CODI_TERMINAL__,
                    CodiBus = filtro.CodiBus,
                    CodiEmbarque = short.Parse(filtro.Sube_en),
                    Nombre = filtro.NOMB,
                    TipoPago = filtro.Tipo_Pago
                };

                var validarTerminalElectronico = VentaRepository.ValidarTerminalElectronico(entidad.CodiEmpresa, entidad.CodiOficina, entidad.CodiPuntoVenta, short.Parse(entidad.CodiTerminal));

                // Seteo 'CodiDocumento'
                if (!string.IsNullOrEmpty(entidad.RucCliente))
                {
                    entidad.AuxCodigoBF_Interno = CodiCorrelativoVentaFactura;
                    entidad.CodiDocumento = "01"; // Factura
                }
                else
                {
                    entidad.AuxCodigoBF_Interno = CodiCorrelativoVentaBoleta;
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
                                        };
                                        break;
                                    case "03": // Boleta
                                        {
                                            if (buscarCorrelativo.SerieBoleto == 0)
                                                return new Response<VentaResponse>(false, valor, Message.MsgErrorSerieBoleto, false);
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
                        if (!string.IsNullOrEmpty(entidad.RucCliente))
                        {
                            var objPanelPrecioValor = ListarPanelControl.Find(x => x.CodiPanel == "145");
                            if (objPanelPrecioValor != null && objPanelPrecioValor.Valor == "1")
                            {
                                entidad.AuxCodigoBF_Interno = CodiCorrelativoCredito;
                                // Busca 'Correlativo'
                                var buscarCorrelativo2 = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, entidad.AuxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, validarTerminalElectronico.Tipo);
                                if (buscarCorrelativo2.SerieBoleto != 0)
                                {
                                    entidad.SerieBoleto = buscarCorrelativo2.SerieBoleto;
                                    entidad.NumeBoleto = buscarCorrelativo2.NumeBoleto;
                                }
                                else
                                {
                                    return new Response<VentaResponse>(false, valor, "Número de correlativo no esta configurado para el tipo " + entidad.AuxCodigoBF_Interno, false);
                                }

                            }
                        }
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

                //Para enviar a grabar
                filtro.Tipo = entidad.Tipo;

                // Busca 'AgenciaEmpresa' (E -> GenerarAdicionales, M -> También se va a necesitar.)
                var buscarAgenciaEmpresa = new AgenciaEntity();
                buscarAgenciaEmpresa = VentaRepository.BuscarAgenciaEmpresa(entidad.CodiEmpresa, entidad.CodiPuntoVenta);
                entidad.EmpDirAgencia = buscarAgenciaEmpresa.Direccion;
                entidad.EmpTelefono1 = buscarAgenciaEmpresa.Telefono1;
                entidad.EmpTelefono2 = buscarAgenciaEmpresa.Telefono2;

                //Valida 'ConsultaPoliza'
                var consultaNroPoliza = new PolizaEntity()
                {
                    NroPoliza = string.Empty,
                    FechaReg = "01/01/1900",
                    FechaVen = "01/01/1900"
                };
                var objPanelPoliza = ListarPanelControl.Find(x => x.CodiPanel == "224");
                if (objPanelPoliza != null && objPanelPoliza.Valor == "1")
                {
                    consultaNroPoliza = VentaRepository.ConsultaNroPoliza(entidad.CodiEmpresa, entidad.CodiBus, entidad.FechaViaje);
                    if (string.IsNullOrEmpty(consultaNroPoliza.NroPoliza))
                        return new Response<VentaResponse>(false, valor, Message.MsgErrorConsultaNroPoliza, false);
                }

                entidad.PolizaNum = consultaNroPoliza.NroPoliza;
                entidad.PolizaFechaReg = consultaNroPoliza.FechaReg;
                entidad.PolizaFechaVen = consultaNroPoliza.FechaVen;

                // Valida 'DocumentoSUNAT'
                ResponseW resValidarDocumentoSUNAT = null;

                if (!filtro.Tipo.Equals("M"))
                    resValidarDocumentoSUNAT = VentaLogic.ValidarDocumentoSUNAT(entidad, ref bodyDocumentoSUNAT);


                if (resValidarDocumentoSUNAT != null || filtro.Tipo.Equals("M"))
                {
                    if ((resValidarDocumentoSUNAT != null && resValidarDocumentoSUNAT.Estado) || filtro.Tipo.Equals("M"))
                    {
                        //Graba Reintegro
                        var res = ReintegroRepository.SaveReintegro(filtro);
                        var ventaRealizada = (res > 0) ? true : false;

                        if (ventaRealizada)
                        {
                            entidad.IdVenta = res;

                            var objAuditoria = new AuditoriaEntity
                            {
                                CodiUsuario = Convert.ToInt16(filtro.Clav_Usuario),
                                NomUsuario = filtro.NomUsuario,
                                Tabla = "VENTA",
                                TipoMovimiento = "BOL-REINTEGRO",
                                Boleto = filtro.BoletoAuditoria,
                                NumeAsiento = filtro.NumAsientoAuditoria.PadLeft(2, '0'),
                                NomOficina = filtro.NomSucursal,
                                NomPuntoVenta = filtro.Punto_Venta.PadLeft(3, '0'),
                                Pasajero = filtro.NOMB,
                                FechaViaje = filtro.Fecha_viaje,
                                HoraViaje = filtro.HORA_V,
                                NomDestino = filtro.NombDestino,
                                Precio = (decimal)filtro.PRECIO_VENTA,
                                Obs1 = "REINTEGRO DE PASAJES",
                                Obs2 = filtro.CODI_PROGRAMACION,
                                Obs3 = "BOL-" + filtro.Serie + "-" + filtro.nume_boleto,
                                Obs4 = filtro.NomMotivo,
                                Obs5 = string.Empty
                            };
                            //Graba Auditoria
                            VentaRepository.GrabarAuditoria(objAuditoria);

                            // Valida 'TipoPago'
                            switch (entidad.TipoPago)
                            {
                                case "02": // Múltiple pago
                                case "03": // Tarjeta de crédito    
                                    {
                                        //  Genera 'CorrelativoAuxiliar'
                                        var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", entidad.CodiOficina.ToString(), entidad.CodiPuntoVenta.ToString(), string.Empty);
                                        if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                                            return new Response<VentaResponse>(false, valor, Message.MsgErrorGenerarCorrelativoAuxiliar, false);

                                        var auxBoletoCompleto = VentaLogic.BoletoFormatoCompleto(validarTerminalElectronico.Tipo, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "7");

                                        var auxCodiDestino = (filtro.CodMotivo.Equals("00003") || filtro.CodMotivo.Equals("00004")) ? filtro.CODI_SUBRUTA : "";

                                        // Graba 'Caja'
                                        var objCajaEntity = new CajaEntity
                                        {
                                            NumeCaja = generarCorrelativoAuxiliar.PadLeft(7, '0'),
                                            CodiEmpresa = entidad.CodiEmpresa,
                                            CodiSucursal = entidad.CodiOficina,
                                            FechaCaja = DataUtility.ObtenerFechaDelSistema(),
                                            TipoVale = "S",
                                            Boleto = auxBoletoCompleto.Substring(1),
                                            NomUsuario = filtro.NomUsuario,
                                            CodiBus = string.Empty,
                                            CodiChofer = string.Empty,
                                            CodiGasto = string.Empty,
                                            ConcCaja = auxBoletoCompleto.Substring(1),
                                            Monto = entidad.PrecioVenta,
                                            CodiUsuario = entidad.CodiUsuario,
                                            IndiAnulado = "F",
                                            TipoDescuento = string.Empty,
                                            TipoDoc = "XX",
                                            TipoGasto = "P",
                                            Liqui = 0M,
                                            Diferencia = 0M,
                                            Recibe = string.Empty,
                                            CodiDestino = auxCodiDestino,
                                            FechaViaje = entidad.FechaViaje,
                                            HoraViaje = entidad.HoraViaje,
                                            CodiPuntoVenta = entidad.CodiPuntoVenta,
                                            Voucher = "PA",
                                            Asiento = string.Empty,
                                            Ruc = "N",
                                            IdVenta = entidad.IdVenta,
                                            Origen = "MT",
                                            Modulo = "PM",
                                            Tipo = entidad.Tipo,

                                            IdCaja = 0
                                        };

                                        var grabarCaja = VentaRepository.GrabarCaja(objCajaEntity);
                                        if (grabarCaja > 0)
                                        {
                                            // Seteo 'NumeCaja'
                                            var auxNumeCaja = entidad.CodiOficina.ToString("D3") + entidad.CodiPuntoVenta.ToString("D3") + generarCorrelativoAuxiliar.PadLeft(7, '0');

                                            // Graba 'PagoTarjetaCredito'
                                            var objTarjetaCreditoEntity = new TarjetaCreditoEntity
                                            {
                                                IdVenta = entidad.IdVenta,
                                                Boleto = auxBoletoCompleto.Substring(1),
                                                CodiTarjetaCredito = filtro.CodiTarjetaCredito,
                                                NumeTarjetaCredito = filtro.NumeTarjetaCredito,
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
                            };


                            if (filtro.stReintegro.Equals("X"))
                            {
                                var objAuditoria2 = new AuditoriaEntity
                                {
                                    CodiUsuario = Convert.ToInt16(filtro.Clav_Usuario),
                                    NomUsuario = filtro.NomUsuario,
                                    Tabla = "VENTA",
                                    TipoMovimiento = "BOL-REI-CRE",
                                    Boleto = filtro.BoletoAuditoria,
                                    NumeAsiento = filtro.NumAsientoAuditoria.PadLeft(2, '0'),
                                    NomOficina = filtro.NomSucursal,
                                    NomPuntoVenta = filtro.Punto_Venta.PadLeft(3, '0'),//
                                    Pasajero = filtro.NOMB,
                                    FechaViaje = filtro.Fecha_viaje,
                                    HoraViaje = filtro.HORA_V,
                                    NomDestino = filtro.NombDestino,
                                    Precio = (decimal)filtro.PRECIO_VENTA,
                                    Obs1 = "REINTEGRO CON CREDITO",
                                    Obs2 = filtro.CODI_PROGRAMACION,
                                    Obs3 = "BOL-" + filtro.nume_boleto,
                                    Obs4 = filtro.NomMotivo,
                                    Obs5 = string.Empty
                                };
                                //Graba Auditoria Adicional
                                VentaRepository.GrabarAuditoria(objAuditoria2);
                            }

                            if (!filtro.Tipo.Equals("M"))
                            {
                                //Registra 'DocumentoSUNAT'
                                var resRegistrarDocumentoSUNAT = VentaLogic.RegistrarDocumentoSUNAT(bodyDocumentoSUNAT);

                                if (resRegistrarDocumentoSUNAT.Estado)
                                {
                                    entidad.SignatureValue = resRegistrarDocumentoSUNAT.SignatureValue ?? string.Empty;
                                }
                                else
                                {
                                    return new Response<VentaResponse>(false, valor, resRegistrarDocumentoSUNAT.MensajeError, false);
                                }
                            }
                        }

                        //Se crea esta entidad para la parte de impresión
                        var auxVentaRealizada = new VentaRealizadaEntity
                        {
                            // Para la vista 'BoletosVendidos'
                            BoletoCompleto = VentaLogic.BoletoFormatoCompleto(validarTerminalElectronico.Tipo, entidad.AuxCodigoBF_Interno, entidad.SerieBoleto, entidad.NumeBoleto, "3", "8"),//ok
                            NumeAsiento = entidad.NumeAsiento.ToString("D2"),
                            // Para el método 'ConvertirVentaToBase64'
                            IdVenta = res,//entidad.IdVenta,
                            BoletoTipo = entidad.Tipo,
                            BoletoSerie = entidad.SerieBoleto.ToString("D3"),
                            BoletoNum = entidad.NumeBoleto.ToString("D8"),
                            CodDocumento = entidad.CodiDocumento,
                            EmisionFecha = DataUtility.ObtenerFechaDelSistema(),
                            EmisionHora = DataUtility.Obtener12HorasDelSistema(),
                            CajeroCod = entidad.CodiUsuario,
                            CajeroNom = filtro.NomUsuario,
                            PasNombreCom = entidad.SplitNombre[0] + " " + entidad.SplitNombre[1] + " " + entidad.SplitNombre[2],
                            PasRuc = entidad.RucCliente,
                            PasRazSocial = entidad.NomEmpresaRuc,
                            PasDireccion = entidad.DirEmpresaRuc,
                            NomOriPas = filtro.NomOrigen,
                            NomDesPas = entidad.NomDestino,
                            DocTipo = VentaLogic.TipoDocumentoHomologadoParaFE(entidad.TipoDocumento),
                            DocNumero = entidad.Dni,
                            PrecioCan = entidad.PrecioVenta,
                            PrecioDes = DataUtility.MontoSolesALetras(DataUtility.ConvertDecimalToStringWithTwoDecimals(entidad.PrecioVenta)),
                            NomServicio = entidad.NomServicio,
                            FechaViaje = entidad.FechaViaje,
                            EmbarqueDir = filtro.DirEmbarque,
                            EmbarqueHora = filtro.Hora_Emb,
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
                        listaVentasRealizadas.Add(auxVentaRealizada);

                        valor.ListaVentasRealizadas = listaVentasRealizadas;
                        valor.CodiProgramacion = Convert.ToInt32(filtro.CODI_PROGRAMACION);

                        return (ventaRealizada) ? new Response<VentaResponse>(true, valor, string.Empty, true) : new Response<VentaResponse>(true, null, Message.MsgNoVentaReintegro, true);
                    }
                    else
                        return new Response<VentaResponse>(true, valor, resValidarDocumentoSUNAT.MensajeError, false);
                }
                else
                {
                    return new Response<VentaResponse>(false, valor, Message.MsgErrorWebServiceFacturacionElectronica, false);
                }
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcVentaReintegro, false);
            }
        }

        public static Response<decimal> ConsultarIgv(string TipoDoc)
        {
            try
            {
                var res = ReintegroRepository.ConsultarIgv(TipoDoc);
                return new Response<decimal>(true, res, string.Empty, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<decimal>(false, 0, Message.MsgExcConsultaIgv, false);
            }
        }

        public static Response<PlanoEntity> ConsultarPrecioRuta(PrecioRutaRequest request)
        {
            try
            {
                // Obtiene 'PrecioAsiento'
                var obtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, request.HoraViaje, request.FechaViaje, request.CodiServicio, request.CodiEmpresa, request.Nivel);

                // En caso de no encontrar resultado
                if (obtenerPrecioAsiento.PrecioNormal == 0 && obtenerPrecioAsiento.PrecioMinimo == 0 && obtenerPrecioAsiento.PrecioMaximo == 0)
                {
                    obtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, string.Empty, request.FechaViaje, request.CodiServicio, request.CodiEmpresa, request.Nivel);
                }

                return new Response<PlanoEntity>(true, obtenerPrecioAsiento, string.Empty, true); ;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PlanoEntity>(false, null, Message.MsgExcConsultaPrecioRuta, false);
            }
        }

        public static Response<bool> UpdateReintegro(UpdateReintegroRequest filtro)
        {
            try
            {
                var res = ReintegroRepository.UpdateReintegro(filtro);
                return new Response<bool>(true, res, "", false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaIgv, false);
            }
        }

        public static Response<ReintegroEntity> ValidaReintegroParaAnualar(ReintegroRequest request)
        {
            try
            {
                var res = ReintegroRepository.ValidaReintegroParaAnualar(request);
                if (res != null && res.IdVenta != 0)
                {
                    return new Response<ReintegroEntity>(true, res, string.Empty, true);
                }
                else
                {
                    return new Response<ReintegroEntity>(false, res, Message.MsgNoConsultaReintegroParaAnular, true);
                }
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReintegroEntity>(false, null, Message.MsgExcConsultaReintegroParaAnular, false);
            }
        }

        public static Response<byte> AnularReintegro(AnularVentaRequest request)
        {
            try
            {
                var anularVentaReintegro = new byte();

                if (request.IdVenta > 0)
                {
                    // Valida 'AnularDocumentoSUNAT'
                    if (request.CodiEsca.Substring(0, 1) != "M")
                    {
                        // Anula 'DocumentoSUNAT'
                        var objVentaReintegro = new VentaEntity
                        {
                            CodiEmpresa = request.CodiEmpresa,
                            SerieBoleto = request.SerieBoleto,
                            NumeBoleto = request.NumeBoleto,
                            Tipo = request.Tipo,
                            FechaVenta = request.FechaVenta
                        };

                        var resAnularDocumentoSUNAT = VentaLogic.AnularDocumentoSUNAT(objVentaReintegro);
                        if (!resAnularDocumentoSUNAT.Estado)
                            return new Response<byte>(false, anularVentaReintegro, resAnularDocumentoSUNAT.MensajeError, false);
                    }

                    // Anula 'Reintegro'
                    anularVentaReintegro = VentaRepository.AnularVenta(request.IdVenta, request.CodiUsuario);
                    if (anularVentaReintegro > 0)
                    {
                        if (request.TipoPago == "03")
                        {
                            // Consulta 'PagoTarjetaVenta'
                            var consultaPagoTarjetaVenta = VentaRepository.ConsultaPagoTarjetaVenta(request.IdVenta);

                            // Actualiza 'CajaAnulacion'
                            VentaRepository.ActualizarCajaAnulacion(consultaPagoTarjetaVenta);
                        }

                        //Elimina Boleto x Contrato si es que tiene
                        ReintegroRepository.EliminarBoletoxContrato(request.IdVenta);

                        //Libera Venta del Reintegro
                        ReintegroRepository.LiberaReintegroEle(request.CodiEsca.Substring(1, 3), request.CodiEsca.Substring(5), request.CodiEmpresa.ToString(), request.CodiEsca.Substring(0, 1));

                        // Genera 'CorrelativoAuxiliar'
                        var generarCorrelativoAuxiliarReintegro = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", request.CodiOficina, request.CodiPuntoVenta, string.Empty);
                        if (string.IsNullOrEmpty(generarCorrelativoAuxiliarReintegro))
                            return new Response<byte>(false, anularVentaReintegro, Message.MsgErrorGenerarCorrelativoAuxiliarReintegro, false);

                        // Graba 'CajaReintegro'
                        var objCajaReintegro = new CajaEntity
                        {
                            NumeCaja = generarCorrelativoAuxiliarReintegro.PadLeft(7, '0'),
                            CodiEmpresa = request.CodiEmpresa,
                            CodiSucursal = short.Parse(request.CodiOficina),
                            FechaCaja = DataUtility.ObtenerFechaDelSistema(),
                            TipoVale = "S",
                            Boleto = String.Format("{0}-{1}", request.SerieBoleto.ToString().PadLeft(3, '0'), request.NumeBoleto.ToString().PadLeft(7, '0')),
                            NomUsuario = String.Format("{0} {1}", request.CodiUsuario, request.NomUsuario),
                            CodiBus = string.Empty,
                            CodiChofer = string.Empty,
                            CodiGasto = string.Empty,
                            ConcCaja = String.Format("{0} {1}-{2}", "ANUL DE BOLETO REINT", request.SerieBoleto.ToString().PadLeft(3, '0'), request.NumeBoleto.ToString().PadLeft(7, '0')),
                            Monto = request.PrecioVenta,
                            CodiUsuario = short.Parse(request.CodiUsuario.ToString()),
                            IndiAnulado = "F",
                            TipoDescuento = "RE",
                            TipoDoc = "16",
                            TipoGasto = "P",
                            Liqui = 0M,
                            Diferencia = 0M,
                            Recibe = "RE",
                            CodiDestino = string.Empty,
                            FechaViaje = "01/01/1900",
                            HoraViaje = string.Empty,
                            CodiPuntoVenta = short.Parse(request.CodiPuntoVenta),
                            Voucher = string.Empty,
                            Asiento = string.Empty,
                            Ruc = string.Empty,
                            IdVenta = 0,
                            Origen = "AR",
                            Modulo = "PR",
                            Tipo = request.Tipo,

                            IdCaja = 0
                        };
                        var grabarCajaReintegro = VentaRepository.GrabarCaja(objCajaReintegro);

                        //Graba Auditoria luego de Liberar Asiento
                        var objAuditoria = new AuditoriaEntity
                        {
                            CodiUsuario = Convert.ToInt16(request.CodiUsuario),
                            NomUsuario = request.NomUsuario,
                            Tabla = "VENTA",
                            TipoMovimiento = "ANUL.REINTEGRO",
                            Boleto = String.Format("{0}{1}-{2}", request.Tipo, request.SerieBoleto.ToString().PadLeft(3, '0'), request.NumeBoleto.ToString().PadLeft(7, '0')),
                            NumeAsiento = "0",
                            NomOficina = request.NomOficina,
                            NomPuntoVenta = request.CodiPuntoVenta.PadLeft(3, '0'),
                            Pasajero = "",
                            FechaViaje = "01/01/1900",
                            HoraViaje = "",
                            NomDestino = "",
                            Precio = 0M,
                            Obs1 = "LIBERACION AL BOLETO : " + request.CodiEsca,
                            Obs2 = "TERMINAL : " + request.Terminal,
                            Obs3 = "",
                            Obs4 = "CAJERO AFECTA " + request.NomUsuario,
                            Obs5 = "NRO VALE SALIDA : " + generarCorrelativoAuxiliarReintegro
                        };
                        VentaRepository.GrabarAuditoria(objAuditoria);

                        //TODO: Falta implementar Usp_Tb_venta_AnulaReintegro_ele
                    }
                }
                return new Response<byte>(true, anularVentaReintegro, "Se anuló el reintegro correctamente", true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<byte>(false, 0, Message.MsgExcAnulaReintegro, false);
            }
        }
    }
}
