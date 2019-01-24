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
            RucEntity objrucEntity;

            objclientePasajeEntity = request.ClientePasajeEntity;
            objrucEntity = request.RucEntity;

            try
            {
                var objPasajero = ClientePasajeRepository.BuscaPasajero(objclientePasajeEntity.TipoDoc, objclientePasajeEntity.NumeroDoc);
                var objEmpresa = RucRepository.BuscarEmpresa(objrucEntity.RucCliente);

                

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
