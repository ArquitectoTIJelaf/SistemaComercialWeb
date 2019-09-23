namespace SisComWeb.Aplication.Models
{
    public class DocumentosEmitidosRequest
    {
        public string Ruc { get; set; }

        public string FechaInicial { get; set; }

        public string FechaFinal { get; set; }

        public int Serie { get; set; }

        public int Numero { get; set; }

        public int CodiEmpresa { get; set; }

        public string Tipo { get; set; }


        public string TipoDocumento { get; set; }

        public string TipoPasEnc { get; set; }

        public string TipoNumDoc { get; set; }
    }

    public class DocumentoEmitidoNC
    {
        public string NitCliente { get; set; }

        public string Fecha { get; set; }

        public int IdVenta { get; set; }

        public string TpoDoc { get; set; }

        public short Serie { get; set; }

        public int Numero { get; set; }

        public short CodiPuntoVenta { get; set; }

        public decimal Total { get; set; }

        public string Tipo { get; set; }

        public string IngIgv { get; set; }

        public string ImpManifiesto { get; set; }
    }
}