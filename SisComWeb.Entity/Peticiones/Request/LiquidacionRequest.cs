using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity.Peticiones.Request
{
    public class LiquidacionRequest
    {
        public string FechaLiquidacion { get; set; }
        public int CodEmpresa { get; set; }
        public int CodSucursal { get; set; }
        public int CodPuntVenta { get; set; }
        public int CodUsuario { get; set; }
        public int CodInterno { get; set; }
        public int TipoProc { get; set; }
        public string tipoLiq { get; set; }
        public string Empresa { get; set; }
        public string Sucursal { get; set; }
        public string PuntoVenta { get; set; }
        public string Usuario { get; set; }
    }
}
