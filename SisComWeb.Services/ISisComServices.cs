using SisComWeb.Entity;
<<<<<<< HEAD
using System.Collections.Generic;
=======
using System;
>>>>>>> eca434352a135e8a9a42eefca29ea430a03694af
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SisComWeb.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISisComServices" in both code and config file together.
    [ServiceContract]
    public interface ISisComServices
    {
<<<<<<< HEAD
        #region BASE
=======
        #region LOGIN

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "ValidaUsuario")]
        ResFiltroUsuario ValidaUsuario(string CodiUsuario, string Password);
>>>>>>> eca434352a135e8a9a42eefca29ea430a03694af

        #endregion

        #region OFICINA, SERVICIO, PUNTO DE VENTA Y EMPRESA

        [OperationContract]
<<<<<<< HEAD
        [WebInvoke(Method = "GET", UriTemplate = "ListaOficinas", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaOficinas();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaPuntosVenta/{CodiSucursal}", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaPuntosVenta(string CodiSucursal);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaUsuarios/{CodiSucursal}/{CodiPuntoVenta}", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaUsuarios(string CodiSucursal, string CodiPuntoVenta);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaServicios", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaServicios();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaEmpresas", ResponseFormat = WebMessageFormat.Json)]
        Response<List<BaseEntity>> ListaEmpresas();

        #endregion

        #region LOGIN

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidaUsuario", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        Response<UsuarioEntity> ValidaUsuario(string CodiUsuario, string Password);

        #endregion
=======
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
>>>>>>> eca434352a135e8a9a42eefca29ea430a03694af

        #endregion

        #region REGISTRO CLIENTE

        [OperationContract]
<<<<<<< HEAD
        [WebInvoke(Method = "POST", UriTemplate = "BuscaPasajero", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        Response<ClientePasajeEntity> BuscaPasajero(string TipoDoc, string NumeroDoc);
=======
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "BuscaPasajero")]
        ResFiltroClientePasaje BuscaPasajero(string TipoDoc, string NumeroDoc);
>>>>>>> eca434352a135e8a9a42eefca29ea430a03694af

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
<<<<<<< HEAD
        [WebInvoke(Method = "POST", UriTemplate = "BuscaItinerarios", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        Response<ItinerarioEntity> BuscaItinerarios(ItinerarioEntity entidad);
=======
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "BuscaItinerarios")]
        ResListaItinerario BuscaItinerarios(ItinerarioEntity entidad);
>>>>>>> eca434352a135e8a9a42eefca29ea430a03694af

        #endregion
    }
}
