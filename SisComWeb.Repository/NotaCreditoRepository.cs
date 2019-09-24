using SisComWeb.Entity;
using SisComWeb.Utility;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class NotaCreditoRepository
    {
        #region Métodos No Transaccionales

        public static string ConsultaTipoTerminalElectronico(int CodiTerminal, int CodiEmpresa)
        {
            var valor = "M";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Terminal_Control_Pasaje_Consulta";
                db.AddParameter("@Terminal", DbType.Int32, ParameterDirection.Input, CodiTerminal);
                db.AddParameter("@emp", DbType.Int32, ParameterDirection.Input, CodiEmpresa);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "tipo");
                        break;
                    }
                }
            }

            return valor;
        }

        public static List<BaseEntity> ListaClientesNC_Autocomplete(string TipoDocumento, string Value)
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaClientesNC_Autocomplete";
                db.AddParameter("@TipoDocumento", DbType.String, ParameterDirection.Input, TipoDocumento);
                db.AddParameter("@Value", DbType.String, ParameterDirection.Input, Value);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var item = new BaseEntity
                        {
                            id = Reader.GetStringValue(drlector, "id"),
                            label = Reader.GetStringValue(drlector, "label")
                        };
                        Lista.Add(item);
                    }
                }
            }

            return Lista;
        }

        public static List<DocumentoEmitidoNCEntity> ListaDocumentosEmitidos(DocumentosEmitidosRequest request)
        {
            var Lista = new List<DocumentoEmitidoNCEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaDocumentosEmitidos_NotaCredito";
                db.AddParameter("@ruc", DbType.String, ParameterDirection.Input, request.Ruc);
                db.AddParameter("@Fecha1", DbType.String, ParameterDirection.Input, request.FechaInicial);
                db.AddParameter("@Fecha2", DbType.String, ParameterDirection.Input, request.FechaFinal);
                db.AddParameter("@serie", DbType.Int32, ParameterDirection.Input, request.Serie);
                db.AddParameter("@numero", DbType.Int32, ParameterDirection.Input, request.Numero);
                db.AddParameter("@emp", DbType.Int32, ParameterDirection.Input, request.CodiEmpresa);
                db.AddParameter("@tipo", DbType.String, ParameterDirection.Input, request.Tipo);

                db.AddParameter("@tipoDocumento", DbType.String, ParameterDirection.Input, request.TipoDocumento);
                db.AddParameter("@tipoPasEnc", DbType.String, ParameterDirection.Input, request.TipoPasEnc);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var item = new DocumentoEmitidoNCEntity
                        {
                            NitCliente = Reader.GetStringValue(drlector, "nit_cliente"),
                            Fecha = Reader.GetDateStringValue(drlector, "fecha"),
                            IdVenta = Reader.GetIntValue(drlector, "id_venta"),
                            TpoDoc = Reader.GetStringValue(drlector, "TpoDoc"),
                            Serie = Reader.GetSmallIntValue(drlector, "Serie"),
                            Numero = Reader.GetIntValue(drlector, "numero"),
                            CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Codi_PuntoVenta"),
                            Total = Reader.GetDecimalValue(drlector, "total"),
                            Tipo = Reader.GetStringValue(drlector, "tipo"),

                            IngIgv = Reader.GetStringValue(drlector, "IngIgv"),
                            ImpManifiesto = Reader.GetStringValue(drlector, "imp_manifiesto")
                        };
                        Lista.Add(item);
                    }
                }
            }

            return Lista;
        }

        #endregion
    }
}
