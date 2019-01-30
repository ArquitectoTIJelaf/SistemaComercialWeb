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
        #region LOGIN

        public ResFiltroUsuario ValidaUsuario(string CodiUsuario, string Password)
        {
            try
            {
                return UsuarioLogic.ValidaUsuario(short.Parse(CodiUsuario), Password);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroUsuario(false, null, Message.MsgErrExcBusqUsuario, false);
            }
        }

        #endregion

        #region OFICINA, SERVICIO, PUNTO DE VENTA Y EMPRESA

        public ResListaOficina ListaOficinas()
        {
            try
            {
                return OficinaLogic.ListarTodos();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaOficina(false, null, Message.MsgErrExcListOficina, false);
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
                return new ResListaServicio(false, null, Message.MsgErrExcListServicio, false);
            }
        }

        public ResListaPuntoVenta ListaPuntosVenta(string Codi_Sucursal)
        {
            try
            {
                return PuntoVentaLogic.ListarTodos(Codi_Sucursal);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaPuntoVenta(false, null, Message.MsgErrExcListPuntoVenta, false);
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
                return new ResListaEmpresa(false, null, Message.MsgErrExcListEmpresa, false);
            }
        }

        #endregion

        #region REGISTRO CLIENTE

        public ResFiltroClientePasaje BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            try
            {
                return ClientePasajeLogic.BuscaPasajero(TipoDoc, NumeroDoc);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResFiltroClientePasaje(false, null, Message.MsgErrExcBusqClientePasaje, false);
            }
        }

        public Response<bool> GrabarPasajero(ClientePasajeEntity entidad)
        {
            try
            {
                return ClientePasajeLogic.GrabarPasajero(entidad);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcGrabarClientePasaje, false);
            }
        }

        #endregion

        #region BÚSQUEDA ITINERARIO

        public ResListaItinerario BuscaItinerarios(ItinerarioEntity entidad)
        {
            try
            {
                return ItinerarioLogic.BuscaItinerarios(entidad);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new ResListaItinerario(false, null, Message.MsgErrExcListItinerario, false);
            }
        }

        #endregion
    }
}
