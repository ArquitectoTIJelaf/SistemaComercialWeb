using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class Plano
    {
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public string Codigo { get; set; }
        public long Color { get; set; }
        public int Edad { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechaVenta { get; set; }
        public string FechaViaje { get; set; }
        public string FlagVenta { get; set; }
        public int Indice { get; set; }
        public string Nacionalidad { get; set; }
        public int Nivel { get; set; }
        public string Nombres { get; set; }
        public int NumeAsiento { get; set; }
        public string NumeroDocumento { get; set; }
        public int PrecioMaximo { get; set; }
        public int PrecioMinimo { get; set; }
        public int PrecioNormal { get; set; }
        public int PrecioVenta { get; set; }
        public string RecogeEn { get; set; }
        public string RucContacto { get; set; }
        public string Telefono { get; set; }
        public string Tipo { get; set; }
        public string TipoDocumento { get; set; }
        public int IDS { get; set; } // Para 'BloqueoAsiento'
        public string Sexo { get; set; }
        public string Sigla { get; set; }
    }

    public class FiltroPlano
    {
        public string PlanoBus { get; set; }
        public int CodiProgramacion { get; set; }
        public int CodiOrigen { get; set; }
        public int CodiDestino { get; set; }
        public string CodiBus { get; set; }
        public string HoraViaje { get; set; }
        public string FechaViaje { get; set; }
        public int CodiServicio { get; set; }
        public int CodiEmpresa { get; set; }
        public string FechaProgramacion { get; set; }
        public long NroViaje { get; set; }
    }

    public class FiltroBloqueoAsiento
    {
        public int CodiProgramacion { get; set; }
        public int NroViaje { get; set; }
        public short CodiOrigen { get; set; }
        public short CodiDestino { get; set; }
        public byte NumeAsiento { get; set; }
        public string FechaProgramacion { get; set; }
        public double Precio { get; set; }
        public int CodiTerminal { get; set; }
    }
}