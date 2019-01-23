using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class ResListaServicio
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<ServicioEntity> Valor { get; set; }

        public ResListaServicio()
        {
        }

        public ResListaServicio(bool esCorrecto, List<ServicioEntity> valor, string mensaje)
        {
            Valor = esCorrecto ? valor : new List<ServicioEntity>();
            Mensaje = mensaje;
        }

        public ResListaServicio(bool esCorrecto, List<ServicioEntity> valor, string mensaje, bool estado)
        {
            Valor = esCorrecto ? valor : new List<ServicioEntity>();
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
