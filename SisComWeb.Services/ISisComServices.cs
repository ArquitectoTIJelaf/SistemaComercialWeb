﻿using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Entity.Peticiones.Response;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SisComWeb.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISisComServices" in both code and config file together.
    [ServiceContract]
    public interface ISisComServices
    {
        #region BASE

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaOficinas", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaOficinas();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaPuntosVenta", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaPuntosVenta();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaUsuarios/{value}", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaUsuarios(string value);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaServicios", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaServicios();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaEmpresas", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaEmpresas();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaTiposDoc", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaTiposDoc();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaTipoPago", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaTipoPago();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaTarjetaCredito", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaTarjetaCredito();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaCiudad", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaCiudad();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListarParentesco", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListarParentesco();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListarGerente", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListarGerente();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListarSocio", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListarSocio();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaHospitales/{codiSucursal}", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaHospitales(string codiSucursal);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaSecciones/{idContrato}", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaSecciones(string idContrato);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaAreas/{idContrato}", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaAreas(string idContrato);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaUsuariosClaveAnuRei", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaUsuariosClaveAnuRei(string Value);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaUsuariosHC", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaUsuariosHC(string Descripcion, short Suc, short Pv);
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaUsuarioControlPwd", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaUsuarioControlPwd(string Value);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ObtenerMensaje", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<MensajeriaEntity> ObtenerMensaje(int CodiUsuario, string Fecha, string Tipo, int CodiSucursal, int CodiPventa);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "EliminarMensaje", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> EliminarMensaje(MensajeriaRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetSucursalControl", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<SucursalControlEntity> GetSucursalControl(string CodiPuntoVenta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaConceptosNC", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaConceptosNC(string RucEmpresa, string TipoTerminalElectronico, string CodDoc, string ElectronicoEmpresa);

        #endregion

        #region LOGIN

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidaUsuario", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<UsuarioEntity> ValidaUsuario(short CodiUsuario, string Password);

        #endregion

        #region GRABA PASAJERO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscaPasajero", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<ClientePasajeEntity> BuscaPasajero(string TipoDoc, string NumeroDoc);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscaEmpresa", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<RucEntity> BuscaEmpresa(string RucContacto);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaRENIEC", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<ReniecEntity> ConsultaRENIEC(string NumeroDoc);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaSUNAT", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<RucEntity> ConsultaSUNAT(string RucContacto);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GrabarPasajero", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> GrabarPasajero(List<ClientePasajeEntity> lista);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscarClientesPasaje", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<List<ClientePasajeEntity>> BuscarClientesPasaje(string campo, string nombres, string paterno, string materno, string TipoDocId);

        #endregion

        #region BÚSQUEDA ITINERARIO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscaItinerarios", ResponseFormat = WebMessageFormat.Json)]
        Response<List<ItinerarioEntity>> BuscaItinerarios(ItinerarioRequest request);

        #endregion

        #region MUESTRA PLANO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "MuestraPlano", ResponseFormat = WebMessageFormat.Json)]
        Response<List<PlanoEntity>> MuestraPlano(PlanoRequest request);

        #endregion

        #region MUESTRA TURNO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "MuestraTurno", ResponseFormat = WebMessageFormat.Json)]
        Response<ItinerarioEntity> MuestraTurno(TurnoRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaManifiestoProgramacion", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> ConsultaManifiestoProgramacion(int Prog, string Suc);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ObtenerStAnulacion", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ObtenerStAnulacion(string CodTab, int Pv, string F);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetNewListaDestinosPas", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<List<DestinoRutaEntity>> GetNewListaDestinosPas(byte CodiEmpresa, short CodiOrigenPas, short CodiOrigenBus, short CodiPuntoVentaBus, short CodiDestinoBus, string Turno, byte CodiServicio, int NroViaje);

        #endregion

        #region BLOQUEO ASIENTO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BloqueoAsiento", ResponseFormat = WebMessageFormat.Json)]
        Response<int> BloqueoAsiento(BloqueoAsientoRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "LiberaAsiento", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> LiberaAsiento(int IDS);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ActualizarAsiOcuTbBloqueoAsientos", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ActualizarAsiOcuTbBloqueoAsientos(TablaBloqueoAsientosRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "LiberaArregloAsientos", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> LiberaArregloAsientos(int[] arregloIDS);

        #endregion

        #region GRABA VENTA

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscaCorrelativo", ResponseFormat = WebMessageFormat.Json)]
        Response<CorrelativoResponse> BuscaCorrelativo(CorrelativoRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GrabaVenta", ResponseFormat = WebMessageFormat.Json)]
        Response<VentaResponse> GrabaVenta(VentaRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaBeneficiarioPase", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<PaseCortesiaResponse> ListaBeneficiarioPase(string CodiSocio, string Anno, string Mes);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidarPase", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<decimal> ValidarSaldoPaseCortesia(string CodiSocio, string Mes, string Anno);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ClavesInternas", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ClavesInternas(int CodiOficina, string Password, string CodiTipo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscarVentaxBoleto", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<VentaBeneficiarioEntity> BuscarVentaxBoleto(string Tipo, short Serie, int Numero, short CodiEmpresa);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "PostergarVenta", ResponseFormat = WebMessageFormat.Json)]
        Response<VentaResponse> PostergarVenta(PostergarVentaRequest filtro);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaSumaBoletosPostergados", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ConsultaSumaBoletosPostergados(string CodTab, string CodEmp, string Tipo, string Numero, string Emp);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "EliminarReserva", ResponseFormat = WebMessageFormat.Json)]
        Response<byte> EliminarReserva(CancelarReservaRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AcompanianteVentaCRUD", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> AcompanianteVentaCRUD(AcompanianteRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaClaveReserva", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> VerificaClaveReserva(int CodiUsr, string Password);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaClaveTbClaveRe", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> VerificaClaveTbClaveRe(int CodiUsr);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaHoraConfirmacion", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> VerificaHoraConfirmacion(int Origen, int Destino);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ObtenerTiempoReserva", ResponseFormat = WebMessageFormat.Json)]
        Response<ReservacionEntity> ObtenerTiempoReserva();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ObtenerValorPNP", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ObtenerValorPNP(string Tabla, int CodiProgramacion);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaDocumentoRepetido", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<byte> VerificaDocumentoRepetido(int CodiProgramacion, int NroViaje, short CodiOrigen, short CodiDestino, string TipoDoc, string Documento);

        #endregion

        #region ANULAR VENTA

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AnularVenta", ResponseFormat = WebMessageFormat.Json)]
        Response<byte> AnularVenta(AnularVentaRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaNC", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<int> VerificaNC(int IdVenta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaControlTiempo", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<decimal> ConsultaControlTiempo(string tipo);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaPanelNiveles", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> ConsultaPanelNiveles(int codigo, int Nivel);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaLiquidacionComiDet", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<int> VerificaLiquidacionComiDet(int IdVenta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaLiquidacionComi", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> VerificaLiquidacionComi(int CodiProgramacion, int Pvta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaVentaIdaV", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ConsultaVentaIdaV(int IdVenta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GrabarAuditoria", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> GrabarAuditoria(AuditoriaEntity entidad);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaClaveAnuRei", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ConsultaClaveAnuRei(int CodiUsuario, string Clave);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaClaveControl", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ConsultaClaveControl(short Usuario, string Pwd);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "InsertarUsuarioPorVenta", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> InsertarUsuarioPorVenta(string Usuario, string Accion, decimal IdVenta, string Motivo);

        #endregion

        #region ENVIAR A FECHA ABIERTA

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ModificarVentaAFechaAbierta", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ModificarVentaAFechaAbierta(VentaToFechaAbiertaRequest request);

        #endregion

        #region CRÉDITO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListarClientesContrato", ResponseFormat = WebMessageFormat.Json)]
        Response<List<ClienteCreditoEntity>> ListarClientesContrato(CreditoRequest request);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListarPanelesControl", ResponseFormat = WebMessageFormat.Json)]
        Response<PanelControlResponse> ListarPanelesControl();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultarContrato", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<ContratoEntity> ConsultarContrato(int idContrato);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificarPrecioNormal", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<PrecioNormalEntity> VerificarPrecioNormal(int idContrato);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscarPrecio", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<decimal> BuscarPrecio(string fechaViaje, string nivel, string hora, string idPrecio);

        #endregion

        #region MANIFIESTO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaManifiestoPorPVenta", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> VerificaManifiestoPorPVenta(int CodiProgramacion, short Pvta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaConfigManifiestoPorHora", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> ConsultaConfigManifiestoPorHora(short CodiEmpresa, short CodiSucursal, short CodiPuntoVenta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ActualizarProgramacionManifiesto", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ActualizarProgramacionManifiesto(ManifiestoRequest request);

        #endregion

        #region IMPRESIÓN

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConvertirVentaToBase64", ResponseFormat = WebMessageFormat.Json)]
        Response<List<ImpresionEntity>> ConvertirVentaToBase64(VentaRealizadaRequest request);

        #endregion

        #region BUSCA

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscaBoletoF9", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<BuscaEntity> BuscaBoletoF9(int Serie, int Numero, string Tipo, int CodEmpresa);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ActualizaBoletoF9", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ActualizaBoletoF9(BoletoF9Request request);
        #endregion

        #region FECHA ABIERTA

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VentaConsultaF6", ResponseFormat = WebMessageFormat.Json)]
        Response<List<FechaAbiertaEntity>> VentaConsultaF6(FechaAbiertaRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidateNivelAsiento", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ValidateNivelAsiento(int IdVenta, string CodiBus, string Asiento);
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidateNumDias", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<int> ValidateNumDias(string FechaVenta, string CodTab);
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VerificaNotaCredito", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<int> VerificaNotaCredito(int IdVenta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VentaUpdatePostergacionEle", ResponseFormat = WebMessageFormat.Json)]
        Response<VentaResponse> VentaUpdatePostergacionEle(FechaAbiertaRequest filtro);

        

        #endregion

        #region REINTEGRO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "VentaConsultaF12", ResponseFormat = WebMessageFormat.Json)]
        Response<ReintegroEntity> VentaConsultaF12(ReintegroRequest filtro);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaOpcionesModificacion", ResponseFormat = WebMessageFormat.Json)]
        Response<List<SelectReintegroEntity>> ListaOpcionesModificacion();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidaExDni", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ValidaExDni(string documento);
        
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveReintegro", ResponseFormat = WebMessageFormat.Json)]
        Response<VentaResponse> SaveReintegro(ReintegroVentaRequest filtro);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultarPrecioRuta", ResponseFormat = WebMessageFormat.Json)]
        Response<PlanoEntity> ConsultarPrecioRuta(PrecioRutaRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateReintegro", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> UpdateReintegro(UpdateReintegroRequest filtro);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidaReintegroParaAnualar", ResponseFormat = WebMessageFormat.Json)]
        Response<ReintegroEntity> ValidaReintegroParaAnualar(ReintegroRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AnularReintegro", ResponseFormat = WebMessageFormat.Json)]
        Response<byte> AnularReintegro(AnularVentaRequest request);
        #endregion

        #region "PASE EN LOTE"

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UpdatePostergacion",ResponseFormat = WebMessageFormat.Json)]
        Response<List<PaseLoteResponse>> UpdatePostergacion(UpdatePostergacionRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidarManifiesto", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> ValidarManifiesto(int CodiProgramacion, int CodiSucursal);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BloqueoAsientoList", ResponseFormat = WebMessageFormat.Json)]
        Response<List<int>> BloqueoAsientoList(BloqueoAsientoRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "DesbloquearAsientosList", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<List<int>> DesbloquearAsientosList(int CodiProgramacion, string CodiTerminal);
        #endregion

        #region "LIQUIDACIÓN"

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaLiquidacion", ResponseFormat = WebMessageFormat.Json)]
        Response<LiquidacionEntity> ListaLiquidacion(LiquidacionRequest request);
        #endregion
        
        #region NOTA DE CRÉDITO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaTipoTerminalElectronico", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> ConsultaTipoTerminalElectronico(int CodiTerminal, int CodiEmpresa);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaClientesNC_Autocomplete", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaClientesNC_Autocomplete(string TipoDocumento, string Value);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaDocumentosEmitidos", ResponseFormat = WebMessageFormat.Json)]
        Response<List<DocumentoEmitidoNCEntity>> ListaDocumentosEmitidos(DocumentosEmitidosRequest request);

        #endregion

        #region "CAMBIAR TIPO PAGO"

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CambiarTipoPago", ResponseFormat = WebMessageFormat.Json)]
        Response<string> CambiarTipoPago(CambiarTPagoRequest request);

        #endregion
    }
}
