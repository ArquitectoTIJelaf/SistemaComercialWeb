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
    [RoutePrefix("reintegro")]
    public class ReintegroController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];
        readonly Usuario usuario = DataSession.UsuarioLogueado;
        // GET: Reintegro
        [HttpPost]
        [Route("save-reintegro")]
        public async Task<ActionResult> SaveReintegro(ReintegroVenta filtro)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var _body = "{" +
                                    "\"Serie\" : \"" + filtro.Serie + "\"" +
                                    "\"nume_boleto\" : \"" + filtro.nume_boleto + "\"" +
                                    "\"Codi_Empresa\" : \"" + filtro.Codi_Empresa + "\"" +
                                    "\"CODI_SUCURSAL\" : \"" + filtro.CODI_SUCURSAL + "\"" +
                                    "\"CODI_PROGRAMACION\" : \"" + filtro.CODI_PROGRAMACION + "\"" +
                                    "\"CODI_SUBRUTA\" : \"" + filtro.CODI_SUBRUTA + "\"" +
                                    "\"CODI_Cliente\" : \"" + filtro.CODI_Cliente + "\"" +
                                    "\"NIT_CLIENTE\" : \"" + filtro.NIT_CLIENTE + "\"" +
                                    "\"PRECIO_VENTA\" : \"" + filtro.PRECIO_VENTA + "\"" +
                                    "\"NUMERO_ASIENTO\" : \"" + "00" + "\"" +
                                    "\"FLAG_VENTA\" : \"" + filtro.FLAG_VENTA + "\"" +
                                    "\"FECH_VENTA\" : \"" + filtro.FECH_VENTA + "\"" +
                                    "\"Recoger\" : \"" + filtro.Recoger + "\"" +
                                    "\"Clav_Usuario\" : \"" + filtro.Clav_Usuario + "\"" +
                                    "\"Dni\" : \"" + filtro.Dni + "\"" +
                                    "\"EDAD\" : \"" + filtro.EDAD + "\"" +
                                    "\"TELEF\" : \"" + filtro.TELEF + "\"" +
                                    "\"NOMB\" : \"" + filtro.NOMB + "\"" +
                                    "\"porcentaje\" : \"" + filtro.porcentaje + "\"" +
                                    "\"Codi_Esca\" : \"" + filtro.Codi_Esca + "\"" +
                                    "\"tota_ruta1\" : \"" + filtro.tota_ruta1 + "\"" +
                                    "\"tota_ruta2\" : \"" + filtro.tota_ruta2 + "\"" +
                                    "\"Estado\" : \"" + " " + "\"" +
                                    "\"Punto_Venta\" : \"" + filtro.Punto_Venta + "\"" +
                                    "\"tipo_doc\" : \"" + filtro.tipo_doc + "\"" +
                                    "\"codi_ori_psj\" : \"" + filtro.codi_ori_psj + "\"" +
                                    "\"Tipo\" : \"" + filtro.Tipo + "\"" +
                                    "\"per_autoriza\" : \"" + "1" + "\"" +
                                    "\"Cod_Cliente\" : \"" + "0" + "\"" +
                                    "\"estado_asiento\" : \"" + "N" + "\"" +
                                    "\"SEXO\" : \"" + "M" + "\"" +
                                    "\"Tipo_Pago\" : \"" + filtro.Tipo_Pago + "\"" +
                                    "\"Vale_Remoto\" : \"" + "" + "\"" +
                                    "\"Tipo_Venta\" : \"" + "N" + "\"" +
                                    "\"Fecha_viaje\" : \"" + filtro.Fecha_viaje + "\"" +
                                    "\"HORA_V\" : \"" + filtro.HORA_V + "\"" +
                                    "\"nacionalidad\" : \"" + filtro.nacionalidad + "\"" +
                                    "\"servicio\" : \"" + filtro.servicio + "\"" +
                                    "\"Sube_en\" : \"" + filtro.Sube_en + "\"" +
                                    "\"Baja_en\" : \"" + filtro.Baja_en + "\"" +
                                    "\"Hora_Emb\" : \"" + filtro.Hora_Emb + "\"" +
                                    "\"nivel\" : \"" + "1" + "\"" +
                                    "\"Codi_Empresa__\" : \"" + filtro.Codi_Empresa__ + "\"" +
                                    "\"CODI_SUCURSAL__\" : \"" + filtro.CODI_SUCURSAL__ + "\"" +
                                    "\"CODI_TERMINAL__\" : \"" + filtro.CODI_TERMINAL__ + "\"" +
                                    "\"Codi_Documento__\" : \"" + filtro.Codi_Documento__ + "\"" +
                                    "\"NUME_CORRELATIVO__\" : \"" + filtro.NUME_CORRELATIVO__ + "\"" +
                                    "\"fecha_venta__\" : \"" + filtro.fecha_venta__ + "\"" +
                                    "\"Pventa__\" : \"" + filtro.Pventa__ + "\"" +
                                    "\"SERIE_BOLETO__\" : \"" + filtro.SERIE_BOLETO__ + "\"" +
                                    "\"Sw_IngManual\" : \"" + "E" + "\"" +
                                "}";
                    HttpResponseMessage response = await client.PostAsync("VerificaHoraConfirmacion", new StringContent(_body, Encoding.UTF8, "application/json"));
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
    }
}