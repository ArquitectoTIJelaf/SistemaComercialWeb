namespace SisComWeb.Entity
{
    public class TablaAsientosBloqueadosEntity
    {
        public string Asientos { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }
    }

    public class TablaBloqueoAsientosEntity
    {
        public short CodiOrigen { get; set; }

        public string AsientosOcupados { get; set; }

        public short CodiDestino { get; set; }

        public string AsientosLiberados { get; set; }
    }
}
