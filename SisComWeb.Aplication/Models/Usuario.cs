namespace SisComWeb.Aplication.Models
{
    public class Usuario
    {
        public int CodiEmpresa { get; set; }
        public int CodiPuntoVenta { get; set; }
        public int CodiSucursal { get; set; }
        public int CodiUsuario { get; set; }
        public string Nombre { get; set; }
        public int Nivel { get; set; }
        public string NomSucursal { get; set; }
        public string NomPuntoVenta { get; set; }

        public int Terminal { get; set; }
    }
}