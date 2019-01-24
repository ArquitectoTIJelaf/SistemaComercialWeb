using SisComWeb.Business;
using SisComWeb.Entity;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SisComServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SisComServices.svc or SisComServices.svc.cs at the Solution Explorer and start debugging.
    public class SisComServices : ISisComServices
    {
        //public ResListaClientePasaje Listar()
        //{
        //    try
        //    {
        //        return ClientePasajeLogic.ListarTodos();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        return new ResListaClientePasaje(false, null, Message.MsgErrExcListClientePasaje);
        //    }

        //}

        //public ResFiltroClientePasaje Filtrar(string id)
        //{
        //    try
        //    {
        //        return ClientePasajeLogic.FiltrarxCodigo(int.Parse(id));
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        return new ResFiltroClientePasaje(false, null, Message.MsgErrExcBusqClientePasaje);
        //    }

        //}

        //public Response<object> Grabar()
        //{
        //    throw new NotImplementedException();
        //}

        //public Response<object> Modificar()
        //{
        //    throw new NotImplementedException();
        //}

        //public Response<object> Eliminar()
        //{
        //    throw new NotImplementedException();
        //}

        public ResFiltroUsuario ValidaUsuario(string CodiUsuario, string Password)
        {
            try
            {
                return UsuarioLogic.ValidaUsuario(short.Parse(CodiUsuario), Password);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroUsuario(false, null, Message.MsgErrExcBusqUsuario);
            }
        }

        public ResListaOficina ListaOficinas()
        {
            try
            {
                return OficinaLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaOficina(false, null, Message.MsgErrExcListOficina);
            }
        }

        public ResListaServicio ListaServicios()
        {
            try
            {
                return ServicioLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaServicio(false, null, Message.MsgErrExcListServicio);
            }
        }

        public ResListaPuntoVenta ListaPuntosVenta()
        {
            try
            {
                return PuntoVentaLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaPuntoVenta(false, null, Message.MsgErrExcListPuntoVenta);
            }
        }

        public ResListaEmpresa ListaEmpresas()
        {
            try
            {
                return EmpresaLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaEmpresa(false, null, Message.MsgErrExcListEmpresa);
            }
        }

        public ResFiltroClientePasaje BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            try
            {
                return ClientePasajeLogic.BuscaPasajero(TipoDoc, NumeroDoc);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroClientePasaje(false, null, Message.MsgErrExcBusqClientePasaje);
            }
        }

        //public Response<bool> GrabaPasajero(ClientePasajeEntity Objeto)
        //{
        //    try
        //    {
        //        return ClientePasajeLogic.GrabaPasajero(ClientePasajeEntity Objeto);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        return new Response(false, null, Message.MsgErrExcBusqClientePasaje);
        //    }
        //}
    }
}
