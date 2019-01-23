using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity
{
    public class ResFiltroUsuario
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public UsuarioPasajesEntity Valor { set; get; }

        public ResFiltroUsuario()
        {
        }

        public ResFiltroUsuario(bool esCorrecto, UsuarioPasajesEntity valor, string mensaje)
        {
            Valor = (esCorrecto ? valor : new UsuarioPasajesEntity());
            Mensaje = mensaje;
        }

        public ResFiltroUsuario(bool esCorrecto, UsuarioPasajesEntity valor, string mensaje, bool estado)
        {
            Valor = (esCorrecto ? valor : new UsuarioPasajesEntity());
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
