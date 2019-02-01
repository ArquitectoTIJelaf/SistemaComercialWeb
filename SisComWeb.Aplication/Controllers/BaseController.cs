using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                    HttpResponseMessage response = await client.GetAsync(url + "ListaPuntosVenta/"+ CodiSucursal);
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
        public async Task<JsonResult> GetUsuarios(string CodiPuntoVenta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaUsuarios");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaUsuarios/" + CodiPuntoVenta);
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