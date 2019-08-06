namespace SisComWeb.Aplication.Models
{
    public class TablaBloqueoAsientos
    {
        public short CodiOrigen { get; set; }

        public string AsientosOcupados { get; set; }

        public short CodiDestino { get; set; }

        public string AsientosLiberados { get; set; }
    }

    public class TablaBloqueoAsientosRequest
    {
        public int CodiProgramacion { get; set; }

        public int CodiOrigen { get; set; }

        public int CodiDestino { get; set; }

        public string AsientosOcupados { get; set; }

        public string AsientosLiberados { get; set; }

        public string Tipo { get; set; }

        public string Fecha { get; set; }


        public int NroViaje { get; set; }
    }
}