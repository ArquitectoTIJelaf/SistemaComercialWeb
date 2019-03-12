﻿using SisComWeb.Business.ServiceFE;
using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Response;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SisComWeb.Business
{
    public static class PaseLogic
    {
        public static Response<string> GrabaPase(List<PaseEntity> listado)
        {
            try
            {
                var response = new Response<string>(false, null, "Error: GrabaPase.", false);
                string valor = string.Empty;

                foreach (var entidad in listado)
                {
                    string auxCodigoBF_Interno = string.Empty;
                    string auxBoletoCompleto = string.Empty;
                    string auxNumeCaja = string.Empty;
                    entidad.UserWebSUNAT = "WEBPASAJES";

                    Response<int> resGrabarVentaFechaAbierta = null;
                    Response<int> resGrabarVenta = null;

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
                    }
                    else
                    {
                        response.Mensaje = resValidarTerminalElectronico.Mensaje;
                        return response;
                    }

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

                    // Seteo 'CodiDocumento'
                    if (!string.IsNullOrEmpty(entidad.RucCliente))
                    {
                        // Seteo 'CodiBF Interno'
                        auxCodigoBF_Interno = "78";
                        // Seteo 'CodiDocumento'
                        entidad.CodiDocumento = "01"; // Factura
                    }
                    else
                    {
                        // Seteo 'CodiBF Interno'
                        auxCodigoBF_Interno = "77";
                        // Seteo 'CodiDocumento'
                        entidad.CodiDocumento = "03"; // Boleta
                    }

                    // Busca 'Correlativo'
                    var resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, auxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.Tipo);
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
                        switch (entidad.Tipo)
                        {
                            case "M":
                                {
                                    switch (entidad.CodiDocumento)
                                    {
                                        case "01": // Factura
                                            {
                                                // Seteo 'CodiBF Interno'
                                                auxCodigoBF_Interno = "77";
                                                // Seteo 'CodiDocumento'
                                                entidad.CodiDocumento = "03"; // Boleta

                                                // Busca 'Correlativo'
                                                resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, auxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.Tipo);
                                                if (resBuscarCorrelativo.Estado)
                                                {
                                                    if (resBuscarCorrelativo.Valor.SerieBoleto == 0)
                                                    {
                                                        // Seteo 'CodiBF Interno'
                                                        auxCodigoBF_Interno = "16";
                                                        resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, auxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.Tipo);
                                                        if (resBuscarCorrelativo.Estado)
                                                        {
                                                            if (resBuscarCorrelativo.Valor.SerieBoleto == 0)
                                                            {
                                                                response.Mensaje = "Error: SerieBoleto o NumeBoleto nulo.";
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
                                                }
                                                else
                                                {
                                                    response.Mensaje = resBuscarCorrelativo.Mensaje;
                                                    return response;
                                                }

                                                break;
                                            }
                                        case "03": // Boleta
                                            {
                                                // Seteo 'CodiBF Interno'
                                                auxCodigoBF_Interno = "16";
                                                resBuscarCorrelativo = VentaRepository.BuscarCorrelativo(entidad.CodiEmpresa, auxCodigoBF_Interno, entidad.CodiOficina, entidad.CodiPuntoVenta, entidad.CodiTerminal, entidad.Tipo);
                                                if (resBuscarCorrelativo.Estado)
                                                {
                                                    if (resBuscarCorrelativo.Valor.SerieBoleto == 0)
                                                    {
                                                        response.Mensaje = "Error: SerieBoleto o NumeBoleto nulo.";
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

                                                break;
                                            }
                                    }

                                    break;
                                }
                            case "E":
                                {
                                    response.Mensaje = "Error: SerieBoleto o NumeBoleto nulo.";
                                    return response;
                                }
                        }
                    }

                    // Valida 'FechaAbierta'
                    if (entidad.FechaAbierta)
                    {
                        // Graba 'VentaFechaAbierta'
                        resGrabarVentaFechaAbierta = PaseRepository.GrabarVentaFechaAbierta(entidad);
                        if (resGrabarVentaFechaAbierta.Estado)
                        {
                            entidad.IdVenta = resGrabarVentaFechaAbierta.Valor;
                        }
                        else
                        {
                            response.Mensaje = resGrabarVentaFechaAbierta.Mensaje;
                            return response;
                        }
                    }
                    else
                    {
                        // Graba 'Venta'
                        resGrabarVenta = VentaRepository.GrabarVenta(entidad);
                        if (resGrabarVenta.Estado)
                        {
                            entidad.IdVenta = resGrabarVenta.Valor;
                        }
                        else
                        {
                            response.Mensaje = resGrabarVenta.Mensaje;
                            return response;
                        }
                    }

                    // Graba 'Acompañante'
                    if (!string.IsNullOrEmpty(entidad.ObjAcompañante.TipoDocumento)
                        && !string.IsNullOrEmpty(entidad.ObjAcompañante.NumeroDocumento))
                    {
                        var resGrabarAcompañanteVenta = VentaRepository.GrabarAcompañanteVenta(entidad.IdVenta, entidad.ObjAcompañante);
                        if (!resGrabarAcompañanteVenta.Estado)
                        {
                            response.Mensaje = resGrabarAcompañanteVenta.Mensaje;
                            return response;
                        }
                    }

                    // Seteo 'auxBoletoCompleto'
                    auxBoletoCompleto = (entidad.Tipo == "M" ? "" : entidad.Tipo) + entidad.SerieBoleto.ToString("D3").ToString() + "-" + entidad.NumeBoleto.ToString("D7").ToString();

                    // Graba 'Facturacion Electrónica'
                    if (resValidarTerminalElectronico.Valor.Tipo == "E")
                    {
                        SetInvoiceRequestBody bodyDocumentoSUNAT = null;

                        // Valida 'DocumentoSUNAT'
                        var resValidarDocumentoSUNAT = ValidarDocumentoSUNAT(entidad, ref bodyDocumentoSUNAT);
                        if (resValidarDocumentoSUNAT.Estado && resValidarDocumentoSUNAT != null)
                        {
                            if (resGrabarVenta.Valor > 0 || resGrabarVentaFechaAbierta.Valor > 0)
                            {
                                // Actualiza 'NumBoleto'
                                bodyDocumentoSUNAT.CInvoice = generarCabecera(entidad);

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
                                response.Mensaje = resValidarDocumentoSUNAT.MensajeError;
                                return response;
                            }
                        }
                        else
                        {
                            response.Mensaje = resValidarDocumentoSUNAT.MensajeError;
                            return response;
                        }
                    }

                    // Actualiza 'ModificarSaldoPaseCortesia'
                    var resModificarSaldoPaseCortesia = PaseRepository.ModificarSaldoPaseCortesia(entidad.CodiSocio, entidad.Mes, entidad.Año);
                    if (!resModificarSaldoPaseCortesia.Estado)
                    {
                        response.Mensaje = resModificarSaldoPaseCortesia.Mensaje;
                        return response;
                    }

                    // Actualiza 'ModificarSaldoPaseCortesia'
                    var resGrabarPaseSocio = PaseRepository.GrabarPaseSocio(entidad.IdVenta, entidad.CodiGerente, entidad.CodiSocio, entidad.Observacion);
                    if (!resGrabarPaseSocio.Estado)
                    {
                        response.Mensaje = resGrabarPaseSocio.Mensaje;
                        return response;
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
                    valor += auxBoletoCompleto + ", ";
                }

                response.EsCorrecto = true;
                response.Valor = valor;
                response.Mensaje = Message.MsgCorrectoGrabaVenta;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrExcGrabaPase, false);
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
                response.Valor.Beneficiarios = ListaBeneficiario;

                response.EsCorrecto = true;
                response.Valor = response.Valor;
                response.Mensaje = Message.MsgErrCorrectoBeneficiarioPase;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PaseCortesiaResponse>(false, null, Message.MsgErrExcBeneficiarioPase, false);
            }
        }

        #region FACTURACIÓN ELETRÓNICA

        public static ResponseW ValidarDocumentoSUNAT(PaseEntity entidad, ref SetInvoiceRequestBody bodyDocumentoSUNAT)
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
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }

        }

        public static string GenerarPersona(PaseEntity entidad)
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
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static string generarCabecera(PaseEntity entidad)
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
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static ArrayOfString GenerarDetalle(PaseEntity entidad)
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
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static ArrayOfString GenerarAdicionales(PaseEntity entidad)
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
                Log.Instance(typeof(PaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }

        }

        #endregion
    }
}
