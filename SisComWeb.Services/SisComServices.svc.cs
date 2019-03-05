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
        #region BASE

        public Response<List<BaseEntity>> ListaOficinas()
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

        public Response<List<BaseEntity>> ListaUsuarios(string value)
        {
            try
            {
                return BaseLogic.ListaUsuarios(value);
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

        public Response<List<BaseEntity>> ListaEmpresas()
        {
            try
            {
                return BaseLogic.ListaEmpresas();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListEmpresa, false);
            }
        }

        public Response<List<BaseEntity>> ListaTiposDoc()
        {
            try
            {
                return BaseLogic.ListaTiposDoc();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListTipoDoc, false);
            }
        }

        public Response<List<BaseEntity>> ListaTipoPago()
        {
            try
            {
                return BaseLogic.ListaTipoPago();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListTipoPago, false);
            }
        }

        public Response<List<BaseEntity>> ListaTarjetaCredito()
        {
            try
            {
                return BaseLogic.ListaTarjetaCredito();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListTarjetaCredito, false);
            }
        }

        public Response<List<BaseEntity>> ListaCiudad()
        {
            try
            {
                return BaseLogic.ListaCiudad();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListCiudad, false);
            }
        }

        public Response<List<BaseEntity>> ListarParentesco()
        {
            try
            {
                return BaseLogic.ListarParentesco();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListCiudad, false);
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
                return new Response<UsuarioEntity>(false, null, Message.MsgErrExcValidaUsuario, false);
            }
        }

        #endregion

        #region GRABA PASAJERO

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

        public Response<bool> GrabarPasajero(List<ClientePasajeEntity> lista)
        {
            try
            {
                return ClientePasajeLogic.GrabarPasajero(lista);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcGrabarClientePasaje, false);
            }
        }

        public Response<RucEntity> ConsultarSUNAT(string RucContacto)
        {
            try
            {
                return ClientePasajeLogic.ConsultarSUNAT(RucContacto);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<RucEntity>(false, null, Message.MsgErrExcConsultarSUNAT, false);
            }
        }

        #endregion

        #region BÚSQUEDA ITINERARIO

        public Response<List<ItinerarioEntity>> BuscaItinerarios(ItinerarioRequest request)
        {
            try
            {
                return ItinerarioLogic.BuscaItinerarios(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ItinerarioEntity>>(false, null, Message.MsgErrExcBuscaItinerarios, false);
            }
        }

        #endregion

        #region MUESTRA PLANO

        public Response<List<PlanoEntity>> MuestraPlano(PlanoRequest request)
        {
            try
            {
                return PlanoLogic.MuestraPlano(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<PlanoEntity>>(false, null, Message.MsgErrExcMuestraPlano, false);
            }
        }

        #endregion

        #region MUESTRA TURNO

        public Response<ItinerarioEntity> MuestraTurno(TurnoRequest request)
        {
            try
            {
                return TurnoLogic.MuestraTurno(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ItinerarioEntity>(false, null, Message.MsgErrExcMuestraTurno, false);
            }
        }

        #endregion

        #region BLOQUEO ASIENTO

        public Response<int> BloqueoAsiento(BloqueoAsientoRequest request)
        {
            try
            {
                return BloqueoAsientoLogic.BloqueoAsiento(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgErrExcBloqueoAsiento, false);
            }
        }

        public Response<bool> LiberaAsiento(int IDS)
        {
            try
            {
                return BloqueoAsientoLogic.LiberaAsiento(IDS);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcLiberaAsiento, false);
            }
        }

        #endregion

        #region GRABA VENTA

        public Response<CorrelativoEntity> BuscaCorrelativo(CorrelativoRequest request)
        {
            try
            {
                return VentaLogic.BuscaCorrelativo(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<CorrelativoEntity>(false, null, Message.MsgErrExcBuscaCorrelativo, false);
            }
        }

        public Response<string> GrabaVenta(List<VentaEntity> listado)
        {
            try
            {
                return VentaLogic.GrabaVenta(listado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrExcGrabaVenta, false);
            }
        }

        #endregion
    }
}
