using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity.Peticiones.Request
{
    public class CambiarTPagoRequest
    {
        public int IdVenta { get; set; }
        public string NewTipoPago { get; set; }
        public string OldTipoPago { get; set; }
        public string NomNewTipoPago { get; set; }
        public decimal Credito { get; set; }
        public string CodiEmpresa { get; set; }
        public string CodiTarjetaCredito { get; set; }
        public string NumeTarjetaCredito { get; set; }
        public string NomTarjetaCredito { get; set; }        
        //Venta
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public decimal PrecioVenta { get; set; }
        public string CodiDestino { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
        public string NumeAsiento { get; set; }
        public string NombDestino { get; set; }
        //Usuario
        public string NomSucursal { get; set; }
        public short CodiOficina { get; set; }
        public string NomUsuario { get; set; }
        public short CodiUsuario { get; set; }
        public short CodiPuntoVenta { get; set; }
    }
}
