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
                Response<bool> ResultadoEmpresa;
                Response<bool> resultadoPasajero;
                string Mensaje = "";

                var objPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDoc, entidad.NumeroDoc);

                if (objPasajero.Valor.NumeroDoc != null)
                {
                    if (!string.IsNullOrEmpty(objPasajero.Valor.RucContacto))
                    {
                        objEmpresa = RucRepository.BuscarEmpresa(objPasajero.Valor.RucContacto);

                        //Referencia al servicio
                        WsConsultaSoapClient Ser = new WsConsultaSoapClient();

                        //Consulta a la SUNAT
                        var responseSunatRUC = Ser.CONSULTAR_RUC(objEmpresa.Valor.RucCliente);

                        if (objEmpresa != null)
                        {
                            if (!string.IsNullOrEmpty(responseSunatRUC.RAZON_SOCIAL) && !string.IsNullOrEmpty(responseSunatRUC.RUC))
                            {
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
                                    RucCliente = objEmpresa.Valor.RucCliente,
                                    RazonSocial = objEmpresa.Valor.RazonSocial,
                                    Direccion = objEmpresa.Valor.Direccion,
                                    Telefono = objPasajero.Valor.Telefono
                                };

                                ResultadoEmpresa = RucRepository.ModificarEmpresa(objEmp);

                                if (ResultadoEmpresa.EsCorrecto == true && ResultadoEmpresa.Estado == true)
                                {
                                    Mensaje = "Se modificó correctamente la empresa, ";
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(responseSunatRUC.RAZON_SOCIAL) && !string.IsNullOrEmpty(responseSunatRUC.RUC))
                            {
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
                                    RucCliente = objEmpresa.Valor.RucCliente,
                                    RazonSocial = objEmpresa.Valor.RazonSocial,
                                    Direccion = objEmpresa.Valor.Direccion,
                                    Telefono = objPasajero.Valor.Telefono
                                };

                                ResultadoEmpresa = RucRepository.GrabarEmpresa(objEmp);

                                if (ResultadoEmpresa.EsCorrecto == true && ResultadoEmpresa.Estado == true && ResultadoEmpresa.Valor == true)
                                {
                                    Mensaje = Mensaje + "Se Insertó correctamente la empresa, ";
                                }
                            }
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
                        RucContacto = (objEmpresa.Valor != null) ? objEmpresa.Valor.RucCliente : string.Empty,
                    };

                    resultadoPasajero = ClientePasajeRepository.ModificarPasajero(objclientePasajeEntity);

                    if (resultadoPasajero.EsCorrecto == true && resultadoPasajero.Estado == true && resultadoPasajero.Valor == true)
                    {
                        Mensaje = Mensaje + "Se modificó correctamente el pasajero, ";
                    }
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

                    resultadoPasajero = ClientePasajeRepository.GrabarPasajero(objclientePasajeEntity);

                    if (resultadoPasajero.EsCorrecto == true && resultadoPasajero.Estado == true && resultadoPasajero.Valor == true)
                    {
                        Mensaje = Mensaje + "Se Insertó correctamente el pasajero.";
                    }
                }

                var response = new ResFiltroClientePasaje
                {
                    Estado = objPasajero.Estado,
                    Mensaje = Mensaje.Substring(0, Mensaje.Length - 2) + ".",
                    Valor = objPasajero.Valor,
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
