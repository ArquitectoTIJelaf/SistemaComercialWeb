using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class ResListaOficina
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<OficinaEntity> Valor { get; set; }

        public ResListaOficina()
        {
        }

        public ResListaOficina(bool esCorrecto, List<OficinaEntity> valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new List<OficinaEntity>());
            Mensaje = mensaje;
        }

        public ResListaOficina(bool esCorrecto, List<OficinaEntity> valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new List<OficinaEntity>());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
