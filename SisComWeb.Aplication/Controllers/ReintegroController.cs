using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [RoutePrefix("reintegro")]
    public class ReintegroController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        readonly Usuario usuario = DataSession.UsuarioLogueado;

        private static List<VentaRealizada> _listVentasRealizadas(JToken list)
        {
            List<VentaRealizada> lista = list.Select(x => new VentaRealizada
            {
                // Para la vista 'BoletosVendidos'
                NumeAsiento = (string)x["NumeAsiento"],
                BoletoCompleto = (string)x["BoletoCompleto"],
                // Para el método 'ConvertirVentaToBase64'
                IdVenta = (int)x["IdVenta"],
                NomTipVenta = (string)x["NomTipVenta"],
                BoletoTipo = (string)x["BoletoTipo"],
                BoletoSerie = (string)x["BoletoSerie"],
                BoletoNum = (string)x["BoletoNum"],
                CodDocumento = (string)x["CodDocumento"],
                EmisionFecha = (string)x["EmisionFecha"],
                EmisionHora = (string)x["EmisionHora"],
                CajeroCod = (short)x["CajeroCod"],
                CajeroNom = (string)x["CajeroNom"],
                PasNombreCom = (string)x["PasNombreCom"],
                PasRuc = (string)x["PasRuc"],
                PasRazSocial = (string)x["PasRazSocial"],
                PasDireccion = (string)x["PasDireccion"],
                NomOriPas = (string)x["NomOriPas"],
                NomDesPas = (string)x["NomDesPas"],
                DocTipo = (byte)x["DocTipo"],
                DocNumero = (string)x["DocNumero"],
                PrecioCan = (string)x["PrecioCan"],
                PrecioDes = (string)x["PrecioDes"],
                NomServicio = (string)x["NomServicio"],
                FechaViaje = (string)x["FechaViaje"],
                EmbarqueDir = (string)x["EmbarqueDir"],
                EmbarqueHora = (string)x["EmbarqueHora"],
                CodigoX_FE = (string)x["CodigoX_FE"],
                CodTerminal = (string)x["CodTerminal"],
                TipImpresora = (byte)x["TipImpresora"],
                CodX = (string)x["CodX"],

                EmpDirAgencia = (string)x["EmpDirAgencia"],
                EmpTelefono1 = (string)x["EmpTelefono1"],
                EmpTelefono2 = (string)x["EmpTelefono2"],
                PolizaNum = (string)x["PolizaNum"],
                PolizaFechaReg = (string)x["PolizaFechaReg"],
                PolizaFechaVen = (string)x["PolizaFechaVen"],

                // Parámetros extras
                EmpCodigo = (byte)x["EmpCodigo"],
                PVentaCodigo = (short)x["PVentaCodigo"],
                BusCodigo = (string)x["BusCodigo"],
                EmbarqueCod = (short)x["EmbarqueCod"]
            }).ToList();

            return lista;
        }

        // GET: Reintegro
        [HttpPost]
        [Route("save-reintegro")]
        public async Task<ActionResult> SaveReintegro(ReintegroVenta filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Serie\" : \"" + filtro.Serie + "\"" +
                                    ",\"nume_boleto\" : \"" + filtro.nume_boleto + "\"" +
                                    ",\"Codi_Empresa\" : \"" + filtro.Codi_Empresa + "\"" +
                                    ",\"CODI_SUCURSAL\" : \"" + filtro.CODI_SUCURSAL + "\"" +
                                    ",\"CODI_PROGRAMACION\" : \"" + filtro.CODI_PROGRAMACION + "\"" +
                                    ",\"CODI_SUBRUTA\" : \"" + filtro.CODI_SUBRUTA + "\"" +
                                    ",\"CODI_Cliente\" : \"" + filtro.CODI_Cliente + "\"" +
                                    ",\"NIT_CLIENTE\" : \"" + (filtro.NIT_CLIENTE ?? "") + "\"" +
                                    ",\"PRECIO_VENTA\" : \"" + filtro.PRECIO_VENTA + "\"" +
                                    ",\"NUMERO_ASIENTO\" : \"" + "00" + "\"" +
                                    ",\"FLAG_VENTA\" : \"" + filtro.FLAG_VENTA + "\"" +
                                    ",\"FECH_VENTA\" : \"" + filtro.FECH_VENTA + "\"" +
                                    ",\"Recoger\" : \"" + (filtro.Recoger ?? "") + "\"" +
                                    ",\"Clav_Usuario\" : \"" + filtro.Clav_Usuario + "\"" +
                                    ",\"Dni\" : \"" + filtro.Dni + "\"" +
                                    ",\"EDAD\" : \"" + filtro.EDAD + "\"" +
                                    ",\"TELEF\" : \"" + (filtro.TELEF ?? "") + "\"" +
                                    ",\"NOMB\" : \"" + filtro.NOMB + "\"" +
                                    ",\"porcentaje\" : \"" + filtro.porcentaje + "\"" +
                                    ",\"Codi_Esca\" : \"" + filtro.Codi_Esca + "\"" +
                                    ",\"tota_ruta1\" : \"" + filtro.tota_ruta1 + "\"" +
                                    ",\"tota_ruta2\" : \"" + filtro.tota_ruta2 + "\"" +
                                    ",\"Estado\" : \"" + " " + "\"" +
                                    ",\"Punto_Venta\" : \"" + filtro.Punto_Venta + "\"" +
                                    ",\"tipo_doc\" : \"" + filtro.tipo_doc + "\"" +
                                    ",\"codi_ori_psj\" : \"" + filtro.codi_ori_psj + "\"" +
                                    ",\"Tipo\" : \"" + filtro.Tipo + "\"" +
                                    ",\"per_autoriza\" : \"" + "1" + "\"" +
                                    ",\"Cod_Cliente\" : \"" + "0" + "\"" +
                                    ",\"estado_asiento\" : \"" + "N" + "\"" +
                                    ",\"SEXO\" : \"" + "M" + "\"" +
                                    ",\"Tipo_Pago\" : \"" + filtro.Tipo_Pago + "\"" +
                                    ",\"Vale_Remoto\" : \"" + "" + "\"" +
                                    ",\"Tipo_Venta\" : \"" + "N" + "\"" +
                                    ",\"Fecha_viaje\" : \"" + filtro.Fecha_viaje + "\"" +
                                    ",\"HORA_V\" : \"" + filtro.HORA_V + "\"" +
                                    ",\"nacionalidad\" : \"" + filtro.nacionalidad + "\"" +
                                    ",\"servicio\" : \"" + filtro.servicio + "\"" +
                                    ",\"Sube_en\" : \"" + filtro.Sube_en + "\"" +
                                    ",\"Baja_en\" : \"" + filtro.Baja_en + "\"" +
                                    ",\"Hora_Emb\" : \"" + filtro.Hora_Emb + "\"" +
                                    ",\"nivel\" : \"" + "1" + "\"" +
                                    ",\"Codi_Empresa__\" : \"" + filtro.Codi_Empresa__ + "\"" +
                                    ",\"CODI_SUCURSAL__\" : \"" + filtro.CODI_SUCURSAL__ + "\"" +
                                    ",\"CODI_TERMINAL__\" : \"" + filtro.CODI_TERMINAL__ + "\"" +
                                    ",\"Codi_Documento__\" : \"" + filtro.Codi_Documento__ + "\"" +
                                    ",\"NUME_CORRELATIVO__\" : \"" + filtro.NUME_CORRELATIVO__ + "\"" +
                                    ",\"fecha_venta__\" : \"" + filtro.fecha_venta__ + "\"" +
                                    ",\"Pventa__\" : \"" + filtro.Pventa__ + "\"" +
                                    ",\"SERIE_BOLETO__\" : \"" + filtro.SERIE_BOLETO__ + "\"" +
                                    ",\"Sw_IngManual\" : \"" + "E" + "\"" +
                                    ",\"stReintegro\" : \"" + (filtro.stReintegro ?? "") + "\"" +
                                    ",\"NomSucursal\" : \"" + usuario.NomSucursal + "\"" +
                                    ",\"NomMotivo\" : \"" + filtro.NomMotivo + "\"" +
                                    ",\"NombDestino\" : \"" + filtro.NombDestino + "\"" +
                                    ",\"CodiBus\" : \"" + filtro.CodiBus + "\"" +
                                    ",\"DirEmbarque\" : \"" + filtro.DirEmbarque + "\"" +
                                    ",\"NomServicio\" : \"" + filtro.NomServicio + "\"" +
                                    ",\"NomEmpresaRuc\" : \"" + (filtro.NomEmpresaRuc ?? "") + "\"" +
                                    ",\"DirEmpresaRuc\" : \"" + (filtro.DirEmpresaRuc ?? "") + "\"" +
                                    ",\"NomUsuario\" : \"" + filtro.NomUsuario + "\"" +
                                    ",\"NomOrigen\" : \"" + filtro.NomOrigen + "\"" +
                                    ",\"id_original\" : \"" + filtro.id_original + "\"" +
                                    ",\"CodMotivo\" : \"" + filtro.CodMotivo + "\"" +
                                    ",\"boleto_original\" : \"" + filtro.boleto_original + "\"" +
                                    ",\"D_DOCUMENTO2\" : \"" + filtro.D_DOCUMENTO2 + "\"" +
                                    ",\"T_DNI2\" : \"" + filtro.T_DNI2 + "\"" +
                                    ",\"NOMB2\" : \"" + filtro.NOMB2 + "\"" +
                                    ",\"TipoOri\" : \"" + (filtro.TipoOri ?? "0") + "\"" +
                                    ",\"CodiTarjetaCredito\" : \"" + (filtro.CodiTarjetaCredito ?? "") + "\"" +
                                    ",\"NumeTarjetaCredito\" : \"" + (filtro.NumeTarjetaCredito ?? "") + "\"" +
                                    ",\"NumAsientoAuditoria\" : \"" + filtro.NumAsientoAuditoria + "\"" +
                                    ",\"BoletoAuditoria\" : \"" + filtro.BoletoAuditoria + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("SaveReintegro", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<VentaResponse> res = new Response<VentaResponse>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new VentaResponse()
                    {
                        ListaVentasRealizadas = _listVentasRealizadas(data["ListaVentasRealizadas"]),
                        CodiProgramacion = (int)data["CodiProgramacion"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<int>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("verifica-igv")]
        public async Task<ActionResult> ConsultarIgv(string TipoDoc)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"TipoDoc\" : \"" + TipoDoc + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultarIgv", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<decimal> res = new Response<decimal>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (decimal)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<decimal>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("consulta-precio-ruta")]
        public async Task<ActionResult> ConsultarPrecioRuta(PrecioRuta filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiOrigen\" : " + filtro.CodiOrigen +
                                    ",\"CodiDestino\" : " + filtro.CodiDestino +
                                    ",\"HoraViaje\" : \"" + filtro.HoraViaje + "\"" +
                                    ",\"FechaViaje\" : \"" + filtro.FechaViaje + "\"" +
                                    ",\"CodiServicio\" : " + filtro.CodiServicio +
                                    ",\"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                    ",\"Nivel\" : \"" + filtro.Nivel + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultarPrecioRuta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Plano> res = new Response<Plano>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Plano
                    {
                        PrecioNormal = (decimal)data["PrecioNormal"],
                        PrecioMinimo = (decimal)data["PrecioMinimo"],
                        PrecioMaximo = (decimal)data["PrecioMaximo"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<decimal>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("update-reintegro")]
        public async Task<ActionResult> UpdateReintegro(int IdVenta, string Programacion, string Destino, string Asiento, string Origen)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\" : " + IdVenta + 
                                    ",\"Programacion\" : \"" + Programacion + "\"" +
                                    ",\"Destino\" : \"" + Destino + "\"" +
                                    ",\"Asiento\" : \"" + Asiento + "\"" +
                                    ",\"Origen\" : \"" + Origen + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("UpdateReintegro", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<decimal>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("verifica-reintegro-anular")]
        public async Task<ActionResult> ValidaReintegroParaAnualar(FiltroReintegro filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Serie\" : " + filtro.Serie +
                                    ",\"Numero\" : " + filtro.Numero +
                                    ",\"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                    ",\"Tipo\" : \"" + (filtro.Tipo ?? "") + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ValidaReintegroParaAnualar", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Reintegro> res = new Response<Reintegro>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Reintegro()
                    {
                        IdVenta = (int)data["IdVenta"],
                        CodiEsca = (string)data["CodiEsca"],
                        Sucursal = (int)data["Sucursal"],
                        PrecioVenta = (decimal)data["PrecioVenta"],
                        TipoPago = (string)data["TipoPago"],
                        ClavUsuario = (string)data["ClavUsuario"],
                        Tipo = (string)data["Tipo"],
                        RucCliente = (string)data["RucCliente"],
                        CodiDestino = (short)data["CodiDestino"],
                        SerieBoleto = (short)data["SerieBoleto"],
                        NumeBoleto = (int)data["NumeBoleto"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<decimal>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("reintegro-anular")]
        public async Task<ActionResult> ReintegroAnualar(AnularVentaRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\": " + request.IdVenta +
                                    ",\"CodiUsuario\": " + usuario.CodiUsuario +
                                    ",\"CodiOficina\": " + usuario.CodiSucursal +
                                    ",\"CodiPuntoVenta\": " + usuario.CodiPuntoVenta +
                                    ",\"NomUsuario\": \"" + usuario.Nombre + "\"" + 
                                    ",\"CodiEsca\": \"" + request.CodiEsca + "\"" +
                                    ",\"CodiDestinoPas\": \"" + request.CodiDestinoPas + "\"" +
                                    ",\"IngresoManualPasajes\": " + request.IngresoManualPasajes.ToString().ToLower() +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("AnularReintegro", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Reintegro> res = new Response<Reintegro>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Reintegro()
                    {
                        IdVenta = (int)data["IdVenta"],
                        CodiEsca = (string)data["CodiEsca"],
                        Sucursal = (int)data["Sucursal"],
                        PrecioVenta = (decimal)data["PrecioVenta"],
                        TipoPago = (string)data["TipoPago"],
                        ClavUsuario = (string)data["ClavUsuario"],
                        Tipo = (string)data["Tipo"],
                        RucCliente = (string)data["RucCliente"],
                        CodiDestino = (short)data["CodiDestino"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<decimal>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }
    }
}