using System.Collections.Generic;

namespace SisComWeb.Aplication.Models
{
    public class BoletosCortesia
    {
        public decimal BoletoTotal { get; set; }
        public decimal BoletoLibre { get; set; }
        public decimal BoletoPrecio { get; set; }
        public List<Beneficiario> ListaBeneficiarios { get; set; }
    }
}