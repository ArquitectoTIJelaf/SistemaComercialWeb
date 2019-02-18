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
        //private static string _defineColor(int CodProgramacion, int StOpcional) => (CodProgramacion != 0) ? "#A5D6A7" : (StOpcional == 1) ? "#FCD8AA" : "#FFFFFF";
        private static string _oneColor(bool ProgramacionCerrada, int AsientosVendidos, int CapacidadBus, string StOpcional)
        {
            var color = "";
            if (ProgramacionCerrada)
            {
                color = "#169BFF"; //Azul
            }
            else
            {
                if (AsientosVendidos == 0 && StOpcional.Equals("0"))
                {
                    color = "#FFFFFF"; //Blanco
                }
                else if (AsientosVendidos > 0 && AsientosVendidos < CapacidadBus && StOpcional.Equals("0"))
                {
                    color = "#A9E36A"; //Verde
                }
                else if (AsientosVendidos > 0 && AsientosVendidos == CapacidadBus && StOpcional.Equals("0"))
                {
                    color = "#E26B67"; //Rojo
                }
                else if (AsientosVendidos == 0 && StOpcional.Equals("1"))
                {
                    color = "#F7C06E"; //Naranja
                }
                else if (CapacidadBus == 0 && StOpcional.Equals("1"))
                {
                    color = "#F7C06E"; //Naranja
                }
                else if (AsientosVendidos > 0 && AsientosVendidos < CapacidadBus && StOpcional.Equals("1"))
                {
                    color = "#F7C06E"; //Naranja y Verde
                }
                else if (AsientosVendidos > 0 && AsientosVendidos == CapacidadBus && StOpcional.Equals("1"))
                {
                    color = "#F7C06E"; //Naranja y Rojo
                }
            }
            return color;
        }

        private static string _twoColor(int AsientosVendidos, int CapacidadBus, string StOpcional)
        {
            var color = "";
            if (AsientosVendidos > 0 && AsientosVendidos < CapacidadBus && StOpcional.Equals("1"))
            {
                color = "#A9E36A"; //Naranja y Verde
            }
            else if (AsientosVendidos > 0 && AsientosVendidos == CapacidadBus && StOpcional.Equals("1"))
            {
                color = "#E26B67"; //Naranja y Rojo
            }
            return color;
        }

        //private static List<Punto> _listPuntos(JToken list)
        //{
        //    List<Punto> itemsEmbarques = list.Select(x => new Punto
        //    {
        //        CodiPuntoVenta = (short)x["CodiPuntoVenta"],
        //        Lugar = (string)x["Lugar"],
        //        Hora = (string)x["Hora"]
        //    }).ToList();

        //    return itemsEmbarques;
        //}

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("lista-itinerarios")]
        public async Task<ActionResult> ChargeList(FiltroItinerario filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"CodiOrigen\" : " + filtro.CodiOrigen +
                                ",\"CodiDestino\" : " + filtro.CodiDestino +
                                ",\"CodiRuta\" : " + filtro.CodiRuta +
                                ",\"Hora\" : \"" + filtro.Hora.Replace(" ", "") + "\"" +
                                ",\"FechaViaje\" : \"" + filtro.FechaViaje + "\"" +
                                ",\"TodosTurnos\" : " + filtro.TodosTurnos.ToLower() +
                                ", \"SoloProgramados\" : " + filtro.SoloProgramados.ToLower() + " }";
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
                        StOpcional = (string)x["StOpcional"],
                        ProgramacionCerrada = (bool)x["ProgramacionCerrada"],
                        Color = _oneColor((bool)x["ProgramacionCerrada"], (int)x["AsientosVendidos"], (int)x["CapacidadBus"], (string)x["StOpcional"]),
                        SecondColor = _twoColor((int)x["AsientosVendidos"], (int)x["CapacidadBus"], (string)x["StOpcional"])/*,*/
                        //ListaArribos = _listPuntos(x["ListaArribos"]),
                        //ListaEmbarques = _listPuntos(x["ListaEmbarques"])
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

        [HttpPost]
        [Route("plano")]
        public async Task<ActionResult> ChargePlano(FiltroPlano filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"PlanoBus\" : \"" + filtro.PlanoBus + "\"" +
                                ",\"CodiProgramacion\" : " + filtro.CodiProgramacion +
                                ",\"CodiOrigen\" : " + filtro.CodiOrigen +
                                ",\"CodiDestino\" : " + filtro.CodiDestino +
                                ",\"CodiBus\" : \"" + filtro.CodiBus + "\"" +
                                ",\"HoraViaje\" : \"" + filtro.HoraViaje + "\"" +
                                ",\"FechaViaje\" : \"" + filtro.FechaViaje + "\"" +
                                ",\"CodiServicio\" : " + filtro.CodiServicio +
                                ",\"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                ",\"FechaProgramacion\" : \"" + filtro.FechaProgramacion + "\"" +
                                ", \"NroViaje\" : " + filtro.NroViaje + " }";
                    HttpResponseMessage response = await client.PostAsync("MuestraPlano", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    List<Plano> items = ((JArray)tmpResult["Valor"]).Select(x => new Plano
                    {
                        ApellidoMaterno = (string)x["ApellidoMaterno"],
                        ApellidoPaterno = (string)x["ApellidoPaterno"],
                        Codigo = (string)x["Codigo"],
                        Color = (long)x["Color"],
                        Edad = (int)x["Edad"],
                        FechaNacimiento = (string)x["FechaNacimiento"],
                        FechaVenta = (string)x["FechaVenta"],
                        FechaViaje = (string)x["FechaViaje"],
                        FlagVenta = (string)x["FlagVenta"],
                        Indice = (int)x["Indice"],
                        Nacionalidad = (string)x["Nacionalidad"],
                        Nivel = (int)x["Nivel"],
                        Nombres = (string)x["Nombres"],
                        NumeAsiento = (int)x["NumeAsiento"],
                        NumeroDocumento = (string)x["NumeroDocumento"],
                        PrecioMaximo = (int)x["PrecioMaximo"],
                        PrecioMinimo = (int)x["PrecioMinimo"],
                        PrecioNormal = (int)x["PrecioNormal"],
                        PrecioVenta = (int)x["PrecioVenta"],
                        RecogeEn = (string)x["RecogeEn"],
                        RucContacto = (string)x["RucContacto"],
                        Telefono = (string)x["Telefono"],
                        Tipo = (string)x["Tipo"],
                        TipoDocumento = (string)x["TipoDocumento"],
                        IsDisabled = false // Para 'BloqueoAsiento'
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

        [HttpPost]
        [Route("bloquearAsiento")]
        public async Task<ActionResult> BloquearAsiento(FiltroBloqueoAsiento filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"CodiProgramacion\" : " + filtro.CodiProgramacion +
                                ",\"NroViaje\" : " + filtro.NroViaje +
                                ",\"CodiOrigen\" : " + filtro.CodiOrigen +
                                ",\"CodiDestino\" : " + filtro.CodiDestino +
                                ",\"NumeAsiento\" : " + filtro.NumeAsiento +
                                ",\"FechaProgramacion\" : \"" + filtro.FechaProgramacion + "\"" +
                                ",\"Precio\" : " + filtro.Precio +
                                ",\"CodiTerminal\" : " + filtro.CodiTerminal + " }";
                    HttpResponseMessage response = await client.PostAsync("BloqueoAsiento", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    int valor = (int)tmpResult["Valor"];
                    return Json(valor, JsonRequestBehavior.AllowGet);
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