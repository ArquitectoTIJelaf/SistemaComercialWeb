namespace SisComWeb.Entity
{
    public class EmpresaEntity
    {
        public string Ruc { get; set; }

        public string RazonSocial { get; set; }

        public string Direccion { get; set; }

        public string Electronico { get; set; }

        public string Contingencia { get; set; }
    }

    public class AgenciaEntity
    {
        public string Direccion { get; set; }

        public string Telefono1 { get; set; }

        public string Telefono2 { get; set; }
    }
}
