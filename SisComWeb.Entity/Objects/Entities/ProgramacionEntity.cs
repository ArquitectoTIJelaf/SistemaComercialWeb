namespace SisComWeb.Entity
{
    public class ProgramacionEntity
    {
        public int CodiProgramacion { get; set; }

        public byte CodiEmpresa { get; set; }

        public short CodiSucursal { get; set; }

        public short CodiRuta { get; set; }

        public string CodiBus { get; set; }

        public string FechaProgramacion { get; set; }

        public string HoraProgramacion { get; set; }

        public byte CodiServicio { get; set; }
    }
}
