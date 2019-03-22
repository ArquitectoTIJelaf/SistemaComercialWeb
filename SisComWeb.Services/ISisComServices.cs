using SisComWeb.Entity;
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
        [WebInvoke(Method = "POST", UriTemplate = "GrabarPasajero", ResponseFormat = WebMessageFormat.Json)]
        Response<bool> GrabarPasajero(List<ClientePasajeEntity> lista);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultarSUNAT", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<RucEntity> ConsultarSUNAT(string RucContacto);

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
        Response<string> BuscaCorrelativo(CorrelativoRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GrabaVenta", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> GrabaVenta(List<VentaEntity> listado, string FlagVenta);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaBeneficiarioPase", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<PaseCortesiaResponse> ListaBeneficiarioPase(string Codi_Socio, string año, string mes);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidarPase", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> ValidarPase(string CodiSocio, string Mes, string Anno);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ClavesInternas", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<ClavesInternasResponse> ClavesInternas(int Codi_Oficina, string Password, string Codi_Tipo);

        #endregion

        #region ANULAR VENTA

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AnularVenta", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<bool> AnularVenta(int Id_Venta, int Codi_Usuario, string CodiOficina, string CodiPuntoVenta, string tipo);

        #endregion
    }
}
