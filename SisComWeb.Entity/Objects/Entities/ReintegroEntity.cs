namespace SisComWeb.Entity
{
    public class ReintegroEntity
    {
        public short SerieBoleto { get; set; }
        public int NumeBoleto { get; set; }
        public byte CodiEmpresa { get; set; }
        public string TipoDocumento { get; set; }
        public string CodiEsca { get; set; }
        public string FlagVenta { get; set; }
        public string IndiAnulado { get; set; }
        public int IdVenta { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string RucCliente { get; set; }
        public byte NumeAsiento { get; set; }
        public decimal PrecioVenta { get; set; }
        public short CodiDestino { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
        public int CodiProgramacion { get; set; }
        public short CodiOrigen { get; set; }
        public short CodiEmbarque { get; set; }
        public short CodiArribo { get; set; }
        public byte Edad { get; set; }
        public string Telefono { get; set; }
        public string Nacionalidad { get; set; }
        public string Tipo { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public short CodiRuta { get; set; }
        public byte CodiServicio { get; set; }
        public int CodiError { get; set; }
        public string FechaNac { get; set; }
        public string CodiBus { get; set; }
        public int CodiPuntoVenta { get; set; }
        public string DirEmbarque { get; set; }
    }

    public class SelectReintegroEntity
    {
        public string id { get; set; }

        public string label { get; set; }

        public decimal monto { get; set; }
    }
}
