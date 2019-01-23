using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity
{
    public class ResListaPuntoVenta
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<PuntoVentaPasajesEntity> Valor { get; set; }

        public ResListaPuntoVenta()
        {
        }

        public ResListaPuntoVenta(bool esCorrecto, List<PuntoVentaPasajesEntity> valor, string mensaje)
        {
            Valor = esCorrecto ? valor : new List<PuntoVentaPasajesEntity>();
            Mensaje = mensaje;
        }

        public ResListaPuntoVenta(bool esCorrecto, List<PuntoVentaPasajesEntity> valor, string mensaje, bool estado)
        {
            Valor = esCorrecto ? valor : new List<PuntoVentaPasajesEntity>();
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
