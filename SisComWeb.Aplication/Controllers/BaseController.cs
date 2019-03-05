using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [RoutePrefix("base")]
    public class BaseController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];

        #region FILTROS

        [HttpGet]
        [Route("get-oficinas")]
        public async Task<JsonResult> GetOficinas()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaOficinas");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaOficinas");
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-puntosventa")]
        public async Task<JsonResult> GetPuntosVenta(string CodiSucursal)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaPuntosVenta");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaPuntosVenta/" + CodiSucursal);
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-usuarios")]
        public async Task<JsonResult> GetUsuarios(string value)
        {
            try
            {
                if (value == string.Empty) { value = "NULL"; }

                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaUsuarios");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaUsuarios/" + value);
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-empresas")]
        public async Task<JsonResult> GetEmpresas()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaEmpresas");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaEmpresas");
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-servicios")]
        public async Task<JsonResult> GetServicios()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaServicios");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaServicios");


                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-tiposDoc")]
        public async Task<JsonResult> GetTiposDoc()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaTiposDoc");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaTiposDoc");
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-tipoPago")]
        public async Task<JsonResult> GetTipoPago()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaTipoPago");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaTipoPago");
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-tarjetasCredito")]
        public async Task<JsonResult> GetTarjetasCredito()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaTarjetaCredito");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaTarjetaCredito");
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-cuidades")]
        public async Task<JsonResult> GetCuidades()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaCiudad");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaCiudad");
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-parentesco")]
        public async Task<JsonResult> GetParentesco()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListarParentesco");
                    HttpResponseMessage response = await client.GetAsync(url + "ListarParentesco");
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                JArray data = (JArray)tmpResult["Valor"];
                List<Base> items = data.Select(x => new Base
                {
                    id = (string)x["id"],
                    label = (string)x["label"]
                }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}