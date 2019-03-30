using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class VentaRequest
    {
        public List<VentaEntity> Listado { get; set; }

        public string FlagVenta { get; set; }
    }
}
