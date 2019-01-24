using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class ClientePasajeLogic
    {
        //public static ResListaClientePasaje ListarTodos()
        //{
        //    try
        //    {
        //        var response = ClientePasajeRepository.ListarTodos();
        //        return new ResListaClientePasaje(response.EsCorrecto, response.Valor, response.Mensaje);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        return new ResListaClientePasaje(false, null, Message.MsgErrExcListClientePasaje);
        //    }
        //}

        //public static ResFiltroClientePasaje FiltrarxCodigo(int Codigo)
        //{
        //    try
        //    {
        //        var response = ClientePasajeRepository.FiltrarxCodigo(Codigo);
        //        return new ResFiltroClientePasaje(response.EsCorrecto, response.Valor, response.Mensaje);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        return new ResFiltroClientePasaje(false, null, Message.MsgErrExcBusqClientePasaje);
        //    }
        //}

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

        //public static ResFiltroClientePasaje BuscaPasajero(string TipoDoc, string NumeroDoc)
        //{
        //    try
        //    {
        //        var response = ClientePasajeRepository.BuscaPasajero(TipoDoc, NumeroDoc);
        //        return new ResFiltroClientePasaje(response.EsCorrecto, response.Valor, response.Mensaje);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance(typeof(ClientePasajeLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        return new ResFiltroClientePasaje(false, null, Message.MsgErrExcBusqClientePasaje);
        //    }
        //}
    }
}
