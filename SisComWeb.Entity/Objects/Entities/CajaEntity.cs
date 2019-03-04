namespace SisComWeb.Entity
{
    public class CajaEntity
    {
        public string NumeCaja { get; set; }

        public byte CodiEmpresa { get; set; }

        public short CodiSucursal { get; set; }

        public string Boleto { get; set; }

        public decimal Monto { get; set; }

        public short CodiUsuario { get; set; }

        public string Recibe { get; set; }

        public string CodiDestino { get; set; }

        public string FechaViaje { get; set; }

        public string HoraViaje { get; set; }

        public short CodiPuntoVenta { get; set; }

        public int IdVenta { get; set; }

        public string Origen { get; set; }

        public string Modulo { get; set; }

        public string Tipo { get; set; }

        public int IdCaja { get; set; }
    }
}
