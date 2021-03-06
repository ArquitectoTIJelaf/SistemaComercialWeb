﻿using SisComWeb.Entity.Peticiones.Response;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public class PaseLoteRepository
    {
        public static List<PaseLoteResponse> UpdatePostergacion(string Lista)
        {
            var lista = new List<PaseLoteResponse>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_UspTbVentaUpdatePostergacionEle_List";
                db.AddParameter("@Lista", DbType.String, ParameterDirection.Input, Lista);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        lista.Add(new PaseLoteResponse
                        {
                            Boleto = Reader.GetStringValue(drlector, "Boleto"),
                            NumeAsiento = Reader.GetStringValue(drlector, "NumeAsiento").PadLeft(2, '0'),
                            Pasajero = Reader.GetStringValue(drlector, "Pasajero"),
                            FechaViaje = Reader.GetStringValue(drlector, "FechaViaje"),
                            HoraViaje = Reader.GetStringValue(drlector, "HoraViaje"),
                            IdVenta = Reader.GetIntValue(drlector, "IdVenta"),
                            CodiProgramacion = Reader.GetStringValue(drlector, "CodiProgramacion")
                        });
                    }
                }
            }
            return lista;
        }

        public static string ValidarManifiesto(int CodiProgramacion, int CodiSucursal)
        {
            var valor = string.Empty;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ObtenerManifiestoProgramacion";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);

                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "Nume_Manifiesto");
                    }
                }
            }
            return valor;
        }

        public static List<int> DesbloquearAsientosList(int CodiProgramacion, string CodiTerminal)
        {
            var lista = new List<int>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_DesbloquearAsientosProgramados";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Codi_Terminal", DbType.String, ParameterDirection.Input, CodiTerminal);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        lista.Add(Reader.GetIntValue(drlector, "NUME_ASIENTO"));
                    }
                }
            }

            return lista;
        }
    }
}
