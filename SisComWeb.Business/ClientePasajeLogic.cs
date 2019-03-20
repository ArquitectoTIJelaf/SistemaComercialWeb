using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
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
                var resBuscaPasajero = ClientePasajeRepository.BuscaPasajero(TipoDoc, NumeroDoc);
                if (!resBuscaPasajero.Estado)
                    return resBuscaPasajero;

                // Consulta 'RENIEC'
                if (TipoDoc == "1" && string.IsNullOrEmpty(resBuscaPasajero.Valor.NumeroDoc))
                {
                    var resConsultaRENIEC = ConsultaRENIEC(NumeroDoc);
                    if (resConsultaRENIEC.Estado)
                    {
                        if (!string.IsNullOrEmpty(resConsultaRENIEC.Valor[0]) &&
                            !string.IsNullOrEmpty(resConsultaRENIEC.Valor[1]) &&
                            !string.IsNullOrEmpty(resConsultaRENIEC.Valor[2]))
                        {
                            resBuscaPasajero.Valor.ApellidoPaterno = resConsultaRENIEC.Valor[0];
                            resBuscaPasajero.Valor.ApellidoMaterno = resConsultaRENIEC.Valor[1];
                            resBuscaPasajero.Valor.NombreCliente = resConsultaRENIEC.Valor[2];
                        }
                    }
                }

                if (!string.IsNullOrEmpty(resBuscaPasajero.Valor.RucContacto))
                {
                    var resBuscarEmpresa = ClientePasajeRepository.BuscarEmpresa(resBuscaPasajero.Valor.RucContacto);
                    if (resBuscarEmpresa.Estado)
                    {
                        resBuscaPasajero.Valor.RazonSocial = resBuscarEmpresa.Valor.RazonSocial;
                        resBuscaPasajero.Valor.Direccion = resBuscarEmpresa.Valor.Direccion;
                    }
                }

                return new Response<ClientePasajeEntity>(resBuscaPasajero.EsCorrecto, resBuscaPasajero.Valor, resBuscaPasajero.Mensaje, resBuscaPasajero.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ClientePasajeEntity>(false, null, Message.MsgErrExcBusqClientePasaje, false);
            }
        }

        public static Response<bool> GrabarPasajero(List<ClientePasajeEntity> lista)
        {
            try
            {
                var response = new Response<bool>(false, false, "Error: GrabarPasajero.", false);

                foreach (var entidad in lista)
                {
                    // Busca 'Pasajero'
                    var resBuscaPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDoc, entidad.NumeroDoc);
                    if (!resBuscaPasajero.Estado)
                    {
                        response.Mensaje = resBuscaPasajero.Mensaje;
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
                        RucContacto = entidad.RucContacto ?? string.Empty,
                        Sexo = entidad.Sexo ?? string.Empty
                    };

                    if (!string.IsNullOrEmpty(resBuscaPasajero.Valor.TipoDoc) && !string.IsNullOrEmpty(resBuscaPasajero.Valor.NumeroDoc))
                    {
                        // Modifica 'Pasajero'
                        var resModificarPasajero = ClientePasajeRepository.ModificarPasajero(objClientePasajeEntity);
                        if (!resModificarPasajero.Estado)
                        {
                            response.Mensaje = resModificarPasajero.Mensaje;
                            return response;
                        }
                    }
                    else
                    {
                        if (entidad.TipoDoc == "1" && !string.IsNullOrEmpty(resBuscaPasajero.Valor.NumeroDoc))
                        {
                            // Consulta 'RENIEC'
                            var resConsultaRENIEC = ConsultaRENIEC(entidad.NumeroDoc);
                            if (resConsultaRENIEC.Estado)
                            {
                                if (!string.IsNullOrEmpty(resConsultaRENIEC.Valor[0]) &&
                                    !string.IsNullOrEmpty(resConsultaRENIEC.Valor[1]) &&
                                    !string.IsNullOrEmpty(resConsultaRENIEC.Valor[2]))
                                {
                                    objClientePasajeEntity.ApellidoPaterno = resConsultaRENIEC.Valor[0];
                                    objClientePasajeEntity.ApellidoMaterno = resConsultaRENIEC.Valor[1];
                                    objClientePasajeEntity.NombreCliente = resConsultaRENIEC.Valor[2];
                                }
                            }
                        }

                        // Graba 'Pasajero'
                        var resGrabarPasajero = ClientePasajeRepository.GrabarPasajero(objClientePasajeEntity);
                        if (!resGrabarPasajero.Estado)
                        {
                            response.Mensaje = resGrabarPasajero.Mensaje;
                            return response;
                        }
                    }

                    // Valida 'RucContacto'
                    if (string.IsNullOrEmpty(entidad.RucContacto))
                        continue;

                    // Busca 'Empresa'
                    var resBuscarEmpresa = ClientePasajeRepository.BuscarEmpresa(entidad.RucContacto);
                    if (!resBuscarEmpresa.Estado)
                    {
                        response.Mensaje = resBuscarEmpresa.Mensaje;
                        return response;
                    }

                    var objEmpresa = new RucEntity
                    {
                        RucCliente = entidad.RucContacto,
                        RazonSocial = resBuscarEmpresa.Valor.RazonSocial ?? string.Empty,
                        Direccion = entidad.Direccion ?? string.Empty,
                        Telefono = entidad.Telefono ?? string.Empty
                    };

                    if (!string.IsNullOrEmpty(resBuscarEmpresa.Valor.RucCliente))
                    {
                        // Modifica 'Empresa'
                        var resModificarEmpresa = ClientePasajeRepository.ModificarEmpresa(objEmpresa);
                        if (!resModificarEmpresa.Estado)
                        {
                            response.Mensaje = resModificarEmpresa.Mensaje;
                            return response;
                        }
                    }
                    else
                    {
                        // Consulta 'SUNAT'
                        var resConsultaSUNAT = ConsultaSUNAT(entidad.RucContacto);
                        if (resConsultaSUNAT.Estado)
                        {
                            objEmpresa.RazonSocial = resConsultaSUNAT.Valor;
                        }

                        // Graba 'Empresa'
                        var resGrabarEmpresa = ClientePasajeRepository.GrabarEmpresa(objEmpresa);
                        if (!resGrabarEmpresa.Estado)
                        {
                            response.Mensaje = resGrabarEmpresa.Mensaje;
                            return response;
                        }
                    }
                }

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = Message.MsgCorrectoGrabarClientePasaje;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcGrabarClientePasaje, false);
            }
        }

        public static Response<RucEntity> ConsultarSUNAT(string RucContacto)
        {
            try
            {
                // Busca 'Empresa'
                var resBuscarEmpresa = ClientePasajeRepository.BuscarEmpresa(RucContacto);
                if (!resBuscarEmpresa.Estado)
                    return resBuscarEmpresa;

                if (string.IsNullOrEmpty(resBuscarEmpresa.Valor.RucCliente))
                {
                    // Consulta 'SUNAT'
                    var resConsultaSUNAT = ConsultaSUNAT(RucContacto);
                    if (resConsultaSUNAT.Estado)
                        resBuscarEmpresa.Valor.RazonSocial = resConsultaSUNAT.Valor;
                }

                return new Response<RucEntity>(resBuscarEmpresa.EsCorrecto, resBuscarEmpresa.Valor, resBuscarEmpresa.Mensaje, resBuscarEmpresa.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<RucEntity>(false, null, Message.MsgErrExcConsultarSUNAT, false);
            }
        }

        public static Response<string> ConsultaSUNAT(string RucContacto)
        {
            try
            {
                var response = new Response<string>(false, null, "Error: ConsultaSUNAT.", false);
                string Valor = "";

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
                    response.Estado = bool.Parse(xn["CONSULTAR_RUCResult"].ChildNodes[0].InnerText);
                    Valor = xn["CONSULTAR_RUCResult"].ChildNodes[3].InnerText;
                };

                if (response.Estado)
                {
                    response.EsCorrecto = true;
                    response.Valor = Valor;
                    response.Mensaje = "Correcto: ConsultaSUNAT.";
                }

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, "Error: ConsultaSUNAT.", false);
            }
        }

        public static Response<string[]> ConsultaRENIEC(string NumeroDoc)
        {
            var response = new Response<string[]>(false, null, "Error: ConsultaRENIEC.", false);

            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri("http://aplicaciones007.jne.gob.pe/srop_publico/Consulta/Afiliado/")
                };

                var res = client.GetAsync("GetNombresCiudadano?DNI=" + NumeroDoc).Result;

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    string[] arrayNombreCompleto = result.Split('|');

                    response.EsCorrecto = true;
                    response.Valor = arrayNombreCompleto;
                    response.Mensaje = "Correcto: ConsultaRENIEC.";
                    response.Estado = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string[]>(false, null, "Error: ConsultaRENIEC.", false);
            }
        }
    }
}
