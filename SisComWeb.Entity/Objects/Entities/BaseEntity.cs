namespace SisComWeb.Entity
{
    public class BaseEntity
    {
        public string id { get; set; }

        public string label { get; set; }

        // Puntos de venta
        public short CodiSucursal { get; set; }

        // Documentos
        public string MinLonDocumento { get; set; }

        public string MaxLonDocumento { get; set; }

        public string TipoDatoDocumento { get; set; }

        // Empresas
        public string Ruc { get; set; }

        public string Direccion { get; set; }

        public string Electronico { get; set; }

        public string Contingencia { get; set; }
    }
}
