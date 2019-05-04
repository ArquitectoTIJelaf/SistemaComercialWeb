namespace SisComWeb.Aplication.Models
{
    public class Correlativo
    {

    }

    public class CorrelativoFiltro
    {
        public byte CodiEmpresa { get; set; }

        public string FlagVenta { get; set; }
    }

    public class CorrelativoResponse
    {
        public string CodiTerminalElectronico { get; set; }

        public string CorrelativoVentaBoleta { get; set; }

        public string CorrelativoVentaFactura { get; set; }

        public string CorrelativoPaseBoleta { get; set; }

        public string CorrelativoPaseFactura { get; set; }
    }
}