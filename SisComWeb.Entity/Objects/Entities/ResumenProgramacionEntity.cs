
namespace SisComWeb.Entity.Objects.Entities
{
    public class ResumenProgramacionEntity
    {
        public string CAP { get; set; }
        public string VTS { get; set; }
        public string VTT { get; set; }
        public string RET { get; set; }
        public string PAS { get; set; }
        public string RVA { get; set; }
        public string LBR { get; set; }
        public string TOT { get; set; }

        public ResumenProgramacionEntity()
        {
            CAP = "0";
            VTS = "0";
            VTT = "0";
            RET = "0";
            PAS = "0";
            RVA = "0";
            LBR = "0";
            TOT = "0";
        }
    }
}
