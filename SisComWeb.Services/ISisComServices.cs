using SisComWeb.Entity;
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
        [WebInvoke(Method = "GET", UriTemplate = "ListaUsuarios", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaUsuarios();

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
        [WebInvoke(Method = "POST", UriTemplate = "ListaTipoPago", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaTipoPago();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaTarjetaCredito", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaTarjetaCredito();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ListaCiudad", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaCiudad();

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
        Response<bool> GrabarPasajero(ClientePasajeEntity entidad);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ConsultaSUNAT", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Response<string> ConsultaSUNAT(string RucContacto);

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
        [WebInvoke(Method = "POST", UriTemplate = "GrabaVenta", ResponseFormat = WebMessageFormat.Json)]
        Response<string> GrabaVenta(VentaEntity entidad);

        #endregion
    }
}
