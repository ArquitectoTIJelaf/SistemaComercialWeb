
namespace SisComWeb.Aplication.Models
{
    public class PrecioRuta
    {
        public short CodiOrigen { get; set; }
        public short CodiDestino { get; set; }
        public string HoraViaje { get; set; }
        public string FechaViaje { get; set; }
        public short CodiServicio { get; set; }
        public byte CodiEmpresa { get; set; }
        public string Nivel { get; set; }
    }
}