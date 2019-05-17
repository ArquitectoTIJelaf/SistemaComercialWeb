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

        #endregion

        #region Métodos Transaccionales


        
        #endregion
    }
}
