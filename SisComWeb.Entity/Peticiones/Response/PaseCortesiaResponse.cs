using System.Collections.Generic;

namespace SisComWeb.Entity.Peticiones.Response
{
    public class PaseCortesiaResponse
    {
        public decimal BoletoTotal { get; set; }
        public decimal BoletoLibre { get; set; }
        public decimal BoletoPrecio { get; set; }
        public List<BeneficiarioEntity> Beneficiarios { get; set; }
    }
}
