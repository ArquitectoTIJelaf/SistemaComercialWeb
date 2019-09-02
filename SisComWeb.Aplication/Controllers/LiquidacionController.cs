using Newtonsoft.Json.Linq;
using SisComWeb.Aplication.Helpers;
using SisComWeb.Aplication.Models;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisComWeb.Aplication.Controllers
{
    [RoutePrefix("liquidacion")]
    public class LiquidacionController : Controller
    {
        private int CodSistema = Convert.ToInt32(ConfigurationManager.AppSettings["CodSistema"]);
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        readonly Usuario usuario = DataSession.UsuarioLogueado;

        // GET: Lista Liquidacion
        [HttpPost]
        [Route("lista-liquidacion")]
        public async Task<ActionResult> ListaLiquidacion(FiltroLiquidacion filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"FechaLiquidacion\" : \"" + filtro.FechaLiquidacion + "\"" +
                                    ",\"CodEmpresa\" : \"" + filtro.CodEmpresa + "\"" +
                                    ",\"CodSucursal\" : \"" + filtro.CodSucursal + "\"" +
                                    ",\"CodPuntVenta\" : \"" + filtro.CodPuntVenta + "\"" +
                                    ",\"CodUsuario\" : \"" + filtro.CodUsuario + "\"" +
                                    ",\"CodInterno\" : \"" + CodSistema + "\"" +
                                    ",\"TipoProc\" : \"" + 1 + "\"" +
                                    ",\"tipoLiq\" : \"" + filtro.tipoLiq + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("ListaLiquidacion", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);
                JObject data = (JObject)tmpResult["Valor"];

                Response<Liquidacion> res = new Response<Liquidacion>()
                {
                    Estado = (bool)tmpResult["Estado"],
                    Mensaje = (string)tmpResult["Mensaje"],
                    Valor = new Liquidacion()
                    {
                        Fecha = (string)data["Fecha"],
                        Empresa = (string)data["Empresa"],
                        CodiEmpresa = (int)data["CodiEmpresa"],
                        Sucursal = (string)data["Sucursal"],
                        CodiSucursal = (int)data["CodiSucursal"],
                        PuntoVenta = (string)data["PuntoVenta"],
                        CodiPuntoVenta = (int)data["CodiPuntoVenta"],
                        Usuario = (string)data["Usuario"],
                        CodiUsuario = (int)data["CodiUsuario"],
                        PasIng = (decimal)data["PasIng"],
                        AfectoPasIng = (byte)data["AfectoPasIng"],
                        VenRem = (decimal)data["VenRem"],
                        AfectoVenRem = (byte)data["AfectoVenRem"],
                        Venrut = (decimal)data["Venrut"],
                        AfectoVenrut = (byte)data["AfectoVenrut"],
                        VenEnc = (decimal)data["VenEnc"],
                        AfectoVenEnc = (byte)data["AfectoVenEnc"],
                        VenExe = (decimal)data["VenExe"],
                        AfectoVenExe = (byte)data["AfectoVenExe"],
                        FacLib = (decimal)data["FacLib"],
                        AfectoFacLib = (byte)data["AfectoFacLib"],
                        GirRec = (decimal)data["GirRec"],
                        AfectoGirRec = (byte)data["AfectoGirRec"],
                        CobDes = (decimal)data["CobDes"],
                        AfectoCobDes = (byte)data["AfectoCobDes"],
                        CobDel = (decimal)data["CobDel"],
                        AfectoCobDel = (byte)data["AfectoCobDel"],
                        IngCaj = (decimal)data["IngCaj"],
                        AfectoIngCaj = (byte)data["AfectoIngCaj"],
                        IngDet = (decimal)data["IngDet"],
                        AfectoIngDet = (byte)data["AfectoIngDet"],
                        TotalAfecto = (decimal)data["TotalAfecto"],
                        AfectoTotalAfecto = (byte)data["AfectoTotalAfecto"],
                        RemEmi = (decimal)data["RemEmi"],
                        AfectoRemEmi = (byte)data["AfectoRemEmi"],
                        BolCre = (decimal)data["BolCre"],
                        AfectoBolCre = (byte)data["AfectoBolCre"],
                        WebEmi = (decimal)data["WebEmi"],
                        AfectoWebEmi = (byte)data["AfectoWebEmi"],
                        RedBus = (decimal)data["RedBus"],
                        AfectoRedBus = (byte)data["AfectoRedBus"],
                        TieVir = (decimal)data["TieVir"],
                        AfectoTieVir = (byte)data["AfectoTieVir"],
                        DelEmi = (decimal)data["DelEmi"],
                        AfectoDelEmi = (byte)data["AfectoDelEmi"],
                        Ventar = (decimal)data["Ventar"],
                        AfectoVentar = (byte)data["AfectoVentar"],
                        Enctar = (decimal)data["Enctar"],
                        AfectoEnctar = (byte)data["AfectoEnctar"],
                        EgrCaj = (decimal)data["EgrCaj"],
                        AfectoEgrCaj = (byte)data["AfectoEgrCaj"],
                        GirEnt = (decimal)data["GirEnt"],
                        AfectoGirEnt = (byte)data["AfectoGirEnt"],
                        BolAnF = (decimal)data["BolAnF"],
                        AfectoBolAnF = (byte)data["AfectoBolAnF"],
                        BolAnR = (decimal)data["BolAnR"],
                        AfectoBolAnR = (byte)data["AfectoBolAnR"],
                        ValAnR = (decimal)data["ValAnR"],
                        AfectoValAnR = (byte)data["AfectoValAnR"],
                        EncPag = (decimal)data["EncPag"],
                        AfectoEncPag = (byte)data["AfectoEncPag"],
                        Ctagui = (decimal)data["Ctagui"],
                        AfectoCtagui = (byte)data["AfectoCtagui"],
                        CtaCan = (decimal)data["CtaCan"],
                        AfectoCtaCan = (byte)data["AfectoCtaCan"],
                        Notcre = (decimal)data["Notcre"],
                        AfectoNotcre = (byte)data["AfectoNotcre"],
                        Totdet = (decimal)data["Totdet"],
                        AfectoTotdet = (byte)data["AfectoTotdet"],
                        Gasrut = (decimal)data["Gasrut"],
                        AfectoGasrut = (byte)data["AfectoGasrut"],
                        TotalInafecto = (decimal)data["TotalInafecto"],
                        AfectoTotalInafecto = (byte)data["AfectoTotalInafecto"],
                        Total = (decimal)data["Total"],
                        AfectoTotal = (byte)data["AfectoTotal"]                        
                    },
                    EsCorrecto = (bool)tmpResult["EsCorrecto"]
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<Liquidacion>(false, Constant.EXCEPCION, new Liquidacion()), JsonRequestBehavior.AllowGet);
            }
        }
    }
}