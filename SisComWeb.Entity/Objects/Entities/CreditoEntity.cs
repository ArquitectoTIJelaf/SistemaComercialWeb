namespace SisComWeb.Entity
{
    public class CreditoEntity
    {

    }

    public class ClienteCreditoEntity
    {
        public string NumeContrato { get; set; }

        public string RucCliente { get; set; }

        public string RazonSocial { get; set; }

        public string St { get; set; }

        public int IdRuc { get; set; }

        public string NombreCorto { get; set; }

        public int IdContrato { get; set; }

        // Variables auxiliares
        public int CntBoletos { get; set; }

        public int SaldoBoletos { get; set; }

        public int IdPrecio { get; set; }

        public decimal Precio { get; set; }
    }

    public class ContratoEntity
    {
        public decimal Saldo { get; set; }

        public string Marcador { get; set; }
    }

    public class ContratoPasajeEntity
    {
        public int IdContrato { get; set; }

        public string RucCliente { get; set; }

        public int IdRuta { get; set; }

        public short CodiSucursal { get; set; }

        public short CodiRuta { get; set; }

        public int IdPrecio { get; set; }

        public byte CodiServicio { get; set; }

        public decimal PrecioReal { get; set; }

        public decimal Precio { get; set; }

        public int CntBoletos { get; set; }

        public int SaldoBoletos { get; set; }

        public string FechaInicial { get; set; }

        public string FechaFinal { get; set; }

        public decimal MontoMas { get; set; }

        public decimal MontoMenos { get; set; }

        public string St { get; set; }

        public int IdRuc { get; set; }
    }

    public class PrecioNormalEntity
    {
        public int IdNormal { get; set; }

        public int IdContrato { get; set; }

        public string TipoPrecio { get; set; }

        public decimal MontoMas { get; set; }

        public decimal MontoMenos { get; set; }

        public int CntBol { get; set; }

        public int Saldo { get; set; }
    }
}
