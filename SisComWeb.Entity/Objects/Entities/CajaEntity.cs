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


        public string NomUsuario { get; set; }

        public string TipoVale { get; set; }

        public string CodiBus { get; set; }

        public string CodiChofer { get; set; }

        public string CodiGasto { get; set; }

        public string IndiAnulado { get; set; }

        public string TipoDescuento { get; set; }

        public string TipoDoc { get; set; }

        public string TipoGasto { get; set; }

        public decimal Liqui { get; set; }

        public decimal Diferencia { get; set; }

        public string Voucher { get; set; }

        public string Asiento { get; set; }

        public string Ruc { get; set; }

        public string ConcCaja { get; set; }
    }
}
