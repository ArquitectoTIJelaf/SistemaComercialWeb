using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity
{
    public class ResListaOficina
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<OficinaPasajesEntity> Valor { get; set; }

        public ResListaOficina()
        {
        }

        public ResListaOficina(bool esCorrecto, List<OficinaPasajesEntity> valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new List<OficinaPasajesEntity>());
            Mensaje = mensaje;
        }

        public ResListaOficina(bool esCorrecto, List<OficinaPasajesEntity> valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new List<OficinaPasajesEntity>());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
