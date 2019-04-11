namespace SisComWeb.Entity
{
    public class UsuarioEntity
    {
        public short CodiUsuario { get; set; }

        public string Login { get; set; }

        public byte CodiEmpresa { get; set; }

        public short CodiSucursal { get; set; }

        public short CodiPuntoVenta { get; set; }

        public string Password { get; set; }

        public byte Nivel { get; set; }

        public string NomSucursal { get; set; }

        public string NomPuntoVenta { get; set; }

        public int Terminal { get; set; }
    }
}
