﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Entity.Peticiones.Request
{
    public class FechaAbiertaRequest
    {
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string CodEmpresa { get; set; }
        public string CodiEsca { get; set; }
        public string CodiServicio { get; set; }
        public string CodiRuta { get; set; }
        public string NumeAsiento { get; set; }
        public int IdVenta { get; set; }
        public string CodiOrigen { get; set; }
        public int CodiProgramacion { get; set; }
        public int Oficina { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }

        public int NroViaje { get; set; }
        public string FechaProgramacion { get; set; }
        public byte CodiEmpresa { get; set; }
        public short CodiSucursal { get; set; }
        public short CodiRutaBus { get; set; }
        public string CodiBus { get; set; }
        public string HoraProgramacion { get; set; }

        public string CodiDestino { get; set; }
        public string NombDestino { get; set; }
        public string Precio { get; set; }
        public short CodiUsuario { get; set; }
        public string NomUsuario { get; set; }
        public string NomSucursal { get; set; }
        public string CodiPuntoVenta { get; set; }
        public string Terminal { get; set; }
    }

    public class VentaToFechaAbiertaRequest
    {
        public int IdVenta { get; set; }

        public int CodiServicio { get; set; }

        public int CodiRuta { get; set; }

        public short CodiUsuario { get; set; }

        public string NomUsuario { get; set; }

        public string CodiTerminal { get; set; }

        public string NomOficina { get; set; }

        public int CodiPuntoVenta { get; set; }

        public string BoletoCompleto { get; set; }

        public byte NumeAsiento { get; set; }

        public string Pasajero { get; set; }

        public string FechaViaje { get; set; }

        public string HoraViaje { get; set; }

        public string NomDestino { get; set; }

        public decimal PrecioVenta { get; set; }

        public string CodiOrigen { get; set; }

        public int CodiProgramacion { get; set; }


        public byte CodiEmpresa { get; set; }

        public string CodiSucursal { get; set; }
    }
}
