using System.Collections.Generic;

namespace SisComWeb.Aplication.Models
{
    public class PanelControl
    {
        public string CodiPanel { get; set; }

        public string Valor { get; set; }

        public string Descripcion { get; set; }

        public string TipoControl { get; set; }
    }

    public class PanelControlResponse
    {
        public List<PanelControl> ListarPanelControl { get; set; }

        public List<PanelControl> ListarPanelControlClave { get; set; }
    }
}