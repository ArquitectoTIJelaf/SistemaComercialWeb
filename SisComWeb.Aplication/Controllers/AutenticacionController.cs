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
                    var _body = "{" +
                                    "\"CodiUsuario\" : " + Codigo +
                                    ", \"Password\" : \"" + Clave + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ValidaUsuario", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Usuario> res = new Response<Usuario>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Usuario
                    {
                        CodiEmpresa = (int)data["CodiEmpresa"],
                        CodiPuntoVenta = (int)data["CodiPuntoVenta"],
                        CodiSucursal = (int)data["CodiSucursal"],
                        CodiUsuario = (int)data["CodiUsuario"],
                        Nombre = (string)data["Login"],
                        Nivel = (int)data["Nivel"],
                        NomSucursal = (string)data["NomSucursal"],
                        NomPuntoVenta = (string)data["NomPuntoVenta"],
                        Terminal = (int)data["Terminal"]
                    }
                };

                if (res.Estado)
                {
                    // Caso: JLMrootCode
                    if (res.Valor.CodiUsuario == int.Parse(JLMrootCode))
                    {
                        res.Valor.CodiSucursal = int.Parse(Sucursal);
                        res.Valor.CodiPuntoVenta = int.Parse(PuntoVenta);
                        res.Valor.NomSucursal = NomSucursal;
                        res.Valor.NomPuntoVenta = NomPuntoVenta;
                    }

                    DataSession.UsuarioLogueado = res.Valor;
                }

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Usuario>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
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
