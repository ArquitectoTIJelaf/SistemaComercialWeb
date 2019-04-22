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
                            Codigo = Reader.GetStringValue(drlector, "Codigo") ?? string.Empty,
                            Tipo = Reader.GetStringValue(drlector, "Tipo") ?? string.Empty,
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
                            TipoDocumento = string.Empty
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
                        entidad.PrecioNormal = Reader.GetRealValue(drlector, "Precio_Nor");
                        entidad.PrecioMinimo = Reader.GetRealValue(drlector, "Precio_Min");
                        entidad.PrecioMaximo = Reader.GetRealValue(drlector, "Precio_Max");
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
                        entidad.TipoDocumento = Reader.GetStringValue(drlector, "TIPO_DOC") ?? string.Empty;
                        entidad.NumeroDocumento = Reader.GetStringValue(drlector, "DNI") ?? string.Empty;
                        entidad.NombreCompleto = Reader.GetStringValue(drlector, "NOMBRE") ?? string.Empty;
                        entidad.FechaNacimiento = Reader.GetDateStringValue(drlector, "FECHAN");
                        entidad.Edad = Reader.GetStringValue(drlector, "EDAD") ?? string.Empty;
                        entidad.Sexo = Reader.GetStringValue(drlector, "SEXO") ?? string.Empty;
                        entidad.Parentesco = Reader.GetStringValue(drlector, "PARENTESCO") ?? string.Empty;
                    }
                }
            }

            return entidad;
        }

        public static List<PlanoEntity> ListarAsientosOcupados(int CodiProgramacion, string FechaProgramacion, int NroViaje, short CodiOrigen, short CodiDestino)
        {
            var Lista = new List<PlanoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarAsientosOcupados";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PlanoEntity
                        {
                            NumeAsiento = Reader.GetTinyIntValue(drlector, "NUME_ASIENTO"),
                            TipoDocumento = Reader.GetStringValue(drlector, "Tipo_Documento") ?? string.Empty,
                            NumeroDocumento = Reader.GetStringValue(drlector, "Numero_Documento") ?? string.Empty,
                            RucContacto = Reader.GetStringValue(drlector, "Ruc_Contacto") ?? string.Empty,
                            FechaViaje = Reader.GetDateStringValue(drlector, "Fecha_Viaje"),
                            FechaVenta = Reader.GetDateStringValue(drlector, "Fecha_Venta"),
                            Nacionalidad = Reader.GetStringValue(drlector, "Nacionalidad") ?? string.Empty,
                            PrecioVenta = Reader.GetRealValue(drlector, "Precio_Venta"),
                            RecogeEn = Reader.GetStringValue(drlector, "Recoge_En") ?? string.Empty,
                            Color = DataUtility.ObtenerColorHexadecimal(Reader.GetStringValue(drlector, "Color")) ?? string.Empty,
                            FlagVenta = Reader.GetStringValue(drlector, "FLAG_VENTA") ?? string.Empty,
                            Sigla = Reader.GetStringValue(drlector, "Sigla") ?? string.Empty,
                            Boleto = Reader.GetStringValue(drlector, "Boleto") ?? string.Empty,
                            TipoBoleto = Reader.GetStringValue(drlector, "tipo") ?? string.Empty,
                            IdVenta = Reader.GetStringValue(drlector, "id_venta") ?? string.Empty,
                            // Para evitar Null's
                            Nombres = string.Empty,
                            ApellidoPaterno = string.Empty,
                            ApellidoMaterno = string.Empty,
                            FechaNacimiento = string.Empty,
                            Telefono = string.Empty,
                            Sexo = string.Empty,
                            RazonSocial = string.Empty,
                            Direccion = string.Empty
                        };
                        Lista.Add(entidad);
                    }
                }
            }

            return Lista;
        }

        public static List<PlanoEntity> ListarAsientosVendidos(int CodiProgramacion)
        {
            var Lista = new List<PlanoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarAsientosVendidos";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PlanoEntity
                        {
                            NumeAsiento = Reader.GetByteValue(drlector, "NUME_ASIENTO"),
                            TipoDocumento = Reader.GetStringValue(drlector, "Tipo_Documento") ?? string.Empty,
                            NumeroDocumento = Reader.GetStringValue(drlector, "Numero_Documento") ?? string.Empty,
                            RucContacto = Reader.GetStringValue(drlector, "Ruc_Contacto") ?? string.Empty,
                            FechaViaje = Reader.GetDateStringValue(drlector, "Fecha_Viaje"),
                            FechaVenta = Reader.GetDateStringValue(drlector, "Fecha_Venta"),
                            Nacionalidad = Reader.GetStringValue(drlector, "Nacionalidad") ?? string.Empty,
                            PrecioVenta = Reader.GetRealValue(drlector, "Precio_Venta"),
                            RecogeEn = Reader.GetStringValue(drlector, "Recoge_En") ?? string.Empty,
                            Color = DataUtility.ObtenerColorHexadecimal(Reader.GetStringValue(drlector, "Color")) ?? string.Empty,
                            FlagVenta = Reader.GetStringValue(drlector, "FLAG_VENTA") ?? string.Empty,
                            Sigla = Reader.GetStringValue(drlector, "Sigla") ?? string.Empty,
                            CodiOrigen = Reader.GetSmallIntValue(drlector, "Codi_Origen"),
                            CodiDestino = Reader.GetSmallIntValue(drlector, "Codi_Destino"),
                            Boleto = Reader.GetStringValue(drlector, "Boleto") ?? string.Empty,
                            TipoBoleto = Reader.GetStringValue(drlector, "tipo") ?? string.Empty,
                            IdVenta = Reader.GetStringValue(drlector, "id_venta") ?? string.Empty,
                            // Para evitar Null's
                            Nombres = string.Empty,
                            ApellidoPaterno = string.Empty,
                            ApellidoMaterno = string.Empty,
                            FechaNacimiento = string.Empty,
                            Telefono = string.Empty,
                            Sexo = string.Empty,
                            RazonSocial = string.Empty,
                            Direccion = string.Empty
                        };
                        Lista.Add(entidad);
                    }
                }
            }

            return Lista;
        }

        public static List<PlanoEntity> ListarAsientosBloqueados(int NroViaje)
        {
            var Lista = new List<PlanoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarAsientosBloqueados";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PlanoEntity
                        {
                            NumeAsiento = Reader.GetByteValue(drlector, "NumeAsiento"),
                            CodiOrigen = Reader.GetSmallIntValue(drlector, "Codi_Origen"),
                            CodiDestino = Reader.GetSmallIntValue(drlector, "Codi_Destino"),
                            // Para evitar Null's
                            TipoDocumento =  string.Empty,
                            NumeroDocumento =  string.Empty,
                            RucContacto =  string.Empty,
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
                            Direccion = string.Empty
                        };
                        Lista.Add(entidad);
                    }
                }
            }

            return Lista;
        }

        public static string ObtenerOrdenOficinaRuta(int NroViaje, short CodiOrigen, short CodiDestino)
        {
            var valor = string.Empty;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerOrdenOficinaRuta";
                db.AddParameter("@Nro_Viaje", DbType.String, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, CodiDestino);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "orden");
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
                        valor = Reader.GetStringValue(drlector, "color");
                    }
                }
            }

            return valor;
        }

        #endregion
    }
}
