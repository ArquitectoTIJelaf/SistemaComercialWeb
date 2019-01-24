using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class ClientePasajeLogic
    {
        public static ResFiltroClientePasaje GrabarPasajero(ResRequestClientePasaje request)
        {
            ClientePasajeEntity objclientePasajeEntity;

            try
            {
                var objPasajero = ClientePasajeRepository.BuscaPasajero(request.ClientePasajeEntity.TipoDoc, request.ClientePasajeEntity.NumeroDoc);
                var objEmpresa = RucRepository.BuscarEmpresa(request.RucEntity.RucCliente);

                if (objPasajero != null)
                {
                    if (!string.IsNullOrWhiteSpace(objPasajero.Valor.RucContacto) == false)
                    {
                        WSDNIRUC.WsConsultaSoapClient ser = new WSDNIRUC.WsConsultaSoapClient();
                        //Consulta a la SUNAT
                        var responseSunatRUC = ser.CONSULTAR_RUC(pasajero.Ruc_Cliente);
                        if (string.IsNullOrWhiteSpace(responseSunatRUC.RAZON_SOCIAL) == false && string.IsNullOrWhiteSpace(responseSunatRUC.RUC) == false)
                        {
                            var responseRuc = oRucDominio.buscar(new RucEntidad { Ruc_Cliente = pasajero.Ruc_Cliente });

                            if (responseRuc.EsCorrecto = false && responseRuc.Valor != null)
                            {
                                oRucEntidad.Ruc_Cliente = responseSunatRUC.RUC;
                                oRucEntidad.Razon_Social = responseSunatRUC.RAZON_SOCIAL;
                                oRucEntidad.Direccion = "";
                            }
                            else
                            {
                                oRucEntidad.Ruc_Cliente = responseSunatRUC.RUC;
                                oRucEntidad.Razon_Social = responseSunatRUC.RAZON_SOCIAL;
                                oRucEntidad.Direccion = responseRuc.Valor.Direccion;
                            }

                        }
                        else
                        {
                            response.Mensaje = Message.MsgErrNoBusqRucPasajero;
                            response.Estado = false;
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
                        RucContacto = objPasajero.Valor.RucContacto,
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
