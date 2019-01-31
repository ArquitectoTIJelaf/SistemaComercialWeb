using SisComWeb.Business;
using SisComWeb.Entity;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SisComServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SisComServices.svc or SisComServices.svc.cs at the Solution Explorer and start debugging.
    public class SisComServices : ISisComServices
    {
<<<<<<< HEAD
        #region BASE

        public Response<List<BaseEntity>> ListaOficinas()
=======
        #region LOGIN

        public ResFiltroUsuario ValidaUsuario(string CodiUsuario, string Password)
>>>>>>> eca434352a135e8a9a42eefca29ea430a03694af
        {
            try
            {
                return BaseLogic.ListaOficinas();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListOficina, false);
            }
        }

        public Response<List<BaseEntity>> ListaPuntosVenta(string CodiSucursal)
        {
            try
            {
                return BaseLogic.ListaPuntosVenta(short.Parse(CodiSucursal));
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListPuntoVenta, false);
            }
        }

<<<<<<< HEAD
        public Response<List<BaseEntity>> ListaUsuarios(string CodiSucursal, string CodiPuntoVenta)
=======
        #endregion

        #region OFICINA, SERVICIO, PUNTO DE VENTA Y EMPRESA

        public ResListaOficina ListaOficinas()
>>>>>>> eca434352a135e8a9a42eefca29ea430a03694af
        {
            try
            {
                return BaseLogic.ListaUsuarios(short.Parse(CodiSucursal), short.Parse(CodiPuntoVenta));
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListUsuario, false);
            }
        }

        public Response<List<BaseEntity>> ListaServicios()
        {
            try
            {
                return BaseLogic.ListaServicios();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListServicio, false);
            }
        }

<<<<<<< HEAD
        public Response<List<BaseEntity>> ListaEmpresas()
        {
            try
            {
                return BaseLogic.ListaEmpresas();
=======
        public ResListaPuntoVenta ListaPuntosVenta(string Codi_Sucursal)
        {
            try
            {
                return PuntoVentaLogic.ListarTodos(Codi_Sucursal);
>>>>>>> eca434352a135e8a9a42eefca29ea430a03694af
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListEmpresa, false);
            }
        }

        #endregion

        #region LOGIN

        public Response<UsuarioEntity> ValidaUsuario(string CodiUsuario, string Password)
        {
            try
            {
                return UsuarioLogic.ValidaUsuario(short.Parse(CodiUsuario), Password);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<UsuarioEntity>(false, null, Message.MsgErrExcBusqUsuario, false);
            }
        }

        #endregion

        #region REGISTRO CLIENTE

        public Response<ClientePasajeEntity> BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            try
            {
                return ClientePasajeLogic.BuscaPasajero(TipoDoc, NumeroDoc);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ClientePasajeEntity>(false, null, Message.MsgErrExcBusqClientePasaje, false);
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

        public Response<ItinerarioEntity> BuscaItinerarios(ItinerarioEntity entidad)
        {
            try
            {
                return ItinerarioLogic.BuscaItinerarios(entidad);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ItinerarioEntity>(false, null, Message.MsgErrExcListItinerario, false);
            }
        }

        #endregion
    }
}
