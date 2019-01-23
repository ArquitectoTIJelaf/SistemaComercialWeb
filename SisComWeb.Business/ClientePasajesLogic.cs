using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public static class ClientePasajesLogic
    {
        public static ResListaCliente ListarTodos()
        {
            try
            {
                var response = ClientePasajesRepository.ListarTodos();
                return new ResListaCliente(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajesLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaCliente(false, null, Message.MsgErrExcListClientePasaje);
            }
        }

        public static ResFiltroCliente FiltrarxCodigo(int Codigo)
        {
            try
            {
                var response = ClientePasajesRepository.FiltrarxCodigo(Codigo);
                return new ResFiltroCliente(response.EsCorrecto, response.Valor, response.Mensaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(ClientePasajesLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroCliente(false, null, Message.MsgErrExcBusqClientePasaje);
            }
        }


    }
}
