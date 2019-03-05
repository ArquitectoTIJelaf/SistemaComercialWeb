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
        private static readonly string CodiTerminal = System.Configuration.ConfigurationManager.AppSettings["CodiTerminal"];
        readonly Usuario usuario = DataSession.UsuarioLogueado;


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

        private static List<Punto> _listPuntos(JToken list)
        {
            List<Punto> lista = list.Select(x => new Punto
            {
                CodiPuntoVenta = (short)x["CodiPuntoVenta"],
                Lugar = (string)x["Lugar"],
                Hora = (string)x["Hora"]
            }).ToList();

            return lista;
        }

        private static List<Plano> _ListaPlanoBus(JToken list)
        {
            List<Plano> lista = list.Select(x => new Plano
            {
                ApellidoMaterno = (string)x["ApellidoMaterno"],
                ApellidoPaterno = (string)x["ApellidoPaterno"],
                Codigo = (string)x["Codigo"],
                Color = (string)x["Color"],
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
                Sexo = (string)x["Sexo"],
                Sigla = (string)x["Sigla"]
            }).ToList();
            return lista;
        }
        
        private static List<DestinoRuta> _ListaDestinosRuta(JToken list)
        {
            List<DestinoRuta> lista = list.Select(x => new DestinoRuta
            {
                CodiSucursal = (short)x["CodiSucursal"],
                NomOficina = (string)x["NomOficina"],
                Sigla = (string)x["Sigla"],
                Color = (string)x["Color"]
            }).ToList();

            return lista;
        }

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
                        SecondColor = _twoColor((int)x["AsientosVendidos"], (int)x["CapacidadBus"], (string)x["StOpcional"])
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
        [Route("turnos")]
        public async Task<ActionResult> ChargeTurnos(FiltroTurno filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                ",\"CodiOrigen\" : " + filtro.CodiOrigen +
                                ",\"CodiDestino\" : " + filtro.CodiDestino +
                                ",\"CodiSucursal\" : " + filtro.CodiSucursal +
                                ",\"CodiRuta\" : " + filtro.CodiRuta +
                                ",\"CodiPuntoVenta\" : " + filtro.CodiPuntoVenta +
                                ",\"CodiServicio\" : " + filtro.CodiServicio +
                                ",\"HoraViaje\" :  \"" + filtro.HoraViaje + "\"" +
                                ",\"FechaViaje\" :  \"" + filtro.FechaViaje + "\"" + " }";
                    HttpResponseMessage response = await client.PostAsync("MuestraTurno", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    Itinerario item = new Itinerario
                    {
                        AsientosVendidos = (int)data["AsientosVendidos"],
                        CapacidadBus = (string)data["CapacidadBus"],
                        CodiBus = (string)data["CodiBus"],
                        CodiDestino = (int)data["CodiDestino"],
                        CodiEmpresa = (int)data["CodiEmpresa"],
                        CodiOrigen = (int)data["CodiOrigen"],
                        CodiProgramacion = (int)data["CodiProgramacion"],
                        CodiPuntoVenta = (int)data["CodiPuntoVenta"],
                        CodiRuta = (int)data["CodiRuta"],
                        CodiServicio = (int)data["CodiServicio"],
                        CodiSucursal = (int)data["CodiSucursal"],
                        Dias = (int)data["Dias"],
                        FechaProgramacion = (string)data["FechaProgramacion"],
                        HoraPartida = (string)data["HoraPartida"],
                        HoraProgramacion = (string)data["HoraProgramacion"],
                        ListaArribos = _listPuntos(data["ListaArribos"]),
                        ListaEmbarques = _listPuntos(data["ListaEmbarques"]),
                        ListaPlanoBus = _ListaPlanoBus(data["ListaPlanoBus"]),
                        NomDestino = (string)data["NomDestino"],
                        NomOrigen = (string)data["NomOrigen"],
                        NomPuntoVenta = (string)data["NomPuntoVenta"],
                        NomRuta = (string)data["NomRuta"],
                        NomServicio = (string)data["NomServicio"],
                        NomSucursal = (string)data["NomSucursal"],
                        NroRuta = (int)data["NroRuta"],
                        NroRutaInt = (int)data["NroRutaInt"],
                        NroViaje = (int)data["NroViaje"],
                        PlacaBus = (string)data["PlacaBus"],
                        PlanoBus = (string)data["PlanoBus"],
                        ProgramacionCerrada = (bool)data["ProgramacionCerrada"],
                        RazonSocial = (string)data["RazonSocial"],
                        StOpcional = (string)data["StOpcional"],
                        CodiChofer = (string)data["CodiChofer"],
                        NombreChofer = (string)data["NombreChofer"],
                        CodiCopiloto = (string)data["CodiCopiloto"],
                        NombreCopiloto = (string)data["NombreCopiloto"],
                        ListaDestinosRuta = _ListaDestinosRuta(data["ListaDestinosRuta"])
                    };
                    return Json(item, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        [Route("consultar-usuario")]
        public async Task<ActionResult> SearchClient(string tipoDoc, string numeroDoc)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"TipoDoc\" :\"" + tipoDoc + "\"" +
                                ",\"NumeroDoc\" :  \"" + numeroDoc + "\"" + " }";
                    HttpResponseMessage response = await client.PostAsync("BuscaPasajero", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    ClientePasaje item = new ClientePasaje
                    {
                        ApellidoMaterno = (string)data["ApellidoMaterno"],
                        ApellidoPaterno = (string)data["ApellidoPaterno"],
                        Direccion = (string)data["Direccion"],
                        Edad = (byte)data["Edad"],
                        Email = (string)data["Email"],
                        FechaNacimiento = (string)data["FechaNacimiento"],
                        IdCliente = (int)data["IdCliente"],
                        NombreCliente = (string)data["NombreCliente"],
                        NumeroDoc = (string)data["NumeroDoc"],
                        RucContacto = (string)data["RucContacto"],
                        Telefono = (string)data["Telefono"],
                        TipoDoc = (string)data["TipoDoc"],
                        Sexo = (string)data["Sexo"]
                    };
                    return Json(item, JsonRequestBehavior.AllowGet);
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
        [Route("grabar-pasajero")]
        public async Task<ActionResult> SaveClient(List<ClientePasaje> listado)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    string _body = string.Empty;

                    _body += "[";
                    for (var i = 0; i < listado.Count; i++)
                    {
                        _body += "{ \"IdCliente\" : " + listado[i].IdCliente +
                                    ",\"TipoDoc\" : \"" + listado[i].TipoDoc + "\"" +
                                    ",\"NumeroDoc\" : \"" + listado[i].NumeroDoc + "\"" +
                                    ",\"NombreCliente\" : \"" + listado[i].NombreCliente + "\"" +
                                    ",\"ApellidoPaterno\" : \"" + listado[i].ApellidoPaterno + "\"" +
                                    ",\"ApellidoMaterno\" : \"" + listado[i].ApellidoMaterno + "\"" +
                                    ",\"FechaNacimiento\" : \"" + listado[i].FechaNacimiento + "\"" +
                                    ",\"Edad\" : " + listado[i].Edad +
                                    ",\"Direccion\" : \"" + listado[i].Direccion + "\"" +
                                    ",\"Telefono\" : \"" + listado[i].Telefono + "\"" +
                                    ",\"Email\" : \"" + listado[i].Email + "\"" +
                                    ",\"Sexo\" :  \"" + listado[i].Sexo + "\"" +
                                    ",\"RucContacto\" :  \"" + listado[i].RucContacto + "\"" + " }";
                        if (i < listado.Count - 1)
                            _body += ",";
                    }
                    _body += "]";

                    HttpResponseMessage response = await client.PostAsync("GrabarPasajero", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);
                bool estado = (bool)tmpResult.SelectToken("Estado");
                string mensaje = (string)tmpResult.SelectToken("Mensaje");
                bool valor = (bool)tmpResult.SelectToken("Valor");
                if (valor)
                {
                    return Json(NotifyJson.BuildJson(KindOfNotify.Informativo, mensaje), JsonRequestBehavior.AllowGet);
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
                                ",\"CodiTerminal\" : " + Convert.ToInt32(CodiTerminal) + " }";
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

        [HttpPost]
        [Route("liberarAsiento")]
        public async Task<ActionResult> LiberarAsiento(int IDS)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"IDS\" : " + IDS + " }";
                    HttpResponseMessage response = await client.PostAsync("LiberaAsiento", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    bool valor = (bool)tmpResult["Valor"];
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

        //FiltroVenta
        [HttpPost]
        [Route("grabar-venta")]
        public async Task<ActionResult> SendVenta(List<FiltroVenta> listado)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    string _body = string.Empty;

                    _body += "[";
                    for (var i = 0; i < listado.Count; i++)
                    {
                        _body += "{ \"SerieBoleto\" : " + 0 + //TODO
                                    ",\"NumeBoleto\" : " + 0 + //TODO
                                    ",\"CodiEmpresa\" : " + listado[i].CodiEmpresa +
                                    ",\"CodiOficina\" : " + usuario.CodiSucursal + //
                                    ",\"CodiPuntoVenta\" : " + usuario.CodiPuntoVenta + //
                                    ",\"CodiOrigen\" : " + listado[i].CodiOrigen +
                                    ",\"CodiDestino\" : " + listado[i].CodiDestino +
                                    ",\"CodiProgramacion\" : " + listado[i].CodiProgramacion +
                                    ",\"RucCliente\" : \"" + listado[i].RucCliente + "\"" +
                                    ",\"NumeAsiento\" : " + listado[i].NumeAsiento +
                                    ",\"FlagVenta\" : \"" + listado[i].FlagVenta + "\"" +
                                    ",\"PrecioVenta\" : " + listado[i].PrecioVenta +
                                    ",\"Nombre\" : \"" + listado[i].Nombre + "\"" +
                                    ",\"Edad\" : " + listado[i].Edad +
                                    ",\"Telefono\" : \"" + listado[i].Telefono + "\"" +
                                    ",\"CodiUsuario\" : \"" + usuario.CodiUsuario + "\"" + //
                                    ",\"Dni\" : \"" + listado[i].Dni + "\"" +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" + //
                                    ",\"TipoDocumento\" : \"" + listado[i].TipoDocumento + "\"" +
                                    ",\"CodiDocumento\" : \"" + "" + "\"" + //TODO
                                    ",\"Tipo\" : \"" + "" + "\"" + //TODO
                                    ",\"Sexo\" : \"" + listado[i].Sexo + "\"" +
                                    ",\"TipoPago\" : \"" + listado[i].TipoPago + "\"" +
                                    ",\"FechaViaje\" : \"" + listado[i].FechaViaje + "\"" +
                                    ",\"HoraViaje\" : \"" + listado[i].HoraViaje + "\"" +
                                    ",\"Nacionalidad\" : \"" + listado[i].Nacionalidad + "\"" + //TODO
                                    ",\"CodiServicio\" : " + listado[i].CodiServicio +
                                    ",\"CodiEmbarque\" : " + listado[i].CodiEmbarque +
                                    ",\"CodiArribo\" : " + listado[i].CodiArribo +
                                    ",\"Hora_Embarque\" : \"" + listado[i].Hora_Embarque + "\"" +
                                    ",\"NivelAsiento\" : " + listado[i].NivelAsiento +
                                    ",\"CodiTerminal\" : " + CodiTerminal + //TODO
                                    ",\"NomOficina\" : \"" + usuario.NomSucursal + "\"" + //
                                    ",\"NomPuntoVenta\" : \"" + usuario.NomPuntoVenta + "\"" + //
                                    ",\"NomDestino\" : \"" + listado[i].NomDestino + "\"" +
                                    ",\"NomEmpresaRuc\" : \"" + listado[i].NomEmpresaRuc + "\"" +
                                    ",\"DirEmpresaRuc\" : \"" + listado[i].DirEmpresaRuc + "\"" +
                                    ",\"NomServicio\" : \"" + listado[i].NomServicio + "\"" +
                                    ",\"NomOrigen\" : \"" + listado[i].NomOrigen + "\"" +
                                    ",\"NroViaje\" : " + listado[i].NroViaje +
                                    ",\"FechaProgramacion\" : \"" + listado[i].FechaProgramacion + "\"" +
                                    ",\"HoraProgramacion\" : \"" + listado[i].HoraProgramacion + "\"" +
                                    ",\"CodiBus\" : \"" + listado[i].CodiBus + "\"" +
                                    ",\"CodiSucursal\" : " + listado[i].CodiSucursal +
                                    ",\"CodiRuta\" : " + listado[i].CodiRuta +
                                    ",\"CodiTarjetaCredito\" : \"" + listado[i].CodiTarjetaCredito + "\"" +
                                    ",\"NumeTarjetaCredito\" : \"" + listado[i].NumeTarjetaCredito + "\"" +
                                    ",\"Direccion\" : \"" + listado[i].Direccion + "\"" +
                                    ",\"Observacion\" : \"" + listado[i].Observacion + "\"" +
                                    ",\"Credito\" : " + listado[i].Credito + " }";
                        if (i < listado.Count - 1)
                            _body += ",";
                    }
                    _body += "]";

                    HttpResponseMessage response = await client.PostAsync("GrabaVenta", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    string valor = (string)tmpResult["Valor"];
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

        [HttpGet]
        [Route("consultar-empresa")]
        public async Task<ActionResult> SearchEmpresa(string rucContacto)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{ \"RucContacto\" :  \"" + rucContacto + "\"" + " }";
                    HttpResponseMessage response = await client.PostAsync("ConsultarSUNAT", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    Ruc item = new Ruc
                    {
                        RucCliente = (string)data["RucCliente"],
                        RazonSocial = (string)data["RazonSocial"],
                        Direccion = (string)data["Direccion"],
                        Telefono = (string)data["Telefono"]
                    };
                    return Json(item, JsonRequestBehavior.AllowGet);
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
        [Route("busca-correlativo")]
        public async Task<ActionResult> BuscaCorrelativo(CorrelativoFiltro filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                "\"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                ",\"CodiDocumento\" : \"" + "16" + "\"" + // TODO
                                ",\"CodiSucursal\" : " + usuario.CodiSucursal +
                                ",\"CodiPuntoVenta\" : " + usuario.CodiPuntoVenta +
                                ",\"CodiTerminal\" : \"" + CodiTerminal + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscaCorrelativo", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                    Correlativo item = new Correlativo
                    {
                        SerieBoleto = (short)data["SerieBoleto"],
                        NumeBoleto = (int)data["NumeBoleto"]
                    };
                    return Json(item, JsonRequestBehavior.AllowGet);
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