using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class PostergarVentaFiltro
    {
        public int IdVenta { get; set; }
        public int CodiProgramacion { get; set; }
        public int NumeAsiento { get; set; }
        public int CodiServicio { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
    }
}