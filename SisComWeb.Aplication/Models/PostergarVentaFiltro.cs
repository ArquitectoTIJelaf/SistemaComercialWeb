
namespace SisComWeb.Aplication.Models
{
    public class PostergarVentaFiltro
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
        public short CodiSucursal { get; set; }
        public short CodiRuta { get; set; }
        public string CodiBus { get; set; }
        public string HoraProgramacion { get; set; }
    }
}