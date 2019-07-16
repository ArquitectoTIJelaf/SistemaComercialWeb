namespace SisComWeb.Entity.Peticiones.Request
{
    public class ReintegroRequest
    {
        public string Tipo { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public int CodiEmpresa { get; set; }        
    }
}
