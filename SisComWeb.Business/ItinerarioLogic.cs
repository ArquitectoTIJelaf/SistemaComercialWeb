using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class ItinerarioLogic
    {
        public static ResListaItinerario BuscaItinerarios(ItinerarioEntity entidad)
        {
            try
            {
                var response = new ResListaItinerario(false, null, "", false);
                //ClientePasajeEntity objClientePasajeEntity;
                //Response<bool> resModificarPasajero;
                //Response<int> resGrabarPasajero;
                //RucEntity objEmp;
                //Response<bool> resEmpresa;


                ////Validación 'BuscaPasajero'
                //var objPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDoc, entidad.NumeroDoc);
                //if (objPasajero.Estado == true) response.Mensaje += objPasajero.Mensaje;
                //else {
                //    response.Mensaje += "Error: BuscaPasajero. ";
                //    return response;
                //}

                //objClientePasajeEntity = new ClientePasajeEntity
                //{
                //    IdCliente = objPasajero.Valor.IdCliente,
                //    TipoDoc = entidad.TipoDoc,
                //    NumeroDoc = entidad.NumeroDoc,
                //    NombreCliente = entidad.NombreCliente,
                //    ApellidoPaterno = entidad.ApellidoPaterno,
                //    ApellidoMaterno = entidad.ApellidoMaterno,
                //    FechaNacimiento = entidad.FechaNacimiento,
                //    Edad = entidad.Edad,
                //    Direccion = entidad.Direccion ?? string.Empty,
                //    Telefono = entidad.Telefono ?? string.Empty,
                //    RucContacto = entidad.RucContacto ?? string.Empty
                //};

                //if (!string.IsNullOrEmpty(objPasajero.Valor.NumeroDoc))
                //{
                //    resModificarPasajero = ClientePasajeRepository.ModificarPasajero(objClientePasajeEntity);
                //    if (resModificarPasajero.Estado == true) response.Mensaje += resModificarPasajero.Mensaje;
                //    else
                //    {
                //        response.Mensaje += "Error: ModificarPasajero. ";
                //        return response;
                //    }
                //}
                //else
                //{
                //    resGrabarPasajero = ClientePasajeRepository.GrabarPasajero(objClientePasajeEntity);
                //    if (resGrabarPasajero.Estado == true) response.Mensaje += resGrabarPasajero.Mensaje;
                //    else
                //    {
                //        response.Mensaje += "Error: GrabarPasajero. ";
                //        return response;
                //    }
                //}


                ////Validación 'RucContacto'
                //if (!string.IsNullOrEmpty(entidad.RucContacto))
                //{
                //    response.Mensaje += "RucContacto: " + entidad.RucContacto + ". ";
                //}
                //else
                //{
                //    response.Estado = true;
                //    response.Mensaje += "Error: RucContacto nulo o vacío. ";
                //    return response;
                //}


                ////Validación 'BuscarEmpresa'
                //var objEmpresa = RucRepository.BuscarEmpresa(entidad.RucContacto);
                //if (objEmpresa.Estado == true) response.Mensaje += objEmpresa.Mensaje;
                //else
                //{
                //    response.Mensaje += "Error: BuscarEmpresa. ";
                //    return response;
                //}

                //objEmp = new RucEntity
                //{
                //    RucCliente = objEmpresa.Valor.RucCliente,
                //    RazonSocial = objEmpresa.Valor.RazonSocial,
                //    Direccion = entidad.Direccion,
                //    Telefono = entidad.Telefono
                //};

                //if (!string.IsNullOrEmpty(objEmpresa.Valor.RucCliente))
                //{
                //    resEmpresa = RucRepository.ModificarEmpresa(objEmp);
                //    if (resEmpresa.Estado == true) response.Mensaje += resEmpresa.Mensaje;
                //    else
                //    {
                //        response.Mensaje += "Error: ModificarEmpresa. ";
                //        return response;
                //    }
                //}
                //else
                //{
                //    //Consulta RUC a la SUNAT
                //    var objSUNAT = ConsultaSUNAT(entidad.RucContacto);
                //    if (objSUNAT.Estado == true) response.Mensaje += objSUNAT.Mensaje;
                //    else
                //    {
                //        response.Mensaje += "Error: ConsultaSUNAT. ";
                //        return response;
                //    }

                //    // Preparamos el objeto para 'GrabarEmpresa'
                //    objEmp.RucCliente = entidad.RucContacto;
                //    objEmp.RazonSocial = objSUNAT.Valor;

                //    resEmpresa = RucRepository.GrabarEmpresa(objEmp);
                //    if (resEmpresa.Estado == true) response.Mensaje += resEmpresa.Mensaje;
                //    else
                //    {
                //        response.Mensaje += "Error: GrabarEmpresa. ";
                //        return response;
                //    }
                //}


                //response.EsCorrecto = true;
                //response.Valor = true;
                //response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ItinerarioLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaItinerario(false, null, Message.MsgErrExcListItinerario, false);
            }
        }
    }
}
