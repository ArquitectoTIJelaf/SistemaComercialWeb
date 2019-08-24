using SisComWeb.Entity.Peticiones.Response;
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
                            NumeAsiento = Reader.GetStringValue(drlector, "NumeAsiento"),
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
    }    
}
