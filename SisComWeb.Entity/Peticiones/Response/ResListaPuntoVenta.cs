using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class ResListaPuntoVenta
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<PuntoVentaEntity> Valor { get; set; }

        public ResListaPuntoVenta()
        {
        }

        public ResListaPuntoVenta(bool esCorrecto, List<PuntoVentaEntity> valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new List<PuntoVentaEntity>());
            Mensaje = mensaje;
        }

        public ResListaPuntoVenta(bool esCorrecto, List<PuntoVentaEntity> valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new List<PuntoVentaEntity>());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
