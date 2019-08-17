using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [RoutePrefix("paseLote")]
    public class PaseLoteController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        readonly Usuario usuario = DataSession.UsuarioLogueado;

        [HttpPost]
        [Route("update-postergacion")]
        public async Task<ActionResult> UpdatePostergacion(FiltroPaseLote filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"NumeroReintegro\" : \"" + "" + "\"" +//
                                    ",\"CodiProgramacion\" : \"" + filtro.CodiProgramacion + "\"" +
                                    ",\"Origen\" : \"" + "" + "\"" +//
                                    ",\"IdVenta\" : " + filtro.IdVenta +
                                    ",\"NumeAsiento\" : \"" + filtro.NumeAsiento + "\"" +
                                    ",\"Ruta\" : \"" + "" + "\"" +//
                                    ",\"CodiServicio\" : \"" + filtro.CodiServicio + "\"" +
                                    ",\"TipoDoc\" : \"" + "" + "\"" +
                                    ",\"FechaViaje\" : \"" + filtro.FechaViaje + "\"" +
                                    ",\"HoraViaje\" : \"" + filtro.HoraViaje + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("UpdatePostergacion", new StringContent(_body, Encoding.UTF8, "application/json"));
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

        [HttpGet]
        [Route("update-programacion")]
        public async Task<ActionResult> UpdateProgramacion(int CodiProgramacion, int IdVenta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"NumeroReintegro\" : " + CodiProgramacion +
                                    ",\"CodiProgramacion\" : " + IdVenta + 
                                "}";
                    HttpResponseMessage response = await client.PostAsync("UpdateProgramacion", new StringContent(_body, Encoding.UTF8, "application/json"));
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
    }
}