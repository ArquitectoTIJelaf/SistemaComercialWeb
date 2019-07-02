using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity.Peticiones.Request
{
    public class FechaAbiertaRequest
    {
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string CodEmpresa { get; set; }
    }
}
