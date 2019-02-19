namespace SisComWeb.Aplication.Models
{
    public class Turno
    {
    }

    public class FiltroTurno
    {
        public byte CodiEmpresa { get; set; }
        public short CodiPuntoVenta { get; set; }
        public short CodiOrigen { get; set; }
        public short CodiDestino { get; set; }
        public short CodiSucursal { get; set; }
        public short CodiRuta { get; set; }
        public byte CodiServicio { get; set; }
        public string HoraViaje { get; set; }
        public string FechaViaje { get; set; }
    }
}