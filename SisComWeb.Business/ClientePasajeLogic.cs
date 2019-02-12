using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace SisComWeb.Business
{
    public class ClientePasajeLogic
    {
        public static Response<ClientePasajeEntity> BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            try
            {
                var response = ClientePasajeRepository.BuscaPasajero(TipoDoc, NumeroDoc);
                return new Response<ClientePasajeEntity>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ClientePasajeEntity>(false, null, Message.MsgErrExcBusqClientePasaje, false);
            }
        }

        public static Response<bool> GrabarPasajero(ClientePasajeEntity entidad)
        {
            try
            {
                var response = new Response<bool>(false, false, "", false);

                // Valida 'TipoDoc' y 'NumeroDoc'
                if (!string.IsNullOrEmpty(entidad.TipoDoc) && !string.IsNullOrEmpty(entidad.NumeroDoc))
                    response.Mensaje += "TipoDoc: " + entidad.TipoDoc + ". NumeroDoc : " + entidad.NumeroDoc + ". ";
                else
                {
                    response.Mensaje += "Error: TipoDoc o NumeroDoc nulo o vacío, no se pudo 'GrabarPasajero'. ";
                    return response;
                }

                // Busca 'Pasajero'
                var resBuscaPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDoc, entidad.NumeroDoc);
                if (resBuscaPasajero.Estado)
                    response.Mensaje += resBuscaPasajero.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscaPasajero. ";
                    return response;
                }

                var objClientePasajeEntity = new ClientePasajeEntity
                {
                    IdCliente = resBuscaPasajero.Valor.IdCliente,
                    TipoDoc = entidad.TipoDoc,
                    NumeroDoc = entidad.NumeroDoc,
                    NombreCliente = entidad.NombreCliente,
                    ApellidoPaterno = entidad.ApellidoPaterno,
                    ApellidoMaterno = entidad.ApellidoMaterno,
                    FechaNacimiento = entidad.FechaNacimiento,
                    Edad = entidad.Edad,
                    Direccion = entidad.Direccion ?? string.Empty,
                    Telefono = entidad.Telefono ?? string.Empty,
                    RucContacto = entidad.RucContacto ?? string.Empty
                };

                if (!string.IsNullOrEmpty(resBuscaPasajero.Valor.NumeroDoc))
                {
                    var resModificarPasajero = ClientePasajeRepository.ModificarPasajero(objClientePasajeEntity);
                    if (resModificarPasajero.Estado)
                        response.Mensaje += resModificarPasajero.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: ModificarPasajero. ";
                        return response;
                    }
                }
                else
                {
                    // Consulta 'RENIEC'
                    var resConsultaRENIEC = ConsultaRENIEC(entidad.NumeroDoc);
                    if (resConsultaRENIEC.Estado && resConsultaRENIEC.Valor[0] != "")
                    {
                        objClientePasajeEntity.ApellidoPaterno = resConsultaRENIEC.Valor[0];
                        objClientePasajeEntity.ApellidoMaterno = resConsultaRENIEC.Valor[1];
                        objClientePasajeEntity.NombreCliente = resConsultaRENIEC.Valor[2];

                        response.Mensaje += resConsultaRENIEC.Mensaje;
                    }
                    else if (resConsultaRENIEC.Estado && resConsultaRENIEC.Valor[0] == "")
                        response.Mensaje += "Mensaje: ConsultaRENIEC: DNI no encontrado. ";
                    else
                        response.Mensaje += "Error: ConsultaRENIEC. ";

                    var resGrabarPasajero = ClientePasajeRepository.GrabarPasajero(objClientePasajeEntity);
                    if (resGrabarPasajero.Estado)
                        response.Mensaje += resGrabarPasajero.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: GrabarPasajero. ";
                        return response;
                    }
                }

                // Valida 'RucContacto'
                if (!string.IsNullOrEmpty(entidad.RucContacto))
                    response.Mensaje += "RucContacto: " + entidad.RucContacto + ". ";
                else
                {
                    response.EsCorrecto = true;
                    response.Valor = true;
                    response.Mensaje += "Fin: RucContacto nulo o vacío, no se pudo 'BuscarEmpresa'. ";
                    response.Estado = true;
                    return response;
                }

                // Busca 'Empresa'
                var resBuscarEmpresa = ClientePasajeRepository.BuscarEmpresa(entidad.RucContacto);
                if (resBuscarEmpresa.Estado)
                    response.Mensaje += resBuscarEmpresa.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscarEmpresa. ";
                    return response;
                }

                var objEmpresa = new RucEntity
                {
                    RucCliente = entidad.RucContacto,
                    RazonSocial = resBuscarEmpresa.Valor.RazonSocial ?? "",
                    Direccion = entidad.Direccion,
                    Telefono = entidad.Telefono
                };

                if (!string.IsNullOrEmpty(resBuscarEmpresa.Valor.RucCliente))
                {
                    var resModificarEmpresa = ClientePasajeRepository.ModificarEmpresa(objEmpresa);
                    if (resModificarEmpresa.Estado)
                        response.Mensaje += resModificarEmpresa.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: ModificarEmpresa. ";
                        return response;
                    }
                }
                else
                {
                    // Consulta 'SUNAT'
                    var resConsultaSUNAT = ConsultaSUNAT(entidad.RucContacto);
                    if (resConsultaSUNAT.Estado && resConsultaSUNAT.Valor != "false")
                    {
                        objEmpresa.RazonSocial = resConsultaSUNAT.Valor;

                        response.Mensaje += resConsultaSUNAT.Mensaje;
                    }
                    else if (resConsultaSUNAT.Estado && resConsultaSUNAT.Valor == "false")
                        response.Mensaje += "Mensaje: ConsultaSUNAT: RUC no encontrado. ";
                    else
                        response.Mensaje += "Error: ConsultaSUNAT. ";

                    var resGrabarEmpresa = ClientePasajeRepository.GrabarEmpresa(objEmpresa);
                    if (resGrabarEmpresa.Estado)
                        response.Mensaje += resGrabarEmpresa.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: GrabarEmpresa. ";
                        return response;
                    }
                }

                response.EsCorrecto = true;
                response.Valor = true;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcGrabarClientePasaje, false);
            }
        }

        public static Response<string> ConsultaSUNAT(string RucContacto)
        {
            try
            {
                var RAZON_SOCIAL = "";

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                var soapXml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:int=\"integradores.jelaf.pe\">" +
                                    "<soapenv:Header/>" +
                                        "<soapenv:Body>" +
                                            "<int:CONSULTAR_RUC><int:RUC>" + RucContacto + "</int:RUC></int:CONSULTAR_RUC>" +
                                        "</soapenv:Body>" +
                                   "</soapenv:Envelope>";
                var responseC = httpClient.PostAsync("http://integradores.jelaf.pe/WsRUC/WsConsulta.asmx", new StringContent(soapXml, Encoding.UTF8, "text/xml")).Result;
                var content = responseC.Content.ReadAsStringAsync().Result;
                XmlDocument document = new XmlDocument();
                document.LoadXml(content);
                XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
                manager.AddNamespace("bhr", "integradores.jelaf.pe");
                XmlNodeList xnList = document.SelectNodes("//bhr:CONSULTAR_RUCResponse", manager);

                foreach (XmlNode xn in xnList)
                {
                    RAZON_SOCIAL = xn["CONSULTAR_RUCResult"].ChildNodes[0].InnerText;
                }

                return new Response<string>(true, RAZON_SOCIAL, "Correcto: ConsultaSUNAT. ", true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, "Error: ConsultaSUNAT. ", false);
            }
        }

        public static Response<string[]> ConsultaRENIEC(string NumeroDoc)
        {
            string[] arrayNombreCompleto = null;

            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri("http://aplicaciones007.jne.gob.pe/srop_publico/Consulta/Afiliado/")
                };

                var response = client.GetAsync("GetNombresCiudadano?DNI=" + NumeroDoc).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    arrayNombreCompleto = result.Split('|');
                }

                return new Response<string[]>(true, arrayNombreCompleto, "Correcto: ConsultaRENIEC. ", true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string[]>(false, null, arrayNombreCompleto[3], false);
            }
        }
    }
}
