﻿namespace SisComWeb.Entity
{
    public class ItinerarioEntity
    {
        public int NroViaje { get; set; }

        public byte CodiEmpresa { get; set; }

        public string RazonSocial { get; set; }

        public int NroRuta { get; set; }

        public short CodiSucursal { get; set; }

        public string NomSucursal { get; set; }

        public short CodiRuta { get; set; }

        public string NomRuta { get; set; }

        public byte CodiServicio { get; set; }

        public string NomServicio { get; set; }

        public short CodiPuntoVenta { get; set; }

        public string NomPuntoVenta { get; set; }

        public string HoraProgramacion { get; set; }

        public string HoraPartida { get; set; }

        public string StOpcional { get; set; }

        public short CodiOrigen { get; set; }

        public string NomOrigen { get; set; }

        public short CodiDestino { get; set; }

        public string NomDestino { get; set; }

        public int NroRutaInt { get; set; }

        public short Dias { get; set; }
    }
}