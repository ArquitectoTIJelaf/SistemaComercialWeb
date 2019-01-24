using SisComWeb.Business.ServicioConsultaDNIRUC;
using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class ClientePasajeLogic
    {
        public static ResFiltroClientePasaje GrabarPasajero(ResRequestClientePasaje request)
        {
            try
            {
                ClientePasajeEntity objclientePasajeEntity;
                Response<RucEntity> objEmpresa = new Response<RucEntity>();

                var objPasajero = ClientePasajeRepository.BuscaPasajero(request.ClientePasajeEntity.TipoDoc, request.ClientePasajeEntity.NumeroDoc);

                if (objPasajero != null)
                {
                    if (!string.IsNullOrWhiteSpace(objPasajero.Valor.RucContacto) == false)
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
                        Telefono = objPasajero.Valor.Telefono,
                        RucContacto = objEmpresa.Valor.RucCliente
                    };

                    var result = ClientePasajeRepository.ModificarPasajero(objclientePasajeEntity);
                }
                else
                {
                    objclientePasajeEntity = new ClientePasajeEntity
                    {
                        TipoDoc = request.ClientePasajeEntity.TipoDoc,
                        NumeroDoc = request.ClientePasajeEntity.NumeroDoc,
                        NombreCliente = request.ClientePasajeEntity.NombreCliente,
                        ApellidoPaterno = request.ClientePasajeEntity.ApellidoPaterno,
                        ApellidoMaterno = request.ClientePasajeEntity.ApellidoMaterno,
                        FechaNacimiento = request.ClientePasajeEntity.FechaNacimiento,
                        Edad = request.ClientePasajeEntity.Edad,
                        Direccion = request.ClientePasajeEntity.Direccion,
                        Telefono = request.ClientePasajeEntity.Telefono,
                        RucContacto = request.ClientePasajeEntity.RucContacto,
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
    }
}
