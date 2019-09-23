namespace SisComWeb.Entity
{
    public class DocumentoEmitidoNCEntity
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


        public string ColumnTipo { get; set; }

        public string ColumnNroDocumento { get; set; }
    }
}
