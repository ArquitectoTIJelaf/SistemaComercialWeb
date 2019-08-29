namespace SisComWeb.Entity
{
    public class MensajeriaEntity
    {
        public int IdMensaje { get; set; }

        public int CodiUsuario { get; set; }

        public int CodiSucursal { get; set; }

        public int CodiPventa { get; set; }

        public int Terminal { get; set; }

        public string Mensaje { get; set; }

        public byte Opt { get; set; }
    }
}
