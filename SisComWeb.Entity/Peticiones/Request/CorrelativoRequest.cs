namespace SisComWeb.Entity
{
    public class CorrelativoRequest
    {
        public byte CodiEmpresa { get; set; }

        public string CodiDocumento { get; set; }

        public short CodiSucursal { get; set; }

        public short CodiPuntoVenta { get; set; }

        public string CodiTerminal { get; set; }

        public string FlagVenta { get; set; }
    }
}
