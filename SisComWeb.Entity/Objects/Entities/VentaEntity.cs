namespace SisComWeb.Entity
{
    public class VentaEntity
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

        public string Hora_Embarque { get; set; }

        public byte NivelAsiento { get; set; }

        public string CodiTerminal { get; set; }

        public string NomOficina { get; set; } // Del Usuario

        public string NomPuntoVenta { get; set; } // Del Usuario

        public string NomDestino { get; set; } // Pasajero

        public int IdVenta { get; set; }

        public string UserWebSUNAT { get; set; }

        public string NomEmpresaRuc { get; set; } 

        public string DirEmpresaRuc { get; set; }

        public string NomServicio { get; set; }

        public string NomOrigen { get; set; } // Pasajero

        public string DescripcionProducto
        {
            get
            {
                string[] splitNombre = Nombre.Split(',');

                return "POR EL SERVICIO DE TRANPORTE DE LA RUTA " + NomOrigen + " - " + NomDestino + " / SERVICIO : " + NomServicio + " NRO ASIENTO: " + NumeAsiento.ToString("0#") + " / PASAJERO: " + splitNombre[0] + " " + splitNombre[1] + " " + splitNombre[2] + " /DNI: " + Dni + " FECHA VIAJE: " + FechaViaje + " / HORA VIAJE: " + HoraViaje + "/1/" + PrecioVenta.ToString("F2");
            }
        }

        public int NroViaje { get; set; }

        public string FechaProgramacion { get; set; }

        public string HoraProgramacion { get; set; }

        public string CodiBus { get; set; }

        public short CodiSucursal { get; set; } // Bus (Origen)

        public short CodiRuta { get; set; } // Bus (Destino)


        public string NumeTarjetaCredito { get; set; }

        public string CodiZona { get; set; }

        public string Direccion { get; set; }

        public string Observacion { get; set; }

        public decimal Credito { get; set; }
    }
}
