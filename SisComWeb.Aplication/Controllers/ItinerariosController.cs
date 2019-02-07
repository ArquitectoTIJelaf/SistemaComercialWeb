using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [RoutePrefix("itinerarios")]
    public class ItinerariosController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        // GET: Itinerarios
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("lista-itinerarios")]
        public async Task<ActionResult> ChargeList(int CodiOrigen, int CodiDestino, int CodiRuta, string Hora, string FechaViaje, string TodosTurnos, string SoloProgramados)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"CodiOrigen\" : " + CodiOrigen +
                                ",\"CodiDestino\" : " + CodiDestino +
                                ",\"CodiRuta\" : " + CodiRuta +
                                ",\"Hora\" : \"" + Hora + "\"" +
                                ",\"FechaViaje\" : \"" + FechaViaje + "\"" +
                                ",\"TodosTurnos\" : " + TodosTurnos.ToLower() +
                                ", \"SoloProgramados\" : " + SoloProgramados.ToLower() + " }";
                    HttpResponseMessage response = await client.PostAsync("BuscaItinerarios", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    JArray data = (JArray)tmpResult["Valor"];
                    return Json(data, JsonRequestBehavior.AllowGet);
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
    }
}