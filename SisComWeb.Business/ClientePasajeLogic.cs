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
                ClientePasajeEntity objClientePasajeEntity;
                Response<bool> resModificarPasajero;
                Response<int> resGrabarPasajero;
                RucEntity objEmp;
                Response<bool> resEmpresa;


                // Validación 'BuscaPasajero'
                var objPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDoc, entidad.NumeroDoc);
                if (objPasajero.Estado == true) response.Mensaje += objPasajero.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscaPasajero. ";
                    return response;
                }

                objClientePasajeEntity = new ClientePasajeEntity
                {
                    IdCliente = objPasajero.Valor.IdCliente,
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

                if (!string.IsNullOrEmpty(objPasajero.Valor.NumeroDoc))
                {
                    resModificarPasajero = ClientePasajeRepository.ModificarPasajero(objClientePasajeEntity);
                    if (resModificarPasajero.Estado == true) response.Mensaje += resModificarPasajero.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: ModificarPasajero. ";
                        return response;
                    }
                }
                else
                {
                    // Consulta DNI a la RENIEC
                    var objRENIEC = ConsultaRENIEC(entidad.NumeroDoc);
                    if (objRENIEC.Estado == true)
                    {
                        objClientePasajeEntity.ApellidoPaterno = objRENIEC.Valor[0];
                        objClientePasajeEntity.ApellidoMaterno = objRENIEC.Valor[1];
                        objClientePasajeEntity.NombreCliente = objRENIEC.Valor[2];

                        response.Mensaje += objRENIEC.Mensaje;
                    }
                    else response.Mensaje += "Error: ConsultaRENIEC. ";

                    resGrabarPasajero = ClientePasajeRepository.GrabarPasajero(objClientePasajeEntity);
                    if (resGrabarPasajero.Estado == true) response.Mensaje += resGrabarPasajero.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: GrabarPasajero. ";
                        return response;
                    }
                }


                // Validación 'RucContacto'
                if (!string.IsNullOrEmpty(entidad.RucContacto)) response.Mensaje += "RucContacto: " + entidad.RucContacto + ". ";
                else
                {
                    response.EsCorrecto = true;
                    response.Valor = true;
                    response.Mensaje += "Fin: RucContacto nulo o vacío, no se pudo 'BuscarEmpresa'. ";
                    response.Estado = true;
                    return response;
                }


                // Validación 'BuscarEmpresa'
                var objEmpresa = ClientePasajeRepository.BuscarEmpresa(entidad.RucContacto);
                if (objEmpresa.Estado == true) response.Mensaje += objEmpresa.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscarEmpresa. ";
                    return response;
                }

                objEmp = new RucEntity
                {
                    RucCliente = entidad.RucContacto,
                    RazonSocial = objEmpresa.Valor.RazonSocial ?? "",
                    Direccion = entidad.Direccion,
                    Telefono = entidad.Telefono
                };

                if (!string.IsNullOrEmpty(objEmpresa.Valor.RucCliente))
                {
                    resEmpresa = ClientePasajeRepository.ModificarEmpresa(objEmp);
                    if (resEmpresa.Estado == true) response.Mensaje += resEmpresa.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: ModificarEmpresa. ";
                        return response;
                    }
                }
                else
                {
                    // Consulta RUC a la SUNAT
                    var objSUNAT = ConsultaSUNAT(entidad.RucContacto);
                    if (objSUNAT.Estado == true)
                    {
                        objEmp.RazonSocial = objSUNAT.Valor;

                        response.Mensaje += objSUNAT.Mensaje;
                    }
                    else response.Mensaje += "Error: ConsultaSUNAT. ";

                    resEmpresa = ClientePasajeRepository.GrabarEmpresa(objEmp);
                    if (resEmpresa.Estado == true) response.Mensaje += resEmpresa.Mensaje;
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
