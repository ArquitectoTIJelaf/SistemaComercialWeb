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
    [RoutePrefix("itinerarios")]
    public class ItinerariosController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];

        [Route("")]
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
                var auxHora = Hora.Replace(" ", "");

                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"CodiOrigen\" : " + CodiOrigen +
                                ",\"CodiDestino\" : " + CodiDestino +
                                ",\"CodiRuta\" : " + CodiRuta +
                                ",\"Hora\" : \"" + auxHora + "\"" +
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
                    List<Itinerario> items = ((JArray)tmpResult["Valor"]).Select(x => new Itinerario
                    {
                        AsientosVendidos = (int)x["AsientosVendidos"],
                        CapacidadBus = (string)x["CapacidadBus"],
                        CodiBus = (string)x["CodiBus"],
                        CodiDestino = (int)x["CodiDestino"],
                        CodiEmpresa = (int)x["CodiEmpresa"],
                        CodiOrigen = (int)x["CodiOrigen"],
                        CodiProgramacion = (int)x["CodiProgramacion"],
                        CodiPuntoVenta = (int)x["CodiPuntoVenta"],
                        CodiRuta = (int)x["CodiRuta"],
                        CodiServicio = (int)x["CodiServicio"],
                        CodiSucursal = (int)x["CodiSucursal"],
                        Dias = (int)x["Dias"],
                        FechaProgramacion = (string)x["FechaProgramacion"],
                        HoraPartida = (string)x["HoraPartida"],
                        HoraProgramacion = (string)x["HoraProgramacion"],
                        NomDestino = (string)x["NomDestino"],
                        NomOrigen = (string)x["NomOrigen"],
                        NomPuntoVenta = (string)x["NomPuntoVenta"],
                        NomRuta = (string)x["NomRuta"],
                        NomServicio = (string)x["NomServicio"],
                        NomSucursal = (string)x["NomSucursal"],
                        NroRuta = (int)x["NroRuta"],
                        NroRutaInt = (long)x["NroRutaInt"],
                        NroViaje = (long)x["NroViaje"],
                        PlacaBus = (string)x["PlacaBus"],
                        PlanoBus = (string)x["PlanoBus"],
                        RazonSocial = (string)x["RazonSocial"],
                        StOpcional = (string)x["StOpcional"]
                    }).ToList();
                    return Json(items, JsonRequestBehavior.AllowGet);
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