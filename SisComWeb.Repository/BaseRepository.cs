using SisComWeb.Entity;
using SisComWeb.Utility;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class BaseRepository
    {
        public static BaseEntity GetItem(IDataReader reader, byte type = 1)
        {
            var item = new BaseEntity();
            switch (type)
            {
                case 1:
                    item.id = DataUtility.ObjectToString(reader["Codi_Sucursal"]);
                    item.label = DataUtility.ObjectToString(reader["Descripcion"]).ToUpper();
                    break;
                case 2:
                    item.id = DataUtility.ObjectToString(reader["Codi_puntoVenta"]);
                    item.label = DataUtility.ObjectToString(reader["Descripcion"]).ToUpper();
                    break;
                case 3:
                    item.id = DataUtility.ObjectToString(reader["Codi_Usuario"]);
                    item.label = DataUtility.ObjectToString(reader["Login"]).ToUpper();
                    break;
                case 4:
                    item.id = DataUtility.ObjectToString(reader["Codi_Servicio"]);
                    item.label = DataUtility.ObjectToString(reader["Descripcion"]).ToUpper();
                    break;
                case 5:
                    item.id = DataUtility.ObjectToString(reader["Codi_Empresa"]);
                    item.label = DataUtility.ObjectToString(reader["Razon_Social"]).ToUpper();
                    break;
                case 6:
                    item.id = DataUtility.ObjectToString(reader["Codi_Documento"]);
                    item.label = DataUtility.ObjectToString(reader["Nom_Documento"]).ToUpper();
                    break;
                case 7:
                    item.id = DataUtility.ObjectToString(reader["Codi_Tipo_Pago"]);
                    item.label = DataUtility.ObjectToString(reader["Nom_Tipo_Pago"]).ToUpper();
                    break;
                case 8:
                    item.id = DataUtility.ObjectToString(reader["Cod_TarjetaCredito"]);
                    item.label = DataUtility.ObjectToString(reader["Nom_TarjetaCredito"]).ToUpper();
                    break;
                case 9:
                    item.id = DataUtility.ObjectToString(reader["Codi_Zona"]);
                    item.label = DataUtility.ObjectToString(reader["Nom_Zona"]).ToUpper();
                    break;

            }
            return item;
        }

        #region Métodos No Transaccionales

        public static Response<List<BaseEntity>> ListaOficinas()
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaOficinas.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarSucursales";
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 1));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaOficinas.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaPuntosVenta(short CodiSucursal)
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaPuntosVenta.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarPuntosVenta";
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 2));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaPuntosVenta.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaUsuarios()
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaUsuarios.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarUsuarios";
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 3));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaUsuarios.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaUsuariosAutocomplete(string value)
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaUsuarios.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Usuario_Autocomplete";
                db.AddParameter("@LOGIN", DbType.String, ParameterDirection.Input, value);
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 3));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaUsuarios.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaServicios()
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaServicios.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarServicios";
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 4));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaServicios.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaEmpresas()
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaEmpresas.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarEmpresas";
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 5));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaEmpresas.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaTiposDoc()
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaTiposDoc.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaTipoDocumento";
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 6));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaTiposDoc.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaTipoPago()
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaTipoPago.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarTipoPago";
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 7));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaTipoPago.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaTarjetaCredito()
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaTarjetaCredito.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarTarjetaCredito";
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 8));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaTarjetaCredito.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BaseEntity>> ListaCiudad()
        {
            var response = new Response<List<BaseEntity>>(false, null, "Error: ListaCiudad.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaDistrito";
                var Lista = new List<BaseEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 9));
                    }
                    response.EsCorrecto = true;
                    response.Valor = Lista;
                    response.Mensaje = "Correcto: ListaCiudad.";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion
    }
}
