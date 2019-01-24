using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SisComWeb.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISisComServices" in both code and config file together.
    [ServiceContract]
    public interface ISisComServices
    {
        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "Listar", ResponseFormat = WebMessageFormat.Json)]
        //ResListaClientePasaje Listar();

        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "Filtrar/{id}", ResponseFormat = WebMessageFormat.Json)]
        //ResFiltroClientePasaje Filtrar(string id);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "Grabar", ResponseFormat = WebMessageFormat.Json)]
        //Response<object> Grabar();

        //[OperationContract]
        //[WebInvoke(Method = "PUT", UriTemplate = "Modificar", ResponseFormat = WebMessageFormat.Json)]
        //Response<object> Modificar();

        //[OperationContract]
        //[WebInvoke(Method = "DELETE", UriTemplate = "Eliminar", ResponseFormat = WebMessageFormat.Json)]
        //Response<object> Eliminar();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ValidaUsuario", ResponseFormat = WebMessageFormat.Json)]
        ResFiltroUsuario ValidaUsuario(string CodiUsuario, string Password);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaOficinas", ResponseFormat = WebMessageFormat.Json)]
        ResListaOficina ListaOficinas();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaServicios", ResponseFormat = WebMessageFormat.Json)]
        ResListaServicio ListaServicios();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaPuntosVenta", ResponseFormat = WebMessageFormat.Json)]
        ResListaPuntoVenta ListaPuntosVenta();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ListaEmpresas", ResponseFormat = WebMessageFormat.Json)]
        ResListaEmpresa ListaEmpresas();

        #region REGISTRO CLIENTE

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "BuscaPasajero", ResponseFormat = WebMessageFormat.Json)]
        ResFiltroClientePasaje BuscaPasajero(string TipoDoc, string NumeroDoc);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GrabarPasajero", ResponseFormat = WebMessageFormat.Json)]
        ResFiltroClientePasaje GrabarPasajero(ResRequestClientePasaje request);

        #endregion
    }
}
