namespace SisComWeb.Aplication.Models
{
    public class Pase : FiltroVenta
    {
        public string CodiGerente { get; set; } //Código de gerente que autoriza el pase

        public string CodiSocio { get; set; } //Código de socio solictante

        public string Mes { get; set; }

        public string Año { get; set; }

        public string Concepto { get; set; }

        public bool FechaAbierta { get; set; }
    }
}