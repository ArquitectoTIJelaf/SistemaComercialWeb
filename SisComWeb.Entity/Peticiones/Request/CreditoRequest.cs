using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class CreditoRequest
    {
        public string FechaViaje { get; set; }

        public short CodiOficina { get; set; } // Origen de pasajero

        public short CodiRuta { get; set; } // Destino de pasajero

        public byte CodiServicio { get; set; }

        public decimal Precio { get; set; }

        public string CodiBus { get; set; }

        public byte NumeAsiento { get; set; }

        public string HoraViaje { get; set; }
    }
}
