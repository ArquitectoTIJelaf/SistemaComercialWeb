using SisComWeb.Business.ServicioConsultaDNIRUC;
using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class ClientePasajeLogic
    {
        public static ResFiltroClientePasaje BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            try
            {
                var response = ClientePasajeRepository.BuscaPasajero(TipoDoc, NumeroDoc);
                return new ResFiltroClientePasaje(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroClientePasaje(false, null, Message.MsgErrExcBusqClientePasaje, false);
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


                //Validación 'BuscaPasajero'
                var objPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDoc, entidad.NumeroDoc);
                if (objPasajero.Estado == true) response.Mensaje += objPasajero.Mensaje;
                else {
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
                    resGrabarPasajero = ClientePasajeRepository.GrabarPasajero(objClientePasajeEntity);
                    if (resGrabarPasajero.Estado == true) response.Mensaje += resGrabarPasajero.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: GrabarPasajero. ";
                        return response;
                    }
                }


                //Validación 'RucContacto'
                if (!string.IsNullOrEmpty(entidad.RucContacto))
                {
                    response.Mensaje += "RucContacto: " + entidad.RucContacto + ". ";
                }
                else
                {
                    response.Estado = true;
                    response.Mensaje += "Error: RucContacto nulo o vacío. ";
                    return response;
                }


                //Validación 'BuscarEmpresa'
                var objEmpresa = RucRepository.BuscarEmpresa(entidad.RucContacto);
                if (objEmpresa.Estado == true) response.Mensaje += objEmpresa.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscarEmpresa. ";
                    return response;
                }

                objEmp = new RucEntity
                {
                    RucCliente = objEmpresa.Valor.RucCliente,
                    RazonSocial = objEmpresa.Valor.RazonSocial,
                    Direccion = entidad.Direccion,
                    Telefono = entidad.Telefono
                };

                if (!string.IsNullOrEmpty(objEmpresa.Valor.RucCliente))
                {
                    resEmpresa = RucRepository.ModificarEmpresa(objEmp);
                    if (resEmpresa.Estado == true) response.Mensaje += resEmpresa.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: ModificarEmpresa. ";
                        return response;
                    }
                }
                else
                {
                    //Consulta RUC a la SUNAT
                    var objSUNAT = ConsultaSUNAT(entidad.RucContacto);
                    if (objSUNAT.Estado == true) response.Mensaje += objSUNAT.Mensaje;
                    else
                    {
                        response.Mensaje += "Error: ConsultaSUNAT. ";
                        return response;
                    }

                    // Preparamos el objeto para 'GrabarEmpresa'
                    objEmp.RucCliente = entidad.RucContacto;
                    objEmp.RazonSocial = objSUNAT.Valor;

                    resEmpresa = RucRepository.GrabarEmpresa(objEmp);
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
            //Referencia al servicio
            WsConsultaSoapClient Ser = new WsConsultaSoapClient();

            //Consulta a la SUNAT
            var responseSunatRUC = Ser.CONSULTAR_RUC(RucContacto);

            try
            {
                return new Response<string>(responseSunatRUC.Estado, responseSunatRUC.RAZON_SOCIAL, "Se consultó correctamente a la SUNAT. ", responseSunatRUC.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, responseSunatRUC.MensajeError, false);
            }
        }
    }
}
