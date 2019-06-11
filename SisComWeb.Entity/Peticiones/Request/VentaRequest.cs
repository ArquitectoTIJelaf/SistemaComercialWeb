using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class VentaRequest
    {
        public List<VentaEntity> Listado { get; set; }

        public string FlagVenta { get; set; }
    }

    public class VentaRealizadaRequest
    {
        public List<VentaRealizadaEntity> ListaVentasRealizadas { get; set; }

        public string TipoImpresion { get; set; } // Impresion, Reimpresion
    }
}
