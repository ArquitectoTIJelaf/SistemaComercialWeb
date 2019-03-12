namespace SisComWeb.Entity
{
    public class PaseEntity : VentaEntity
    {
        public string CodiGerente { get; set; }

        public string CodiSocio { get; set; }

        public string Mes { get; set; }

        public string Año { get; set; }

        public string Concepto { get; set; }

        public bool FechaAbierta { get; set; }
    }
}
