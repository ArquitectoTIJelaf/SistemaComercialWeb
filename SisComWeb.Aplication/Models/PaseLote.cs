using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class PaseLote
    {
    }

    public class FiltroPaseLote
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