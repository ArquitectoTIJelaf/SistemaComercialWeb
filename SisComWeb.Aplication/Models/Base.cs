namespace SisComWeb.Aplication.Models
{
    public class Base
    {
        public string id { get; set; }
        public string label { get; set; }
    }

    public class PuntoVentaBase : Base
    {
        public short CodiSucursal { get; set; }
    }

    public class DocumentoBase : Base
    {
        public string MinLonDocumento { get; set; }

        public string MaxLonDocumento { get; set; }

        public string TipoDatoDocumento { get; set; }
    }

    public class EmpresaBase : Base
    {
        public string Ruc { get; set; }

        public string Direccion { get; set; }

        public string Electronico { get; set; }

        public string Contingencia { get; set; }
    }
}