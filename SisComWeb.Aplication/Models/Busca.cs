using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class Busca : Venta
    {
        public int CodiSubruta { get; set; }
        public int CodiCliente { get; set; }
        public string RecoVenta { get; set; }
        public int ClavUsuario { get; set; }
        public string IndiAnulado { get; set; }
        public string FechaAnulacion { get; set; }
        public string CodiEsca { get; set; }
        public string PerAutoriza { get; set; }
        public int ClavUsuario1 { get; set; }
        public string EstadoAsiento { get; set; }
        public int CodiSucursalVenta { get; set; }
        public string ValeRemoto { get; set; }
        public int IdVentaWeb { get; set; }
        public decimal ImpManifiesto { get; set; }
        public string TipoVenta { get; set; }
        public string RazonSocial { get; set; }
        public string DireccionFiscal { get; set; }
    }

    public class FiltroBoleto
    {
        public int IdVenta { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Ruc { get; set; }
        public int Edad { get; set; }
        public string Telefono { get; set; }
        public string RecoVenta { get; set; }
        public string TipoDoc { get; set; }
        public string Nacionalidad { get; set; }
        public string SerieBoleto { get; set; }
        public string NumeBoleto { get; set; }
        public string Precio { get; set; }
        public string NombDestino { get; set; }
        public string NumAsiento { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
    }
}