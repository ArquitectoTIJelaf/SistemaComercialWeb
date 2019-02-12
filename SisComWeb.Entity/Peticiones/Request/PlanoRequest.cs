namespace SisComWeb.Entity
{
    public class PlanoRequest
    {
        public string PlanoBus { get; set; }

        public int CodiProgramacion { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

        public string CodiBus { get; set; }

        public string HoraViaje { get; set; }

        public string FechaViaje { get; set; }

        public byte CodiServicio { get; set; }

        public byte CodiEmpresa { get; set; }

        public string FechaProgramacion { get; set; }

        public int NroViaje { get; set; }
    }
}
