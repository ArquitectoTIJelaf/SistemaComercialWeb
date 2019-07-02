using System.Collections.Generic;

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
        public List<Punto> ListaArribos { get; set; }
        public List<Punto> ListaEmbarques { get; set; }
        public List<Plano> ListaPlanoBus { get; set; }
        public string CodiChofer { get; set; }
        public string NombreChofer { get; set; }
        public string CodiCopiloto { get; set; }
        public string NombreCopiloto { get; set; }
        public List<DestinoRuta> ListaDestinosRuta { get; set; }
        public string FechaViaje { get; set; }
        public List<Base> ListaAuxDestinosRuta { get; set; }
        public string DescServicio { get; set; }
        public string X_Estado { get; set; }

        public short CantidadMaxBloqAsi { get; set; }
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

        public string NomDestino { get; set; }
    }
}