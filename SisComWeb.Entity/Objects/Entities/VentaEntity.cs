namespace SisComWeb.Entity
{
    public class VentaEntity
    {
        public short SerieBoleto { get; set; }

        public int NumeBoleto { get; set; }

        public byte CodiEmpresa { get; set; }

        public short CodiOficina { get; set; }

        public short CodiPuntoVenta { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

        public int CodiProgramacion { get; set; }

        public string RucCliente { get; set; }

        public byte NumeAsiento { get; set; }

        public string FlagVenta { get; set; }

        public decimal PrecioVenta { get; set; }

        public string Nombre { get; set; }

        public byte Edad { get; set; }

        public string Telefono { get; set; }

        public short CodiUsuario { get; set; }

        public string Dni { get; set; }

        public string NomUsuario { get; set; }

        public string TipoDocumento { get; set; }

        public string CodiDocumento { get; set; }

        public string Tipo { get; set; }

        public string Sexo { get; set; }

        public string TipoPago { get; set; }

        public string FechaViaje { get; set; }

        public string HoraViaje { get; set; }

        public string Nacionalidad { get; set; }

        public byte CodiServicio { get; set; }

        public short CodiEmbarque { get; set; }

        public short CodiArribo { get; set; }

        public string Hora_Embarque { get; set; }

        public byte NivelAsiento { get; set; }

        public short CodiTerminal { get; set; }

        public string NomOficina { get; set; }

        public string NomPuntoVenta { get; set; }

        public string NomDestino { get; set; }

        public int IdVenta { get; set; }
    }
}
