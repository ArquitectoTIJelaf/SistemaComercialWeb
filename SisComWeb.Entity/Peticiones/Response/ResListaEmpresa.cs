using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class ResListaEmpresa
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; }

        public List<EmpresaEntity> Valor { get; set; }

        public ResListaEmpresa()
        {
        }

        public ResListaEmpresa(bool esCorrecto, List<EmpresaEntity> valor, string mensaje)
        {
            Valor = esCorrecto ? valor : new List<EmpresaEntity>();
            Mensaje = mensaje;
        }

        public ResListaEmpresa(bool esCorrecto, List<EmpresaEntity> valor, string mensaje, bool estado)
        {
            Valor = esCorrecto ? valor : new List<EmpresaEntity>();
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
