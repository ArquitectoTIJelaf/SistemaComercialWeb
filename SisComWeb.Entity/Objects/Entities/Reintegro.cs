namespace SisComWeb.Entity
{
    public class ReintegroEntity
    {
        public short ClavUsuario { get; set; }

        public short SucVenta { get; set; }

        public decimal PrecVenta { get; set; }
    }

    public class SelectReintegroEntity
    {
        public string id { get; set; }

        public string label { get; set; }

        public decimal monto { get; set; }
    }
}
