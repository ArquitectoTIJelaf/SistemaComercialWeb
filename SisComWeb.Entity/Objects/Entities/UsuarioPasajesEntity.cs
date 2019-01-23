using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity
{
    public class UsuarioPasajesEntity
    {
        public short CodiUsuario { get; set; }

        public byte CodiEmpresa { get; set; }

        public short CodiSucursal { get; set; }

        public short CodiPuntoVenta { get; set; }

        public string Password { get; set; }

        public byte Nivel { get; set; }
    }
}
