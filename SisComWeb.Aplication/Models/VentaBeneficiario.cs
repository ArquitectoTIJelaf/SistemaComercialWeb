﻿namespace SisComWeb.Aplication.Models
{
    public class VentaBeneficiario
    {
        public int IdVenta { get; set; }

        public string NombresConcat { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

        public string FechaViaje { get; set; }

        public string HoraViaje { get; set; }

        public byte NumeAsiento { get; set; }

        public byte CodiServicio { get; set; }

        public int CodiProgramacion { get; set; }

        public short CodiPuntoVenta { get; set; }

        public string FechaProgramacion { get; set; }

        public byte CodiServicioProgramacion { get; set; }

        public string FlagVenta { get; set; }

        public string TipoDocumento { get; set; }

        public string Documento { get; set; }

        public string ImpManifiesto { get; set; }

        public string Cierre { get; set; }

        public string NivelAsiento { get; set; }

        public short CodiRuta { get; set; }

        public decimal PrecioVenta { get; set; }
    }
}