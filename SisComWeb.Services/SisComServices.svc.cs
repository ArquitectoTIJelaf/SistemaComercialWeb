using SisComWeb.Business;
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaOficinas, false);
            }
        }

        public Response<List<BaseEntity>> ListaPuntosVenta()
        {
            try
            {
                return BaseLogic.ListaPuntosVenta();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaPuntosVenta, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuarios, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaServicios, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaEmpresas, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTiposDoc, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTipoPago, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTarjetaCredito, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaCiudad, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarParentesco, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarGerente, false);
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
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarSocio, false);
            }
        }

        public Response<List<BaseEntity>> ListaHospitales(string codiSucursal)
        {
            try
            {
                return BaseLogic.ListaHospitales(int.Parse(codiSucursal));
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaHospitales, false);
            }
        }

        public Response<List<BaseEntity>> ListaSecciones(string idContrato)
        {
            try
            {
                return BaseLogic.ListaSecciones(int.Parse(idContrato));
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaSecciones, false);
            }
        }

        public Response<List<BaseEntity>> ListaAreas(string idContrato)
        {
            try
            {
                return BaseLogic.ListaAreas(int.Parse(idContrato));
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaAreas, false);
            }
        }

        public Response<List<BaseEntity>> ListaUsuariosClaveAnuRei(string Value)
        {
            try
            {
                return BaseLogic.ListaUsuariosClaveAnuRei(Value);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuariosClaveAnuRei, false);
            }
        }

        public Response<List<BaseEntity>> ListaUsuariosHC(string Descripcion, short Suc, short Pv)
        {
            try
            {
                return BaseLogic.ListaUsuariosHC(Descripcion, Suc, Pv);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuarios, false);
            }
        }

        public Response<List<BaseEntity>> ListaUsuarioControlPwd(string Value)
        {
            try
            {
                return BaseLogic.ListaUsuarioControlPwd(Value);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuarios, false);
            }
        }

        public Response<MensajeriaEntity> ObtenerMensaje(int CodiUsuario, string Fecha, string Tipo, int CodiSucursal, int CodiPventa)
        {
            try
            {
                return BaseLogic.ObtenerMensaje(CodiUsuario, Fecha, Tipo, CodiSucursal, CodiPventa);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<MensajeriaEntity>(false, null, Message.MsgExcObtenerMensaje, false);
            }
        }

        public Response<bool> EliminarMensaje(MensajeriaRequest request)
        {
            try
            {
                return BaseLogic.EliminarMensaje(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcEliminarMensaje, false);
            }
        }

        public Response<SucursalControlEntity> GetSucursalControl(string CodiPuntoVenta)
        {
            try
            {
                return BaseLogic.GetSucursalControl(CodiPuntoVenta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<SucursalControlEntity>(false, null, Message.MsgExcGetSucursalControl, false);
            }
        }

        #endregion

        #region LOGIN

        public Response<UsuarioEntity> ValidaUsuario(short CodiUsuario, string Password)
        {
            try
            {
                return UsuarioLogic.ValidaUsuario(CodiUsuario, Password);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<UsuarioEntity>(false, null, Message.MsgExcValidaUsuario, false);
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
                return new Response<ClientePasajeEntity>(false, null, Message.MsgExcBuscaPasajero, false);
            }
        }

        public Response<RucEntity> BuscaEmpresa(string RucContacto)
        {
            try
            {
                return ClientePasajeLogic.BuscaEmpresa(RucContacto);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<RucEntity>(false, null, Message.MsgExcBuscaEmpresa, false);
            }
        }

        public Response<ReniecEntity> ConsultaRENIEC(string NumeroDoc)
        {
            try
            {
                return ClientePasajeLogic.ConsultaRENIEC(NumeroDoc);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReniecEntity>(false, null, Message.MsgExcConsultaRENIEC, false);
            }
        }

        public Response<RucEntity> ConsultaSUNAT(string RucContacto)
        {
            try
            {
                return ClientePasajeLogic.ConsultaSUNAT(RucContacto);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<RucEntity>(false, null, Message.MsgExcConsultaRENIEC, false);
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
                return new Response<bool>(false, false, Message.MsgExcGrabarClientePasaje, false);
            }
        }

        public Response<List<ClientePasajeEntity>> BuscarClientesPasaje(string campo, string nombres, string paterno, string materno, string TipoDocId)
        {
            try
            {
                return ClientePasajeLogic.BuscarClientesPasaje(campo, nombres, paterno, materno, TipoDocId);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ClientePasajeEntity>>(false, null, Message.MsgExcBuscarClientesPasaje, false);
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
                return new Response<List<ItinerarioEntity>>(false, null, Message.MsgExcBuscaItinerarios, false);
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
                return new Response<List<PlanoEntity>>(false, null, Message.MsgExcMuestraPlano, false);
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
                return new Response<ItinerarioEntity>(false, null, Message.MsgExcMuestraTurno, false);
            }
        }

        public Response<string> ConsultaManifiestoProgramacion(int Prog, string Suc)
        {
            try
            {
                return TurnoLogic.ConsultaManifiestoProgramacion(Prog, Suc);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaManifiestoProgramacion, false);
            }
        }

        public Response<bool> ObtenerStAnulacion(string CodTab, int Pv, string F)
        {
            try
            {
                return TurnoLogic.ObtenerStAnulacion(CodTab, Pv, F);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcObtenerStAnulacion, false);
            }
        }

        public Response<List<DestinoRutaEntity>> GetNewListaDestinosPas(byte CodiEmpresa, short CodiOrigenPas, short CodiOrigenBus, short CodiPuntoVentaBus, short CodiDestinoBus, string Turno, byte CodiServicio, int NroViaje)
        {
            try
            {
                return TurnoLogic.GetNewListaDestinosPas(CodiEmpresa, CodiOrigenPas, CodiOrigenBus, CodiPuntoVentaBus, CodiDestinoBus, Turno, CodiServicio, NroViaje);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<DestinoRutaEntity>>(false, null, Message.MsgExcGetNewListaDestinosPas, false);
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
                return new Response<int>(false, 0, Message.MsgExcBloqueoAsiento, false);
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
                return new Response<bool>(false, false, Message.MsgExcLiberaAsiento, false);
            }
        }

        public Response<bool> ActualizarAsiOcuTbBloqueoAsientos(TablaBloqueoAsientosRequest request)
        {
            try
            {
                return BloqueoAsientoLogic.ActualizarAsiOcuTbBloqueoAsientos(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcActualizarAsiOcuTbBloqueoAsientos, false);
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
                return new Response<bool>(false, false, Message.MsgExcLiberaArregloAsientos, false);
            }
        }

        #endregion

        #region GRABA VENTA

        public Response<CorrelativoResponse> BuscaCorrelativo(CorrelativoRequest request)
        {
            try
            {
                return VentaLogic.BuscaCorrelativo(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<CorrelativoResponse>(false, null, Message.MsgExcBuscaCorrelativo, false);
            }
        }

        public Response<VentaResponse> GrabaVenta(VentaRequest request)
        {
            try
            {
                switch (request.FlagVenta)
                {
                    case "R": // Reserva
                        return VentaLogic.GrabaReserva(request.Listado);
                    default:
                        return VentaLogic.GrabaVenta(request.Listado, request.FlagVenta);
                }
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcGrabaVenta, false);
            }
        }

        public Response<PaseCortesiaResponse> ListaBeneficiarioPase(string CodiSocio, string Anno, string Mes)
        {
            try
            {
                return VentaLogic.ListaBeneficiarioPase(CodiSocio, Anno, Mes);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PaseCortesiaResponse>(false, null, Message.MsgExcListaBeneficiarioPase, false);
            }
        }

        public Response<decimal> ValidarSaldoPaseCortesia(string CodiSocio, string Mes, string Anno)
        {
            try
            {
                return PaseLogic.ValidarSaldoPaseCortesia(CodiSocio, Mes, Anno);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<decimal>(false, 0, Message.MsgExcValidarSaldoPaseCortesia, false);
            }
        }

        public Response<bool> ClavesInternas(int CodiOficina, string Password, string CodiTipo)
        {
            try
            {
                return VentaLogic.ClavesInternas(CodiOficina, Password, CodiTipo);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcClavesInternas, false);
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
                return new Response<VentaBeneficiarioEntity>(false, null, Message.MsgExcBuscarVentaxBoleto, false);
            }
        }

        public Response<VentaResponse> PostergarVenta(PostergarVentaRequest filtro)
        {
            try
            {
                return VentaLogic.PostergarVenta(filtro);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcPostergarVenta, false);
            }
        }

        public Response<string> ConsultaPos(string CodTab, string CodEmp)
        {
            try
            {
                return VentaLogic.ConsultaPos(CodTab, CodEmp);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaPos, false);
            }
        }

        public Response<int> ConsultaSumaBoletosPostergados(string Tipo, string Numero, string Emp)
        {
            try
            {
                return VentaLogic.ConsultaSumaBoletosPostergados(Tipo, Numero, Emp);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcConsultaSumaBoletosPostergados, false);
            }
        }

        public Response<byte> EliminarReserva(CancelarReservaRequest request)
        {
            try
            {
                return VentaLogic.EliminarReserva(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<byte>(false, 0, Message.MsgExcEliminarReserva, false);
            }
        }

        public Response<bool> AcompanianteVentaCRUD(AcompanianteRequest request)
        {
            try
            {
                return VentaLogic.AcompanianteVentaCRUD(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcAcompanianteVentaCRUD, false);
            }
        }

        public Response<string> VerificaClaveReserva(int CodiUsr, string Password)
        {
            try
            {
                return VentaLogic.VerificaClaveReserva(CodiUsr, Password);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaClaveReserva, false);
            }
        }

        public Response<string> VerificaClaveTbClaveRe(int CodiUsr)
        {
            try
            {
                return VentaLogic.VerificaClaveTbClaveRe(CodiUsr);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaClaveReserva, false);
            }
        }

        public Response<string> VerificaHoraConfirmacion(int Origen, int Destino)
        {
            try
            {
                return VentaLogic.VerificaHoraConfirmacion(Origen, Destino);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaHoraConfirmacion, false);
            }
        }

        public Response<ReservacionEntity> ObtenerTiempoReserva()
        {
            try
            {
                return VentaLogic.ObtenerTiempoReserva();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReservacionEntity>(false, null, Message.MsgExcObtenerTiempoReserva, false);
            }
        }

        #endregion

        #region ANULAR VENTA

        public Response<byte> AnularVenta(AnularVentaRequest request)
        {
            try
            {
                return VentaLogic.AnularVenta(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<byte>(false, 0, Message.MsgExcAnularVenta, false);
            }
        }

        public Response<int> VerificaNC(int IdVenta)
        {
            try
            {
                return VentaLogic.VerificaNC(IdVenta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcVerificaNC, false);
            }
        }

        public Response<decimal> ConsultaControlTiempo(string tipo)
        {
            try
            {
                return VentaLogic.ConsultaControlTiempo(tipo);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<decimal>(false, 0, Message.MsgExcConsultaControlTiempo, false);
            }
        }

        public Response<string> ConsultaPanelNiveles(int codigo, int Nivel)
        {
            try
            {
                return VentaLogic.ConsultaPanelNiveles(codigo, Nivel);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaPanelNiveles, false);
            }
        }

        public Response<int> VerificaLiquidacionComiDet(int IdVenta)
        {
            try
            {
                return VentaLogic.VerificaLiquidacionComiDet(IdVenta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcVerificaLiquidacionComiDet, false);
            }
        }

        public Response<string> VerificaLiquidacionComi(int CodiProgramacion, int Pvta)
        {
            try
            {
                return VentaLogic.VerificaLiquidacionComi(CodiProgramacion, Pvta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaLiquidacionComi, false);
            }
        }

        public Response<bool> ConsultaVentaIdaV(int IdVenta)
        {
            try
            {
                return VentaLogic.ConsultaVentaIdaV(IdVenta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaVentaIdaV, false);
            }
        }

        public Response<bool> GrabarAuditoria(AuditoriaEntity entidad)
        {
            try
            {
                return VentaLogic.GrabarAuditoria(entidad);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcGrabarAuditoria, false);
            }
        }

        public Response<bool> ConsultaClaveAnuRei(int CodiUsuario, string Clave)
        {
            try
            {
                return VentaLogic.ConsultaClaveAnuRei(CodiUsuario, Clave);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaClaveAnuRei, false);
            }
        }

        public Response<bool> ConsultaClaveControl(short Usuario, string Pwd)
        {
            try
            {
                return VentaLogic.ConsultaClaveControl(Usuario, Pwd);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaClaveControl, false);
            }
        }

        public Response<bool> InsertarUsuarioPorVenta(string Usuario, string Accion, decimal IdVenta, string Motivo)
        {
            try
            {
                return VentaLogic.InsertarUsuarioPorVenta(Usuario, Accion, IdVenta, Motivo);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcInsertarUsuarioPorVenta, false);
            }
        }

        #endregion

        #region FECHA ABIERTA

        public Response<bool> ModificarVentaAFechaAbierta(VentaToFechaAbiertaRequest request)
        {
            try
            {
                return VentaLogic.ModificarVentaAFechaAbierta(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcModificarVentaAFechaAbierta, false);
            }
        }

        #endregion

        #region CRÉDITO

        public Response<List<ClienteCreditoEntity>> ListarClientesContrato(CreditoRequest request)
        {
            try
            {
                return CreditoLogic.ListarClientesContrato(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ClienteCreditoEntity>>(false, null, Message.MsgExcListarClientesContrato, false);
            }
        }

        public Response<PanelControlResponse> ListarPanelesControl()
        {
            try
            {
                return CreditoLogic.ListarPanelesControl();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PanelControlResponse>(false, null, Message.MsgExcListarPanelesControl, false);
            }
        }

        public Response<ContratoEntity> ConsultarContrato(int idContrato)
        {
            try
            {
                return CreditoLogic.ConsultarContrato(idContrato);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ContratoEntity>(false, null, Message.MsgExcConsultarContrato, false);
            }
        }

        public Response<PrecioNormalEntity> VerificarPrecioNormal(int idContrato)
        {
            try
            {
                return CreditoLogic.VerificarPrecioNormal(idContrato);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PrecioNormalEntity>(false, null, Message.MsgExcVerificarPrecioNormal, false);
            }
        }

        public Response<decimal> BuscarPrecio(string fechaViaje, string nivel, string hora, string idPrecio)
        {
            try
            {
                return CreditoLogic.BuscarPrecio(fechaViaje, nivel, hora, idPrecio);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<decimal>(false, 0, Message.MsgExcBuscarPrecio, false);
            }
        }

        #endregion

        #region MANIFIESTO

        public Response<string> VerificaManifiestoPorPVenta(int CodiProgramacion, short Pvta)
        {
            try
            {
                return VentaLogic.VerificaManifiestoPorPVenta(CodiProgramacion, Pvta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcVerificaManifiestoPorPVenta, false);
            }
        }

        public Response<string> ConsultaConfigManifiestoPorHora(short CodiEmpresa, short CodiSucursal, short CodiPuntoVenta)
        {
            try
            {
                return VentaLogic.ConsultaConfigManifiestoPorHora(CodiEmpresa, CodiSucursal, CodiPuntoVenta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaConfigManifiestoPorHora, false);
            }
        }

        public Response<bool> ActualizarProgramacionManifiesto(ManifiestoRequest request)
        {
            try
            {
                return ManifiestoLogic.ActualizarProgramacionManifiesto(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcActualizarProgramacionManifiesto, false);
            }
        }

        #endregion

        #region IMPRESIÓN

        public Response<List<ImpresionEntity>> ConvertirVentaToBase64(VentaRealizadaRequest request)
        {
            try
            {
                return VentaLogic.ConvertirVentaToBase64(request.ListaVentasRealizadas, request.TipoImpresion);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ImpresionEntity>>(false, null, Message.MsgExcConvertirVentaToBase64, false);
            }
        }

        #endregion

        #region BUSCA

        public Response<BuscaEntity> BuscaBoletoF9(int Serie, int Numero, string Tipo, int CodEmpresa)
        {
            try
            {
                return VentaLogic.BuscaBoletoF9(Serie, Numero, Tipo, CodEmpresa);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<BuscaEntity>(false, null, Message.MsgExcBuscaBoletoF9, false);
            }
        }

        public Response<bool> ActualizaBoletoF9(BoletoF9Request request)
        {
            try
            {
                return VentaLogic.ActualizaBoletoF9(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExActualizaBoletoF9, false);
            }
        }
        #endregion

        #region FECHA ABIERTA

        public Response<List<FechaAbiertaEntity>> VentaConsultaF6(FechaAbiertaRequest request)
        {
            try
            {
                return FechaAbiertaLogic.VentaConsultaF6(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<FechaAbiertaEntity>>(false, null, Message.MsgExActualizaBoletoF9, false);
            }
        }

        public Response<bool> ValidateNivelAsiento(int IdVenta, string CodiBus, string Asiento)
        {
            try
            {
                return FechaAbiertaLogic.ValidateNivelAsiento(IdVenta, CodiBus, Asiento);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExActualizaBoletoF9, false);
            }
        }

        public Response<int> ValidateNumDias(string FechaVenta, string CodTab)
        {
            try
            {
                return FechaAbiertaLogic.ValidateNumDias(FechaVenta, CodTab);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcValidateNumDias, false);
            }
        }

        public Response<int> VerificaNotaCredito(int IdVenta)
        {
            try
            {
                return FechaAbiertaLogic.VerificaNotaCredito(IdVenta);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcVerificaNotaCredito, false);
            }
        }

        public Response<VentaResponse> VentaUpdatePostergacionEle(FechaAbiertaRequest filtro)
        {
            try
            {
                return FechaAbiertaLogic.VentaUpdatePostergacionEle(filtro);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcVentaUpdatePostergacionEle, false);
            }
        }

        public Response<int> TablasPnpConsulta(string Tabla)
        {
            try
            {
                return FechaAbiertaLogic.TablasPnpConsulta(Tabla);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcTablasPnpConsulta, false);
            }
        }

        #endregion

        #region REINTEGRO

        public Response<ReintegroEntity> VentaConsultaF12(ReintegroRequest request)
        {
            try
            {
                return ReintegroLogic.VentaConsultaF12(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReintegroEntity>(false, null, Message.MsgExcVentaConsultaF12, false);
            }
        }

        public Response<List<SelectReintegroEntity>> ListaOpcionesModificacion()
        {
            try
            {
                return ReintegroLogic.ListaOpcionesModificacion();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<SelectReintegroEntity>>(false, null, Message.MsgExcListaOpcionesModificacion, false);
            }
        }

        public Response<bool> ValidaExDni(string documento)
        {
            try
            {
                return ReintegroLogic.ValidaExDni(documento);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcValidaExDni, false);
            }
        }

        public Response<VentaResponse> SaveReintegro(ReintegroVentaRequest filtro)
        {
            try
            {
                return ReintegroLogic.SaveReintegro(filtro);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcVentaReintegro, false);
            }
        }

        public Response<PlanoEntity> ConsultarPrecioRuta(PrecioRutaRequest request)
        {
            try
            {
                return ReintegroLogic.ConsultarPrecioRuta(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PlanoEntity>(false, null, Message.MsgExcConsultaPrecioRuta, false);
            }
        }

        public Response<bool> UpdateReintegro(UpdateReintegroRequest filtro)
        {
            try
            {
                return ReintegroLogic.UpdateReintegro(filtro);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, "", false);
            }
        }

        public Response<ReintegroEntity> ValidaReintegroParaAnualar(ReintegroRequest request)
        {
            try
            {
                return ReintegroLogic.ValidaReintegroParaAnualar(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReintegroEntity>(false, null, Message.MsgExcAnulaReintegro, false);
            }
        }

        public Response<byte> AnularReintegro(AnularVentaRequest request)
        {
            try
            {
                return ReintegroLogic.AnularReintegro(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<byte>(false, 0, Message.MsgExcAnulaReintegro, false);
            }
        }
        #endregion

        #region "PASE EN LOTE"

        public Response<List<PaseLoteResponse>> UpdatePostergacion(UpdatePostergacionRequest request)
        {
            try
            {
                return PaseLoteLogic.UpdatePostergacion(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<PaseLoteResponse>>(false, null, Message.MsgExcPaseLote, false);
            }
        }
        
        public Response<string> ValidarManifiesto(int CodiProgramacion, int CodiSucursal)
        {
            try
            {
                return PaseLoteLogic.ValidarManifiesto(CodiProgramacion, CodiSucursal);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, string.Empty, Message.MsgValidarManifiesto, false);
            }
        }

        public Response<List<int>> BloqueoAsientoList(BloqueoAsientoRequest request)
        {
            try
            {
                return PaseLoteLogic.BloqueoAsientoList(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<int>>(false, new List<int>(), Message.MsgBloquearAsientos, false);
            }
        }

        public Response<List<int>> DesbloquearAsientosList(int CodiProgramacion, string CodiTerminal)
        {
            try
            {
                return PaseLoteLogic.DesbloquearAsientosList(CodiProgramacion, CodiTerminal);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<int>>(false, new List<int>(), Message.MsgDesbloquearAsientos, false);
            }
        }
        #endregion

        #region "LIQUIDACIÓN"

        public Response<LiquidacionEntity> ListaLiquidacion(LiquidacionRequest request)
        {
            try
            {
                return LiquidacionLogic.ListaLiquidacion(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<LiquidacionEntity>(false, new LiquidacionEntity(), Message.MsgExcLiquidacion, false);
            }
        }
        #endregion

        #region "CAMBIO TIPO PAGO"

        public Response<string> CambiarTipoPago(CambiarTPagoRequest request)
        {
            try
            {
                return CambiarTPagoLogic.CambiarTipoPago(request);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(SisComServices)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, string.Empty, "", false);
            }
        }
        #endregion
    }
}
