using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [RoutePrefix("login")]
    public class AutenticacionController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        readonly string JLMrootCode = System.Configuration.ConfigurationManager.AppSettings["JLMrootCode"].ToString();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("redirect")]
        public ActionResult Redirect()
        {

            return RedirectToAction("Index", "Principal");
        }

        [HttpPost]
        [Route("post-usuario")]
        public async Task<ActionResult> POST(short Codigo, string Clave, string Sucursal, string PuntoVenta, string NomSucursal, string NomPuntoVenta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"CodiUsuario\" : " + Codigo + ", \"Password\" : \"" + Clave + "\" }";
                    HttpResponseMessage response = await client.PostAsync("ValidaUsuario", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                if (estado)
                {
                    JObject data = (JObject)tmpResult["Valor"];
                    Usuario user = new Usuario
                    {
                        CodiEmpresa = (int)data["CodiEmpresa"],
                        CodiPuntoVenta = (int)data["CodiPuntoVenta"],
                        CodiSucursal = (int)data["CodiSucursal"],
                        CodiUsuario = (int)data["CodiUsuario"],
                        Nombre = (string)data["Login"],
                        Nivel = (int)data["Nivel"],
                        NomSucursal = (string)data["NomSucursal"],
                        NomPuntoVenta = (string)data["NomPuntoVenta"]
                    };

                    // Caso: JLMrootCode
                    if (user.CodiUsuario == int.Parse(JLMrootCode))
                    {
                        user.CodiSucursal = int.Parse(Sucursal);
                        user.CodiPuntoVenta = int.Parse(PuntoVenta);
                        user.NomSucursal = NomSucursal;
                        user.NomPuntoVenta = NomPuntoVenta;
                    }

                    DataSession.UsuarioLogueado = user;
                    return Json(NotifyJson.BuildJson(KindOfNotify.Informativo, "Log success"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, mensaje), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(NotifyJson.BuildJson(KindOfNotify.Advertencia, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            DataSession.UsuarioLogueado = null;
            return RedirectToAction("Index", "Autenticacion");
        }

        public ActionResult AjaxSessionExpired()
        {
            Response.StatusCode = 403;
            var dict = new Dictionary<string, object> { { "CustomErrorCode", 5001 } };
            return Content(JsonConvert.SerializeObject(dict), "application/json");
        }
    }
}
