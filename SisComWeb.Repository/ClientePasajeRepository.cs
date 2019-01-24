﻿using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ClientePasajeRepository
    {
        #region MÉTODOS NO TRANSACIONALES

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

        #region MÉTODOS TRANSACIONALES

        public static Response<bool> GrabarPasajero(ClientePasajeEntity entidad)
        {
            var response = new Response<bool>(false, false, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarPasajero";
                db.AddParameter("@Tipo_Doc_id", DbType.String, ParameterDirection.Input, entidad.TipoDoc);
                db.AddParameter("@Numero_Doc", DbType.String, ParameterDirection.Input, entidad.NumeroDoc);
                db.AddParameter("@Nombre_Clientes", DbType.String, ParameterDirection.Input, entidad.NombreCliente);
                db.AddParameter("@Apellido_P", DbType.String, ParameterDirection.Input, entidad.ApellidoPaterno);
                db.AddParameter("@Apellido_M", DbType.String, ParameterDirection.Input, entidad.ApellidoMaterno);
                db.AddParameter("@fec_nac", DbType.DateTime, ParameterDirection.Input, entidad.FechaNacimiento);
                db.AddParameter("@edad", DbType.Byte, ParameterDirection.Input, entidad.Edad);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, entidad.Direccion);
                db.AddParameter("@telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.AddParameter("@ruc_contacto", DbType.String, ParameterDirection.Input, entidad.RucContacto);
                db.AddParameter("@Id_Clientes", DbType.Int32, ParameterDirection.Output, entidad.IdCliente);

                db.Execute();

                response.EsCorrecto = true;
                response.Estado = true;
                response.Valor = true;
            }
            return response;
        }

        public static Response<bool>ModificarPasajero(ClientePasajeEntity entidad)
        {
            var response = new Response<bool>(false, false, "", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarPasajero";
                db.AddParameter("@Id_Clientes", DbType.Int32, ParameterDirection.Input, entidad.IdCliente);
                db.AddParameter("@Tipo_Doc_id", DbType.String, ParameterDirection.Input, entidad.TipoDoc);
                db.AddParameter("@Numero_Doc", DbType.String, ParameterDirection.Input, entidad.NumeroDoc);
                db.AddParameter("@Nombre_Clientes", DbType.String, ParameterDirection.Input, entidad.NombreCliente);
                db.AddParameter("@Apellido_P", DbType.String, ParameterDirection.Input, entidad.ApellidoPaterno);
                db.AddParameter("@Apellido_M", DbType.String, ParameterDirection.Input, entidad.ApellidoMaterno);
                db.AddParameter("@fec_nac", DbType.DateTime, ParameterDirection.Input, entidad.FechaNacimiento);
                db.AddParameter("@edad", DbType.Byte, ParameterDirection.Input, entidad.Edad);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, entidad.Direccion);
                db.AddParameter("@telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.AddParameter("@ruc_contacto", DbType.String, ParameterDirection.Input, entidad.RucContacto);

                db.Execute();

                response.EsCorrecto = true;
                response.Estado = true;
                response.Valor = true;
            }
            return response;
        }

        #endregion
    }
}
