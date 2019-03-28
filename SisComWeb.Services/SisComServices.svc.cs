﻿using SisComWeb.Business;
using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Entity.Peticiones.Response;
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

        public Response<List<BaseEntity>> ListarGerente()
        {
            try
            {
                return BaseLogic.ListarGerente();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListCiudad, false);
            }
        }

        public Response<List<BaseEntity>> ListarSocio()
        {
            try
            {
                return BaseLogic.ListarSocio();
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

        public Response<RucEntity> ConsultarSUNAT(string RucContacto, bool CondicionEmpresa)
        {
            try
            {
                return ClientePasajeLogic.ConsultarSUNAT(RucContacto, CondicionEmpresa);
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

        public Response<bool> LiberaArregloAsientos(int[] arregloIDS)
        {
            try
            {
                return BloqueoAsientoLogic.LiberaArregloAsientos(arregloIDS);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcLiberaArregloAsientos, false);
            }
        }

        #endregion

        #region GRABA VENTA

        public Response<string> BuscaCorrelativo(CorrelativoRequest request)
        {
            try
            {
                return VentaLogic.BuscaCorrelativo(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrExcBuscaCorrelativo, false);
            }
        }

        public Response<string> GrabaVenta(List<VentaEntity> listado, string FlagVenta)
        {
            try
            {
                switch (FlagVenta)
                {
                    case "R": // Reserva
                        return VentaLogic.GrabaReserva(listado);
                    default:
                        return VentaLogic.GrabaVenta(listado, FlagVenta);
                }
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrExcGrabaVenta, false);
            }
        }

        public Response<PaseCortesiaResponse> ListaBeneficiarioPase(string Codi_Socio, string año, string mes)
        {
            try
            {
                return VentaLogic.ListaBeneficiarioPase(Codi_Socio, año, mes);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PaseCortesiaResponse>(false, null, Message.MsgErrExcBeneficiarioPase, false);
            }
        }

        public Response<bool> ValidarPase(string CodiSocio, string Mes, string Anno)
        {
            try
            {
                return PaseLogic.ValidarSaldoPaseCortesia(CodiSocio, Mes, Anno);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcValidarPase, false);
            }
        }

        public Response<ClavesInternasResponse> ClavesInternas(int Codi_Oficina, string Password, string Codi_Tipo)
        {
            try
            {
                return VentaLogic.ClavesInternas(Codi_Oficina, Password, Codi_Tipo);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ClavesInternasResponse>(false, null, Message.MsgErrExcClavesInternas, false);
            }
        }


        public Response<VentaBeneficiarioEntity> BuscarVentaxBoleto(string Tipo, short Serie, int Numero, short CodiEmpresa)
        {
            try
            {
                return VentaLogic.BuscarVentaxBoleto(Tipo, Serie, Numero, CodiEmpresa);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaBeneficiarioEntity>(false, null, Message.MsgErrBuscarVentaxBoleto, false);
            }
        }

        public Response<string> PostergarVenta(PostergarVentaRequest filtro)
        {
            try
            {
                return VentaLogic.PostergarVenta(filtro);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrPostergarVenta, false);
            }
        }

        public Response<bool> EliminarReserva(int IdVenta)
        {
            try
            {
                return VentaLogic.EliminarReserva(IdVenta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcEliminarReserva, false);
            }
        }

        #endregion

        #region ANULAR VENTA

        public Response<bool> AnularVenta(int Id_Venta, int Codi_Usuario, string CodiOficina, string CodiPuntoVenta, string tipo)
        {
            try
            {
                return VentaLogic.AnularVenta(Id_Venta, Codi_Usuario, CodiOficina, CodiPuntoVenta, tipo);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgErrExcAnularVenta, false);
            }
        }

        #endregion

        #region FECHA ABIERTA

        public Response<string> ModificarVentaAFechaAbierta(int IdVenta, int CodiServicio, int CodiRuta)
        {
            try
            {
                return VentaLogic.ModificarVentaAFechaAbierta(IdVenta, CodiServicio, CodiRuta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgErrModificarVentaAFechaAbierta, false);
            }
        }
        #endregion
    }
}
