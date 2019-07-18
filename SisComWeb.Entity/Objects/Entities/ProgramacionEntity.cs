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


        public int NroViaje { get; set; }


        // Para 'AuditoriaProg'
        public string CodiUsuario { get; set; }

        public string NomUsuario { get; set; }

        public string CodiPuntoVenta { get; set; }

        public string Terminal { get; set; }

        public string CodiOrigen { get; set; }

        public string CodiDestino { get; set; }

        public string NomOrigen { get; set; }
    }
}
