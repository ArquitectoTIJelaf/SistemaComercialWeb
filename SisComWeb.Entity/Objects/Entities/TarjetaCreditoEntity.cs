namespace SisComWeb.Entity
{
    public class TarjetaCreditoEntity
    {
        public int IdVenta { get; set; }

        public string Boleto { get; set; }

        public string CodiTarjetaCredito { get; set; }

        public string NumeTarjetaCredito { get; set; }

        public string Vale { get; set; }

        public int IdCaja { get; set; }

        public string Tipo { get; set; }
    }
}
