namespace SisComWeb.Entity
{
    public class ManifiestoRequest
    {
        public byte CodiEmpresa { get; set; }

        public int CodiProgramacion { get; set; }

        public short CodiSucursal { get; set; }

        public bool TipoApertura { get; set; }

        public short CodiSucursalBus { get; set; }

        public short CodiUsuario { get; set; }

        public string NomUsuario { get; set; }

        public string NumBoleto { get; set; }

        public string CodiPuntoVenta { get; set; }
    }
}
