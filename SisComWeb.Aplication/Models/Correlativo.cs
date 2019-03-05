
namespace SisComWeb.Aplication.Models
{
    public class Correlativo
    {
        public short SerieBoleto { get; set; }

        public int NumeBoleto { get; set; }
    }

    public class CorrelativoFiltro
    {
        public byte CodiEmpresa { get; set; }

        public string CodiDocumento { get; set; }

        public short CodiSucursal { get; set; }

        public short CodiPuntoVenta { get; set; }

        public string CodiTerminal { get; set; }
    }
}