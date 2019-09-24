using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [SessionExpire]
    [RoutePrefix("notaCredito")]
    public class NotaCreditoController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        readonly Usuario usuario = DataSession.UsuarioLogueado;

        [HttpPost]
        [Route("consultaTipoTerminalElectronico")]
        public async Task<ActionResult> ConsultaTipoTerminalElectronico(int CodiEmpresa)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiTerminal\": " + usuario.Terminal +
                                    ",\"CodiEmpresa\" : " + CodiEmpresa +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultaTipoTerminalElectronico", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("listaClientesNC_Autocomplete")]
        public async Task<JsonResult> ListaClientesNC_Autocomplete(string TipoDocumento, string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(Value))
                    Value = "null";

                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaClientesNC_Autocomplete");
                    var _body = "{" +
                                    "\"TipoDocumento\" : \"" + TipoDocumento + "\"" +
                                    ",\"Value\" : \"" + Value + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ListaClientesNC_Autocomplete", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList(),
                    EsCorrecto = (bool)tmpResult.SelectToken("EsCorrecto")
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("listaDocumentosEmitidos")]
        public async Task<JsonResult> ListaDocumentosEmitidos(DocumentosEmitidosRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaDocumentosEmitidos");
                    var _body = "{" +
                                    "\"Ruc\" : \"" + request.Ruc + "\"" +
                                    ",\"FechaInicial\" : \"" + request.FechaInicial + "\"" +
                                    ",\"FechaFinal\" : \"" + request.FechaFinal + "\"" +
                                    ",\"Serie\" : " + request.Serie +
                                    ",\"Numero\" : " + request.Numero +
                                    ",\"CodiEmpresa\" : " + request.CodiEmpresa +
                                    ",\"Tipo\" : \"" + request.Tipo + "\"" +

                                    ",\"TipoDocumento\" : \"" + request.TipoDocumento + "\"" +
                                    ",\"TipoPasEnc\" : \"" + request.TipoPasEnc + "\"" +
                                    ",\"TipoNumDoc\" : \"" + request.TipoNumDoc + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ListaDocumentosEmitidos", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<DocumentoEmitidoNC>> res = new Response<List<DocumentoEmitidoNC>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new DocumentoEmitidoNC
                    {
                        NitCliente = (string)x["NitCliente"],
                        Fecha = (string)x["Fecha"],
                        IdVenta = (int)x["IdVenta"],
                        TpoDoc = (string)x["TpoDoc"],
                        Serie = (short)x["Serie"],
                        Numero = (int)x["Numero"],
                        CodiPuntoVenta = (short)x["CodiPuntoVenta"],
                        Total = (decimal)x["Total"],
                        Tipo = (string)x["Tipo"],
                        IngIgv = (string)x["IngIgv"],
                        ImpManifiesto = (string)x["ImpManifiesto"],

                        ColumnTipo = (string)x["ColumnTipo"],
                        ColumnNroDocumento = (string)x["ColumnNroDocumento"],
                        ImporteNC = string.Empty,
                        Plano = new bool()
                    }).ToList(),
                    EsCorrecto = (bool)tmpResult.SelectToken("EsCorrecto")
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<DocumentoEmitidoNC>>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }
    }
}
