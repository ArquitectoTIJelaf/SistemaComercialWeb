﻿using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using SisComWeb.Utility;
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
                PrecioMaximo = (decimal)x["PrecioMaximo"],
                PrecioMinimo = (decimal)x["PrecioMinimo"],
                PrecioNormal = (decimal)x["PrecioNormal"],
                PrecioVenta = (decimal)x["PrecioVenta"],
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
                NomUsuario = (string)x["NomUsuario"],
                NumeSolicitud = (string)x["NumeSolicitud"],
                HoraVenta = (string)x["HoraVenta"],
                EmbarqueCod = (short)x["EmbarqueCod"],
                EmbarqueDir = (string)x["EmbarqueDir"],
                EmbarqueHora = (string)x["EmbarqueHora"],
                ImpManifiesto = (string)x["ImpManifiesto"],
                CodiSucursal = (short)x["CodiSucursal"],
                ClavUsuarioReintegro = (short)x["ClavUsuarioReintegro"],
                SucVentaReintegro = (short)x["SucVentaReintegro"],
                PrecVentaReintegro = (decimal)x["PrecVentaReintegro"],
                TipoPago = (string)x["TipoPago"],
                ValeRemoto = (string)x["ValeRemoto"],
                CodiEsca = (string)x["CodiEsca"],
                CodiEmpresa = (byte)x["CodiEmpresa"],
                FechaReservacion = (string)x["FechaReservacion"],
                HoraReservacion = (string)x["HoraReservacion"],
                Info = (string)x["Info"],
                Observacion = (string)x["Observacion"],
                Especial = (string)x["Especial"],

                Correo = (string)x["Correo"],
            }).ToList();

            return lista;
        }

        private static Acompaniante _ObjetoAcompaniante(JToken obj)
        {
            var objeto = new Acompaniante()
            {
                CodiTipoDoc = string.Empty,
                Documento = string.Empty,
                NombreCompleto = string.Empty,
                FechaNac = string.Empty,
                Edad = string.Empty,
                Sexo = string.Empty,
                Parentesco = string.Empty
            };

            // Valida 'obj'
            if (string.IsNullOrEmpty(obj.ToString()))
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
                Color = (string)x["Color"],

                Cantidad = (int)x["Cantidad"]
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
                // Para el método 'ConvertirVentaToBase64'
                IdVenta = (int)x["IdVenta"],
                BoletoTipo = (string)x["BoletoTipo"],
                BoletoSerie = (string)x["BoletoSerie"],
                BoletoNum = (string)x["BoletoNum"],
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
                EmbarqueHora = (string)x["EmbarqueHora"],
                CodigoX_FE = (string)x["CodigoX_FE"],
                TipoTerminalElectronico = (string)x["TipoTerminalElectronico"],
                TipoImpresora = (byte)x["TipoImpresora"],
                EmpDirAgencia = (string)x["EmpDirAgencia"],
                EmpTelefono1 = (string)x["EmpTelefono1"],
                EmpTelefono2 = (string)x["EmpTelefono2"],
                PolizaNum = (string)x["PolizaNum"],
                PolizaFechaReg = (string)x["PolizaFechaReg"],
                PolizaFechaVen = (string)x["PolizaFechaVen"],
                EmpRuc = (string)x["EmpRuc"],
                EmpRazSocial = (string)x["EmpRazSocial"],
                EmpDireccion = (string)x["EmpDireccion"],
                EmpElectronico = (string)x["EmpElectronico"],

                TipoPago = (string)x["TipoPago"],
                FlagVenta = (string)x["FlagVenta"],

                // Parámetros extras
                EmpCodigo = (byte)x["EmpCodigo"],
                PVentaCodigo = (short)x["PVentaCodigo"],
                BusCodigo = (string)x["BusCodigo"],
                EmbarqueCod = (short)x["EmbarqueCod"]
            }).ToList();

            return lista;
        }

        private static List<PanelControl> _listaPanelControl(JToken list)
        {
            List<PanelControl> lista = list.Select(x => new PanelControl
            {
                CodiPanel = (string)x["CodiPanel"],
                Valor = (string)x["Valor"],
                Descripcion = (string)x["Descripcion"],
                TipoControl = (string)x["TipoControl"]
            }).ToList();

            return lista;
        }

        private static List<PanelControlNivel> _listaPanelControlNivel(JToken list)
        {
            List<PanelControlNivel> lista = list.Select(x => new PanelControlNivel
            {
                Codigo = (int)x["Codigo"],
                Descripcion = (string)x["Descripcion"],
                Nivel = (byte)x["Nivel"]
            }).ToList();

            return lista;
        }

        private static TablaBloqueoAsientos _ObjetoTablaBloqueoAsientos(JToken obj)
        {
            var objeto = new TablaBloqueoAsientos()
            {
                AsientosOcupados = string.Empty,
                AsientosLiberados = string.Empty,
            };

            // Valida 'obj'
            if (string.IsNullOrEmpty(obj.ToString()))
                return objeto;
            // ------------

            JObject data = (JObject)obj;
            
            objeto.AsientosOcupados = (string)data["AsientosOcupados"];
            objeto.AsientosLiberados = (string)data["AsientosLiberados"];
            objeto.CodiOrigen = (short)data["CodiOrigen"];
            objeto.CodiDestino = (short)data["CodiDestino"];

            return objeto;
        }

        private static ResumenProgramacion _ObjetoResumenProgramacion(JToken obj)
        {
            var objeto = new ResumenProgramacion();

             // Valida 'obj'
            if (string.IsNullOrEmpty(obj.ToString()))
                return objeto;
            // ------------

            JObject data = (JObject)obj;

            objeto.CAP = (string)data["CAP"];
            objeto.VTS = (string)data["VTS"];
            objeto.VTT = (string)data["VTT"];
            objeto.RET = (string)data["RET"];
            objeto.PAS = (string)data["PAS"];
            objeto.RVA = (string)data["RVA"];
            objeto.LBR = (string)data["LBR"];
            objeto.TOT = (string)data["TOT"];

            return objeto;
        }

        [Route("")]
        public ActionResult Index()
        {
            return View(usuario);
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
                                    ",\"SoloProgramados\" : " + filtro.SoloProgramados.ToLower() +
                                    ",\"NomDestino\" : \"" + filtro.NomDestino + "\"" +
                                    ",\"CodiServicio\" : \"" + filtro.CodiServicio + "\"" +
                                " }";
                    HttpResponseMessage response = await client.PostAsync("BuscaItinerarios", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                        ProgramacionCerrada = (string)x["ProgramacionCerrada"],
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
                                    ",\"HoraViaje\" : \"" + filtro.HoraViaje.Replace(" ", "") + "\"" +
                                    ",\"FechaViaje\" : \"" + filtro.FechaViaje + "\"" +
                                    ",\"CodiPvUsuario\" : " + usuario.CodiPuntoVenta +

                                    ",\"CodiSucursalUsuario\" : " + usuario.CodiSucursal +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("MuestraTurno", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                        ProgramacionCerrada = (string)data["ProgramacionCerrada"],
                        RazonSocial = (string)data["RazonSocial"],
                        StOpcional = (string)data["StOpcional"],
                        CodiChofer = (string)data["CodiChofer"],
                        NombreChofer = (string)data["NombreChofer"],
                        CodiCopiloto = (string)data["CodiCopiloto"],
                        NombreCopiloto = (string)data["NombreCopiloto"],
                        ListaDestinosRuta = _ListaDestinosRuta(data["ListaDestinosRuta"]),
                        ListaAuxDestinosRuta = _ListaAuxDestinosRuta(data["ListaDestinosRuta"]),
                        DescServicio = (string)data["DescServicio"],
                        X_Estado = (string)data["X_Estado"],
                        Activo = (string)data["Activo"],
                        CantidadMaxBloqAsi = (short)data["CantidadMaxBloqAsi"],
                        TablaBloqueoAsientos = _ObjetoTablaBloqueoAsientos(data["TablaBloqueoAsientos"]),

                        Color = (string)data["Color"],
                        SecondColor = (string)data["SecondColor"],
                        ListaResumenProgramacion = _ObjetoResumenProgramacion(data["ListaResumenProgramacion"])
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Itinerario>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
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
                                    "\"TipoDoc\" : \"" + tipoDoc + "\"" +
                                    ",\"NumeroDoc\" : \"" + numeroDoc + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscaPasajero", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                        FechaNacimiento = (string)data["FechaNacimiento"],
                        IdCliente = (int)data["IdCliente"],
                        NombreCliente = (string)data["NombreCliente"],
                        NumeroDoc = (string)data["NumeroDoc"],
                        RucContacto = (string)data["RucContacto"],
                        Telefono = (string)data["Telefono"],
                        TipoDoc = (string)data["TipoDoc"],
                        Sexo = (string)data["Sexo"],
                        RazonSocial = (string)data["RazonSocial"],
                        Direccion = (string)data["Direccion"],
                        Especial = (string)data["Especial"],

                        Correo = (string)data["Correo"]
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
                                    "\"RucContacto\" : \"" + rucContacto + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscaEmpresa", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                                    "\"NumeroDoc\" : \"" + numeroDoc + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultaRENIEC", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<ReniecEntity>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
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
                                    "\"RucContacto\" : \"" + rucContacto + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultaSUNAT", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                                    ",\"Sexo\" : \"" + listado[i].Sexo + "\"" +
                                    ",\"RucContacto\" : \"" + listado[i].RucContacto + "\"" +
                                    ",\"RazonSocial\" : \"" + listado[i].RazonSocial + "\"" +
                                    ",\"Direccion\" : \"" + listado[i].Direccion + "\"" +

                                    ",\"Correo\" : \"" + listado[i].Correo + "\"" +
                                 "}";
                        if (i < listado.Count - 1)
                            _body += ",";
                    }
                    _body += "]";

                    HttpResponseMessage response = await client.PostAsync("GrabarPasajero", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                                    ",\"Precio\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(filtro.Precio) + "\"" +
                                    ",\"CodiTerminal\" : " + int.Parse(usuario.Terminal.ToString("D3")) +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BloqueoAsiento", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                        result = await response.Content.ReadAsStringAsync();
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
                                    ",\"PrecioVenta\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(Listado[i].PrecioVenta) + "\"" +
                                    ",\"Nombre\" : \"" + Listado[i].Nombre + "\"" +
                                    ",\"Edad\" : " + Listado[i].Edad +
                                    ",\"Telefono\" : \"" + Listado[i].Telefono + "\"" +
                                    ",\"CodiUsuario\" : \"" + usuario.CodiUsuario + "\"" +
                                    ",\"Dni\" : \"" + Listado[i].Dni + "\"" +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" +
                                    ",\"TipoDocumento\" : \"" + Listado[i].TipoDocumento + "\"" +
                                    ",\"CodiDocumento\" : \"" + string.Empty + "\"" +
                                    ",\"Tipo\" : \"" + string.Empty + "\"" +
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
                                    ",\"Credito\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(Listado[i].Credito) + "\"" +
                                    ",\"DirEmbarque\" : \"" + Listado[i].DirEmbarque + "\"" +
                                    ",\"PrecioNormal\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(Listado[i].PrecioNormal) + "\"" +
                                    ",\"ValidadorDescuento\" : " + Listado[i].ValidadorDescuento.ToString().ToLower() +
                                    ",\"ObservacionDescuento\" : \"" + Listado[i].ObservacionDescuento + "\"" +
                                    ",\"ValidadorDescuentoControl\" : " + Listado[i].ValidadorDescuentoControl.ToString().ToLower() +
                                    ",\"DescuentoTipoDC\" : \"" + Listado[i].DescuentoTipoDC + "\"" +
                                    ",\"ImporteDescuentoDC\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(Listado[i].ImporteDescuentoDC) + "\"" +
                                    ",\"ImporteDescontadoDC\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(Listado[i].ImporteDescontadoDC) + "\"" +
                                    ",\"AutorizadoDC\" : \"" + Listado[i].AutorizadoDC + "\"" +
                                    ",\"ObjAcompaniante\" : " +
                                    "{" +
                                        "\"TipoDocumento\" : \"" + Listado[i].ObjAcompaniante.CodiTipoDoc + "\"" +
                                        ",\"NumeroDocumento\" : \"" + Listado[i].ObjAcompaniante.Documento + "\"" +
                                        ",\"NombreCompleto\" : \"" + Listado[i].ObjAcompaniante.NombreCompleto + "\"" +
                                        ",\"FechaNacimiento\" : \"" + Listado[i].ObjAcompaniante.FechaNac + "\"" +
                                        ",\"Edad\" : \"" + Listado[i].ObjAcompaniante.Edad + "\"" +
                                        ",\"Sexo\" : \"" + Listado[i].ObjAcompaniante.Sexo + "\"" +
                                        ",\"Parentesco\" : \"" + Listado[i].ObjAcompaniante.Parentesco + "\"" +
                                    "}" +
                                    ",\"IngresoManualPasajes\" : " + Listado[i].IngresoManualPasajes.ToString().ToLower() +
                                    ",\"EstadoAsiento\" : \"" + Listado[i].EstadoAsiento + "\"" +

                                    ",\"NomEmpresa\" : \"" + Listado[i].NomEmpresa + "\"" +
                                    ",\"RucEmpresa\" : \"" + Listado[i].RucEmpresa + "\"" +
                                    ",\"DireccionEmpresa\" : \"" + Listado[i].DireccionEmpresa + "\"" +
                                    ",\"ElectronicoEmpresa\" : \"" + Listado[i].ElectronicoEmpresa + "\"" +
                                    ",\"TipoTerminalElectronico\" : \"" + Listado[i].TipoTerminalElectronico + "\"" +
                                    ",\"TipoImpresora\" : " + Listado[i].TipoImpresora +
                                    
                                    // PASE DE CORTESÍA
                                    ",\"CodiGerente\" : \"" + Listado[i].CodiGerente + "\"" +
                                    ",\"CodiSocio\" : \"" + Listado[i].CodiSocio + "\"" +
                                    ",\"Mes\" : \"" + DataUtility.ObtenerMesDelSistema() + "\"" +
                                    ",\"Anno\" : \"" + DataUtility.ObtenerAñoDelSistema() + "\"" +
                                    ",\"Concepto\" : \"" + Listado[i].Concepto + "\"" +
                                    ",\"FechaAbierta\" : " + Listado[i].FechaAbierta.ToString().ToLower() +
                                    // CRÉDITO
                                    ",\"IdContrato\" : " + Listado[i].IdContrato +
                                    ",\"IdPrecio\" : " + Listado[i].IdPrecio +
                                    ",\"NroSolicitud\" : \"" + Listado[i].NroSolicitud + "\"" +
                                    ",\"IdArea\" : " + Listado[i].IdArea +
                                    ",\"FlgIda\" : \"" + Listado[i].FlgIda + "\"" +
                                    ",\"FechaCita\" : \"" + Listado[i].FechaCita + "\"" +
                                    ",\"IdHospital\" : " + Listado[i].IdHospital +
                                     ",\"FlagPrecioNormal\" : " + Listado[i].FlagPrecioNormal.ToString().ToLower() +
                                     ",\"IdRuc\" : " + Listado[i].IdRuc +
                                    // RESERVA
                                    ",\"IdVenta\" : " + Listado[i].IdVenta +

                                    ",\"FechaReservacion\" : \"" + (Listado[i].FechaReservacion ?? "") + "\"" +
                                    ",\"HoraReservacion\" : \"" + (Listado[i].HoraReservacion ?? "").Replace(" ", "") + "\"" +

                                    ",\"HoraEscala\" : \"" + (Listado[i].HoraEscala ?? "").Replace(" ", "") + "\"" +
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
                        result = await response.Content.ReadAsStringAsync();
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
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<CorrelativoResponse> res = new Response<CorrelativoResponse>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new CorrelativoResponse()
                    {
                        CorrelativoVentaBoleta = (string)data["CorrelativoVentaBoleta"],
                        CorrelativoVentaFactura = (string)data["CorrelativoVentaFactura"],
                        CorrelativoPaseBoleta = (string)data["CorrelativoPaseBoleta"],
                        CorrelativoPaseFactura = (string)data["CorrelativoPaseFactura"],
                        CorrelativoCredito = (string)data["CorrelativoCredito"],

                        TipoTerminalElectronico = (string)data["TipoTerminalElectronico"],
                        TipoImpresora = (string)data["TipoImpresora"]
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
                                    ",\"Anno\" : \"" + DataUtility.ObtenerAñoDelSistema() + "\"" +
                                    ",\"Mes\" : \"" + DataUtility.ObtenerMesDelSistema() + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ListaBeneficiarioPase", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                                    ",\"Mes\" : \"" + DataUtility.ObtenerMesDelSistema() + "\"" +
                                    ",\"Anno\" : \"" + DataUtility.ObtenerAñoDelSistema() + "\"" +

                                "}";
                    HttpResponseMessage response = await client.PostAsync("ValidarPase", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        public async Task<ActionResult> ClaveAutorizacion(string password, string CodiTipo)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiOficina\": " + usuario.CodiPuntoVenta +
                                    ",\"Password\": \"" + password + "\"" +
                                    ",\"CodiTipo\": \"" + CodiTipo.PadLeft(3, '0') + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ClavesInternas", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<bool> res = new Response<bool>()
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
        [Route("anular-venta")]
        public async Task<ActionResult> AnularVenta(AnularVentaRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\": " + request.IdVenta +
                                    ",\"CodiUsuario\": " + usuario.CodiUsuario +
                                    ",\"CodiOficina\": " + usuario.CodiSucursal +
                                    ",\"CodiPuntoVenta\": " + usuario.CodiPuntoVenta +
                                    ",\"Tipo\": \"" + request.Tipo + "\"" +
                                    ",\"FlagVenta\": \"" + request.FlagVenta + "\"" +
                                    ",\"PrecioVenta\": \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(request.PrecioVenta) + "\"" +
                                    ",\"FechaViaje\": \"" + request.FechaViaje + "\"" +
                                    ",\"FechaVenta\": \"" + request.FechaVenta + "\"" +
                                    ",\"TipoPago\": \"" + request.TipoPago + "\"" +
                                    ",\"ValeRemoto\": \"" + request.ValeRemoto + "\"" +
                                    ",\"CodiUsuarioBoleto\": " + request.CodiUsuarioBoleto +
                                    ",\"NomUsuario\": \"" + usuario.Nombre + "\"" +
                                    ",\"NumeAsiento\": " + request.NumeAsiento +
                                    ",\"NomOficina\": \"" + usuario.NomSucursal + "\"" +
                                    ",\"NomPasajero\": \"" + request.NomPasajero + "\"" +
                                    ",\"HoraViaje\": \"" + request.HoraViaje.Replace(" ", "") + "\"" +
                                    ",\"NomDestinoPas\": \"" + request.NomDestinoPas + "\"" +
                                    ",\"Terminal\": " + usuario.Terminal +
                                    ",\"CodiEsca\": \"" + request.CodiEsca + "\"" +
                                    ",\"CodiDestinoPas\": \"" + request.CodiDestinoPas + "\"" +
                                    ",\"IngresoManualPasajes\": " + request.IngresoManualPasajes.ToString().ToLower() +
                                    ",\"NomOrigenPas\": \"" + request.NomOrigenPas + "\"" +

                                    ",\"RucEmpresa\": \"" + request.RucEmpresa + "\"" +
                                    ",\"ElectronicoEmpresa\": \"" + request.ElectronicoEmpresa + "\"" +
                                "}";

                    HttpResponseMessage response = await client.PostAsync("AnularVenta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                return Json(new Response<byte>(false, Constant.EXCEPCION, 0, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("verificaNC")]
        public async Task<ActionResult> VerificaNC(int IdVenta)
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

                    HttpResponseMessage response = await client.PostAsync("VerificaNC", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<int> res = new Response<int>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (int)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<int>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("consultaControlTiempo")]
        public async Task<ActionResult> ConsultaControlTiempo(string tipo)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"tipo\": \"" + tipo + "\"" +
                                "}";

                    HttpResponseMessage response = await client.PostAsync("ConsultaControlTiempo", new StringContent(_body, Encoding.UTF8, "application/json"));
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

        [HttpPost]
        [Route("consultaPanelNiveles")]
        public async Task<ActionResult> ConsultaPanelNiveles(int codigo, int Nivel)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"codigo\": " + codigo +
                                    "\"Nivel\": " + Nivel +
                                "}";

                    HttpResponseMessage response = await client.PostAsync("ConsultaPanelNiveles", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("verificaLiquidacionComiDet")]
        public async Task<ActionResult> VerificaLiquidacionComiDet(int IdVenta)
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

                    HttpResponseMessage response = await client.PostAsync("VerificaLiquidacionComiDet", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<int> res = new Response<int>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (int)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<int>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("verificaLiquidacionComi")]
        public async Task<ActionResult> VerificaLiquidacionComi(int CodiProgramacion, int Pvta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiProgramacion\": " + CodiProgramacion +
                                    ",\"Pvta\": " + Pvta +
                                "}";

                    HttpResponseMessage response = await client.PostAsync("VerificaLiquidacionComi", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("consultaVentaIdaV")]
        public async Task<ActionResult> ConsultaVentaIdaV(int IdVenta)
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

                    HttpResponseMessage response = await client.PostAsync("ConsultaVentaIdaV", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        [Route("grabarAuditoria")]
        public async Task<ActionResult> GrabarAuditoria(AuditoriaRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiUsuario\" : " + request.CodiUsuario +
                                    ",\"NomUsuario\" :  \"" + (request.NomUsuario ?? "") + "\"" +
                                    ",\"Tabla\" : \"" + (request.Tabla ?? "") + "\"" +
                                    ",\"TipoMovimiento\" : \"" + (request.TipoMovimiento ?? "") + "\"" +
                                    ",\"Boleto\" : \"" + (request.Boleto ?? "") + "\"" +
                                    ",\"NumeAsiento\" : \"" + (request.NumeAsiento ?? "") + "\"" +
                                    ",\"NomOficina\" : \"" + (request.NomOficina ?? "") + "\"" +
                                    ",\"NomPuntoVenta\" : \"" + (request.NomPuntoVenta ?? "") + "\"" +
                                    ",\"Pasajero\" : \"" + (request.Pasajero ?? "") + "\"" +
                                    ",\"FechaViaje\" : \"" + (request.FechaViaje ?? "") + "\"" +
                                    ",\"HoraViaje\" : \"" + (request.HoraViaje ?? "").Replace(" ", "") + "\"" +
                                    ",\"NomDestino\" : \"" + (request.NomDestino ?? "") + "\"" +
                                    ",\"Precio\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(request.Precio) + "\"" +
                                    ",\"Obs1\" : \"" + (request.Obs1 ?? "").ToUpper() + "\"" +
                                    ",\"Obs2\" : \"" + (request.Obs2 ?? "").ToUpper() + "\"" +
                                    ",\"Obs3\" : \"" + (request.Obs3 ?? "").ToUpper() + "\"" +
                                    ",\"Obs4\" : \"" + (request.Obs4 ?? "").ToUpper() + "\"" +
                                    ",\"Obs5\" : \"" + (request.Obs5 ?? "").ToUpper() + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("GrabarAuditoria", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        [Route("consultaClaveAnuRei")]
        public async Task<ActionResult> ConsultaClaveAnuRei(int CodiUsuario, string Clave)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiUsuario\": " + CodiUsuario +
                                    ",\"Clave\": \"" + Clave + "\"" +
                                "}";

                    HttpResponseMessage response = await client.PostAsync("ConsultaClaveAnuRei", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        [Route("consultaClaveControl")]
        public async Task<ActionResult> ConsultaClaveControl(short Usuario, string Pwd)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Usuario\": " + Usuario +
                                    ",\"Pwd\": \"" + Pwd + "\"" +
                                "}";

                    HttpResponseMessage response = await client.PostAsync("ConsultaClaveControl", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        [Route("insertarUsuarioPorVenta")]
        public async Task<ActionResult> InsertarUsuarioPorVenta(string Usuario, string Accion, decimal IdVenta, string Motivo)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Usuario\": \"" + Usuario + "\"" +
                                    ",\"Accion\": \"" + Accion + "\"" +
                                    ",\"IdVenta\": " + IdVenta +
                                    ",\"Motivo\": \"" + Motivo + "\"" +
                                "}";

                    HttpResponseMessage response = await client.PostAsync("InsertarUsuarioPorVenta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                                    ",\"CodiEmpresa\" : " + CodiEmpresa +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscarVentaxBoleto", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<VentaBeneficiario> res = new Response<VentaBeneficiario>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new VentaBeneficiario
                    {
                        IdVenta = (int)data["IdVenta"],
                        NombresConcat = (string)data["NombresConcat"],
                        CodiOrigen = (short)data["CodiOrigen"],
                        CodiDestino = (short)data["CodiDestino"],
                        FechaViaje = (string)data["FechaViaje"],
                        HoraViaje = (string)data["HoraViaje"],
                        NumeAsiento = (byte)data["NumeAsiento"],
                        CodiServicio = (byte)data["CodiServicio"],
                        CodiProgramacion = (int)data["CodiProgramacion"],
                        CodiPuntoVenta = (short)data["CodiPuntoVenta"],
                        FechaProgramacion = (string)data["FechaProgramacion"],
                        CodiServicioProgramacion = (byte)data["CodiServicioProgramacion"],
                        FlagVenta = (string)data["FlagVenta"],
                        TipoDocumento = (string)data["TipoDocumento"],
                        Documento = (string)data["Documento"],
                        ImpManifiesto = (string)data["ImpManifiesto"],
                        Cierre = (string)data["Cierre"],
                        NivelAsiento = (string)data["NivelAsiento"],
                        CodiRuta = (short)data["CodiRuta"],
                        PrecioVenta = (decimal)data["PrecioVenta"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
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
                                    ",\"CodiUsuario\" : " + usuario.CodiUsuario +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" +
                                    ",\"CodiPuntoVenta\" : " + usuario.CodiPuntoVenta +
                                    ",\"CodiTerminal\" : \"" + usuario.Terminal.ToString("D3") + "\"" +
                                    ",\"CodiOrigen\" : " + filtro.CodiOrigen +
                                    ",\"CodiDestino\" : " + filtro.CodiDestino +
                                    ",\"NomOrigen\" : \"" + filtro.NomOrigen + "\"" +
                                    ",\"CodiOrigenBoleto\" : \"" + filtro.CodiOrigenBoleto + "\"" +
                                    ",\"CodiRutaBoleto\" : \"" + filtro.CodiRutaBoleto + "\"" +
                                    ",\"BoletoCompleto\" : \"" + filtro.BoletoCompleto + "\"" +
                                    ",\"CodiEmpresaUsuario\" : \"" + usuario.CodiEmpresa + "\"" +
                                    ",\"CodiSucursalUsuario\" : \"" + usuario.CodiSucursal + "\"" +
                                    ",\"NomSucursalUsuario\" : \"" + usuario.NomSucursal + "\"" +
                                    ",\"NomPasajero\" : \"" + filtro.NomPasajero + "\"" +
                                    ",\"FechaViajeBoleto\" : \"" + filtro.FechaViajeBoleto + "\"" +
                                    ",\"HoraViajeBoleto\" : \"" + filtro.HoraViajeBoleto + "\"" +
                                    ",\"NomDestinoBoleto\" : \"" + filtro.NomDestinoBoleto + "\"" +
                                    ",\"TipoPostergacion\" : \"" + filtro.TipoPostergacion + "\"" +
                                    ",\"NumeAsientoBoleto\" : " + filtro.NumeAsientoBoleto +
                                    ",\"NomDestino\" : \"" + filtro.NomDestino + "\"" +
                                    ",\"PrecioVenta\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(filtro.PrecioVenta) + "\"" +

                                    ",\"PrecioVentaBoleto\" : \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(filtro.PrecioVentaBoleto) + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("PostergarVenta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                return Json(new Response<VentaResponse>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("modificar-venta-a-fecha-abierta")]
        public async Task<ActionResult> ModificarVentaAFechaAbierta(VentaToFechaAbiertaRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\" : " + request.IdVenta +
                                    ",\"CodiServicio\" : " + request.CodiServicio +
                                    ",\"CodiRuta\" : " + request.CodiRuta +
                                    ",\"CodiUsuario\" : " + usuario.CodiUsuario +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" +
                                    ",\"CodiTerminal\" : \"" + usuario.Terminal.ToString("D3") + "\"" +
                                    ",\"NomOficina\" : \"" + usuario.NomSucursal + "\"" +
                                    ",\"CodiPuntoVenta\" : " + usuario.CodiPuntoVenta +
                                    ",\"BoletoCompleto\" : \"" + request.BoletoCompleto + "\"" +
                                    ",\"NumeAsiento\" : " + request.NumeAsiento +
                                    ",\"Pasajero\" : \"" + request.Pasajero + "\"" +
                                    ",\"FechaViaje\" : \"" + request.FechaViaje + "\"" +
                                    ",\"HoraViaje\" : \"" + request.HoraViaje.Replace(" ", "") + "\"" +
                                    ",\"NomDestino\" : \"" + request.NomDestino + "\"" +
                                    ",\"PrecioVenta\" : " + request.PrecioVenta +
                                    ",\"CodiOrigen\" : \"" + request.CodiOrigen + "\"" +
                                    ",\"CodiProgramacion\" : " + request.CodiProgramacion +

                                    ",\"CodiEmpresa\" : " + request.CodiEmpresa +
                                    ",\"CodiSucursal\" : \"" + usuario.CodiSucursal + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ModificarVentaAFechaAbierta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<bool> res = new Response<bool>()
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
        [Route("consultaSumaBoletosPostergados")]
        public async Task<ActionResult> ConsultaSumaBoletosPostergados(string CodTab, string CodEmp, string Tipo, string Numero, string Emp)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodTab\" : \"" + CodTab + "\"" +
                                    ",\"CodEmp\" : \"" + CodEmp + "\"" +
                                    ",\"Tipo\": \"" + Tipo + "\"" +
                                    ",\"Numero\": \"" + Numero + "\"" +
                                    ",\"Emp\": \"" + Emp + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultaSumaBoletosPostergados", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        [Route("cancelar-reserva")]
        public async Task<ActionResult> EliminarReserva(CancelarReservaRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\": " + request.IdVenta +
                                    ",\"Boleto\" : \"" + request.Boleto + "\"" +
                                    ",\"NumeAsiento\": " + request.NumeAsiento +
                                    ",\"NomPasajero\": \"" + request.NomPasajero + "\"" +
                                    ",\"FechaViaje\": \"" + request.FechaViaje + "\"" +
                                    ",\"HoraViaje\": \"" + request.HoraViaje.Replace(" ", "") + "\"" +
                                    ",\"NomDestinoPas\": \"" + request.NomDestinoPas + "\"" +
                                    ",\"PrecioVenta\": \"" + DataUtility.ConvertDecimalToStringWithTwoDecimals(request.PrecioVenta) + "\"" +
                                    ",\"CodiUsuario\" : " + usuario.CodiUsuario +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" +
                                    ",\"NomOficina\": \"" + usuario.NomSucursal + "\"" +
                                    ",\"NomPuntoVenta\": \"" + usuario.NomPuntoVenta + "\"" +
                                    ",\"Terminal\": " + usuario.Terminal +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("EliminarReserva", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
                return Json(new Response<byte>(false, Constant.EXCEPCION, 0, false), JsonRequestBehavior.AllowGet);
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
                        result = await response.Content.ReadAsStringAsync();
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
                    }).ToList(),
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<ClienteCredito>>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("convertir-venta-to-base64")]
        public async Task<ActionResult> ConvertirVentaToBase64(List<VentaRealizada> ListaVentasRealizadas, string TipoImpresion)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    string _body = string.Empty;

                    _body += "{";

                    _body += "\"ListaVentasRealizadas\" : [";
                    for (var i = 0; i < ListaVentasRealizadas.Count; i++)
                    {
                        _body += "{" +
                                    "\"IdVenta\": " + ListaVentasRealizadas[i].IdVenta +
                                    ",\"NumeAsiento\" : \"" + ListaVentasRealizadas[i].NumeAsiento + "\"" +
                                    ",\"BoletoCompleto\" : \"" + ListaVentasRealizadas[i].BoletoCompleto + "\"" +
                                    ",\"BoletoTipo\" : \"" + ListaVentasRealizadas[i].BoletoTipo + "\"" +
                                    ",\"BoletoSerie\" : \"" + ListaVentasRealizadas[i].BoletoSerie + "\"" +
                                    ",\"BoletoNum\" : \"" + ListaVentasRealizadas[i].BoletoNum + "\"" +
                                    ",\"CodDocumento\" : \"" + ListaVentasRealizadas[i].CodDocumento + "\"" +
                                    ",\"EmisionFecha\" : \"" + ListaVentasRealizadas[i].EmisionFecha + "\"" +
                                    ",\"EmisionHora\" : \"" + ListaVentasRealizadas[i].EmisionHora + "\"" +
                                    ",\"CajeroCod\": " + ListaVentasRealizadas[i].CajeroCod +
                                    ",\"CajeroNom\" : \"" + ListaVentasRealizadas[i].CajeroNom + "\"" +
                                    ",\"PasNombreCom\" : \"" + ListaVentasRealizadas[i].PasNombreCom + "\"" +
                                    ",\"PasRuc\" : \"" + ListaVentasRealizadas[i].PasRuc + "\"" +
                                    ",\"PasRazSocial\" : \"" + ListaVentasRealizadas[i].PasRazSocial + "\"" +
                                    ",\"PasDireccion\" : \"" + ListaVentasRealizadas[i].PasDireccion + "\"" +
                                    ",\"NomOriPas\" : \"" + ListaVentasRealizadas[i].NomOriPas + "\"" +
                                    ",\"NomDesPas\" : \"" + ListaVentasRealizadas[i].NomDesPas + "\"" +
                                    ",\"DocTipo\": " + ListaVentasRealizadas[i].DocTipo +
                                    ",\"DocNumero\" : \"" + ListaVentasRealizadas[i].DocNumero + "\"" +
                                    ",\"PrecioCan\" : \"" + ListaVentasRealizadas[i].PrecioCan + "\"" +
                                    ",\"PrecioDes\" : \"" + ListaVentasRealizadas[i].PrecioDes + "\"" +
                                    ",\"NomServicio\" : \"" + ListaVentasRealizadas[i].NomServicio + "\"" +
                                    ",\"FechaViaje\" : \"" + ListaVentasRealizadas[i].FechaViaje + "\"" +
                                    ",\"EmbarqueDir\" : \"" + ListaVentasRealizadas[i].EmbarqueDir + "\"" +
                                    ",\"EmbarqueHora\" : \"" + ListaVentasRealizadas[i].EmbarqueHora + "\"" +
                                    ",\"CodigoX_FE\" : \"" + ListaVentasRealizadas[i].CodigoX_FE + "\"" +
                                    ",\"TipoTerminalElectronico\" : \"" + ListaVentasRealizadas[i].TipoTerminalElectronico + "\"" +
                                    ",\"TipoImpresora\": " + ListaVentasRealizadas[i].TipoImpresora +
                                    ",\"EmpDirAgencia\" : \"" + ListaVentasRealizadas[i].EmpDirAgencia + "\"" +
                                    ",\"EmpTelefono1\" : \"" + ListaVentasRealizadas[i].EmpTelefono1 + "\"" +
                                    ",\"EmpTelefono2\" : \"" + ListaVentasRealizadas[i].EmpTelefono2 + "\"" +
                                    ",\"PolizaNum\" : \"" + ListaVentasRealizadas[i].PolizaNum + "\"" +
                                    ",\"PolizaFechaReg\" : \"" + ListaVentasRealizadas[i].PolizaFechaReg + "\"" +
                                    ",\"PolizaFechaVen\" : \"" + ListaVentasRealizadas[i].PolizaFechaVen + "\"" +

                                    ",\"EmpRuc\" : \"" + ListaVentasRealizadas[i].EmpRuc + "\"" +
                                    ",\"EmpRazSocial\" : \"" + ListaVentasRealizadas[i].EmpRazSocial + "\"" +
                                    ",\"EmpDireccion\" : \"" + ListaVentasRealizadas[i].EmpDireccion + "\"" +
                                    ",\"EmpElectronico\": \"" + ListaVentasRealizadas[i].EmpElectronico + "\"" +

                                    // Parámetros extras
                                    ",\"EmpCodigo\" : " + ListaVentasRealizadas[i].EmpCodigo +
                                    ",\"PVentaCodigo\" : " + ListaVentasRealizadas[i].PVentaCodigo +
                                    ",\"BusCodigo\" : \"" + ListaVentasRealizadas[i].BusCodigo + "\"" +
                                    ",\"EmbarqueCod\" : " + ListaVentasRealizadas[i].EmbarqueCod +
                                    // Para 'Reimpresión'
                                    ",\"UsuarioCodigo\" : " + usuario.CodiUsuario +
                                    ",\"UsuarioNombre\" : \"" + usuario.Nombre + "\"" +
                                    ",\"UsuarioCodOficina\" : " + usuario.CodiSucursal +
                                    ",\"UsuarioNomOficina\" : \"" + usuario.NomSucursal + "\"" +
                                    ",\"UsuarioCodPVenta\" : " + usuario.CodiPuntoVenta +
                                    ",\"UsuarioCodTerminal\" : \"" + usuario.Terminal.ToString("D3") + "\"" +

                                    ",\"ValidateCaja\" : " + ListaVentasRealizadas[i].ValidateCaja.ToString().ToLower() +
                                    ",\"HoraViaje\" : \"" + (ListaVentasRealizadas[i].HoraViaje ?? "").Replace(" ", "") + "\"" +
                                    //NEW
                                    ",\"TipoPago\" : \"" + ListaVentasRealizadas[i].TipoPago + "\"" +
                                    ",\"FlagVenta\" : \"" + ListaVentasRealizadas[i].FlagVenta + "\"" +
                                    ",\"CodiEsca\" : \"" + ListaVentasRealizadas[i].CodiEsca + "\"" + // Solo para 'Reintegro'
                                "}";

                        if (i < ListaVentasRealizadas.Count - 1)
                            _body += ",";
                    }
                    _body += "]";

                    // TIPO DE VENTA
                    _body += ",\"TipoImpresion\" : \"" + TipoImpresion + "\"";
                    // -------------

                    _body += "}";

                    HttpResponseMessage response = await client.PostAsync("ConvertirVentaToBase64", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Impresion>> res = new Response<List<Impresion>>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Impresion
                    {
                        Original = (string)x["Original"],
                        Copia1 = (string)x["Copia1"],
                        Copia2 = (string)x["Copia2"]
                    }).ToList(),
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Impresion>>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("listarPanelesControl")]
        public async Task<ActionResult> ListarPanelesControl()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListarPanelesControl");
                    HttpResponseMessage response = await client.GetAsync(url + "ListarPanelesControl");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<PanelControlResponse> res = new Response<PanelControlResponse>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new PanelControlResponse
                    {
                        ListarPanelControl = _listaPanelControl(data["ListarPanelControl"]),
                        ListarPanelControlClave = _listaPanelControl(data["ListarPanelControlClave"]),
                        ListarPanelControlNivel = _listaPanelControlNivel(data["ListarPanelControlNivel"])
                    }
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<PanelControlResponse>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
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
                        result = await response.Content.ReadAsStringAsync();
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
                        result = await response.Content.ReadAsStringAsync();
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

        [HttpPost]
        [Route("buscarClientesPasaje")]
        public async Task<ActionResult> BuscarClientesPasaje(string campo, string nombres, string paterno, string materno, string TipoDocId)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"campo\" : \"" + campo + "\"" +
                                    ",\"nombres\" : \"" + nombres + "\"" +
                                    ",\"paterno\" : \"" + paterno + "\"" +
                                    ",\"materno\" : \"" + materno + "\"" +
                                    ",\"TipoDocId\" : \"" + TipoDocId + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscarClientesPasaje", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<ClientePasaje>> res = new Response<List<ClientePasaje>>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new ClientePasaje
                    {
                        NumeroDoc = (string)x["NumeroDoc"],
                        NombreCliente = (string)x["NombreCliente"],
                        ApellidoPaterno = (string)x["ApellidoPaterno"],
                        ApellidoMaterno = (string)x["ApellidoMaterno"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<ClientePasaje>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("verificaManifiestoPorPVenta")]
        public async Task<ActionResult> VerificaManifiestoPorPVenta(int CodiProgramacion, short Pvta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiProgramacion\" : " + CodiProgramacion +
                                    ",\"Pvta\" : " + Pvta +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VerificaManifiestoPorPVenta", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("consultaConfigManifiestoPorHora")]
        public async Task<ActionResult> ConsultaConfigManifiestoPorHora(short CodiEmpresa, short CodiSucursal, short CodiPuntoVenta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiEmpresa\" : " + CodiEmpresa +
                                    ",\"CodiSucursal\" : " + CodiSucursal +
                                    ",\"CodiPuntoVenta\" : " + CodiPuntoVenta +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultaConfigManifiestoPorHora", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("actualizarProgramacionManifiesto")]
        public async Task<ActionResult> ActualizarProgramacionManifiesto(ManifiestoRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiEmpresa\" : " + request.CodiEmpresa +
                                    ",\"CodiProgramacion\" : " + request.CodiProgramacion +
                                    ",\"CodiSucursal\" : " + usuario.CodiSucursal +
                                    ",\"TipoApertura\" : " + request.TipoApertura.ToString().ToLower() +
                                    ",\"CodiSucursalBus\" : " + request.CodiSucursalBus +
                                    ",\"CodiUsuario\" : " + usuario.CodiUsuario +
                                    ",\"NomUsuario\" : \"" + usuario.Nombre + "\"" +
                                    ",\"NumBoleto\" : \"" + request.NumBoleto + "\"" +
                                    ",\"CodiPuntoVenta\" : \"" + usuario.CodiPuntoVenta + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ActualizarProgramacionManifiesto", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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

        //Buscar para Modificación
        [HttpPost]
        [Route("buscaBoletoF9")]
        public async Task<ActionResult> BuscaBoletoF9(int Serie, int Numero, string Tipo, int CodEmpresa)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Serie\" : " + Serie +
                                    ",\"Numero\" : " + Numero +
                                    ",\"Tipo\" : \"" + Tipo + "\"" +
                                    ",\"CodEmpresa\" : " + CodEmpresa +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("BuscaBoletoF9", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Busca> res = new Response<Busca>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Busca()
                    {
                        FechaProgramacion = (string)data["FechaProgramacion"],
                        Tipo = (string)data["Tipo"],
                        SerieBoleto = (short)data["SerieBoleto"],
                        NumeBoleto = (int)data["NumeBoleto"],
                        CodiEmpresa = (byte)data["CodiEmpresa"],
                        CodiSucursal = (short)data["CodiSucursal"],
                        CodiProgramacion = (int)data["CodiProgramacion"],
                        CodiSubruta = (int)data["CodiSubruta"],
                        CodiCliente = (int)data["CodiCliente"],
                        RucCliente = (string)data["RucCliente"],
                        PrecioVenta = (int)data["PrecioVenta"],
                        NumeAsiento = (byte)data["NumeAsiento"],
                        FlagVenta = (string)data["FlagVenta"],
                        FechaVenta = (string)data["FechaVenta"],
                        RecoVenta = (string)data["RecoVenta"],
                        ClavUsuario = (int)data["ClavUsuario"],
                        IndiAnulado = (string)data["IndiAnulado"],
                        FechaAnulacion = (string)data["FechaAnulacion"],
                        Dni = (string)data["Dni"],
                        Edad = (byte)data["Edad"],
                        Telefono = (string)data["Telefono"],
                        Nombre = (string)data["Nombre"],
                        CodiEsca = (string)data["CodiEsca"],
                        CodiPuntoVenta = (short)data["CodiPuntoVenta"],
                        TipoDocumento = (string)data["TipoDocumento"],
                        CodiOrigen = (short)data["CodiOrigen"],
                        PerAutoriza = (string)data["PerAutoriza"],
                        ClavUsuario1 = (int)data["ClavUsuario1"],
                        EstadoAsiento = (string)data["EstadoAsiento"],
                        Sexo = (string)data["Sexo"],
                        TipoPago = (string)data["TipoPago"],
                        CodiSucursalVenta = (int)data["CodiSucursalVenta"],
                        IdVenta = (int)data["IdVenta"],
                        ValeRemoto = (string)data["ValeRemoto"],
                        IdVentaWeb = (int)data["IdVentaWeb"],
                        ImpManifiesto = (int)data["ImpManifiesto"],
                        TipoVenta = (string)data["TipoVenta"],
                        CodiRuta = (short)data["CodiRuta"],
                        HoraProgramacion = (string)data["HoraProgramacion"],
                        CodiServicio = (byte)data["CodiServicio"],
                        Nacionalidad = (string)data["Nacionalidad"],
                        RazonSocial = (string)data["RazonSocial"],
                        DireccionFiscal = (string)data["DireccionFiscal"],
                        FechaViaje = (string)data["FechaViaje"],
                        HoraViaje = (string)data["HoraViaje"]
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Busca>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        //Enviar Modificación
        [HttpPost]
        [Route("actualizaBoletoF9")]
        public async Task<ActionResult> ActualizaBoletoF9(FiltroBoleto request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\" : \"" + request.IdVenta + "\"" +
                                    ",\"Dni\" : \"" + request.Dni + "\"" +
                                    ",\"Nombre\" : \"" + request.Nombre + "\"" +
                                    ",\"Ruc\" : \"" + request.Ruc + "\"" +
                                    ",\"Edad\" : \"" + request.Edad + "\"" +
                                    ",\"Telefono\" : \"" + request.Telefono + "\"" +
                                    ",\"RecoVenta\" : \"" + request.RecoVenta + "\"" +
                                    ",\"TipoDoc\" : \"" + request.TipoDoc + "\"" +
                                    ",\"Nacionalidad\" :  \"" + request.Nacionalidad + "\"" +

                                    ",\"CodiUsuario\" :  \"" + usuario.CodiUsuario + "\"" +
                                    ",\"NombUsuario\" :  \"" + usuario.Nombre + "\"" +
                                    ",\"NomSucursal\" :  \"" + usuario.NomSucursal + "\"" +
                                    ",\"CodiPuntoVenta\" :  \"" + usuario.CodiPuntoVenta + "\"" +
                                    ",\"Terminal\" :  \"" + usuario.Terminal + "\"" +
                                    ",\"Boleto\" :  \"" + request.SerieBoleto.PadLeft(3, '0') + "-" + request.NumeBoleto.PadLeft(7, '0') + "\"" +
                                    ",\"Precio\" :  \"" + request.Precio + "\"" +
                                    ",\"NombDestino\" :  \"" + request.NombDestino + "\"" +
                                    ",\"NumAsiento\" :  \"" + request.NumAsiento.PadLeft(2, '0') + "\"" +
                                    ",\"FechaViaje\" :  \"" + request.FechaViaje + "\"" +
                                    ",\"HoraViaje\" :  \"" + request.HoraViaje + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ActualizaBoletoF9", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        [Route("consultaManifiestoProgramacion")]
        public async Task<ActionResult> ConsultaManifiestoProgramacion(int Prog, string Suc)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Prog\" : " + Prog +
                                    ",\"Suc\" : \"" + Suc + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ConsultaManifiestoProgramacion", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("obtenerStAnulacion")]
        public async Task<ActionResult> ObtenerStAnulacion(string CodTab, int Pv, string F)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodTab\" : \"" + CodTab + "\"" +
                                    ",\"Pv\" : " + Pv +
                                    ",\"F\" : \"" + F + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ObtenerStAnulacion", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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

        //Buscar Fecha Abierta
        [HttpPost]
        [Route("ventaConsultaF6")]
        public async Task<ActionResult> VentaConsultaF6(FiltroFechaAbierta filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Nombre\" : \"" + (filtro.Nombre ?? "") + "\"" +
                                    ",\"Dni\" : \"" + (filtro.Dni ?? "") + "\"" +
                                    ",\"Fecha\" : \"" + (filtro.Fecha ?? "") + "\"" +
                                    ",\"Tipo\" : \"" + (filtro.Tipo ?? "") + "\"" +
                                    ",\"Serie\" : \"" + (filtro.Serie ?? "0") + "\"" +
                                    ",\"Numero\" : \"" + (filtro.Numero ?? "0") + "\"" +
                                    ",\"CodEmpresa\" : \"" + (filtro.CodEmpresa ?? "0") + "\"" +
                                    ",\"CodiDestino\" : \"" + (filtro.CodiDestino ?? "0") + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VentaConsultaF6", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<FechaAbierta>> res = new Response<List<FechaAbierta>>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new FechaAbierta
                    {
                        Nombre = (string)x["Nombre"],
                        Tipo = (string)x["Tipo"],
                        Serie = (string)x["Serie"],
                        Numero = (string)x["Numero"],
                        FechaVenta = (string)x["FechaVenta"],
                        PrecioVenta = (string)x["PrecioVenta"],
                        CodiSubruta = (string)x["CodiSubruta"],
                        CodiOrigen = (string)x["CodiOrigen"],
                        CodiEmpresa = (string)x["CodiEmpresa"],
                        IdVenta = (string)x["IdVenta"],
                        StRemoto = (string)x["StRemoto"],
                        Dni = (string)x["Dni"],
                        TipoDoc = (string)x["TipoDoc"],
                        CodiEsca = (string)x["CodiEsca"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<bool>(false, Constant.EXCEPCION, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("validateNivelAsiento")]
        public async Task<ActionResult> ValidateNivelAsiento(int IdVenta, string CodiBus, string Asiento)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\" : " + IdVenta +
                                    ",\"CodiBus\" : \"" + (CodiBus ?? "000") + "\"" +
                                    ",\"Asiento\" : \"" + (Asiento ?? "00") + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ValidateNivelAsiento", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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

        [HttpGet]
        [Route("validateNumDias")]
        public async Task<ActionResult> ValidateNumDias(string FechaVenta, string CodTab)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"FechaVenta\" : \"" + FechaVenta + "\"" +
                                    ",\"CodTab\" : \"" + CodTab + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ValidateNumDias", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<int> res = new Response<int>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (int)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<int>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("verificaNotaCredito")]
        public async Task<ActionResult> VerificaNotaCredito(int IdVenta)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\" : " + IdVenta +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VerificaNotaCredito", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<int> res = new Response<int>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (int)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<int>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }

        //Confrimar Fecha Abierta
        [HttpPost]
        [Route("confirmar-fecha-abierta")]
        public async Task<ActionResult> VentaUpdatePostergacionEle(FiltroFechaAbierta filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiEsca\" : \"" + (filtro.CodiEsca ?? "") + "\"" +
                                    ",\"CodiProgramacion\" : " + filtro.CodiProgramacion +
                                    ",\"CodiOrigen\" : \"" + (filtro.CodiOrigen ?? "") + "\"" +
                                    ",\"IdVenta\" : " + filtro.IdVenta +
                                    ",\"NumeAsiento\" : \"" + (filtro.NumeAsiento ?? "") + "\"" +
                                    ",\"CodiRuta\" : \"" + (filtro.CodiRuta ?? "") + "\"" +
                                    ",\"CodiServicio\" : \"" + (filtro.CodiServicio ?? "") + "\"" +
                                    ",\"Tipo\" : \"" + (filtro.Tipo ?? "") + "\"" +
                                    ",\"Oficina\" : " + filtro.Oficina +
                                    ",\"FechaViaje\" : \"" + (filtro.FechaViaje ?? "") + "\"" +
                                    ",\"HoraViaje\" : \"" + (filtro.HoraViaje ?? "").Replace(" ", "") + "\"" +
                                    ",\"NroViaje\" : " + filtro.NroViaje +
                                    ",\"FechaProgramacion\" : \"" + filtro.FechaProgramacion + "\"" +
                                    ",\"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                    ",\"CodiSucursal\" : " + filtro.CodiSucursal +
                                    ",\"CodiRutaBus\" : " + filtro.CodiRutaBus +
                                    ",\"CodiBus\" : \"" + filtro.CodiBus + "\"" +
                                    ",\"HoraProgramacion\" : \"" + filtro.HoraProgramacion + "\"" +
                                    ",\"CodiDestino\" : \"" + (filtro.CodiDestino ?? "") + "\"" +
                                    ",\"NombDestino\" : \"" + (filtro.NombDestino ?? "") + "\"" +
                                    ",\"Precio\" : \"" + (filtro.Precio ?? "0.00") + "\"" +
                                    ",\"CodiUsuario\" : " + usuario.CodiUsuario +
                                    ",\"NomUsuario\" : \"" + (usuario.Nombre ?? "") + "\"" +
                                    ",\"NomSucursal\" : \"" + (usuario.NomSucursal ?? "") + "\"" +
                                    ",\"CodiPuntoVenta\" : \"" + usuario.CodiPuntoVenta.ToString() + "\"" +
                                    ",\"Terminal\" : \"" + usuario.Terminal.ToString() + "\"" +
                                    ",\"Nombre\" : \"" + (filtro.Nombre ?? "") + "\"" +
                                    ",\"Serie\" : \"" + (filtro.Serie ?? "") + "\"" +
                                    ",\"Numero\" : \"" + (filtro.Numero ?? "") + "\"" +
                                "}";
                    var _test = usuario.CodiUsuario;
                    HttpResponseMessage response = await client.PostAsync("VentaUpdatePostergacionEle", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<VentaResponse> res = new Response<VentaResponse>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new VentaResponse()
                    {
                        ListaVentasRealizadas = _listVentasRealizadas(data["ListaVentasRealizadas"]),
                        CodiProgramacion = (int)data["CodiProgramacion"]
                    }
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<bool>(false, Constant.EXCEPCION, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("acompanianteVentaCRUD")]
        public async Task<ActionResult> AcompanianteVentaCRUD(AcompanianteRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"IdVenta\" : " + request.IdVenta +
                                    ",\"Acompaniante\" : { " +
                                        "\"TipoDocumento\" : \"" + request.Acompaniante.CodiTipoDoc + "\"" +
                                        ",\"NumeroDocumento\" : \"" + request.Acompaniante.Documento + "\"" +
                                        ",\"NombreCompleto\" : \"" + request.Acompaniante.NombreCompleto + "\"" +
                                        ",\"FechaNacimiento\" : \"" + request.Acompaniante.FechaNac + "\"" +
                                        ",\"Edad\" : \"" + request.Acompaniante.Edad + "\"" +
                                        ",\"Sexo\" : \"" + request.Acompaniante.Sexo + "\"" +
                                        ",\"Parentesco\" : \"" + request.Acompaniante.Parentesco + "\"" +
                                    " }" +
                                    ",\"ActionType\" : " + request.ActionType +
                                "}";

                    HttpResponseMessage response = await client.PostAsync("AcompanianteVentaCRUD", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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

        //Busca Reintegro
        [HttpPost]
        [Route("ventaConsultaF12")]
        public async Task<ActionResult> VentaConsultaF12(FiltroReintegro filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Serie\" : " + filtro.Serie +
                                    ",\"Numero\" : " + filtro.Numero +
                                    ",\"CodiEmpresa\" : " + filtro.CodiEmpresa +
                                    ",\"Tipo\" : \"" + (filtro.Tipo ?? "") + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VentaConsultaF12", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Reintegro> res = new Response<Reintegro>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Reintegro()
                    {
                        SerieBoleto = (short)data["SerieBoleto"],
                        NumeBoleto = (int)data["NumeBoleto"],
                        CodiEmpresa = (byte)data["CodiEmpresa"],
                        TipoDocumento = (string)data["TipoDocumento"],
                        CodiEsca = (string)data["CodiEsca"],
                        FlagVenta = (string)data["FlagVenta"],
                        IndiAnulado = (string)data["IndiAnulado"],
                        IdVenta = (int)data["IdVenta"],
                        Dni = (string)data["Dni"],
                        Nombre = (string)data["Nombre"],
                        RucCliente = (string)data["RucCliente"],
                        NumeAsiento = (byte)data["NumeAsiento"],
                        PrecioVenta = (decimal)data["PrecioVenta"],
                        CodiDestino = (short)data["CodiDestino"],
                        FechaViaje = (string)data["FechaViaje"],
                        HoraViaje = (string)data["HoraViaje"],
                        CodiProgramacion = (int)data["CodiProgramacion"],
                        CodiOrigen = (short)data["CodiOrigen"],
                        CodiEmbarque = (short)data["CodiEmbarque"],
                        CodiArribo = (short)data["CodiArribo"],
                        Edad = (byte)data["Edad"],
                        Telefono = (string)data["Telefono"],
                        Nacionalidad = (string)data["Nacionalidad"],
                        Tipo = (string)data["Tipo"],
                        RazonSocial = (string)data["RazonSocial"],
                        Direccion = (string)data["Direccion"],
                        CodiRuta = (byte)data["CodiRuta"],
                        CodiServicio = (byte)data["CodiServicio"],
                        CodiError = (int)data["CodiError"],
                        FechaNac = (string)data["FechaNac"],
                        CodiBus = (string)data["CodiBus"],
                        DirEmbarque = (string)data["DirEmbarque"],
                        FechaVenta = (string)data["FechaVenta"],
                        TipoPago = (string)data["TipoPago"],
                        CodiTarjetaCredito = (string)data["CodiTarjetaCredito"],
                        NumeTarjetaCredito = (string)data["NumeTarjetaCredito"]

                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Reintegro>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("validaExDni")]
        public async Task<ActionResult> ValidaExDni(string documento)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"documento\" : \"" + documento + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ValidaExDni", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        [Route("verificaClaveReserva")]
        public async Task<ActionResult> VerificaClaveReserva(int CodiUsr, string Password)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiUsr\" : " + CodiUsr +
                                    ",\"Password\" : \"" + Password + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VerificaClaveReserva", new StringContent(_body, Encoding.UTF8, "application/json"));

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
                return Json(new Response<string>(false, Constant.EXCEPCION, null, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("verificaClaveTbClaveRe")]
        public async Task<ActionResult> VerificaClaveTbClaveRe(int CodiUsr)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiUsr\" : " + CodiUsr +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VerificaClaveTbClaveRe", new StringContent(_body, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("verificaHoraConfirmacion")]
        public async Task<ActionResult> VerificaHoraConfirmacion(int Origen, int Destino)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Origen\" : " + Origen +
                                    ",\"Destino\" : " + Destino +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VerificaHoraConfirmacion", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<string> res = new Response<string>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (string)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("actualizarAsiOcuTbBloqueoAsientos")]
        public async Task<ActionResult> ActualizarAsiOcuTbBloqueoAsientos(TablaBloqueoAsientosRequest request)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiProgramacion\" : " + request.CodiProgramacion +
                                    ",\"CodiOrigen\" : " + request.CodiOrigen +
                                    ",\"CodiDestino\" : " + request.CodiDestino +
                                    ",\"AsientosOcupados\" : \"" + (request.AsientosOcupados ?? string.Empty) + "\"" +
                                    ",\"AsientosLiberados\" : \"" + request.AsientosLiberados + "\"" +
                                    ",\"Fecha\" : \"" + request.Fecha + "\"" +

                                    ",\"NroViaje\" : " + request.NroViaje +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ActualizarAsiOcuTbBloqueoAsientos", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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

        [HttpGet]
        [Route("obtenerTiempoReserva")]
        public async Task<ActionResult> ObtenerTiempoReserva()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ObtenerTiempoReserva");
                    HttpResponseMessage response = await client.GetAsync(url + "ObtenerTiempoReserva");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Reservacion> res = new Response<Reservacion>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Reservacion
                    {
                        FechaReservacion = (string)data["FechaReservacion"],
                        HoraReservacion = (string)data["HoraReservacion"],
                    }
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<string>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("liberarArregloAsientos")]
        public async Task<ActionResult> LiberarArregloAsientos(int[] arregloIDS)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    string _body = string.Empty;

                    _body += "[";
                    for (var i = 0; i < arregloIDS.Length; i++)
                    {
                        _body += arregloIDS[i];
                        if (i < arregloIDS.Length - 1) _body += ",";
                    }
                    _body += "]";

                    HttpResponseMessage response = await client.PostAsync("LiberaArregloAsientos", new StringContent(_body, Encoding.UTF8, "application/json"));
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
                return Json(new Response<bool>(false, Constant.EXCEPCION, false), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("getSucursalControl")]
        public async Task<ActionResult> GetSucursalControl()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiPuntoVenta\" : \"" + usuario.CodiPuntoVenta + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("GetSucursalControl", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<SucursalControl> res = new Response<SucursalControl>
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new SucursalControl()
                    {
                        Reserva = (string)data["Reserva"],
                        FechaAbierta = (string)data["FechaAbierta"],
                        Bloqueo = (string)data["Bloqueo"]
                    }
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<SucursalControl>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("obtenerValorPNP")]
        public async Task<ActionResult> ObtenerValorPNP(string Tabla, int CodiProgramacion)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Tabla\" : \"" + Tabla + "\"" +
                                    ",\"CodiProgramacion\" : " + CodiProgramacion +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ObtenerValorPNP", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
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
        [Route("getNewListaDestinosPas")]
        public async Task<ActionResult> GetNewListaDestinosPas(byte CodiEmpresa, short CodiOrigenPas, short CodiOrigenBus, short CodiPuntoVentaBus, short CodiDestinoBus, string Turno, byte CodiServicio, int NroViaje)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiEmpresa\" : " + CodiEmpresa +
                                    ",\"CodiOrigenPas\" : " + CodiOrigenPas +
                                    ",\"CodiOrigenBus\" : " + CodiOrigenBus +
                                    ",\"CodiPuntoVentaBus\" : " + CodiPuntoVentaBus +
                                    ",\"CodiDestinoBus\" : " + CodiDestinoBus +
                                    ",\"Turno\" : \"" + Turno.Replace(" ", "") + "\"" +
                                    ",\"CodiServicio\" : " + CodiServicio +
                                    ",\"NroViaje\" : " + NroViaje +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("GetNewListaDestinosPas", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["CodiSucursal"],
                        label = (string)x["NomOficina"]
                    }).ToList(),
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("verificaDocumentoRepetido")]
        public async Task<ActionResult> VerificaDocumentoRepetido(int CodiProgramacion, int NroViaje, short CodiOrigen, short CodiDestino, string TipoDoc, string Documento)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"CodiProgramacion\" : " + CodiProgramacion +
                                    ",\"NroViaje\" : " + NroViaje +
                                    ",\"CodiOrigen\" : " + CodiOrigen +
                                    ",\"CodiDestino\" : " + CodiDestino +
                                    ",\"TipoDoc\" : \"" + TipoDoc + "\"" +
                                    ",\"Documento\" : \"" + Documento + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VerificaDocumentoRepetido", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<byte> res = new Response<byte>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = (byte)tmpResult["Valor"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<byte>(false, Constant.EXCEPCION, 0), JsonRequestBehavior.AllowGet);
            }
        }
    }
}