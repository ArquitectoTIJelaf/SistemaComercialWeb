namespace SisComWeb.Aplication.Models
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

    public class ClienteCredito
    {
        public string NumeContrato { get; set; }

        public string RucCliente { get; set; }

        public string RazonSocial { get; set; }

        public string St { get; set; }

        public int IdRuc { get; set; }

        public string NombreCorto { get; set; }

        public int IdContrato { get; set; }
    }
}