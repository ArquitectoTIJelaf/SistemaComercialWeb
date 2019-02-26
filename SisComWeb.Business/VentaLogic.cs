﻿using SisComWeb.Business.ServiceFE;
using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Text;

namespace SisComWeb.Business
{
    public static class VentaLogic
    {
        //public static Response<CorrelativoEntity> BuscaCorrelativo(byte CodiEmpresa, short CodiSucursal, short CodiPuntoVenta, short CodiTerminal)
        //{
        //    try
        //    {
        //        var response = new Response<string>(false, null, "Error: BuscaCorrelativo.", false);

                
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        return new Response<CorrelativoEntity>(false, null, Message.MsgErrExcBuscaCorrelativo, false);
        //    }
        //}

        public static Response<string> GrabaVenta(VentaEntity entidad)
        {
            try
            {
                var response = new Response<string>(false, null, "Error: GrabaVenta.", false);
                entidad.UserWebSUNAT = "WEBPASAJES";

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

                // Busca 'Correlativo'
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
                //    SetInvoiceRequestBody bodyDocumentoSUNAT = null;

                //    // Valida 'DocumentoSUNAT'
                //    var resValidarDocumentoSUNAT = ValidarDocumentoSUNAT(entidad, ref bodyDocumentoSUNAT);
                //    if (resValidarDocumentoSUNAT.Estado && resValidarDocumentoSUNAT != null)
                //    {
                //        if (resGrabarVenta.Valor > 0)
                //        {
                //            // Actualiza 'NumBoleto'
                //            bodyDocumentoSUNAT.CInvoice = generarCabecera(entidad);

                //            // Registra 'DocumentoSUNAT'
                //            var resRegistrarDocumentoSUNAT = RegistrarDocumentoSUNAT(bodyDocumentoSUNAT);
                //            if (!resRegistrarDocumentoSUNAT.Estado)
                //            {
                //                response.Mensaje = resRegistrarDocumentoSUNAT.MensajeError;
                //                return response;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        response.Mensaje = resValidarDocumentoSUNAT.MensajeError;
                //        return response;
                //    }
                //}

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

        #region FACTURACIÓN ELETRÓNICA

        public static ResponseW ValidarDocumentoSUNAT(VentaEntity entidad, ref SetInvoiceRequestBody bodyDocumentoSUNAT)
        {
            try
            {
                Ws_SeeFacteSoapClient serviceFE = new Ws_SeeFacteSoapClient();
                SetInvoiceRequestBody entidadFE = new SetInvoiceRequestBody();
                Security seguridadFE = new Security
                {
                    ID = entidad.RucCliente,
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
                    if (entidad.TipoDocumento == "1")
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
                    sb = sb.Replace("[RazonNombres]", entidad.NomEmpresa);
                    sb = sb.Replace("[RazonComercial]", string.Empty);
                    sb = sb.Replace("[DireccionFiscal]", entidad.DirEmpresa);
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
                sb = sb.Replace("[Serie]", entidad.SerieBoleto.ToString());
                sb = sb.Replace("[Numero]", entidad.NumeBoleto.ToString("0######"));
                sb = sb.Replace("[FecEmision]", DateTime.Now.ToString("dd/MM/yyyy"));
                sb = sb.Replace("[HoraEmision]", DateTime.Now.ToString("HH:mm:ss"));
                sb = sb.Replace("[TMoneda]", "PEN");
                sb = sb.Replace("[ImporteTotalVenta]", entidad.PrecioVenta.ToString("F2"));
                sb = sb.Replace("[EnviarEmail]", "");
                sb = sb.Replace("[CorreoCliente]", string.Empty);
                sb = sb.Replace("[TipoCambio]", string.Empty);
                sb = sb.Replace("[TValorOperacionGravada]", "0.00");
                sb = sb.Replace("[TValorOperacionInafecta]", entidad.PrecioVenta.ToString("F2"));
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
                sb = sb.Replace("[MontoLetras]", DataUtility.MontoSolesALetras(entidad.PrecioVenta.ToString("F2")));
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
                sb = sb.Replace("[ValorUnitario]", entidad.PrecioVenta.ToString("F2"));
                sb = sb.Replace("[PrecioVenta]", entidad.PrecioVenta.ToString("F2"));
                sb = sb.Replace("[CodAFIgvxItem]", "10");
                sb = sb.Replace("[AFIgvxItem]", "0.00");
                sb = sb.Replace("[CodAFIscxItem]", "02");
                sb = sb.Replace("[AFIscxItem]", "0.00");
                sb = sb.Replace("[AFOtroxItem]", "0.00");
                sb = sb.Replace("[ValorVenta]", entidad.PrecioVenta.ToString("F2"));
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
