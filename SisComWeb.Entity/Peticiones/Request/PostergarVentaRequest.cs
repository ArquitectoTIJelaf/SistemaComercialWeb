using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
