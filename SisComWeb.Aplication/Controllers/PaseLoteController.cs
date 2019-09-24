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
using System.Xml;
using System.Xml.Serialization;

namespace SisComWeb.Aplication.Controllers
{
    [SessionExpire]
    [RoutePrefix("paseLote")]
    public class PaseLoteController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        readonly Usuario usuario = DataSession.UsuarioLogueado;

        [HttpPost]
        [Route("update-postergacion")]
        public async Task<ActionResult> UpdatePostergacion(List<FiltroPaseLote> list)
        {
            try
            {
                string result = string.Empty;

                XmlSerializer ser = new XmlSerializer(typeof(List<FiltroPaseLote>), new XmlRootAttribute("PaseLoteList"));
                StringBuilder sb = new StringBuilder();
                using (XmlWriter xml = XmlWriter.Create(sb))
                {
                    ser.Serialize(xml, list);
                }
                string cadxml = sb.ToString();

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Lista\" : \"" + cadxml.Replace('"', '\'') + "\"" +
                                    ",\"CodiUsuario\" : \"" + usuario.CodiUsuario + "\"" +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" +
                                    ",\"NomSucursal\" : \"" + usuario.NomSucursal + "\"" +
                                    ",\"PuntoVenta\" : \"" + usuario.CodiPuntoVenta + "\"" +
                                    ",\"Terminal\" : \"" + usuario.Terminal + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("UpdatePostergacion", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<PaseLote>> res = new Response<List<PaseLote>>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new PaseLote
                    {
                        Boleto = (string)x["Boleto"],
                        NumeAsiento = (string)x["NumeAsiento"],
                        Pasajero = (string)x["Pasajero"],
                        FechaViaje = (string)x["FechaViaje"],
                        HoraViaje = (string)x["HoraViaje"],
                        IdVenta = (int)x["IdVenta"],
                        CodiProgramacion = (string)x["CodiProgramacion"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<decimal>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("validar-manifiesto")]
        public async Task<ActionResult> ValidarManifiesto(int CodiProgramacion, int CodiSucursal)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiProgramacion\" : " + CodiProgramacion +
                                    ",\"CodiSucursal\" : " + CodiSucursal +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ValidarManifiesto", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }
                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>
                {
                    EsCorrecto = (bool)tmpResult["EsCorrecto"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"],
                    Estado = (bool)tmpResult["Estado"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, "", false), JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [Route("bloquearAsientoList")]
        public async Task<ActionResult> bloquearAsientoList(FiltroBloqueoAsiento filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiProgramacion\" : " + filtro.CodiProgramacion +
                                    ",\"NroViaje\" : " + filtro.NroViaje +
                                    ",\"CodiOrigen\" : " + filtro.CodiOrigen +
                                    ",\"CodiDestino\" : " + filtro.CodiDestino +
                                    ",\"NumeAsientos\" : [ " + String.Join(", ", filtro.NumeAsientos.ToArray()) + " ]" +
                                    ",\"FechaProgramacion\" : \"" + filtro.FechaProgramacion + "\"" +
                                    ",\"Precio\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(filtro.Precio) + "\"" +
                                    ",\"CodiTerminal\" : " + int.Parse(usuario.Terminal.ToString("D3")) +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BloqueoAsientoList", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<int>> res = new Response<List<int>>
                {
                    EsCorrecto = (bool)tmpResult["EsCorrecto"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = tmpResult["Valor"].ToObject<List<int>>(),
                    Estado = (bool)tmpResult["Estado"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<int>>(false, Constant.EXCEPCION, new List<int>(), false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("desbloquearAsientoList")]
        public async Task<ActionResult> desbloquearAsientoList(int CodiProgramacion)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiProgramacion\" : " + CodiProgramacion +
                                    ",\"CodiTerminal\" : \"" + usuario.Terminal + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("DesbloquearAsientosList", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<int>> res = new Response<List<int>>
                {
                    EsCorrecto = (bool)tmpResult["EsCorrecto"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = tmpResult["Valor"].ToObject<List<int>>(),
                    Estado = (bool)tmpResult["Estado"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<int>>(false, Constant.EXCEPCION, new List<int>(), false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("crearProgramacion")]
        public async Task<ActionResult> CrearProgramacion(FiltroBloqueoAsiento filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiProgramacion\" : " + filtro.CodiProgramacion +
                                    ",\"NroViaje\" : " + filtro.NroViaje +
                                    ",\"CodiOrigen\" : " + filtro.CodiOrigen +
                                    ",\"CodiDestino\" : " + filtro.CodiDestino +
                                    ",\"NumeAsientos\" : [ " + String.Join(", ", filtro.NumeAsientos.ToArray()) + " ]" +
                                    ",\"FechaProgramacion\" : \"" + filtro.FechaProgramacion + "\"" +
                                    ",\"Precio\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(filtro.Precio) + "\"" +
                                    ",\"CodiTerminal\" : " + int.Parse(usuario.Terminal.ToString("D3")) +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("CrearProgramacion", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<int>> res = new Response<List<int>>
                {
                    EsCorrecto = (bool)tmpResult["EsCorrecto"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = tmpResult["Valor"].ToObject<List<int>>(),
                    Estado = (bool)tmpResult["Estado"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<int>>(false, Constant.EXCEPCION, new List<int>(), false), JsonRequestBehavior.AllowGet);
            }
        }
    }
}