using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using System;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public class FechaAbiertaRepository
    {
        #region Métodos No Transaccionales

        public static List<FechaAbiertaEntity> VentaConsultaF6(FechaAbiertaRequest filtro)
        {
            var lista = new List<FechaAbiertaEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_VentaConsultaF6Elec";
                db.AddParameter("@nombre", DbType.String, ParameterDirection.Input, filtro.Nombre);
                db.AddParameter("@dni", DbType.String, ParameterDirection.Input, filtro.Dni);
                db.AddParameter("@fecha", DbType.String, ParameterDirection.Input, filtro.Fecha);
                db.AddParameter("@serie", DbType.Int32, ParameterDirection.Input, Convert.ToInt32(filtro.Serie));
                db.AddParameter("@nume", DbType.Int32, ParameterDirection.Input, Convert.ToInt32(filtro.Numero));
                db.AddParameter("@tipo", DbType.String, ParameterDirection.Input, filtro.Tipo);
                db.AddParameter("@empresa", DbType.Int32, ParameterDirection.Input, Convert.ToInt32(filtro.CodEmpresa));
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        lista.Add(new FechaAbiertaEntity
                        {
                            Nombre = Reader.GetStringValue(drlector, "NOMBRE"),
                            Tipo = Reader.GetStringValue(drlector, "tipo"),
                            Serie = Reader.GetStringValue(drlector, "serie_BOLETO"),
                            Numero = Reader.GetStringValue(drlector, "NUME_BOLETO"),
                            FechaVenta = Reader.GetDateStringValue(drlector, "FECH_VENTA"),
                            PrecioVenta = Reader.GetStringValue(drlector, "PREC_VENTA"),
                            CodiSubruta = Reader.GetStringValue(drlector, "CODI_SUBRUTA"),
                            CodiOrigen = Reader.GetStringValue(drlector, "COD_ORIGEN"),
                            CodiEmpresa = Reader.GetStringValue(drlector, "CODI_EMPRESA"),
                            IdVenta = Reader.GetStringValue(drlector, "id_venta")
                        });
                    }
                }
            }

            return lista;
        }
        #endregion
    }
}
