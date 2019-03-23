﻿namespace SisComWeb.Aplication.Models
{
    public class VentaBeneficiario
    {
        public long IdVenta { get; set; }
        public string NombresConcat { get; set; }
        public int CodiOrigen { get; set; }
        public string NombOrigen { get; set; }
        public int CodiDestino { get; set; }
        public string NombDestino { get; set; }
        public string NombServicio { get; set; }
        public string FechViaje { get; set; }
        public string HoraViaje { get; set; }
        public int NumeAsiento { get; set; }
    }
}