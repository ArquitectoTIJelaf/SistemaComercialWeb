namespace SisComWeb.Entity
{
    public class ItinerarioRequest
    {
        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

        public short CodiRuta { get; set; }

        public string Hora { get; set; }

        public string FechaViaje { get; set; }

        public bool TodosTurnos { get; set; }

        public bool SoloProgramados { get; set; }

        public string NomDestino { get; set; }
    }
}
