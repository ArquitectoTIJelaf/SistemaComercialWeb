using Newtonsoft.Json.Linq;
using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

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

                // Valida 'FechaNacimiento'
                if (buscaPasajero.FechaNacimiento == "01/01/1900")
                {
                    buscaPasajero.FechaNacimiento = string.Empty;
                    buscaPasajero.Edad = 0;
                }

                var buscaEmpresa = new RucEntity();

                if (!string.IsNullOrEmpty(buscaPasajero.RucContacto))
                {
                    buscaEmpresa = ClientePasajeRepository.BuscarEmpresa(buscaPasajero.RucContacto);
                    buscaPasajero.RazonSocial = buscaEmpresa.RazonSocial;
                    buscaPasajero.Direccion = buscaEmpresa.Direccion;
                }
                // Con esto nos aseguramos que estamos tomando solo la 'Direccion' de la empresa
                else
                    buscaPasajero.Direccion = string.Empty;

                // Consulta 'RENIEC'
                if (TipoDoc == "1" && string.IsNullOrEmpty(buscaPasajero.NumeroDoc))
                {
                    var resConsultaRENIEC = ConsultaRENIEC(NumeroDoc);
                    if (resConsultaRENIEC.EsCorrecto)
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
                        if (resConsultaSUNAT.EsCorrecto)
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
                    if (resConsultaSUNAT.EsCorrecto)
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
                    var result = res.Content.ReadAsStringAsync().Result ?? string.Empty;
                    string[] arrayNombreCompleto = result.Split('|');

                    // Valida Split
                    if (arrayNombreCompleto.Length != 3)
                        arrayNombreCompleto = new string[3];
                    // ------------

                    entidad.ApellidoPaterno = arrayNombreCompleto[0];
                    entidad.ApellidoMaterno = arrayNombreCompleto[1];
                    entidad.Nombres = arrayNombreCompleto[2];

                    if (!string.IsNullOrEmpty(entidad.ApellidoPaterno) && !string.IsNullOrEmpty(entidad.ApellidoMaterno) && !string.IsNullOrEmpty(entidad.Nombres))
                        return new Response<ReniecEntity>(true, entidad, Message.MsgCorrectoConsultaRENIEC, true);
                    else
                        return new Response<ReniecEntity>(false, entidad, Message.MsgErrorConsultaRENIEC, true);
                }
                else
                    return new Response<ReniecEntity>(false, entidad, Message.MsgErrorServicioConsultaRENIEC, true);
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

                var client = new HttpClient
                {
                    BaseAddress = new Uri(ServiceSUNAT)
                };

                var response = client.GetAsync(RucContacto).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (result != "[]") {
                        JToken tmpResult = JObject.Parse(result);

                        entidad.RazonSocial = (string)tmpResult["razon_social"];
                        entidad.Direccion = (string)tmpResult["domicilio_fiscal"];

                        return new Response<RucEntity>(true, entidad, Message.MsgCorrectoConsultaSUNAT, true);
                    }
                    else
                        return new Response<RucEntity>(false, entidad, Message.MsgErrorConsultaSUNAT, true);
                }
                else
                    return new Response<RucEntity>(false, entidad, Message.MsgErrorServicioConsultaSUNAT, true);
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
                    if (entidad.IdCliente > 0)
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
                    if (string.IsNullOrEmpty(entidad.RucContacto))
                        continue;

                    // Busca 'Empresa'
                    var buscarEmpresa = ClientePasajeRepository.BuscarEmpresa(entidad.RucContacto);

                    var objEmpresa = new RucEntity
                    {
                        RucCliente = entidad.RucContacto ?? string.Empty,
                        RazonSocial = entidad.RazonSocial ?? string.Empty,
                        Direccion = entidad.Direccion ?? string.Empty,
                        Telefono = string.Empty
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

        public static Response<List<ClientePasajeEntity>> BuscarClientesPasaje(string campo, string nombres, string paterno, string materno, string TipoDocId)
        {
            try
            {
                var BuscarClientesPasaje = ClientePasajeRepository.BuscarClientesPasaje(campo, nombres, paterno, materno, TipoDocId);

                return new Response<List<ClientePasajeEntity>>(true, BuscarClientesPasaje, Message.MsgCorrectoBuscarClientesPasaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ClientePasajeEntity>>(false, null, Message.MsgExcBuscarClientesPasaje, false);
            }
        }
    }
}
