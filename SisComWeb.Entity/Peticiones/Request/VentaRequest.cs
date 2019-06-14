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

    public class AnularVentaRequest
    {
        public int IdVenta { get; set; }

        public int CodiUsuario { get; set; }

        public string CodiOficina { get; set; }

        public string CodiPuntoVenta { get; set; }

        public string Tipo { get; set; }

        public string FlagVenta { get; set; }

        public bool StAnulacion { get; set; }

        public bool IngresoManualPasajes { get; set; }
    }
}
