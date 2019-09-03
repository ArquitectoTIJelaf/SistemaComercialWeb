﻿namespace SisComWeb.Aplication.Models
{
    public class PostergarVentaFiltro
    {
        public int IdVenta { get; set; }
        public int CodiProgramacion { get; set; }
        public int NumeAsiento { get; set; }
        public int CodiServicio { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
        public int NroViaje { get; set; }
        public string FechaProgramacion { get; set; }
        public byte CodiEmpresa { get; set; }
        public short CodiSucursal { get; set; }
        public short CodiRuta { get; set; }
        public string CodiBus { get; set; }
        public string HoraProgramacion { get; set; }
        public short CodiOrigen { get; set; }
        public short CodiDestino { get; set; }
        public string NomOrigen { get; set; }

        public string CodiEsca { get; set; }
        public string CodiOrigenBoleto { get; set; }
        public string CodiRutaBoleto { get; set; }
        public int CodiProgramacionBoleto { get; set; }
        public string BoletoCompleto { get; set; }
        public string NomPasajero { get; set; }
        public string FechaViajeBoleto { get; set; }
        public string HoraViajeBoleto { get; set; }
        public string NomDestinoBoleto { get; set; }
        public decimal PrecioVenta { get; set; }
    }
}