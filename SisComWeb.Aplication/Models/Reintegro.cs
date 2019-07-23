using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class Reintegro
    {
        public short SerieBoleto { get; set; }
        public int NumeBoleto { get; set; }
        public byte CodiEmpresa { get; set; }
        public string TipoDocumento { get; set; }
        public string CodiEsca { get; set; }
        public string FlagVenta { get; set; }
        public string IndiAnulado { get; set; }
        public int IdVenta { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string RucCliente { get; set; }
        public byte NumeAsiento { get; set; }
        public decimal PrecioVenta { get; set; }
        public short CodiDestino { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
        public int CodiProgramacion { get; set; }
        public short CodiOrigen { get; set; }
        public short CodiEmbarque { get; set; }
        public short CodiArribo { get; set; }
        public byte Edad { get; set; }
        public string Telefono { get; set; }
        public string Nacionalidad { get; set; }
        public string Tipo { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public short CodiRuta { get; set; }
        public byte CodiServicio { get; set; }
        public int CodiError { get; set; }
        public string FechaNac { get; set; }
        public string[] SplitNombre
        {
            get
            {
                var tmpNombre = Nombre ?? string.Empty;
                var tmpSplitNombre = tmpNombre.Split(',');

                if (tmpSplitNombre.Length != 3)
                    tmpSplitNombre = new string[3];
                return tmpSplitNombre;
            }
        }

    }

    public class SelectReintegro : Base
    {
        public decimal monto { get; set; }
    }

    public class FiltroReintegro
    {
        public string Tipo { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public int CodiEmpresa { get; set; }
    }
}