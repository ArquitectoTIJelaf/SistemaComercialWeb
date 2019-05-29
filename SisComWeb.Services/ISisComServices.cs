using SisComWeb.Entity;
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
        [WebInvoke(Method = "GET", UriTemplate = "ListaPuntosVenta/{CodiSucursal}", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaPuntosVenta(string CodiSucursal);

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

        #endregion

        #region LOGIN

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidaUsuario", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<UsuarioEntity> ValidaUsuario(string CodiUsuario, string Password);

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

        #endregion

        #region BLOQUEO ASIENTO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BloqueoAsiento", ResponseFormat = WebMessageFormat.Json)]
        Response<int> BloqueoAsiento(BloqueoAsientoRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "LiberaAsiento", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> LiberaAsiento(int IDS);

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
        [WebInvoke(Method = "POST", UriTemplate = "EliminarReserva", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<byte> EliminarReserva(int IdVenta);

        #endregion

        #region ANULAR VENTA

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AnularVenta", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<byte> AnularVenta(int IdVenta, int CodiUsuario, string CodiOficina, string CodiPuntoVenta, string Tipo, string FlagVenta);

        #endregion

        #region FECHA ABIERTA

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ModificarVentaAFechaAbierta", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<byte> ModificarVentaAFechaAbierta(int IdVenta, int CodiServicio, int CodiRuta);

        #endregion

        #region CRÉDITO

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListarClientesContrato", ResponseFormat = WebMessageFormat.Json)]
        Response<List<ClienteCreditoEntity>> ListarClientesContrato(CreditoRequest request);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListarPanelControl", ResponseFormat = WebMessageFormat.Json)]
        Response<List<PanelControlEntity>> ListarPanelControl();

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

        #region IMPRESIÓN

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConvertirVentaToBase64", ResponseFormat = WebMessageFormat.Json)]
        Response<List<ImpresionEntity>> ConvertirVentaToBase64(List<VentaRealizada> Listado);

        #endregion
    }
}
