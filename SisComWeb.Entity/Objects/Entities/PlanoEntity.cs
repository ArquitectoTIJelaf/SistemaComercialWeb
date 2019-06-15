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

        public string Color { get; set; }

        public string FlagVenta { get; set; }

        public string Nombres { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public string FechaNacimiento { get; set; }

        public byte Edad { get; set; }

        public string Telefono { get; set; }

        public string Sexo { get; set; }

        public string Sigla { get; set; }

        public string RazonSocial { get; set; }

        public string Direccion { get; set; }

        public string Boleto { get; set; }

        public string TipoBoleto { get; set; }

        public string IdVenta { get; set; }

        public AcompanianteEntity ObjAcompaniante { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

        public string OrdenOrigen { get; set; }

        public string NomOrigen { get; set; }

        public string NomDestino { get; set; }

        public short CodiPuntoVenta { get; set; }

        public string NomPuntoVenta { get; set; }

        public short CodiUsuario { get; set; }

        public string NomUsuario { get; set; }

        public string NumeSolicitud { get; set; }

        public string HoraVenta { get; set; }

        public short EmbarqueCod { get; set; }

        public string EmbarqueDir { get; set; }

        public string EmbarqueHora { get; set; }

        public string ImpManifiesto { get; set; }


        public short CodiSucursal { get; set; }
    }
}
