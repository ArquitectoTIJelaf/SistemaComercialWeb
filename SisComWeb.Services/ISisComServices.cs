using SisComWeb.Entity;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SisComWeb.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISisComServices" in both code and config file together.
    [ServiceContract]
    public interface ISisComServices
    {
        #region LOGIN

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "ValidaUsuario")]
        ResFiltroUsuario ValidaUsuario(string CodiUsuario, string Password);

        #endregion

        #region OFICINA, SERVICIO, PUNTO DE VENTA Y EMPRESA

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "ListaOficinas")]
        ResListaOficina ListaOficinas();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "ListaServicios")]
        ResListaServicio ListaServicios();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "ListaPuntosVenta/{Codi_Sucursal}")]
        ResListaPuntoVenta ListaPuntosVenta(string Codi_Sucursal);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "ListaEmpresas")]
        ResListaEmpresa ListaEmpresas();

        #endregion

        #region REGISTRO CLIENTE

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "BuscaPasajero")]
        ResFiltroClientePasaje BuscaPasajero(string TipoDoc, string NumeroDoc);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GrabarPasajero")]
        Response<bool> GrabarPasajero(ClientePasajeEntity entidad);

        #endregion

        #region BÚSQUEDA ITINERARIO

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "BuscaItinerarios")]
        ResListaItinerario BuscaItinerarios(ItinerarioEntity entidad);

        #endregion
    }
}
