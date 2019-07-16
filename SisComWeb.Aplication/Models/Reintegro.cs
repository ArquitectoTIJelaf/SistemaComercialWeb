using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class Reintegro
    {
    }

    public class SelectReintegro : Base
    {
        public decimal monto { get; set; }
    }

    public class FiltroReintegro
    {
        public string Tipo { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public int CodiEmpresa { get; set; }
    }
}