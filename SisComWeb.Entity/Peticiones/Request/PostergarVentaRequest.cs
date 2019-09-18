
namespace SisComWeb.Entity.Peticiones.Request
{
    public class PostergarVentaRequest
    {
        public int IdVenta { get; set; }
        public int CodiProgramacion { get; set; }
        public int NumeAsiento { get; set; }
        public int CodiServicio { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
        public int NroViaje { get; set; }
        public string FechaProgramacion { get; set; }
        public byte CodiEmpresa { get; set; }
        public short CodiSucursal { get; set; } // Origen Bus
        public short CodiRuta { get; set; }
        public string CodiBus { get; set; }
        public string HoraProgramacion { get; set; }
        public short CodiUsuario { get; set; }
        public string NomUsuario { get; set; }
        public short CodiPuntoVenta { get; set; }
        public string CodiTerminal { get; set; }
        public short CodiOrigen { get; set; }
        public short CodiDestino { get; set; }
        public string NomOrigen { get; set; }
        public string CodiOrigenBoleto { get; set; }
        public string CodiRutaBoleto { get; set; }
        public string BoletoCompleto { get; set; }
        public string CodiEmpresaUsuario { get; set; }
        public string CodiSucursalUsuario { get; set; }
        public string NomSucursalUsuario { get; set; }
        public string NomPasajero { get; set; }
        public string FechaViajeBoleto { get; set; }
        public string HoraViajeBoleto { get; set; }
        public string NomDestinoBoleto { get; set; }
        public decimal PrecioVenta { get; set; }
        public string TipoPostergacion { get; set; }
        public int NumeAsientoBoleto { get; set; }
        public string NomDestino { get; set; }

        public decimal PrecioVentaBoleto { get; set; }
    }
}
