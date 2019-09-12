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

        #endregion
    }
}
