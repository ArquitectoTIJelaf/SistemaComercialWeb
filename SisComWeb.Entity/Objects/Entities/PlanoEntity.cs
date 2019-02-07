namespace SisComWeb.Entity
{
    public class PlanoEntity
    {
        public string Codigo { get; set; }

        public string Tipo { get; set; }

        public int Indice { get; set; }

        public int Nivel { get; set; }

        public double PrecioNormal { get; set; }

        public double PrecioMinimo { get; set; }

        public double PrecioMaximo { get; set; }

        public byte NumeAsiento { get; set; }

        public string TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }

        public string RucContacto { get; set; }

        public string FechaViaje { get; set; }

        public string FechaVenta { get; set; }

        public string Nacionalidad { get; set; }

        public double PrecioVenta { get; set; }

        public string RecogeEn { get; set; }

        public long Color { get; set; }

        public string FlagVenta { get; set; }

        public string Nombres { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public string FechaNacimiento { get; set; }

        public byte Edad { get; set; }

        public string Telefono { get; set; }
    }
}
