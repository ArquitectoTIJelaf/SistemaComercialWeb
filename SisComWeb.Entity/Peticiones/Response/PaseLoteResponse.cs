
namespace SisComWeb.Entity.Peticiones.Response
{
    public class PaseLoteResponse
    {
        public string Boleto { get; set; }
        public string NumeAsiento { get; set; }
        public string Pasajero { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
        public int IdVenta { get; set; }
        public string CodiProgramacion { get; set; }
    }
}