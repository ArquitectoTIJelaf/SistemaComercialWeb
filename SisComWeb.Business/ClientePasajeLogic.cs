using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace SisComWeb.Business
{
    public class ClientePasajeLogic
    {
        private static readonly string ServiceRENIEC = ConfigurationManager.AppSettings["serviceRENIEC"].ToString();
        private static readonly string ServiceSUNAT = ConfigurationManager.AppSettings["serviceSUNAT"].ToString();

        public static Response<ClientePasajeEntity> BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            try
            {
                var buscaPasajero = ClientePasajeRepository.BuscaPasajero(TipoDoc, NumeroDoc);
                var buscaEmpresa = new RucEntity();

                if (!string.IsNullOrEmpty(buscaPasajero.RucContacto))
                {
                    buscaEmpresa = ClientePasajeRepository.BuscarEmpresa(buscaPasajero.RucContacto);
                    buscaPasajero.RazonSocial = buscaEmpresa.RazonSocial;
                    buscaPasajero.Direccion = buscaEmpresa.Direccion;
                }

                // Consulta 'RENIEC'
                if (TipoDoc == "1" && string.IsNullOrEmpty(buscaPasajero.NumeroDoc))
                {
                    var resConsultaRENIEC = ConsultaRENIEC(NumeroDoc);
                    if (resConsultaRENIEC.Estado)
                    {
                        buscaPasajero.ApellidoPaterno = resConsultaRENIEC.Valor.ApellidoPaterno;
                        buscaPasajero.ApellidoMaterno = resConsultaRENIEC.Valor.ApellidoMaterno;
                        buscaPasajero.NombreCliente = resConsultaRENIEC.Valor.Nombres;
                    }
                    else
                        return new Response<ClientePasajeEntity>(false, buscaPasajero, resConsultaRENIEC.Mensaje, true);
                }

                // Consulta 'SUNAT'
                if (!string.IsNullOrEmpty(buscaPasajero.RucContacto))
                {
                    if (string.IsNullOrEmpty(buscaEmpresa.RucCliente))
                    {
                        var resConsultaSUNAT = ConsultaSUNAT(buscaPasajero.RucContacto);
                        if (resConsultaSUNAT.Estado)
                        {
                            buscaPasajero.RazonSocial = resConsultaSUNAT.Valor.RazonSocial;
                            buscaPasajero.Direccion = resConsultaSUNAT.Valor.Direccion;
                        }
                        else
                            return new Response<ClientePasajeEntity>(false, buscaPasajero, resConsultaSUNAT.Mensaje, true);
                    }
                }

                return new Response<ClientePasajeEntity>(true, buscaPasajero, Message.MsgCorrectoBuscaPasajero, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ClientePasajeEntity>(false, null, Message.MsgExcBuscaPasajero, false);
            }
        }

        public static Response<RucEntity> BuscaEmpresa(string RucContacto)
        {
            try
            {
                var buscaEmpresa = ClientePasajeRepository.BuscarEmpresa(RucContacto);

                // Consulta 'SUNAT'
                if (string.IsNullOrEmpty(buscaEmpresa.RucCliente))
                {
                    var resConsultaSUNAT = ConsultaSUNAT(RucContacto);
                    if (resConsultaSUNAT.Estado)
                    {
                        buscaEmpresa.RazonSocial = resConsultaSUNAT.Valor.RazonSocial;
                        buscaEmpresa.Direccion = resConsultaSUNAT.Valor.Direccion;
                    }
                    else
                        return new Response<RucEntity>(false, buscaEmpresa, resConsultaSUNAT.Mensaje, true);
                }

                return new Response<RucEntity>(true, buscaEmpresa, Message.MsgCorrectoBuscaEmpresa, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<RucEntity>(false, null, Message.MsgExcBuscaEmpresa, false);
            }
        }

        public static Response<ReniecEntity> ConsultaRENIEC(string NumeroDoc)
        {
            try
            {
                ReniecEntity entidad = new ReniecEntity
                {
                    ApellidoPaterno = string.Empty,
                    ApellidoMaterno = string.Empty,
                    Nombres = string.Empty
                };

                var client = new HttpClient
                {
                    BaseAddress = new Uri(ServiceRENIEC)
                };

                var res = client.GetAsync("GetNombresCiudadano?DNI=" + NumeroDoc).Result;

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    string[] arrayNombreCompleto = result.Split('|');

                    entidad.ApellidoPaterno = arrayNombreCompleto[0];
                    entidad.ApellidoMaterno = arrayNombreCompleto[1];
                    entidad.Nombres = arrayNombreCompleto[2];

                    return new Response<ReniecEntity>(true, entidad, Message.MsgCorrectoConsultaRENIEC, true);
                }
                else
                    return new Response<ReniecEntity>(false, entidad, Message.MsgErrorConsultaRENIEC, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReniecEntity>(false, null, Message.MsgExcConsultaRENIEC, false);
            }
        }

        public static Response<RucEntity> ConsultaSUNAT(string RucContacto)
        {
            try
            {
                RucEntity entidad = new RucEntity
                {
                    RazonSocial = string.Empty,
                    Direccion = string.Empty
                };
                var auxEstado = new bool();

                var soapXml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:int=\"integradores.jelaf.pe\">" +
                                    "<soapenv:Header/>" +
                                        "<soapenv:Body>" +
                                            "<int:CONSULTAR_RUC><int:RUC>" + RucContacto + "</int:RUC></int:CONSULTAR_RUC>" +
                                        "</soapenv:Body>" +
                                "</soapenv:Envelope>";

                var client = new HttpClient
                {
                    BaseAddress = new Uri(ServiceSUNAT),
                };

                var res = client.PostAsync(client.BaseAddress, new StringContent(soapXml, Encoding.UTF8, "text/xml")).Result;

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;

                    var document = new XmlDocument();
                    document.LoadXml(result);
                    var manager = new XmlNamespaceManager(document.NameTable);
                    manager.AddNamespace("bhr", "integradores.jelaf.pe");
                    XmlNodeList xnList = document.SelectNodes("//bhr:CONSULTAR_RUCResponse", manager);
                    foreach (XmlNode xn in xnList)
                    {
                        auxEstado = bool.Parse(xn["CONSULTAR_RUCResult"].ChildNodes[0].InnerText);
                        entidad.RazonSocial = xn["CONSULTAR_RUCResult"].ChildNodes[3].InnerText ?? "";
                        entidad.Direccion = xn["CONSULTAR_RUCResult"].ChildNodes[4].InnerText ?? "";
                    };

                    if (auxEstado)
                    {
                        // Para limpiar: "-</td>\r\n </tr>\r\n\r\n <tr>\r\n "
                        if (!string.IsNullOrEmpty(entidad.Direccion))
                        {
                            var auxLenght = entidad.Direccion.Length;
                            entidad.Direccion = entidad.Direccion.Substring(0, auxLenght - 25);
                        }

                        return new Response<RucEntity>(true, entidad, Message.MsgCorrectoConsultaSUNAT, true);
                    }
                    else
                        return new Response<RucEntity>(false, entidad, Message.MsgErrorConsultaSUNAT, false);
                }
                else
                    return new Response<RucEntity>(false, entidad, Message.MsgErrorServicioConsultaSUNAT, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<RucEntity>(false, null, Message.MsgExcConsultaSUNAT, false);
            }
        }

        public static Response<bool> GrabarPasajero(List<ClientePasajeEntity> lista)
        {
            try
            {
                foreach (var entidad in lista)
                {
                    // Busca 'Pasajero'
                    var buscaPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDoc, entidad.NumeroDoc);

                    if (buscaPasajero.IdCliente > 0)
                    {
                        // Modifica 'Pasajero'
                        var modificarPasajero = ClientePasajeRepository.ModificarPasajero(entidad);
                        if (!modificarPasajero)
                            new Response<bool>(false, modificarPasajero, Message.MsgErrorModificarPasajero, false);
                    }
                    else
                    {
                        // Graba 'Pasajero'
                        var grabarPasajero = ClientePasajeRepository.GrabarPasajero(entidad);
                        if (grabarPasajero <= 0)
                            new Response<bool>(false, false, Message.MsgErrorGrabarPasajero, false);
                    }

                    // Valida 'RucContacto'
                    if (string.IsNullOrEmpty(entidad.RucContacto)) continue;

                    // Busca 'Empresa'
                    var buscarEmpresa = ClientePasajeRepository.BuscarEmpresa(entidad.RucContacto);

                    var objEmpresa = new RucEntity
                    {
                        RucCliente = entidad.RucContacto ?? string.Empty,
                        RazonSocial = entidad.RazonSocial ?? string.Empty,
                        Direccion = entidad.Direccion ?? string.Empty,
                        Telefono = entidad.Telefono ?? string.Empty
                    };

                    if (!string.IsNullOrEmpty(buscarEmpresa.RucCliente))
                    {
                        // Modifica 'Empresa'
                        var modificarEmpresa = ClientePasajeRepository.ModificarEmpresa(objEmpresa);
                        if (!modificarEmpresa)
                            new Response<bool>(false, modificarEmpresa, Message.MsgErrorModificarEmpresa, false);
                    }

                    else
                    {
                        // Graba 'Empresa'
                        var grabarEmpresa = ClientePasajeRepository.GrabarEmpresa(objEmpresa);
                        if (!grabarEmpresa)
                            new Response<bool>(false, grabarEmpresa, Message.MsgErrorGrabarEmpresa, false);
                    }
                }

                return new Response<bool>(true, true, Message.MsgCorrectoGrabarClientePasaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcGrabarClientePasaje, false);
            }
        }
    }
}
