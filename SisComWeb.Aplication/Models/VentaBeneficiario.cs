namespace SisComWeb.Aplication.Models
{
    public class VentaBeneficiario
    {
        public int IdVenta { get; set; }

        public string NombresConcat { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

        public string FechaViaje { get; set; }

        public string HoraViaje { get; set; }

        public byte NumeAsiento { get; set; }

        public byte CodiServicio { get; set; }


        public int CodiProgramacion { get; set; }

        public short CodiPuntoVenta { get; set; }
    }
}