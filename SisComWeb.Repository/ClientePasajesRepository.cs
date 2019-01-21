using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ClientePasajesRepository
    {


        #region Metodos No Transaccionales
        public static Response<List<ClientePasajesEntity>> ListarTodos()
        {
            var response = new Response<List<ClientePasajesEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "usp_BuscarTodosCliente";
                var Lista = new List<ClientePasajesEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new ClientePasajesEntity();
                        entidad.IdCliente = Reader.GetIntValue(drlector, "Id_Clientes");
                        entidad.TipoDocId = Reader.GetStringValue(drlector, "Tipo_Doc_id");
                        entidad.NumeroDoc = Reader.GetStringValue(drlector, "Numero_Doc");
                        entidad.NombreCliente = Reader.GetStringValue(drlector, "Nombre_Clientes");
                        entidad.ApellidoP = Reader.GetStringValue(drlector, "Apellido_P");
                        entidad.ApellidoM = Reader.GetStringValue(drlector, "Apellido_M");
                        entidad.Direccion = Reader.GetStringValue(drlector, "Direccion");
                        entidad.Telefono = Reader.GetStringValue(drlector, "telefono");
                        entidad.Email = Reader.GetStringValue(drlector, "Email");
                        entidad.Edad = Reader.GetTinyIntValue(drlector, "edad");
                        entidad.FechaIng = Reader.GetDateTimeValue(drlector, "fecha_ing");
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Estado = true;
                    response.Valor = Lista;
                }
            }
            return response;
        }

        public static Response<ClientePasajesEntity> FiltrarxCodigo(int Codigo)
        {
            var response = new Response<ClientePasajesEntity>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "usp_BuscarxIdCliente";
                db.AddParameter("@Id_Clientes", DbType.Int32, ParameterDirection.Input, Codigo);
                var entidad = new ClientePasajesEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {                        
                        entidad.IdCliente = Reader.GetIntValue(drlector, "Id_Clientes");
                        entidad.TipoDocId = Reader.GetStringValue(drlector, "Tipo_Doc_id");
                        entidad.NumeroDoc = Reader.GetStringValue(drlector, "Numero_Doc");
                        entidad.NombreCliente = Reader.GetStringValue(drlector, "Nombre_Clientes");
                        entidad.ApellidoP = Reader.GetStringValue(drlector, "Apellido_P");
                        entidad.ApellidoM = Reader.GetStringValue(drlector, "Apellido_M");
                        entidad.Direccion = Reader.GetStringValue(drlector, "Direccion");
                        entidad.Telefono = Reader.GetStringValue(drlector, "telefono");
                        entidad.Email = Reader.GetStringValue(drlector, "Email");
                        entidad.Edad = Reader.GetTinyIntValue(drlector, "edad");
                        entidad.FechaIng = Reader.GetDateTimeValue(drlector, "fecha_ing");
                    }
                    response.EsCorrecto = true;
                    response.Estado = true;
                    response.Valor = entidad;
                }
            }
            return response;
        }
        #endregion

        //#region Metodos Transaccionales
        //public static Response<bool> Grabar(ClientePasajesEntity entidad)
        //{
        //    var response = new Response<bool>(false, null, "", false);
        //    using (IDatabase db = DatabaseHelper.GetDatabase())
        //    {
        //        db.ProcedureName = "usp_GrabarCliente";
        //        db.AddParameter("@Tipo_Doc_id", DbType.Int32, ParameterDirection.Input, entidad.TipoDocId);
        //        db.AddParameter("@Numero_Doc", DbType.Int32, ParameterDirection.Input, entidad.NumeroDoc);
        //        db.AddParameter("@Nombre_Clientes", DbType.Int32, ParameterDirection.Input, entidad.NombreCliente);
        //        db.AddParameter("@Apellido_P", DbType.Int32, ParameterDirection.Input, entidad.ApellidoP);
        //        db.AddParameter("@Apellido_M", DbType.Int32, ParameterDirection.Input, entidad.ApellidoM);
        //        db.AddParameter("@Direccion", DbType.Int32, ParameterDirection.Input, entidad.Direccion);
        //        db.AddParameter("@telefono", DbType.Int32, ParameterDirection.Input, entidad.Telefono);
        //        db.AddParameter("@Email", DbType.Int32, ParameterDirection.Input, entidad.Email);
        //        db.AddParameter("@edad", DbType.Int32, ParameterDirection.Input, entidad.Edad);
        //        db.AddParameter("@fecha_ing", DbType.Int32, ParameterDirection.Input, entidad.FechaIng);
        //        db.AddParameter("@Id_Clientes", DbType.Int32, ParameterDirection.Output, entidad.IdCliente);

        //        db.Execute();

        //        response.EsCorrecto = true;
        //        response.Estado = true;
        //        response.Valor = true;
        //    }
        //    return response;
        //}
        //#endregion


    }
}
