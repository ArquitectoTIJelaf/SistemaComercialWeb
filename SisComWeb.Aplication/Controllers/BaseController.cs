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
    [RoutePrefix("base")]
    public class BaseController : Controller
    {
        private static readonly string url = System.Configuration.ConfigurationManager.AppSettings["urlService"];

        #region FILTROS

        [HttpGet]
        [Route("get-oficinas")]
        public async Task<JsonResult> GetOficinas()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaOficinas");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaOficinas");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-puntosventa")]
        public async Task<JsonResult> GetPuntosVenta()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaPuntosVenta");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaPuntosVenta");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<PuntoVentaBase>> res = new Response<List<PuntoVentaBase>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new PuntoVentaBase
                    {
                        id = (string)x["id"],
                        label = (string)x["label"],
                        CodiSucursal = (short)x["CodiSucursal"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<PuntoVentaBase>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-usuarios")]
        public async Task<JsonResult> GetUsuarios(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    value = "null";

                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaUsuarios");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaUsuarios/" + value);
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-empresas")]
        public async Task<JsonResult> GetEmpresas()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaEmpresas");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaEmpresas");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-servicios")]
        public async Task<JsonResult> GetServicios()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaServicios");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaServicios");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-tiposDoc")]
        public async Task<JsonResult> GetTiposDoc()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaTiposDoc");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaTiposDoc");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<DocumentoBase>> res = new Response<List<DocumentoBase>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new DocumentoBase
                    {
                        id = (string)x["id"],
                        label = (string)x["label"],

                        MinLonDocumento = (string)x["MinLonDocumento"],
                        MaxLonDocumento = (string)x["MaxLonDocumento"],
                        TipoDatoDocumento = (string)x["TipoDatoDocumento"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<DocumentoBase>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-tipoPago")]
        public async Task<JsonResult> GetTipoPago()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaTipoPago");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaTipoPago");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-tarjetasCredito")]
        public async Task<JsonResult> GetTarjetasCredito()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaTarjetaCredito");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaTarjetaCredito");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-cuidades")]
        public async Task<JsonResult> GetCuidades()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaCiudad");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaCiudad");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-parentesco")]
        public async Task<JsonResult> GetParentesco()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListarParentesco");
                    HttpResponseMessage response = await client.GetAsync(url + "ListarParentesco");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-gerentes")]
        public async Task<JsonResult> GetGerentes()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListarGerente");
                    HttpResponseMessage response = await client.GetAsync(url + "ListarGerente");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-socios")]
        public async Task<JsonResult> GetSocios()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListarSocio");
                    HttpResponseMessage response = await client.GetAsync(url + "ListarSocio");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-hospitales")]
        public async Task<JsonResult> GetHospitales(int codiSucursal)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaHospitales");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaHospitales/" + codiSucursal);
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-secciones")]
        public async Task<JsonResult> GetSecciones(string idContrato)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaSecciones");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaSecciones/" + idContrato);
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-areas")]
        public async Task<JsonResult> GetAreas(string idContrato)
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaAreas");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaAreas/" + idContrato);
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("get-usuario-claveAnuRei")]
        public async Task<JsonResult> GetUsuariosClaveAnuRei(string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(Value))
                    Value = "null";

                string result = string.Empty;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaUsuariosClaveAnuRei");

                    var _body = "{" +
                                    "\"Value\" : \"" + Value + "\"" +
                                " }";

                    HttpResponseMessage response = await client.PostAsync(url + "ListaUsuariosClaveAnuRei", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("get-opciones-modificacion")]
        public async Task<JsonResult> GetOpcionesModificacion()
        {
            try
            {
                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaOpcionesModificacion");
                    HttpResponseMessage response = await client.GetAsync(url + "ListaOpcionesModificacion");
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<SelectReintegro>> res = new Response<List<SelectReintegro>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new SelectReintegro
                    {
                        id = (string)x["id"],
                        label = (string)x["label"],
                        monto = (decimal)x["monto"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<Base>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("get-usuariosHC")]
        public async Task<JsonResult> GetUsuariosHC(string Descripcion, int Suc, int Pv)
        {
            try
            {
                if (string.IsNullOrEmpty(Descripcion))
                    Descripcion = "null";

                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaUsuariosHC");
                    var _body = "{" +
                                    "\"Descripcion\" : \"" + Descripcion + "\"" +
                                    ",\"Suc\" : " + Suc +
                                    ",\"Pv\" : " + Pv +
                                " }";
                    HttpResponseMessage response = await client.PostAsync("ListaUsuariosHC", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<SelectReintegro>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [Route("get-usuarioControlPwd")]
        public async Task<JsonResult> GetUsuarioControlPwd(string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(Value))
                    Value = "null";

                string result = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url + "ListaUsuarioControlPwd");
                    var _body = "{" +
                                    "\"Value\" : \"" + Value + "\"" +
                                " }";
                    HttpResponseMessage response = await client.PostAsync("ListaUsuarioControlPwd", new StringContent(_body, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                        result = await response.Content.ReadAsStringAsync();
                }

                JToken tmpResult = JObject.Parse(result);

                Response<List<Base>> res = new Response<List<Base>>()
                {
                    Estado = (bool)tmpResult.SelectToken("Estado"),
                    Mensaje = (string)tmpResult.SelectToken("Mensaje"),
                    Valor = ((JArray)tmpResult["Valor"]).Select(x => new Base
                    {
                        id = (string)x["id"],
                        label = (string)x["label"]
                    }).ToList()
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new Response<List<SelectReintegro>>(false, Constant.EXCEPCION, null), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}