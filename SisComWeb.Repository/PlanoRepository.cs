using SisComWeb.Entity;
using SisComWeb.Utility;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class PlanoRepository
    {
        #region Métodos No Transaccionales

        public static List<PlanoEntity> BuscarPlanoBus(string CodiPlano)
        {
            var Lista = new List<PlanoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarPlanoBus";
                db.AddParameter("@Codi_Plano", DbType.String, ParameterDirection.Input, CodiPlano);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PlanoEntity
                        {
                            Codigo = Reader.GetStringValue(drlector, "Codigo"),
                            Tipo = Reader.GetStringValue(drlector, "Tipo"),
                            Indice = Reader.GetIntValue(drlector, "Indice"),
                            // Para evitar Null's
                            ApellidoMaterno = string.Empty,
                            ApellidoPaterno = string.Empty,
                            Boleto = string.Empty,
                            Color = string.Empty,
                            Direccion = string.Empty,
                            FechaNacimiento = string.Empty,
                            FechaVenta = string.Empty,
                            FechaViaje = string.Empty,
                            FlagVenta = string.Empty,
                            IdVenta = string.Empty,
                            Nacionalidad = string.Empty,
                            Nombres = string.Empty,
                            NumeroDocumento = string.Empty,
                            RazonSocial = string.Empty,
                            RecogeEn = string.Empty,
                            RucContacto = string.Empty,
                            Sexo = string.Empty,
                            Sigla = string.Empty,
                            Telefono = string.Empty,
                            TipoBoleto = string.Empty,
                            TipoDocumento = string.Empty,
                            OrdenOrigen = string.Empty,
                            NomOrigen = string.Empty,
                            NomDestino = string.Empty,
                            NomPuntoVenta = string.Empty,
                            NomUsuario = string.Empty,
                            NumeSolicitud = string.Empty,
                            HoraVenta = string.Empty,
                            EmbarqueDir = string.Empty,
                            EmbarqueHora = string.Empty,
                            ImpManifiesto = string.Empty,
                            TipoPago = string.Empty,
                            ValeRemoto = string.Empty,
                            CodiEsca = string.Empty,
                            FechaReservacion = string.Empty,
                            HoraReservacion = string.Empty,
                            Info = string.Empty,
                            Observacion = string.Empty,
                            Correo = string.Empty,
                            Especial = string.Empty
                        };
                        Lista.Add(entidad);
                    }
                }
            }

            return Lista;
        }

        public static string ObtenerNivelAsiento(string CodiBus, int NumeAsiento)
        {
            var valor = string.Empty;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerNivelAsiento";
                db.AddParameter("@Codi_Bus", DbType.String, ParameterDirection.Input, CodiBus);
                db.AddParameter("@Nume_Asiento", DbType.Int32, ParameterDirection.Input, NumeAsiento);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "Nivel");
                        break;
                    }
                }
            }

            return valor;
        }

        public static PlanoEntity ObtenerPrecioAsiento(short CodiOrigen, short CodiDestino, string Hora, string Fecha, short CodiServicio, byte CodiEmpresa, string Nivel)
        {
            var entidad = new PlanoEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerPrecioAsiento";
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);
                db.AddParameter("@Fecha", DbType.String, ParameterDirection.Input, Fecha);
                db.AddParameter("@Codi_Servicio", DbType.String, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Codi_Empresa", DbType.String, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Nivel", DbType.String, ParameterDirection.Input, Nivel);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.PrecioNormal = Reader.GetDecimalValue(drlector, "Precio_Nor");
                        entidad.PrecioMinimo = Reader.GetDecimalValue(drlector, "Precio_Min");
                        entidad.PrecioMaximo = Reader.GetDecimalValue(drlector, "Precio_Max");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static AcompanianteEntity BuscaAcompaniante(string IdVenta)
        {
            var entidad = new AcompanianteEntity
            {
                TipoDocumento = string.Empty,
                NumeroDocumento = string.Empty,
                NombreCompleto = string.Empty,
                FechaNacimiento = string.Empty,
                Edad = string.Empty,
                Sexo = string.Empty,
                Parentesco = string.Empty
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarAcompaniante";
                db.AddParameter("@IdVenta", DbType.String, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.TipoDocumento = Reader.GetStringValue(drlector, "TIPO_DOC");
                        entidad.NumeroDocumento = Reader.GetStringValue(drlector, "DNI");
                        entidad.NombreCompleto = Reader.GetStringValue(drlector, "NOMBRE");
                        entidad.FechaNacimiento = Reader.GetDateStringValue(drlector, "FECHAN");
                        entidad.Edad = Reader.GetStringValue(drlector, "EDAD");
                        entidad.Sexo = Reader.GetStringValue(drlector, "SEXO");
                        entidad.Parentesco = Reader.GetStringValue(drlector, "PARENTESCO");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static List<PlanoEntity> ListarAsientosVendidos(int NroViaje, int CodiProgramacion, string FechaProgramacion)
        {
            var Lista = new List<PlanoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarAsientosVendidos";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PlanoEntity
                        {
                            NumeAsiento = Reader.GetByteValue(drlector, "NUME_ASIENTO"),
                            TipoDocumento = Reader.GetStringValue(drlector, "Tipo_Documento"),
                            NumeroDocumento = Reader.GetStringValue(drlector, "Numero_Documento"),
                            RucContacto = Reader.GetStringValue(drlector, "Ruc_Contacto"),
                            FechaViaje = Reader.GetDateStringValue(drlector, "Fecha_Viaje"),
                            FechaVenta = Reader.GetDateStringValue(drlector, "Fecha_Venta"),
                            Nacionalidad = Reader.GetStringValue(drlector, "Nacionalidad"),
                            PrecioVenta = Reader.GetDecimalValue(drlector, "Precio_Venta"),
                            RecogeEn = Reader.GetStringValue(drlector, "Recoge_En"),
                            Color = DataUtility.ObtenerColorHexadecimal(Reader.GetStringValue(drlector, "Color")),
                            FlagVenta = Reader.GetStringValue(drlector, "FLAG_VENTA"),
                            Sigla = Reader.GetStringValue(drlector, "Sigla"),
                            CodiOrigen = Reader.GetSmallIntValue(drlector, "Codi_Origen"),
                            CodiDestino = Reader.GetSmallIntValue(drlector, "Codi_Destino"),
                            Boleto = Reader.GetStringValue(drlector, "Boleto"),
                            TipoBoleto = Reader.GetStringValue(drlector, "tipo"),
                            IdVenta = Reader.GetStringValue(drlector, "id_venta"),
                            NomOrigen = Reader.GetStringValue(drlector, "Nom_Origen"),
                            NomDestino = Reader.GetStringValue(drlector, "Nom_Destino"),
                            CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_Punto_Venta"),
                            NomPuntoVenta = Reader.GetStringValue(drlector, "Nom_Punto_Venta"),
                            CodiUsuario = Reader.GetSmallIntValue(drlector, "Codi_Usuario"),
                            NomUsuario = Reader.GetStringValue(drlector, "Nom_Usuario"),
                            NumeSolicitud = Reader.GetStringValue(drlector, "nume_solicitud"),
                            HoraVenta = Reader.GetStringValue(drlector, "HORA_VENTA"),
                            EmbarqueCod = Reader.GetSmallIntValue(drlector, "EmbarqueCod"),
                            EmbarqueDir = Reader.GetStringValue(drlector, "EmbarqueDir"),
                            EmbarqueHora = Reader.GetStringValue(drlector, "EmbarqueHora"),
                            ImpManifiesto = Reader.GetStringValue(drlector, "imp_manifiesto"),
                            CodiSucursal = Reader.GetSmallIntValue(drlector, "CODI_SUCURSAL"),
                            Nombres = Reader.GetStringValue(drlector, "NombreCompleto"),
                            Edad = Reader.GetByteValue(drlector, "EDAD"),
                            Telefono = Reader.GetStringValue(drlector, "TELEFONO"),
                            Sexo = Reader.GetStringValue(drlector, "SEXO"),
                            TipoPago = Reader.GetStringValue(drlector, "tipo_pago"),
                            ValeRemoto = Reader.GetStringValue(drlector, "vale_remoto"),
                            CodiEsca = Reader.GetStringValue(drlector, "CODI_ESCA"),
                            CodiEmpresa = Reader.GetByteValue(drlector, "CODI_EMPRESA"),
                            // Para evitar Null's
                            ApellidoPaterno = string.Empty,
                            ApellidoMaterno = string.Empty,
                            FechaNacimiento = string.Empty,
                            RazonSocial = string.Empty,
                            Direccion = string.Empty,
                            Codigo = string.Empty,
                            OrdenOrigen = string.Empty,
                            Tipo = string.Empty,
                            FechaReservacion = string.Empty,
                            HoraReservacion = string.Empty,
                            Info = string.Empty,
                            Observacion = string.Empty,
                            Correo = string.Empty,
                            Especial = string.Empty
                        };
                        Lista.Add(entidad);
                    }
                }
            }

            return Lista;
        }

        public static List<PlanoEntity> ListarAsientosBloqueados(int NroViaje, int CodiProgramacion, string FechaProgramacion)
        {
            var Lista = new List<PlanoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarAsientosBloqueados";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PlanoEntity
                        {
                            NumeAsiento = Reader.GetByteValue(drlector, "Nume_Asiento"),
                            CodiOrigen = Reader.GetSmallIntValue(drlector, "Codi_Origen"),
                            CodiDestino = Reader.GetSmallIntValue(drlector, "Codi_Destino"),
                            // Para evitar Null's
                            TipoDocumento = string.Empty,
                            NumeroDocumento = string.Empty,
                            RucContacto = string.Empty,
                            FechaViaje = string.Empty,
                            FechaVenta = string.Empty,
                            Nacionalidad = string.Empty,
                            RecogeEn = string.Empty,
                            Color = string.Empty,
                            FlagVenta = string.Empty,
                            Sigla = string.Empty,
                            Boleto = string.Empty,
                            TipoBoleto = string.Empty,
                            IdVenta = string.Empty,
                            Nombres = string.Empty,
                            ApellidoPaterno = string.Empty,
                            ApellidoMaterno = string.Empty,
                            FechaNacimiento = string.Empty,
                            Telefono = string.Empty,
                            Sexo = string.Empty,
                            RazonSocial = string.Empty,
                            Direccion = string.Empty,
                            Codigo = string.Empty,
                            OrdenOrigen = string.Empty,
                            Tipo = string.Empty,
                            NomOrigen = string.Empty,
                            NomDestino = string.Empty,
                            NomPuntoVenta = string.Empty,
                            NomUsuario = string.Empty,
                            NumeSolicitud = string.Empty,
                            HoraVenta = string.Empty,
                            EmbarqueDir = string.Empty,
                            EmbarqueHora = string.Empty,
                            ImpManifiesto = string.Empty,
                            TipoPago = string.Empty,
                            ValeRemoto = string.Empty,
                            CodiEsca = string.Empty,
                            FechaReservacion = string.Empty,
                            HoraReservacion = string.Empty,
                            Info = string.Empty,
                            Observacion = string.Empty,
                            Correo = string.Empty,
                            Especial = string.Empty
                        };
                        Lista.Add(entidad);
                    }
                }
            }

            return Lista;
        }

        public static string ObtenerOrdenOficinaRuta(int NroViaje, short CodiOriDes)
        {
            var valor = "0";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerOrdenOficinaRuta";
                db.AddParameter("@Nro_Viaje", DbType.String, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_OriDes", DbType.Int16, ParameterDirection.Input, CodiOriDes);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "orden") ?? "0";
                        break;
                    }
                }
            }

            return valor;
        }

        public static string ObtenerColorDestino(byte CodiServicio, short CodiDestino)
        {
            var valor = string.Empty;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerColorDestino";
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                db.AddParameter("@Codi_Servicio", DbType.Byte, ParameterDirection.Input, CodiServicio);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = DataUtility.ObtenerColorHexadecimal(Reader.GetStringValue(drlector, "color"));
                        break;
                    }
                }
            }

            return valor;
        }

        #endregion
    }
}
