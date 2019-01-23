using SisComWeb.Business;
using SisComWeb.Entity;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SisComWeb.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SisComServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SisComServices.svc or SisComServices.svc.cs at the Solution Explorer and start debugging.
    public class SisComServices : ISisComServices
    {

        public ResListaCliente Listar()
        {

            try
            {
                return ClientePasajesLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaCliente(false, null, Message.MsgErrExcListClientePasaje);
            }

        }

        public ResFiltroCliente Filtrar(string id)
        {

            try
            {
                return ClientePasajesLogic.FiltrarxCodigo(int.Parse(id));
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroCliente(false, null, Message.MsgErrExcBusqClientePasaje);
            }

        }

        public Response<object> Grabar()
        {
            throw new NotImplementedException();
        }

        public Response<object> Modificar()
        {
            throw new NotImplementedException();
        }

        public Response<object> Eliminar()
        {
            throw new NotImplementedException();
        }

        public ResFiltroUsuario ValidaUsuario(string CodiUsuario, string Password)
        {

            try
            {
                return UsuarioPasajesLogic.ValidaUsuario(short.Parse(CodiUsuario), Password);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroUsuario(false, null, Message.MsgErrExcBusqUsuarioPasaje);
            }

        }

        public ResListaOficina ListaOficinas()
        {
            try
            {
                return OficinaPasajesLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaOficina(false, null, Message.MsgErrExcListOficinaPasaje);
            }
        }

        public ResListaServicio ListaServicios()
        {
            try
            {
                return ServicioPasajesLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaServicio(false, null, Message.MsgErrExcListServicioPasaje);
            }
        }

        public ResListaPuntoVenta ListaPuntosVenta()
        {
            try
            {
                return PuntoVentaPasajesLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaPuntoVenta(false, null, Message.MsgErrExcListPuntoVentaPasaje);
            }
        }

        public ResListaEmpresa ListaEmpresas()
        {
            try
            {
                return EmpresaPasajesLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaEmpresa(false, null, Message.MsgErrExcListEmpresaPasaje);
            }
        }
    }
}
