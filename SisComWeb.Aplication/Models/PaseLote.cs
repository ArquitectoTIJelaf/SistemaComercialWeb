using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class PaseLote
    {
        public string Boleto { get; set; }
        public string NumeAsiento { get; set; }
        public string Pasajero { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
        public int IdVenta { get; set; }
        public string CodiProgramacion { get; set; }
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
        public string FlagVenta { get; set; }
        public string CodiEsca { get; set; }
        public int CodiEmpresa { get; set; }
        public string Boleto { get; set; }
        public string Pasajero { get; set; }
    }
}