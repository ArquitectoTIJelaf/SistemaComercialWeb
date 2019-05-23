using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class CreditoRepository
    {
        #region Métodos No Transaccionales

        public static List<ClienteCreditoEntity> ListarClientesContrato(int idRuc)
        {
            var Lista = new List<ClienteCreditoEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_CleintesContrato_Consulta";
                db.AddParameter("@id_ruc", DbType.Int32, ParameterDirection.Input, idRuc);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new ClienteCreditoEntity
                        {
                            NumeContrato = Reader.GetStringValue(drlector, "nume_contrato"),
                            RucCliente = Reader.GetStringValue(drlector, "ruc_cliente"),
                            RazonSocial = Reader.GetStringValue(drlector, "razon_social"),
                            St = Reader.GetStringValue(drlector, "st"),
                            IdRuc = Reader.GetIntValue(drlector, "idRuc"),
                            NombreCorto = Reader.GetStringValue(drlector, "nombrecorto"),
                            IdContrato = Reader.GetIntValue(drlector, "IdContrato")
                        };

                        Lista.Add(entidad);
                    }
                }
            }

            return Lista;
        }

        public static ContratoEntity ConsultarContrato(int idContrato)
        {
            var entidad = new ContratoEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_contrato_Consulta";
                db.AddParameter("@idContrato", DbType.Int32, ParameterDirection.Input, idContrato);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Saldo = Reader.GetDecimalValue(drlector, "saldo");
                        entidad.Marcador = Reader.GetStringValue(drlector, "marcador");
                    }
                }
            }

            return entidad;
        }

        public static ContratoPasajeEntity VerificarContratoPasajes(string ruc, string f1, string f2, string ori, string des, string ser, int id)
        {
            var entidad = new ContratoPasajeEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Contrato_Pasajes_Verifica";
                db.AddParameter("@ruc", DbType.String, ParameterDirection.Input, ruc);
                db.AddParameter("@f1", DbType.String, ParameterDirection.Input, f1);
                db.AddParameter("@f2", DbType.String, ParameterDirection.Input, f2);
                db.AddParameter("@ori", DbType.String, ParameterDirection.Input, ori);
                db.AddParameter("@des", DbType.String, ParameterDirection.Input, des);
                db.AddParameter("@ser", DbType.String, ParameterDirection.Input, ser);
                db.AddParameter("@id", DbType.Int32, ParameterDirection.Input, id);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.IdContrato = Reader.GetIntValue(drlector, "IdContrato");
                        entidad.RucCliente = Reader.GetStringValue(drlector, "RucCliente");
                        entidad.IdRuta = Reader.GetIntValue(drlector, "IdRuta");
                        entidad.CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal");
                        entidad.CodiRuta = Reader.GetSmallIntValue(drlector, "Codi_Ruta");
                        entidad.IdPrecio = Reader.GetIntValue(drlector, "IdPrecio");
                        entidad.CodiServicio = Reader.GetByteValue(drlector, "Codi_Servicio");
                        entidad.PrecioReal = Reader.GetDecimalValue(drlector, "PrecioReal");
                        entidad.Precio = Reader.GetDecimalValue(drlector, "precio");
                        entidad.CntBoletos = Reader.GetIntValue(drlector, "CntBoletos");
                        entidad.SaldoBoletos = Reader.GetIntValue(drlector, "SaldoBoletos");
                        entidad.FechaInicial = Reader.GetStringValue(drlector, "Fecha_inicial");
                        entidad.FechaFinal = Reader.GetStringValue(drlector, "Fecha_Final");
                        entidad.MontoMas = Reader.GetDecimalValue(drlector, "Monto_Mas");
                        entidad.MontoMenos = Reader.GetDecimalValue(drlector, "Monto_Menos");
                        entidad.St = Reader.GetStringValue(drlector, "st");
                        entidad.IdRuc = Reader.GetIntValue(drlector, "idRuc");
                    }
                }
            }

            return entidad;
        }

        public static bool ValidarClientePrecioNormal(int id, string f1, string f2)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_vw_ValidaClientePrecioNormal_Consulta";
                db.AddParameter("@id", DbType.Int32, ParameterDirection.Input, id);
                db.AddParameter("@f1", DbType.String, ParameterDirection.Input, f1);
                db.AddParameter("@f2", DbType.String, ParameterDirection.Input, f2);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = true;
                        break;
                    }
                }
            }

            return valor;
        }

        public static PrecioNormalEntity VerificarPrecioNormal(int IdContrato)
        {
            var entidad = new PrecioNormalEntity
            {
                IdNormal = -1
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Scwsp_tb_precioNormal_Verifica";
                db.AddParameter("@IdContrato", DbType.Int32, ParameterDirection.Input, IdContrato);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.IdNormal = Reader.GetIntValue(drlector, "IdNormal");
                        entidad.IdContrato = Reader.GetIntValue(drlector, "IdContrato");
                        entidad.TipoPrecio = Reader.GetStringValue(drlector, "TipoPrecio");
                        entidad.MontoMas = Reader.GetDecimalValue(drlector, "MontoMas");
                        entidad.MontoMenos = Reader.GetDecimalValue(drlector, "MontoMenos");
                        entidad.CntBol = Reader.GetIntValue(drlector, "CntBol");
                        entidad.Saldo = Reader.GetIntValue(drlector, "Saldo");
                    }
                }
            }

            return entidad;
        }

        public static string ConsultarAsientoNivel(string bus, string asi)
        {
            var valor = "0";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_TB_ASIENTONIVEL_Asi_Consulta";
                db.AddParameter("@bus", DbType.String, ParameterDirection.Input, bus);
                db.AddParameter("@asi", DbType.String, ParameterDirection.Input, asi);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "N");
                    }
                }
            }

            return valor;
        }

        public static decimal BuscarPrecio(string fechaViaje, string nivel, string hora, string idPrecio)
        {
            var valor = new decimal() ;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_FindPrecio";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetDecimalValue(drlector, "");
                    }
                }
            }

            return valor;
        }

        public static List<PanelControlEntity> ListarPanelControl()
        {
            var Lista = new List<PanelControlEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Panel_Consulta";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new PanelControlEntity
                        {
                            CodiPanel = Reader.GetStringValue(drlector, "codi_panel"),
                            Valor = Reader.GetStringValue(drlector, "valor"),
                            Descripcion = Reader.GetStringValue(drlector, "descripcion"),
                            TipoControl = Reader.GetStringValue(drlector, "Tipo_Control")
                        };

                        Lista.Add(entidad);
                    }
                }
            }

            return Lista;
        }

        #endregion

        #region Métodos Transaccionales

        public static bool ActualizarBoletosStock(string St, string IdPrecio, string Tipo)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_UpdateBoletosStock";
                db.AddParameter("@St", DbType.String, ParameterDirection.Input, St);
                db.AddParameter("@IdPrecio", DbType.String, ParameterDirection.Input, IdPrecio);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, Tipo);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        #endregion
    }
}
