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
        public string CodiEsca { get; set; }
        public string CodiServicio { get; set; }
        public string CodiRuta { get; set; }
        public string NumeAsiento { get; set; }
        public int IdVenta { get; set; }
        public string CodiOrigen { get; set; }
        public string CodiProgramacion { get; set; }
        public int Oficina { get; set; }

        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
    }
}
