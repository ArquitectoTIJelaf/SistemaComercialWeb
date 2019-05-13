using System.Collections.Generic;

namespace SisComWeb.Entity.Peticiones.Response
{
    public class VentaResponse
    {
        public List<VentaRealizada> ListaVentasRealizadas { get; set; }

        public int CodiProgramacion { get; set; } 
    }
}
