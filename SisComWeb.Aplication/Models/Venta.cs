﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisComWeb.Aplication.Models
{
    public class Venta
    {

    }

    public class FiltroVenta
    {
        public short SerieBoleto { get; set; }
        public int NumeBoleto { get; set; }
        public byte CodiEmpresa { get; set; }
        public short CodiOficina { get; set; } // Usuario
        public short CodiPuntoVenta { get; set; } // Usuario
        public short CodiOrigen { get; set; } // Pasajero
        public short CodiDestino { get; set; } // Pasajero
        public int CodiProgramacion { get; set; }
        public string RucCliente { get; set; }
        public byte NumeAsiento { get; set; }
        public string FlagVenta { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Nombre { get; set; }
        public byte Edad { get; set; }
        public string Telefono { get; set; }
        public short CodiUsuario { get; set; }
        public string Dni { get; set; }
        public string NomUsuario { get; set; }
        public string TipoDocumento { get; set; }
        public string CodiDocumento { get; set; }
        public string Tipo { get; set; }
        public string Sexo { get; set; }
        public string TipoPago { get; set; }
        public string FechaViaje { get; set; }
        public string HoraViaje { get; set; }
        public string Nacionalidad { get; set; }
        public byte CodiServicio { get; set; }
        public short CodiEmbarque { get; set; }
        public short CodiArribo { get; set; }
        public string HoraEmbarque { get; set; }
        public byte NivelAsiento { get; set; }
        public string CodiTerminal { get; set; }
        public string NomOficina { get; set; } // Del Usuario
        public string NomPuntoVenta { get; set; } // Del Usuario
        public string NomDestino { get; set; } // Pasajero
        public string NomEmpresaRuc { get; set; }
        public string DirEmpresaRuc { get; set; }
        public string NomServicio { get; set; }
        public string NomOrigen { get; set; } // Pasajero
        public int NroViaje { get; set; }
        public string FechaProgramacion { get; set; }
        public string HoraProgramacion { get; set; }
        public string CodiBus { get; set; }
        public short CodiSucursal { get; set; } // Bus (Origen)
        public short CodiRuta { get; set; } // Bus (Destino)
        public string CodiTarjetaCredito { get; set; }
        public string NumeTarjetaCredito { get; set; }
        public string CodiZona { get; set; }
        public string Direccion { get; set; }
        public string Observacion { get; set; }
        public decimal Credito { get; set; }
        public string DirEmbarque { get; set; }
        public Acompaniante ObjAcompaniante { get; set; }

        // PASE DE CORTESÍA
        public string CodiGerente { get; set; } // Código de gerente que autoriza el pase
        public string CodiSocio { get; set; } // Código de socio solictante
        public string Mes { get; set; }
        public string Anno { get; set; }
        public string Concepto { get; set; }
        public bool FechaAbierta { get; set; }

        // RESERVA
        public int IdVenta { get; set; }

        // CRÉDITO
        public int IdContrato { get; set; }
        public int IdPrecio { get; set; } // IdServicioContrato     // @IdTabla
        public string NroSolicitud { get; set; }
        public int IdArea { get; set; }
        public string FlgIda { get; set; }
        public string FechaCita { get; set; }
        public int IdHospital { get; set; }
    }

    public class VentaResponse
    {
        public List<VentaRealizada> ListaVentasRealizadas { get; set; }

        public int CodiProgramacion { get; set; }
    }

    public class VentaRealizada
    {
        public int IdVenta { get; set; }

        public string NomTipVenta { get; set; }

        public string NumeAsiento { get; set; }

        public string BoletoCompleto { get; set; }

        public string BoletoTipo { get; set; }

        public string BoletoSerie { get; set; }

        public string BoletoNum { get; set; }

        public string EmpRuc { get; set; }

        public string EmpRazSocial { get; set; }

        public string EmpDireccion { get; set; }

        public string EmpDirAgencia { get; set; }

        public string EmpTelefono1 { get; set; }

        public string EmpTelefono2 { get; set; }

        public string CodDocumento { get; set; }

        public string EmisionFecha { get; set; }

        public string EmisionHora { get; set; }

        public short CajeroCod { get; set; }

        public string CajeroNom { get; set; }

        public string PasNombreCom { get; set; }

        public string PasRuc { get; set; }

        public string PasRazSocial { get; set; }

        public string PasDireccion { get; set; }

        public string NomOriPas { get; set; }

        public string NomDesPas { get; set; }

        public byte DocTipo { get; set; }

        public string DocNumero { get; set; }

        public string PrecioCan { get; set; }

        public string PrecioDes { get; set; }

        public string NomServicio { get; set; }

        public string FechaViaje { get; set; }

        public string EmbarqueDir { get; set; }

        public string EmbarqueHora { get; set; }

        public string CodigoX_FE { get; set; }

        public string LinkPag_FE { get; set; }

        public string CodPoliza { get; set; }

        public string CodTerminal { get; set; }

        public byte TipImpresora { get; set; }

        public string CodX { get; set; }
    }
}