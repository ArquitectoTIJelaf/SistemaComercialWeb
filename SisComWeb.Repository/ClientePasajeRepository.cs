using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ClientePasajeRepository
    {
        #region Métodos no transaccionales

        public static Response<List<ClientePasajeEntity>> ListarTodos()
        {
            var response = new Response<List<ClientePasajeEntity>>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "usp_BuscarTodosCliente";
                var Lista = new List<ClientePasajeEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        var entidad = new ClientePasajeEntity
                        {
                            IdCliente = Reader.GetIntValue(drlector, "Id_Clientes"),
                            TipoDoc = Reader.GetStringValue(drlector, "Tipo_Doc_id"),
                            NumeroDoc = Reader.GetStringValue(drlector, "Numero_Doc"),
                            NombreCliente = Reader.GetStringValue(drlector, "Nombre_Clientes"),
                            ApellidoPaterno = Reader.GetStringValue(drlector, "Apellido_P"),
                            ApellidoMaterno = Reader.GetStringValue(drlector, "Apellido_M"),
                            Direccion = Reader.GetStringValue(drlector, "Direccion"),
                            Telefono = Reader.GetStringValue(drlector, "telefono"),
                            Email = Reader.GetStringValue(drlector, "Email"),
                            Edad = Reader.GetTinyIntValue(drlector, "edad"),
                            FechaIng = Reader.GetDateTimeValue(drlector, "fecha_ing")
                        };
                        Lista.Add(entidad);
                    }
                    response.EsCorrecto = true;
                    response.Estado = true;
                    response.Valor = Lista;
                }
            }
            return response;
        }

        public static Response<ClientePasajeEntity> FiltrarxCodigo(int Codigo)
        {
            var response = new Response<ClientePasajeEntity>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "usp_BuscarxIdCliente";
                db.AddParameter("@Id_Clientes", DbType.Int32, ParameterDirection.Input, Codigo);
                var entidad = new ClientePasajeEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {                        
                        entidad.IdCliente = Reader.GetIntValue(drlector, "Id_Clientes");
                        entidad.TipoDoc = Reader.GetStringValue(drlector, "Tipo_Doc_id");
                        entidad.NumeroDoc = Reader.GetStringValue(drlector, "Numero_Doc");
                        entidad.NombreCliente = Reader.GetStringValue(drlector, "Nombre_Clientes");
                        entidad.ApellidoPaterno = Reader.GetStringValue(drlector, "Apellido_P");
                        entidad.ApellidoMaterno = Reader.GetStringValue(drlector, "Apellido_M");
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

        public static Response<ClientePasajeEntity> BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            var response = new Response<ClientePasajeEntity>(false, null, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarPasajero";
                db.AddParameter("@Tipo_Doc_Id", DbType.String, ParameterDirection.Input, TipoDoc);
                db.AddParameter("@Numero_Doc", DbType.String, ParameterDirection.Input, NumeroDoc);
                var entidad = new ClientePasajeEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.IdCliente = Reader.GetIntValue(drlector, "Id_Clientes");
                        entidad.TipoDoc = Reader.GetStringValue(drlector, "Tipo_Doc_id");
                        entidad.NumeroDoc = Reader.GetStringValue(drlector, "Numero_Doc");
                        entidad.NombreCliente = Reader.GetStringValue(drlector, "Nombre_Clientes");
                        entidad.ApellidoPaterno = Reader.GetStringValue(drlector, "Apellido_P");
                        entidad.ApellidoMaterno = Reader.GetStringValue(drlector, "Apellido_M");
                        entidad.FechaNacimiento = Reader.GetDateTimeValue(drlector, "fec_nac");
                        entidad.Edad = Reader.GetTinyIntValue(drlector, "edad");
                        entidad.Direccion = Reader.GetStringValue(drlector, "Direccion");
                        entidad.Telefono = Reader.GetStringValue(drlector, "telefono");
                        entidad.RucContacto = Reader.GetStringValue(drlector, "ruc_contacto");
                    }
                    response.EsCorrecto = true;
                    response.Estado = true;
                    response.Valor = entidad;
                }
            }
            return response;
        }

        #endregion

        //#region Métodos transaccionales

        //public static Response<bool> Grabar(ClientePasajeEntity entidad)
        //{
        //    var response = new Response<bool>(false, null, "", false);
        //    using (IDatabase db = DatabaseHelper.GetDatabase())
        //    {
        //        db.ProcedureName = "usp_GrabarCliente";
        //        db.AddParameter("@Tipo_Doc_id", DbType.Int32, ParameterDirection.Input, entidad.TipoDoc);
        //        db.AddParameter("@Numero_Doc", DbType.Int32, ParameterDirection.Input, entidad.NumeroDoc);
        //        db.AddParameter("@Nombre_Clientes", DbType.Int32, ParameterDirection.Input, entidad.NombreCliente);
        //        db.AddParameter("@Apellido_P", DbType.Int32, ParameterDirection.Input, entidad.ApellidoPaterno);
        //        db.AddParameter("@Apellido_M", DbType.Int32, ParameterDirection.Input, entidad.ApellidoMaterno);
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
