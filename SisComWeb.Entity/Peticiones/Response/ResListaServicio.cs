using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity
{
    public class ResListaServicio
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<ServicioPasajesEntity> Valor { get; set; }

        public ResListaServicio()
        {
        }

        public ResListaServicio(bool esCorrecto, List<ServicioPasajesEntity> valor, string mensaje)
        {
            Valor = esCorrecto ? valor : new List<ServicioPasajesEntity>();
            Mensaje = mensaje;
        }

        public ResListaServicio(bool esCorrecto, List<ServicioPasajesEntity> valor, string mensaje, bool estado)
        {
            Valor = esCorrecto ? valor : new List<ServicioPasajesEntity>();
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
