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

        public decimal PrecioVenta { get; set; }

        public string FechaViaje { get; set; }

        public string FechaVenta { get; set; }

        public string TipoPago { get; set; }

        public string ValeRemoto { get; set; }

        public short CodiUsuarioBoleto { get; set; }


        public string NomUsuario { get; set; }

        public byte NumeAsiento { get; set; }

        public string NomOficina { get; set; }

        public string NomPasajero { get; set; }

        public string HoraViaje { get; set; }

        public string NomDestinoPas { get; set; }

        public int Terminal { get; set; }

        public string CodiEsca { get; set; }

        public string CodiDestinoPas { get; set; }

        public bool IngresoManualPasajes { get; set; }


        public string NomOrigenPas { get; set; }
    }
}
