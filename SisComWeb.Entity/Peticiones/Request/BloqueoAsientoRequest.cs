using System.Collections.Generic;

namespace SisComWeb.Entity
{
    public class BloqueoAsientoRequest
    {
        public int CodiProgramacion { get; set; }

        public int NroViaje { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

        public byte NumeAsiento { get; set; }

        public string FechaProgramacion { get; set; }

        public decimal Precio { get; set; }

        public int CodiTerminal { get; set; }

        public List<int> NumeAsientos { get; set; }
    }
}
