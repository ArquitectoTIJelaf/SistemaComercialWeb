
namespace SisComWeb.Entity.Peticiones.Request
{
    public class BoletoF9Request
    {
        public int IdVenta { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Ruc { get; set; }
        public int Edad { get; set; }
        public string Telefono { get; set; }
        public string RecoVenta { get; set; }
        public string TipoDoc { get; set; }
        public string Nacionalidad { get; set; }
        //Auditoria
        public string CodiUsuario { get; set; }
        public string NombUsuario { get; set; }
        public string NomSucursal { get; set; }
        public string CodiPuntoVenta { get; set; }
        public string Terminal { get; set; }
        public string Boleto { get; set; }
        public decimal Precio { get; set; }
        public string NombDestino { get; set; }
        public string NumAsiento { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
    }
}
