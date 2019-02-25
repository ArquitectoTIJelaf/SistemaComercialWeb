using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ClientePasajeRepository
    {
        #region Métodos No Transaccionales

        public static Response<ClientePasajeEntity> BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            var response = new Response<ClientePasajeEntity>(false, null, "Error: BuscaPasajero.", false);
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
                        entidad.FechaNacimiento = (Reader.GetDateTimeValue(drlector, "fec_nac").ToString("dd/MM/yyyy"));
                        entidad.Edad = Reader.GetTinyIntValue(drlector, "edad");
                        entidad.Direccion = Reader.GetStringValue(drlector, "Direccion");
                        entidad.Telefono = Reader.GetStringValue(drlector, "telefono");
                        entidad.RucContacto = Reader.GetStringValue(drlector, "ruc_contacto");
                        entidad.Sexo = Reader.GetStringValue(drlector, "sexo");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: BuscaPasajero.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<RucEntity> BuscarEmpresa(string RucCliente)
        {
            var response = new Response<RucEntity>(false, null, "Error: BuscarEmpresa.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, RucCliente);
                var entidad = new RucEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.RucCliente = Reader.GetStringValue(drlector, "Ruc_Cliente");
                        entidad.RazonSocial = Reader.GetStringValue(drlector, "Razon_Social");
                        entidad.Direccion = Reader.GetStringValue(drlector, "Direccion");
                        entidad.Telefono = Reader.GetStringValue(drlector, "Telefono");
                    }

                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: BuscarEmpresa.";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion

        #region Métodos Transaccionales

        public static Response<int> GrabarPasajero(ClientePasajeEntity entidad)
        {
            var response = new Response<int>(false, 0, "Error: GrabarPasajero.", false);
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
                db.AddParameter("@sexo", DbType.String, ParameterDirection.Input, entidad.Sexo);
                db.AddParameter("@Id_Clientes", DbType.Int32, ParameterDirection.Output, entidad.IdCliente);

                db.Execute();

                int _outputIdCliente = int.Parse(db.GetParameter("@Id_Clientes").ToString());

                response.EsCorrecto = true;
                response.Valor = _outputIdCliente;
                response.Mensaje = "Correcto: GrabarPasajero.";
                response.Estado = true;
            }
            return response;
        }

        public static Response<bool> ModificarPasajero(ClientePasajeEntity entidad)
        {
            var response = new Response<bool>(false, false, "Error: ModificarPasajero.", false);
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
                db.AddParameter("@sexo", DbType.String, ParameterDirection.Input, entidad.Sexo);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: ModificarPasajero.";
                response.Estado = true;
            }
            return response;
        }

        public static Response<bool> GrabarEmpresa(RucEntity entidad)
        {
            var response = new Response<bool>(false, false, "Error: GrabarEmpresa.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, entidad.RucCliente);
                db.AddParameter("@Razon_Social", DbType.String, ParameterDirection.Input, entidad.RazonSocial);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, entidad.Direccion);
                db.AddParameter("@Telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarEmpresa.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> ModificarEmpresa(RucEntity entidad)
        {
            var response = new Response<bool>(false, false, "Error: ModificarEmpresa.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, entidad.RucCliente);
                db.AddParameter("@Razon_Social", DbType.String, ParameterDirection.Input, entidad.RazonSocial);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, entidad.Direccion);
                db.AddParameter("@Telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: ModificarEmpresa.";
                response.Estado = true;
            }

            return response;
        }

        #endregion
    }
}
