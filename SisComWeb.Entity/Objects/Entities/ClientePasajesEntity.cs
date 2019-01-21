using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity
{
    public class ClientePasajesEntity
    {
        public int IdCliente { get; set; }
        public string TipoDocId { get; set; }
        public string NumeroDoc { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public byte Edad { get; set; }
        public DateTime FechaIng { get; set; }

    }
}
