
namespace SisComWeb.Entity
{
    public class DocumentosEmitidosRequest
    {
        public string Ruc { get; set; }

        public string FechaInicial { get; set; }

        public string FechaFinal { get; set; }

        public int Serie { get; set; }

        public int Numero { get; set; }

        public int CodiEmpresa { get; set; }

        public string Tipo { get; set; }


        public string TipoDocumento { get; set; }

        public string TipoPasEnc { get; set; }

        public string TipoNumDoc { get; set; }
    }
}
