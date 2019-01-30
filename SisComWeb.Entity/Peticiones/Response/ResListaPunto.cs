using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class ResListaPunto
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<PuntoEntity> Valor { set; get; }

        public ResListaPunto()
        {
        }

        public ResListaPunto(bool esCorrecto, List<PuntoEntity> valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new List<PuntoEntity>());
            Mensaje = mensaje;
        }

        public ResListaPunto(bool esCorrecto, List<PuntoEntity> valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new List<PuntoEntity>());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
