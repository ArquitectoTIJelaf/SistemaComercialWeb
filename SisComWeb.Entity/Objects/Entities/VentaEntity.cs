namespace SisComWeb.Entity
{
    public class VentaEntity
    {
        public short SerieBoleto { get; set; }

        public int NumeBoleto { get; set; }

        public byte CodiEmpresa { get; set; }

        public short CodiOficina { get; set; }

        public short CodiPuntoVenta { get; set; }

        public short CodiOrigen { get; set; }

        public short CodiDestino { get; set; }

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

        public string NomDestino { get; set; } // Del Usuario

        public int IdVenta { get; set; }

        public string UserWebSUNAT { get; set; }

        public string NomEmpresa { get; set; }

        public string DirEmpresa { get; set; }

        public string NomServicio { get; set; }

        public string NomOrigenPas { get; set; }

        public string NomDestinoPas { get; set; }

        public string DescripcionProducto
        {
            get
            {
                string[] splitNombre = Nombre.Split(',');

                return "POR EL SERVICIO DE TRANPORTE DE LA RUTA " + NomOrigenPas + " - " + NomDestinoPas + " / SERVICIO : " + NomServicio + " NRO ASIENTO: " + NumeAsiento.ToString("0#") + " / PASAJERO: " + splitNombre[0] + " " + splitNombre[1] + " " + splitNombre[2] + " /DNI: " + Dni + " FECHA VIAJE: " + FechaViaje + " / HORA VIAJE: " + HoraViaje + "/1/" + PrecioVenta.ToString("F2");
            }
        }
    }
}
