namespace SisComWeb.Entity
{
    public class ClientePasajeEntity
    {
        public int IdCliente { get; set; }

        public string TipoDoc { get; set; }

        public string NumeroDoc { get; set; }

        public string NombreCliente { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public string FechaNacimiento { get; set; }

        public byte Edad { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string RucContacto { get; set; }

        public string Sexo { get; set; }

        public string RazonSocial { get; set; }


        public string Especial { get; set; }
    }
}
