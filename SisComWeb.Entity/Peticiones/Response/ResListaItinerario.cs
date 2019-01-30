using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class ResListaItinerario
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<ItinerarioEntity> Valor { set; get; }

        public ResListaItinerario()
        {
        }

        public ResListaItinerario(bool esCorrecto, List<ItinerarioEntity> valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new List<ItinerarioEntity>());
            Mensaje = mensaje;
        }

        public ResListaItinerario(bool esCorrecto, List<ItinerarioEntity> valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new List<ItinerarioEntity>());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
