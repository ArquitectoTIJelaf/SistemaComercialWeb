using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class ResListaCliente
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        public List<ClientePasajesEntity> Valor { set; get; }

        public ResListaCliente()
        {
        }

        public ResListaCliente(bool esCorrecto, List<ClientePasajesEntity> valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new List<ClientePasajesEntity>());
            Mensaje = mensaje;
        }

        public ResListaCliente(bool esCorrecto, List<ClientePasajesEntity> valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new List<ClientePasajesEntity>());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
