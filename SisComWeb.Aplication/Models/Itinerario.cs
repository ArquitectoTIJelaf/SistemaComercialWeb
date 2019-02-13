using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class Itinerario
    {
        public int AsientosVendidos { get; set; }
        public string CapacidadBus { get; set; }
        public string CodiBus { get; set; }
        public int CodiDestino { get; set; }
        public int CodiEmpresa { get; set; }
        public int CodiOrigen { get; set; }
        public int CodiProgramacion { get; set; }
        public int CodiPuntoVenta { get; set; }
        public int CodiRuta { get; set; }
        public int CodiServicio { get; set; }
        public int CodiSucursal { get; set; }
        public int Dias { get; set; }
        public string FechaProgramacion { get; set; }
        public string HoraPartida { get; set; }
        public string HoraProgramacion { get; set; }
        public string NomDestino { get; set; }
        public string NomOrigen { get; set; }
        public string NomPuntoVenta { get; set; }
        public string NomRuta { get; set; }
        public string NomServicio { get; set; }
        public string NomSucursal { get; set; }
        public int NroRuta { get; set; }
        public long NroRutaInt { get; set; }
        public long NroViaje { get; set; }
        public string PlacaBus { get; set; }
        public string PlanoBus { get; set; }
        public string RazonSocial { get; set; }
        public string StOpcional { get; set; }
        public bool ProgramacionCerrada { get; set; }
        public string Color { get; set; }
        public string SecondColor { get; set; }
    }

    public class FiltroItinerario
    {
        public int CodiOrigen { get; set; }
        public int CodiDestino { get; set; }
        public int CodiRuta { get; set; }
        public string Hora { get; set; }
        public string FechaViaje { get; set; }
        public string TodosTurnos { get; set; }
        public string SoloProgramados { get; set; }
    }
}