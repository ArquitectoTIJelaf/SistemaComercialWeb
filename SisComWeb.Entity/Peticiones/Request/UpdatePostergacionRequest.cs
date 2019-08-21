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
        public string FlagVenta { get; set; }
        public string CodiEsca { get; set; }
        public int CodiEmpresa { get; set; }
        public int CodiUsuario { get; set; }
        public string NomUsuario { get; set; }
        public string Boleto { get; set; }
        public string Pasajero { get; set; }
        public string NomSucursal { get; set; }
        public string PuntoVenta { get; set; }
        public string Terminal { get; set; }
    }
}
