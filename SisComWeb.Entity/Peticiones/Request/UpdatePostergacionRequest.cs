using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity.Peticiones.Request
{
    public class UpdatePostergacionRequest
    {
        public string NumeroReintegro { get; set; }
        public string CodiProgramacion { get; set; }
        public string Origen { get; set; }
        public int IdVenta { get; set; }
        public string NumeAsiento { get; set; }
        public string Ruta { get; set; }
        public string CodiServicio { get; set; }
        public string TipoDoc { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
    }
}
