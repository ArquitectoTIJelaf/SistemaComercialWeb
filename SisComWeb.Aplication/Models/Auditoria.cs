namespace SisComWeb.Aplication.Models
{
    public class Auditoria
    {

    }

    public class AuditoriaRequest
    {
        public short CodiUsuario { get; set; }

        public string NomUsuario { get; set; }

        public string Tabla { get; set; }

        public string TipoMovimiento { get; set; }

        public string Boleto { get; set; }

        public string NumeAsiento { get; set; }

        public string NomOficina { get; set; }

        public string NomPuntoVenta { get; set; }

        public string Pasajero { get; set; }

        public string FechaViaje { get; set; }

        public string HoraViaje { get; set; }

        public string NomDestino { get; set; }

        public decimal Precio { get; set; }

        public string Obs1 { get; set; }

        public string Obs2 { get; set; }

        public string Obs3 { get; set; }

        public string Obs4 { get; set; }

        public string Obs5 { get; set; }
    }
}