using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity
{
    public class ResListaEmpresa
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<EmpresaPasajesEntity> Valor { get; set; }

        public ResListaEmpresa()
        {
        }

        public ResListaEmpresa(bool esCorrecto, List<EmpresaPasajesEntity> valor, string mensaje)
        {
            Valor = esCorrecto ? valor : new List<EmpresaPasajesEntity>();
            Mensaje = mensaje;
        }

        public ResListaEmpresa(bool esCorrecto, List<EmpresaPasajesEntity> valor, string mensaje, bool estado)
        {
            Valor = esCorrecto ? valor : new List<EmpresaPasajesEntity>();
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
