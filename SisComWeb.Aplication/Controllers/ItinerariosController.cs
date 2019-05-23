using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        readonly Usuario usuario = DataSession.UsuarioLogueado;

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
                Sigla = (string)x["Sigla"],
                RazonSocial = (string)x["RazonSocial"],
                Direccion = (string)x["Direccion"],
                Boleto = (string)x["Boleto"],
                TipoBoleto = (string)x["TipoBoleto"],
                IdVenta = (string)x["IdVenta"],
                ObjAcompaniante = _ObjetoAcompaniante(x["ObjAcompaniante"]),

                CodiOrigen = (short)x["CodiOrigen"],
                CodiDestino = (short)x["CodiDestino"],
                NomOrigen = (string)x["NomOrigen"],
                NomDestino = (string)x["NomDestino"],
                CodiPuntoVenta = (short)x["CodiPuntoVenta"],
                NomPuntoVenta = (string)x["NomPuntoVenta"],
                CodiUsuario = (short)x["CodiUsuario"],
                NomUsuario = (string)x["NomUsuario"]
            }).ToList();

            return lista;
        }

        private static Acompaniante _ObjetoAcompaniante(JToken obj)
        {
            Acompaniante objeto = new Acompaniante();

            // Valida 'obj'
            var auxValidator = obj.ToString();
            if (string.IsNullOrEmpty(auxValidator))
                return objeto;
            // ------------

            JObject data = (JObject)obj;
            
            objeto.CodiTipoDoc = (string)data["TipoDocumento"];
            objeto.Documento = (string)data["NumeroDocumento"];
            objeto.NombreCompleto = (string)data["NombreCompleto"];
            objeto.FechaNac = (string)data["FechaNacimiento"];
            objeto.Edad = (string)data["Edad"];
            objeto.Sexo = (string)data["Sexo"];
            objeto.Parentesco = (string)data["Parentesco"];

            return objeto;
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

        private static List<Base> _ListaAuxDestinosRuta(JToken list)
        {
            List<Base> lista = list.Select(x => new Base
            {
                id = (string)x["CodiSucursal"],
                label = (string)x["NomOficina"]
            }).ToList();

            return lista;
        }

        private static List<Beneficiario> _listaBeneficiario(JToken list)
        {
            List<Beneficiario> lista = list.Select(x => new Beneficiario
            {
                NombreBeneficiario = (string)x["NombreBeneficiario"],
                TipoDocumento = (string)x["TipoDocumento"],
                Documento = (string)x["Documento"],
                NumeroDocumento = (string)x["NumeroDocumento"],
                Sexo = (string)x["Sexo"]
            }).ToList();

            return lista;
        }

        private static List<VentaRealizada> _listVentasRealizadas(JToken list)
        {
            List<VentaRealizada> lista = list.Select(x => new VentaRealizada
            {
                // Para la vista 'BoletosVendidos'
                NumeAsiento = (string)x["NumeAsiento"],
                BoletoCompleto = (string)x["BoletoCompleto"],
                // Para la tabla 'tb_impresion'
                IdVenta = (int)x["IdVenta"],
                NomTipVenta = (string)x["NomTipVenta"],
                BoletoTipo = (string)x["BoletoTipo"],
                BoletoSerie = (string)x["BoletoSerie"],
                BoletoNum = (string)x["BoletoNum"],
                EmpRuc = (string)x["EmpRuc"],
                EmpRazSocial = (string)x["EmpRazSocial"],
                EmpDireccion = (string)x["EmpDireccion"],
                EmpDirAgencia = (string)x["EmpDirAgencia"],
                EmpTelefono1 = (string)x["EmpTelefono1"],
                EmpTelefono2 = (string)x["EmpTelefono2"],
                CodDocumento = (string)x["CodDocumento"],
                EmisionFecha = (string)x["EmisionFecha"],
                EmisionHora = (string)x["EmisionHora"],
                CajeroCod = (short)x["CajeroCod"],
                CajeroNom = (string)x["CajeroNom"],
                PasNombreCom = (string)x["PasNombreCom"],
                PasRuc = (string)x["PasRuc"],
                PasRazSocial = (string)x["PasRazSocial"],
                PasDireccion = (string)x["PasDireccion"],
                NomOriPas = (string)x["NomOriPas"],
                NomDesPas = (string)x["NomDesPas"],
                DocTipo = (byte)x["DocTipo"],
                DocNumero = (string)x["DocNumero"],
                PrecioCan = (string)x["PrecioCan"],
                PrecioDes = (string)x["PrecioDes"],
                NomServicio = (string)x["NomServicio"],
                FechaViaje = (string)x["FechaViaje"],

                EmbarqueDir = (string)x["EmbarqueDir"],
                EmbarqueHora = (string)x["BoletoCoEmbarqueHorampleto"],
                CodigoX_FE = (string)x["CodigoX_FE"],
                LinkPag_FE = (string)x["LinkPag_FE"],
                CodPoliza = (string)x["CodPoliza"],

                CodTerminal = (string)x["CodTerminal"],
                TipImpresora = (byte)x["TipImpresora"],
                CodX = (string)x["CodX"]
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
                    var _body = "{" +
                                    "\"CodiOrigen\" : " + filtro.CodiOrigen +
                                    ",\"CodiDestino\" : " + filtro.CodiDestino +
                                    ",\"CodiRuta\" : " + filtro.CodiRuta +
                                    ",\"Hora\" : \"" + (filtro.Hora ?? "").Replace(" ", "") + "\"" +
                                    ",\"FechaViaje\" : \"" + filtro.FechaViaje + "\"" +
                                    ",\"TodosTurnos\" : " + filtro.TodosTurnos.ToLower() +
                                    ", \"SoloProgramados\" : " + filtro.SoloProgramados.ToLower() +

                                    ", \"NomDestino\" : \"" + filtro.NomDestino + "\"" +
                                " }";
                    HttpResponseMessage response = await client.PostAsync("BuscaItinerarios", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Itinerario>> res = new Response<List<Itinerario>>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Itinerario
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
                        FechaViaje = (string)x["FechaViaje"],
                        Color = (string)x["Color"],
                        SecondColor = (string)x["SecondColor"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Itinerario>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
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
                    var _body = "{" +
                                    "\"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                    ",\"CodiOrigen\" : " + filtro.CodiOrigen +
                                    ",\"CodiDestino\" : " + filtro.CodiDestino +
                                    ",\"CodiSucursal\" : " + filtro.CodiSucursal +
                                    ",\"CodiRuta\" : " + filtro.CodiRuta +
                                    ",\"CodiPuntoVenta\" : " + filtro.CodiPuntoVenta +
                                    ",\"CodiServicio\" : " + filtro.CodiServicio +
                                    ",\"HoraViaje\" :  \"" + filtro.HoraViaje.Replace(" ", "") + "\"" +
                                    ",\"FechaViaje\" :  \"" + filtro.FechaViaje + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("MuestraTurno", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Itinerario> res = new Response<Itinerario>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Itinerario
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
                        ListaDestinosRuta = _ListaDestinosRuta(data["ListaDestinosRuta"]),
                        ListaAuxDestinosRuta = _ListaAuxDestinosRuta(data["ListaDestinosRuta"])
                    }
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Itinerario>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
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
                    var _body = "{" +
                                    "\"TipoDoc\" :\"" + tipoDoc + "\"" +
                                    ",\"NumeroDoc\" :  \"" + numeroDoc + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscaPasajero", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<ClientePasaje> res = new Response<ClientePasaje>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new ClientePasaje
                    {
                        ApellidoMaterno = (string)data["ApellidoMaterno"],
                        ApellidoPaterno = (string)data["ApellidoPaterno"],
                        Edad = (byte)data["Edad"],
                        Email = (string)data["Email"],
                        FechaNacimiento = (string)data["FechaNacimiento"],
                        IdCliente = (int)data["IdCliente"],
                        NombreCliente = (string)data["NombreCliente"],
                        NumeroDoc = (string)data["NumeroDoc"],
                        RucContacto = (string)data["RucContacto"],
                        Telefono = (string)data["Telefono"],
                        TipoDoc = (string)data["TipoDoc"],
                        Sexo = (string)data["Sexo"],
                        RazonSocial = (string)data["RazonSocial"],
                        Direccion = (string)data["Direccion"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<ClientePasaje>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
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
                    var _body = "{" +
                                    "\"RucContacto\" :  \"" + rucContacto + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscaEmpresa", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Ruc> res = new Response<Ruc>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Ruc()
                    {
                        RucCliente = (string)data["RucCliente"],
                        RazonSocial = (string)data["RazonSocial"],
                        Direccion = (string)data["Direccion"],
                        Telefono = (string)data["Telefono"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Ruc>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("consulta-reniec")]
        public async Task<ActionResult> ConsultaReniec(string numeroDoc)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"NumeroDoc\" :  \"" + numeroDoc + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultaRENIEC", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<ReniecEntity> res = new Response<ReniecEntity>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new ReniecEntity()
                    {
                        ApellidoPaterno = (string)data["ApellidoPaterno"],
                        ApellidoMaterno = (string)data["ApellidoMaterno"],
                        Nombres = (string)data["Nombres"]
                    }
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<ReniecEntity>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("consulta-sunat")]
        public async Task<ActionResult> ConsultaSunat(string rucContacto)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"RucContacto\" :  \"" + rucContacto + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultaSUNAT", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Ruc> res = new Response<Ruc>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Ruc()
                    {
                        RazonSocial = (string)data["RazonSocial"],
                        Direccion = (string)data["Direccion"]
                    }
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Ruc>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
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
                        _body += "{" +
                                    "\"IdCliente\" : " + listado[i].IdCliente +
                                    ",\"TipoDoc\" : \"" + listado[i].TipoDoc + "\"" +
                                    ",\"NumeroDoc\" : \"" + listado[i].NumeroDoc + "\"" +
                                    ",\"NombreCliente\" : \"" + listado[i].NombreCliente + "\"" +
                                    ",\"ApellidoPaterno\" : \"" + listado[i].ApellidoPaterno + "\"" +
                                    ",\"ApellidoMaterno\" : \"" + listado[i].ApellidoMaterno + "\"" +
                                    ",\"FechaNacimiento\" : \"" + listado[i].FechaNacimiento + "\"" +
                                    ",\"Edad\" : " + listado[i].Edad +
                                    ",\"Telefono\" : \"" + listado[i].Telefono + "\"" +
                                    ",\"Email\" : \"" + listado[i].Email + "\"" +
                                    ",\"Sexo\" :  \"" + listado[i].Sexo + "\"" +
                                    ",\"RucContacto\" :  \"" + listado[i].RucContacto + "\"" +
                                    ",\"RazonSocial\" :  \"" + listado[i].RazonSocial + "\"" +
                                    ",\"Direccion\" : \"" + listado[i].Direccion + "\"" +
                                 "}";
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

                Response<bool> res = new Response<bool>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (bool)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<bool>(false, Constant.EXCEPCION, false), JsonRequestBehavior.AllowGet);
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
                    var _body = "{" +
                                    "\"CodiProgramacion\" : " + filtro.CodiProgramacion +
                                    ",\"NroViaje\" : " + filtro.NroViaje +
                                    ",\"CodiOrigen\" : " + filtro.CodiOrigen +
                                    ",\"CodiDestino\" : " + filtro.CodiDestino +
                                    ",\"NumeAsiento\" : " + filtro.NumeAsiento +
                                    ",\"FechaProgramacion\" : \"" + filtro.FechaProgramacion + "\"" +
                                    ",\"Precio\" : " + filtro.Precio +
                                    ",\"CodiTerminal\" : " + int.Parse(usuario.Terminal.ToString("D3")) +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BloqueoAsiento", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }
                JToken tmpResult = JObject.Parse(result);

                Response<int> res = new Response<int>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (int)tmpResult["Valor"],
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<int>(false, Constant.EXCEPCION, 0, false), JsonRequestBehavior.AllowGet);
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
                    var _body = "{" +
                                    "\"IDS\" : " + IDS +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("LiberaAsiento", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<bool> res = new Response<bool>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (bool)tmpResult["Valor"],
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<bool>(false, Constant.EXCEPCION, false, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("grabar-venta")]
        public async Task<ActionResult> SendVenta(List<FiltroVenta> Listado, string FlagVenta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    string _body = string.Empty;

                    _body += "{";

                    _body += "\"Listado\" : [";
                    for (var i = 0; i < Listado.Count; i++)
                    {
                        _body += "{" +
                                    "\"SerieBoleto\" : " + 0 +
                                    ",\"NumeBoleto\" : " + 0 +
                                    ",\"CodiEmpresa\" : " + Listado[i].CodiEmpresa +
                                    ",\"CodiOficina\" : " + usuario.CodiSucursal +
                                    ",\"CodiPuntoVenta\" : " + usuario.CodiPuntoVenta +
                                    ",\"CodiOrigen\" : " + Listado[i].CodiOrigen +
                                    ",\"CodiDestino\" : " + Listado[i].CodiDestino +
                                    ",\"CodiProgramacion\" : " + Listado[i].CodiProgramacion +
                                    ",\"RucCliente\" : \"" + Listado[i].RucCliente + "\"" +
                                    ",\"NumeAsiento\" : " + Listado[i].NumeAsiento +
                                    ",\"FlagVenta\" : \"" + Listado[i].FlagVenta + "\"" +
                                    ",\"PrecioVenta\" : " + Listado[i].PrecioVenta +
                                    ",\"Nombre\" : \"" + Listado[i].Nombre + "\"" +
                                    ",\"Edad\" : " + Listado[i].Edad +
                                    ",\"Telefono\" : \"" + Listado[i].Telefono + "\"" +
                                    ",\"CodiUsuario\" : \"" + usuario.CodiUsuario + "\"" +
                                    ",\"Dni\" : \"" + Listado[i].Dni + "\"" +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" +
                                    ",\"TipoDocumento\" : \"" + Listado[i].TipoDocumento + "\"" +
                                    ",\"CodiDocumento\" : \"" + "" + "\"" +
                                    ",\"Tipo\" : \"" + "" + "\"" +
                                    ",\"Sexo\" : \"" + Listado[i].Sexo + "\"" +
                                    ",\"TipoPago\" : \"" + Listado[i].TipoPago + "\"" +
                                    ",\"FechaViaje\" : \"" + Listado[i].FechaViaje + "\"" +
                                    ",\"HoraViaje\" : \"" + Listado[i].HoraViaje.Replace(" ", "") + "\"" +
                                    ",\"Nacionalidad\" : \"" + Listado[i].Nacionalidad + "\"" +
                                    ",\"CodiServicio\" : " + Listado[i].CodiServicio +
                                    ",\"CodiEmbarque\" : " + Listado[i].CodiEmbarque +
                                    ",\"CodiArribo\" : " + Listado[i].CodiArribo +
                                    ",\"HoraEmbarque\" : \"" + Listado[i].HoraEmbarque + "\"" +
                                    ",\"NivelAsiento\" : " + Listado[i].NivelAsiento +
                                    ",\"CodiTerminal\" : \"" + usuario.Terminal.ToString("D3") + "\"" +
                                    ",\"NomOficina\" : \"" + usuario.NomSucursal + "\"" +
                                    ",\"NomPuntoVenta\" : \"" + usuario.NomPuntoVenta + "\"" +
                                    ",\"NomDestino\" : \"" + Listado[i].NomDestino + "\"" +
                                    ",\"NomEmpresaRuc\" : \"" + Listado[i].NomEmpresaRuc + "\"" +
                                    ",\"DirEmpresaRuc\" : \"" + Listado[i].DirEmpresaRuc + "\"" +
                                    ",\"NomServicio\" : \"" + Listado[i].NomServicio + "\"" +
                                    ",\"NomOrigen\" : \"" + Listado[i].NomOrigen + "\"" +
                                    ",\"NroViaje\" : " + Listado[i].NroViaje +
                                    ",\"FechaProgramacion\" : \"" + Listado[i].FechaProgramacion + "\"" +
                                    ",\"HoraProgramacion\" : \"" + Listado[i].HoraProgramacion + "\"" +
                                    ",\"CodiBus\" : \"" + Listado[i].CodiBus + "\"" +
                                    ",\"CodiSucursal\" : " + Listado[i].CodiSucursal +
                                    ",\"CodiRuta\" : " + Listado[i].CodiRuta +
                                    ",\"CodiTarjetaCredito\" : \"" + Listado[i].CodiTarjetaCredito + "\"" +
                                    ",\"NumeTarjetaCredito\" : \"" + Listado[i].NumeTarjetaCredito + "\"" +
                                    ",\"CodiZona\" : \"" + Listado[i].CodiZona + "\"" +
                                    ",\"Direccion\" : \"" + Listado[i].Direccion + "\"" +
                                    ",\"Observacion\" : \"" + Listado[i].Observacion + "\"" +
                                    ",\"Credito\" : " + Listado[i].Credito +
                                    ",\"DirEmbarque\" : \"" + Listado[i].DirEmbarque + "\"" +
                                    ",\"ObjAcompaniante\" : " +
                                    "{" +
                                        "\"TipoDocumento\" : \"" + (Listado[i].ObjAcompaniante == null ? "" : Listado[i].ObjAcompaniante.CodiTipoDoc) + "\"" +
                                        ",\"NumeroDocumento\" : \"" + (Listado[i].ObjAcompaniante == null ? "" : Listado[i].ObjAcompaniante.Documento) + "\"" +
                                        ",\"NombreCompleto\" : \"" + (Listado[i].ObjAcompaniante == null ? "" : Listado[i].ObjAcompaniante.NombreCompleto) + "\"" +
                                        ",\"FechaNacimiento\" : \"" + (Listado[i].ObjAcompaniante == null ? "" : Listado[i].ObjAcompaniante.FechaNac) + "\"" +
                                        ",\"Edad\" : \"" + (Listado[i].ObjAcompaniante == null ? "" : Listado[i].ObjAcompaniante.Edad) + "\"" +
                                        ",\"Sexo\" : \"" + (Listado[i].ObjAcompaniante == null ? "" : Listado[i].ObjAcompaniante.Sexo) + "\"" +
                                        ",\"Parentesco\" : \"" + (Listado[i].ObjAcompaniante == null ? "" : Listado[i].ObjAcompaniante.Parentesco) + "\"" +
                                    "}" +
                                    // PASE DE CORTESÍA
                                    ",\"CodiGerente\" : \"" + Listado[i].CodiGerente + "\"" +
                                    ",\"CodiSocio\" : \"" + Listado[i].CodiSocio + "\"" +
                                    ",\"Mes\" : \"" + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + "\"" +
                                    ",\"Anno\" : \"" + DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture) + "\"" +
                                    ",\"Concepto\" : \"" + Listado[i].Concepto + "\"" +
                                    ",\"FechaAbierta\" : \"" + Listado[i].FechaAbierta.ToString().ToLower() + "\"" +
                                    // RESERVA
                                    ",\"IdVenta\" : " + Listado[i].IdVenta +
                                    // CRÉDITO
                                    ",\"IdContrato\" : " + Listado[i].IdContrato +
                                    ",\"IdPrecio\" : " + Listado[i].IdPrecio +
                                    ",\"NroSolicitud\" : \"" + Listado[i].NroSolicitud + "\"" +
                                    ",\"IdArea\" : " + Listado[i].IdArea +
                                    ",\"FlgIda\" : \"" + Listado[i].FlgIda + "\"" +
                                    ",\"FechaCita\" : \"" + Listado[i].FechaCita + "\"" +
                                    ",\"IdHospital\" : " + Listado[i].IdHospital +
                                     ",\"FlagPrecioNormal\" : " + Listado[i].FlagPrecioNormal.ToString().ToLower() +
                                 "}";

                        if (i < Listado.Count - 1)
                            _body += ",";
                    }
                    _body += "]";

                    // TIPO DE VENTA
                    _body += ",\"FlagVenta\" : \"" + FlagVenta + "\"";
                    // -------------

                    _body += "}";

                    HttpResponseMessage response = await client.PostAsync("GrabaVenta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<VentaResponse> res = new Response<VentaResponse>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new VentaResponse()
                    {
                        ListaVentasRealizadas = _listVentasRealizadas(data["ListaVentasRealizadas"]),
                        CodiProgramacion = (int)data["CodiProgramacion"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Venta>>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
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
                                    ",\"CodiSucursal\" : " + usuario.CodiSucursal +
                                    ",\"CodiPuntoVenta\" : " + usuario.CodiPuntoVenta +
                                    ",\"CodiTerminal\" : \"" + usuario.Terminal.ToString("D3") + "\"" +
                                    ",\"FlagVenta\" : \"" + filtro.FlagVenta + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscaCorrelativo", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<CorrelativoResponse> res = new Response<CorrelativoResponse>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new CorrelativoResponse()
                    {
                        CodiTerminalElectronico = (string)data["CodiTerminalElectronico"],
                        CorrelativoVentaBoleta = (string)data["CorrelativoVentaBoleta"],
                        CorrelativoVentaFactura = (string)data["CorrelativoVentaFactura"],
                        CorrelativoPaseBoleta = (string)data["CorrelativoPaseBoleta"],
                        CorrelativoPaseFactura = (string)data["CorrelativoPaseFactura"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<CorrelativoResponse>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("listaBeneficiarioPase")]
        public async Task<ActionResult> ListaBeneficiarioPase(string CodiSocio)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiSocio\" : \"" + CodiSocio + "\"" +
                                    ",\"Anno\" : \"" + DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture) + "\"" +
                                    ",\"Mes\" : \"" + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ListaBeneficiarioPase", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<BoletosCortesia> res = new Response<BoletosCortesia>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new BoletosCortesia
                    {
                        BoletoTotal = (decimal)data["BoletoTotal"],
                        BoletoLibre = (decimal)data["BoletoLibre"],
                        BoletoPrecio = (decimal)data["BoletoPrecio"],
                        ListaBeneficiarios = _listaBeneficiario(data["ListaBeneficiarios"])
                    }
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<BoletosCortesia>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("validarPase")]
        public async Task<ActionResult> ValidarSaldoPaseCortesia(string CodiSocio)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiSocio\" : \"" + CodiSocio + "\"" +
                                    ",\"Mes\" : \"" + DateTime.Now.ToString("MM", CultureInfo.InvariantCulture) + "\"" +
                                    ",\"Anno\" : \"" + DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture) + "\"" +

                                "}";
                    HttpResponseMessage response = await client.PostAsync("ValidarPase", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<decimal> res = new Response<decimal>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (decimal)tmpResult["Valor"],
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<decimal>(false, Constant.EXCEPCION, 0, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("clave-autorizacion")]
        public async Task<ActionResult> ClaveAutorizacion(string password)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiOficina\": " + usuario.CodiPuntoVenta + ","
                                    + "\"Password\": \"" + password + "\","
                                    + "\"CodiTipo\": " + Constant.CLAVE_ACOMPAÑANTE_CON_MAYOR_EDAD.ToString("D3") +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ClavesInternas", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<bool> res = new Response<bool>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (bool)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<bool>(false, Constant.EXCEPCION, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("anular-venta")]
        public async Task<ActionResult> AnularVenta(int IdVenta, string Tipo)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" 
                                + "\"IdVenta\": " + IdVenta + ","
                                + "\"CodiUsuario\": " + usuario.CodiUsuario + ","
                                + "\"CodiOficina\": " + usuario.CodiSucursal + ","
                                + "\"CodiPuntoVenta\": " + usuario.CodiPuntoVenta + ","
                                + "\"Tipo\": \"" + Tipo + "\""
                                + "}";

                    HttpResponseMessage response = await client.PostAsync("AnularVenta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<byte> res = new Response<byte>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (byte)tmpResult["Valor"],
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<bool>(false, Constant.EXCEPCION, false, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("buscar-venta-x-boleto")]
        public async Task<ActionResult> BuscarVentaxBoleto(string Tipo, short Serie, int Numero, short CodiEmpresa)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Tipo\" : \"" + Tipo + "\"" +
                                    ",\"Serie\" : " + Serie +
                                    ",\"Numero\" : " + Numero +
                                    ", \"CodiEmpresa\" : " + CodiEmpresa +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscarVentaxBoleto", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<VentaBeneficiario> res = new Response<VentaBeneficiario>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new VentaBeneficiario
                    {
                        IdVenta = (long)data["IdVenta"],
                        NombresConcat = (string)data["NombresConcat"],
                        CodiOrigen = (int)data["CodiOrigen"],
                        NombOrigen = (string)data["NombOrigen"],
                        CodiDestino = (int)data["CodiDestino"],
                        NombDestino = (string)data["NombDestino"],
                        NombServicio = (string)data["NombServicio"],
                        FechViaje = (string)data["FechViaje"],
                        HoraViaje = (string)data["HoraViaje"],
                        NumeAsiento = (int)data["NumeAsiento"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"],
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<VentaBeneficiario>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("postergar-venta")]
        public async Task<ActionResult> PostergarVenta(PostergarVentaFiltro filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\" : " + filtro.IdVenta +
                                    ",\"CodiProgramacion\" : " + filtro.CodiProgramacion +
                                    ",\"NumeAsiento\" : " + filtro.NumeAsiento +
                                    ",\"CodiServicio\" : " + filtro.CodiServicio +
                                    ",\"FechaViaje\" : \"" + filtro.FechaViaje + "\"" +
                                    ",\"HoraViaje\" : \"" + filtro.HoraViaje.Replace(" ", "") + "\"" +

                                    ",\"NroViaje\" : " + filtro.NroViaje +
                                    ",\"FechaProgramacion\" : \"" + filtro.FechaProgramacion + "\"" +
                                    ",\"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                    ",\"CodiSucursal\" : " + filtro.CodiSucursal +
                                    ",\"CodiRuta\" : " + filtro.CodiRuta +
                                    ",\"CodiBus\" : \"" + filtro.CodiBus + "\"" +
                                    ",\"HoraProgramacion\" : \"" + filtro.HoraProgramacion + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("PostergarVenta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<VentaResponse> res = new Response<VentaResponse>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new VentaResponse()
                    {
                        ListaVentasRealizadas = _listVentasRealizadas(data["ListaVentasRealizadas"]),
                        CodiProgramacion = (int)data["CodiProgramacion"]
                    },
                    EsCorrecto = (bool)tmpResult["Estado"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<byte>(false, Constant.EXCEPCION, 0, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("modificar-venta-a-fecha-abierta")]
        public async Task<ActionResult> ModificarVentaAFechaAbierta(int IdVenta, int CodiServicio, int CodiRuta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\" : " + IdVenta +
                                    ",\"CodiServicio\" : " + CodiServicio +
                                    ", \"CodiRuta\" : " + CodiRuta +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ModificarVentaAFechaAbierta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<byte> res = new Response<byte>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (byte)tmpResult["Valor"],
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<bool>(false, Constant.EXCEPCION, false, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("cancelar-reserva")]
        public async Task<ActionResult> EliminarReserva(int IdVenta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\": " + IdVenta +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("EliminarReserva", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<byte> res = new Response<byte>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (byte)tmpResult["Valor"],
                    EsCorrecto = (bool)tmpResult["EsCorrecto"],
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<bool>(false, Constant.EXCEPCION, false, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("listarClientesContrato")]
        public async Task<ActionResult> ListarClientesContrato(CreditoRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"FechaViaje\" : \"" + request.FechaViaje + "\"" +
                                    ",\"CodiOficina\" : " + request.CodiOficina +
                                    ",\"CodiRuta\" : " + request.CodiRuta +
                                    ",\"CodiServicio\" : " + request.CodiServicio +
                                    ",\"CodiBus\" : \"" + request.CodiBus + "\"" +
                                    ",\"NumeAsiento\" : " + request.NumeAsiento +
                                    ",\"HoraViaje\" : \"" + request.HoraViaje.Replace(" ", "") + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ListarClientesContrato", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);


                Response<List<ClienteCredito>> res = new Response<List<ClienteCredito>>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new ClienteCredito
                    {
                        NumeContrato = (string)x["NumeContrato"],
                        RucCliente = (string)x["RucCliente"],
                        RazonSocial = (string)x["RazonSocial"],
                        St = (string)x["St"],
                        IdRuc = (int)x["IdRuc"],
                        NombreCorto = (string)x["NombreCorto"],
                        IdContrato = (int)x["IdContrato"],

                        CntBoletos = (int)x["CntBoletos"],
                        SaldoBoletos = (int)x["SaldoBoletos"],
                        IdPrecio = (int)x["IdPrecio"],
                        Precio = (decimal)x["Precio"]

                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<ClienteCredito>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("convertir-venta-to-base64")]
        public async Task<ActionResult> ConvertirVentaToBase64(List<VentaRealizada> Listado)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    string _body = string.Empty;

                    _body += "[";
                    for (var i = 0; i < Listado.Count; i++)
                    {
                        _body += "{"
                                    + "\"IdVenta\": " + Listado[i].IdVenta
                                    + ",\"NomTipVenta\" : \"" + Listado[i].NomTipVenta + "\""
                                    + ",\"NumeAsiento\" : \"" + Listado[i].NumeAsiento + "\""
                                    + ",\"BoletoCompleto\" : \"" + Listado[i].BoletoCompleto + "\""
                                    + ",\"BoletoTipo\" : \"" + Listado[i].BoletoTipo + "\""
                                    + ",\"BoletoSerie\" : \"" + Listado[i].BoletoSerie + "\""
                                    + ",\"BoletoNum\" : \"" + Listado[i].BoletoNum + "\""
                                    + ",\"EmpRuc\" : \"" + Listado[i].EmpRuc + "\""
                                    + ",\"EmpRazSocial\" : \"" + Listado[i].EmpRazSocial + "\""
                                    + ",\"EmpDireccion\" : \"" + Listado[i].EmpDireccion + "\""
                                    + ",\"EmpDirAgencia\" : \"" + Listado[i].EmpDirAgencia + "\""
                                    + ",\"EmpTelefono1\" : \"" + Listado[i].EmpTelefono1 + "\""
                                    + ",\"EmpTelefono2\" : \"" + Listado[i].EmpTelefono2 + "\""
                                    + ",\"CodDocumento\" : \"" + Listado[i].CodDocumento + "\""
                                    + ",\"EmisionFecha\" : \"" + Listado[i].EmisionFecha + "\""
                                    + ",\"EmisionHora\" : \"" + Listado[i].EmisionHora + "\""
                                    + ",\"CajeroCod\": " + Listado[i].CajeroCod
                                    + ",\"CajeroNom\" : \"" + Listado[i].CajeroNom + "\""
                                    + ",\"PasNombreCom\" : \"" + Listado[i].PasNombreCom + "\""
                                    + ",\"PasRuc\" : \"" + Listado[i].PasRuc + "\""
                                    + ",\"PasRazSocial\" : \"" + Listado[i].PasRazSocial + "\""
                                    + ",\"PasDireccion\" : \"" + Listado[i].PasDireccion + "\""
                                    + ",\"NomOriPas\" : \"" + Listado[i].NomOriPas + "\""
                                    + ",\"NomDesPas\" : \"" + Listado[i].NomDesPas + "\""
                                    + ",\"DocTipo\": " + Listado[i].DocTipo
                                    + ",\"DocNumero\" : \"" + Listado[i].DocNumero + "\""
                                    + ",\"PrecioCan\" : \"" + Listado[i].PrecioCan + "\""
                                    + ",\"PrecioDes\" : \"" + Listado[i].PrecioDes + "\""
                                    + ",\"NomServicio\" : \"" + Listado[i].NomServicio + "\""
                                    + ",\"FechaViaje\" : \"" + Listado[i].FechaViaje + "\""
                                    + ",\"EmbarqueDir\" : \"" + Listado[i].EmbarqueDir + "\""
                                    + ",\"EmbarqueHora\" : \"" + Listado[i].EmbarqueHora + "\""
                                    + ",\"CodigoX_FE\" : \"" + Listado[i].CodigoX_FE + "\""
                                    + ",\"LinkPag_FE\" : \"" + Listado[i].LinkPag_FE + "\""
                                    + ",\"CodPoliza\" : \"" + Listado[i].CodPoliza + "\""
                                    + ",\"CodTerminal\" : \"" + Listado[i].CodTerminal + "\""
                                    + ",\"TipImpresora\": " + Listado[i].TipImpresora
                                    + ",\"CodX\" : \"" + Listado[i].CodX + "\""
                                + "}";

                        if (i < Listado.Count - 1)
                            _body += ",";
                    }
                    _body += "]";

                    HttpResponseMessage response = await client.PostAsync("ConvertirVentaToBase64", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<string>> res = new Response<List<string>>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = tmpResult["Valor"].ToObject<List<string>>()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("listarPanelControl")]
        public async Task<ActionResult> ListarPanelControl()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListarPanelControl");
                    HttpResponseMessage response = await client.GetAsync(url + "ListarPanelControl");
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<PanelControlEntity>> res = new Response<List<PanelControlEntity>>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new PanelControlEntity
                    {
                        CodiPanel = (string)x["CodiPanel"],
                        Valor = (string)x["Valor"],
                        Descripcion = (string)x["Descripcion"],
                        TipoControl = (string)x["TipoControl"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<ClienteCredito>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("consultarContrato")]
        public async Task<ActionResult> ConsultarContrato(int idContrato)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"idContrato\" : " + idContrato +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultarContrato", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Contrato> res = new Response<Contrato>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Contrato()
                    {
                        Saldo = (decimal)data["Saldo"],
                        Marcador = (string)data["Marcador"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Contrato>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("verificarPrecioNormal")]
        public async Task<ActionResult> VerificarPrecioNormal(int idContrato)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"idContrato\" : " + idContrato +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VerificarPrecioNormal", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<PrecioNormal> res = new Response<PrecioNormal>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new PrecioNormal()
                    {
                        IdNormal = (int)data["IdNormal"],
                        IdContrato = (int)data["IdContrato"],
                        TipoPrecio = (string)data["TipoPrecio"],
                        MontoMas = (decimal)data["MontoMas"],
                        MontoMenos = (decimal)data["MontoMenos"],
                        CntBol = (int)data["CntBol"],
                        Saldo = (int)data["Saldo"]
                    }
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<PrecioNormal>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("buscarPrecio")]
        public async Task<ActionResult> BuscarPrecio(string fechaViaje, string nivel, string hora, string idPrecio)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"fechaViaje\": \"" + fechaViaje + "\"" +
                                    ",\"nivel\": \"" + nivel + "\"" +
                                    ",\"hora\": \"" + hora.Replace(" ", "") + "\"" +
                                    ",\"idPrecio\": \"" + idPrecio + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscarPrecio", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();
                    }
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