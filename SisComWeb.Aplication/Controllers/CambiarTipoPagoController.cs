using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [RoutePrefix("cTipoPago")]
    public class CambiarTipoPagoController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        readonly Usuario usuario = DataSession.UsuarioLogueado;

        [HttpPost]
        [Route("change-tipo-pago")]
        public async Task<ActionResult> CambiarTipoPago(CambiarTPagoRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\": " + request.IdVenta +
                                    ",\"NewTipoPago\": \"" + request.NewTipoPago + "\"" +
                                    ",\"OldTipoPago\": \"" + request.OldTipoPago + "\"" +
                                    ",\"NomNewTipoPago\": \"" + request.NomNewTipoPago + "\"" +
                                    ",\"Credito\": \"" + "0" + "\"" + //HC
                                    ",\"CodiEmpresa\": \"" + request.CodiEmpresa + "\"" +
                                    ",\"CodiTarjetaCredito\": \"" + request.CodiTarjetaCredito + "\"" +
                                    ",\"NumeTarjetaCredito\": \"" + request.NumeTarjetaCredito + "\"" +
                                    ",\"NomTarjetaCredito\": \"" + request.NomTarjetaCredito + "\"" +
                                    ",\"Nombre\": \"" + request.Nombre + "\"" +
                                    ",\"Tipo\": \"" + request.Tipo + "\"" +
                                    ",\"Serie\": \"" + request.Serie + "\"" +
                                    ",\"Numero\": \"" + request.Numero + "\"" +
                                    ",\"PrecioVenta\": \"" + request.PrecioVenta + "\"" +
                                    ",\"CodiDestino\": \"" + request.CodiDestino + "\"" +
                                    ",\"FechaViaje\": \"" + request.FechaViaje + "\"" +
                                    ",\"HoraViaje\": \"" + request.HoraViaje + "\"" +
                                    ",\"NumeAsiento\": \"" + request.NumeAsiento + "\"" +
                                    ",\"NombDestino\": \"" + request.NombDestino + "\"" +
                                    ",\"NomSucursal\": \"" + usuario.NomSucursal + "\"" +
                                    ",\"CodiOficina\": \"" + usuario.CodiSucursal + "\"" +
                                    ",\"NomUsuario\": \"" + usuario.Nombre + "\"" +
                                    ",\"CodiUsuario\": \"" + usuario.CodiUsuario + "\"" +
                                    ",\"CodiPuntoVenta\": \"" + usuario.CodiPuntoVenta + "\"" +                                    
                                "}";
                    HttpResponseMessage response = await client.PostAsync("CambiarTipoPago", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"],
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, string.Empty), JsonRequestBehavior.AllowGet);
            }
        }
    }
}