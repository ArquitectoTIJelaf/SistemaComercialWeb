using System.Collections.Generic;

namespace SisComWeb.Entity
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

        public string CodiBus { get; set; }

        public string PlacaBus { get; set; }

        public string PlanoBus { get; set; }

        public int AsientosVendidos { get; set; }

        public string CapacidadBus { get; set; }

        public int CodiProgramacion { get; set; }

        public List<PuntoEntity> ListaEmbarques { get; set; }

        public List<PuntoEntity> ListaArribos { get; set; }

        public List<PlanoEntity> ListaPlanoBus { get; set; }

        public short Dias { get; set; }
        
        public string FechaProgramacion { get; set; } // Calculado

        public bool ProgramacionCerrada { get; set; }

        public string CodiChofer { get; set; }

        public string NombreChofer { get; set; }

        public string CodiCopiloto { get; set; }

        public string NombreCopiloto { get; set; }

        public List<DestinoRutaEntity> ListaDestinosRuta { get; set; }

        public string FechaViaje { get; set; }

        public string Color { get; set; }

        public string SecondColor { get; set; }

        public string DescServicio { get; set; }

        public string X_Estado { get; set; }


        public string Activo { get; set; }
    }
}
