using SisComWeb.Business.ServicioConsultaDNIRUC;
using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class ClientePasajeLogic
    {
        public static ResFiltroClientePasaje GrabarPasajero(ClientePasajeEntity entidad)
        {
            try
            {
                ClientePasajeEntity objclientePasajeEntity;
                Response<RucEntity> objEmpresa = new Response<RucEntity>();

                var objPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDoc, entidad.NumeroDoc);

                if (objPasajero != null)
                {
                    if (string.IsNullOrWhiteSpace(objPasajero.Valor.RucContacto) == false)
                    {
                        //Referencia al servicio
                        WsConsultaSoapClient ser = new WsConsultaSoapClient();

                        //Consulta a la SUNAT
                        var responseSunatRUC = ser.CONSULTAR_RUC(objPasajero.Valor.RucContacto);
                        if (string.IsNullOrWhiteSpace(responseSunatRUC.RAZON_SOCIAL) == false && string.IsNullOrWhiteSpace(responseSunatRUC.RUC) == false)
                        {
                            objEmpresa = RucRepository.BuscarEmpresa(objPasajero.Valor.RucContacto);

                            if (objEmpresa.EsCorrecto = false && objEmpresa.Valor != null)
                            {
                                objEmpresa.Valor.RucCliente = responseSunatRUC.RUC;
                                objEmpresa.Valor.RazonSocial = responseSunatRUC.RAZON_SOCIAL;
                                objEmpresa.Valor.Direccion = "";
                            }
                            else
                            {
                                objEmpresa.Valor.RucCliente = responseSunatRUC.RUC;
                                objEmpresa.Valor.RazonSocial = responseSunatRUC.RAZON_SOCIAL;
                                objEmpresa.Valor.Direccion = objEmpresa.Valor.Direccion;
                            }

                            var objEmp = new RucEntity
                            {
                                RucCliente = responseSunatRUC.RUC,
                                RazonSocial = responseSunatRUC.RAZON_SOCIAL,
                                Direccion = objPasajero.Valor.Direccion,
                                Telefono = objPasajero.Valor.Telefono
                            };

                            RucRepository.ModificarEmpresa(objEmp);
                        }
                        else
                        {
                            var responses = new ResFiltroClientePasaje
                            {
                                Estado = objPasajero.Estado,
                                Mensaje = objPasajero.Mensaje
                            };
                        }
                    }

                    objclientePasajeEntity = new ClientePasajeEntity
                    {
                        TipoDoc = objPasajero.Valor.TipoDoc,
                        NumeroDoc = objPasajero.Valor.NumeroDoc,
                        NombreCliente = objPasajero.Valor.NombreCliente,
                        ApellidoPaterno = objPasajero.Valor.ApellidoPaterno,
                        ApellidoMaterno = objPasajero.Valor.ApellidoMaterno,
                        FechaNacimiento = objPasajero.Valor.FechaNacimiento,
                        Edad = objPasajero.Valor.Edad,
                        Direccion = objPasajero.Valor.Direccion,
                        Telefono = (objPasajero.Valor.Telefono != null) ? objPasajero.Valor.Telefono : string.Empty,
                        RucContacto = (objEmpresa.Valor.RucCliente != null) ? objEmpresa.Valor.RucCliente : string.Empty
                    };

                    var result = ClientePasajeRepository.ModificarPasajero(objclientePasajeEntity);
                }
                else
                {
                    objclientePasajeEntity = new ClientePasajeEntity
                    {
                        TipoDoc = entidad.TipoDoc,
                        NumeroDoc = entidad.NumeroDoc,
                        NombreCliente = entidad.NombreCliente,
                        ApellidoPaterno = entidad.ApellidoPaterno,
                        ApellidoMaterno = entidad.ApellidoMaterno,
                        FechaNacimiento = entidad.FechaNacimiento,
                        Edad = entidad.Edad,
                        Direccion = entidad.Direccion,
                        Telefono = entidad.Telefono,
                        RucContacto = entidad.RucContacto,
                    };

                    var result = ClientePasajeRepository.GrabarPasajero(objclientePasajeEntity);
                }

                var response = new ResFiltroClientePasaje
                {
                    Estado = objPasajero.Estado,
                    Mensaje = objPasajero.Mensaje,
                    Valor = objPasajero.Valor
                };

                return response;

            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroClientePasaje(false, null, Message.MsgErrExcBusqClientePasaje);
            }
        }

        public static ResFiltroClientePasaje BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            try
            {
                var response = ClientePasajeRepository.BuscaPasajero(TipoDoc, NumeroDoc);
                return new ResFiltroClientePasaje(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroClientePasaje(false, null, Message.MsgErrExcBusqClientePasaje);
            }
        }

    }
}
