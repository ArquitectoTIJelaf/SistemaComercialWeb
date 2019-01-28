using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class ResListaClientePasaje
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<ClientePasajeEntity> Valor { set; get; }

        public ResListaClientePasaje()
        {
        }

        public ResListaClientePasaje(bool esCorrecto, List<ClientePasajeEntity> valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new List<ClientePasajeEntity>());
            Mensaje = mensaje;
        }

        public ResListaClientePasaje(bool esCorrecto, List<ClientePasajeEntity> valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new List<ClientePasajeEntity>());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
