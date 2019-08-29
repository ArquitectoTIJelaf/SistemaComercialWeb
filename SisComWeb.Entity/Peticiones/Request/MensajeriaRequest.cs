namespace SisComWeb.Entity
{
    public class MensajeriaRequest
    {
        public int IdMensaje { get; set; }

        public int CodiUsuario { get; set; }

        public int CodiSucursal { get; set; }

        public int Terminal { get; set; }

        public short CajeroCod { get; set; }

        public string CajeroNom { get; set; }

        public string CajeroNomSuc { get; set; }

        public short CajeroCodPven { get; set; }

        public string CajeroTer { get; set; }
    }
}
