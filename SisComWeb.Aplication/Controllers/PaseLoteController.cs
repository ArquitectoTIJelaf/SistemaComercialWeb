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
                                    "\"NumeroReintegro\" : \"" + string.Empty + "\"" + 
                                    ",\"CodiProgramacion\" : \"" + filtro.CodiProgramacion + "\"" +
                                    ",\"Origen\" : \"" + string.Empty + "\"" + 
                                    ",\"IdVenta\" : " + filtro.IdVenta +
                                    ",\"NumeAsiento\" : \"" + filtro.NumeAsiento + "\"" +
                                    ",\"Ruta\" : \"" + string.Empty + "\"" + 
                                    ",\"CodiServicio\" : \"" + filtro.CodiServicio + "\"" +
                                    ",\"TipoDoc\" : \"" + string.Empty + "\"" + 
                                    ",\"FechaViaje\" : \"" + filtro.FechaViaje + "\"" +
                                    ",\"HoraViaje\" : \"" + filtro.HoraViaje + "\"" +
                                    ",\"FlagVenta\" : \"" + filtro.FlagVenta + "\"" +
                                    ",\"CodiEsca\" : \"" + filtro.CodiEsca + "\"" +
                                    ",\"CodiEmpresa\" : \"" + filtro.CodiEmpresa + "\"" +
                                    ",\"CodiUsuario\" : \"" + usuario.CodiUsuario + "\"" +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" +
                                    ",\"Boleto\" : \"" + filtro.Boleto + "\"" +
                                    ",\"Pasajero\" : \"" + filtro.Pasajero + "\"" +
                                    ",\"NomSucursal\" : \"" + usuario.NomSucursal + "\"" +
                                    ",\"PuntoVenta\" : \"" + usuario.CodiPuntoVenta + "\"" +
                                    ",\"Terminal\" : \"" + usuario.Terminal + "\"" +
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
    }
}