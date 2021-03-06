﻿namespace SisComWeb.Entity
{
    public class TurnoRequest
    {
        public byte CodiEmpresa { get; set; }

        public short CodiPuntoVenta { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

        public short CodiSucursal { get; set; }

        public short CodiRuta { get; set; }

        public byte CodiServicio { get; set; }

        public string HoraViaje { get; set; }

        public string FechaViaje { get; set; }

        public short CodiPvUsuario { get; set; }


        public int CodiSucursalUsuario { get; set; }
    }
}
