using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity.Objects.Entities
{
    public class BuscaEntity : VentaEntity
    {
        public int CodiSubruta { get; set; }
        public int CodiCliente { get; set; }
        public string RecoVenta { get; set; }
        public int ClavUsuario { get; set; }
        public string IndiAnulado { get; set; }
        public int ClavUsuario1 { get; set; }
        public int CodiSucursalVenta { get; set; }
        public string ValeRemoto { get; set; }
        public int IdVentaWeb { get; set; }
        public decimal ImpManifiesto { get; set; }
        public string TipoVenta { get; set; }
        public string RazonSocial { get; set; }
        public string DireccionFiscal { get; set; }
        public string HoraViaje { get; set; }
    }
}
