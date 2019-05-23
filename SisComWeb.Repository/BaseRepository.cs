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
                case 10:
                    item.id = DataUtility.ObjectToString(reader["Codi_Parentesco"]);
                    item.label = DataUtility.ObjectToString(reader["Nom_Parentesco"]).ToUpper();
                    break;
                case 11:
                    item.id = DataUtility.ObjectToString(reader["Codi_Gerente"]);
                    item.label = DataUtility.ObjectToString(reader["Nom_Gerente"]).ToUpper();
                    break;
                case 12:
                    item.id = DataUtility.ObjectToString(reader["Codi_Socio"]);
                    item.label = DataUtility.ObjectToString(reader["Nom_Socio"]).ToUpper();
                    break;
                case 13:
                    item.id = DataUtility.ObjectToString(reader["CODI_HOSPITAL"]);
                    item.label = DataUtility.ObjectToString(reader["DESCRIPCION"]).ToUpper();
                    break;
                case 14:
                    item.id = DataUtility.ObjectToString(reader["Codi_Zonas"]);
                    item.label = DataUtility.ObjectToString(reader["Descripcion"]).ToUpper();
                    break;
                case 15:
                    item.id = DataUtility.ObjectToString(reader["idAreaContrato"]);
                    item.label = DataUtility.ObjectToString(reader["Descripcion"]).ToUpper();
                    break;
            }
            return item;
        }

        #region Métodos No Transaccionales

        public static List<BaseEntity> ListaOficinas()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarSucursales";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 1));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaPuntosVenta(short CodiSucursal)
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarPuntosVenta";
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 2));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaUsuarios()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarUsuarios";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 3));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaUsuariosAutocomplete(string value)
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Usuario_Autocomplete";
                db.AddParameter("@LOGIN", DbType.String, ParameterDirection.Input, value);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 3));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaServicios()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarServicios";
                
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 4));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaEmpresas()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarEmpresas";
                
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 5));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaTiposDoc()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaTipoDocumento";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 6));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaTipoPago()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarTipoPago";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 7));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaTarjetaCredito()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarTarjetaCredito";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 8));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaCiudad()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaDistrito";
                
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 9));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListarParentesco()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarParentesco";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 10));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListarGerente()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarGerente";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 11));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListarSocio()
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarSocio";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 12));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaHospitales(int codiSucursal)
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListaHospitales";
                db.AddParameter("@CODI_SUCURSAL", DbType.Int32, ParameterDirection.Input, codiSucursal);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 13));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaSecciones(int idContrato)
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_SeccionesZona_Consulta";
                db.AddParameter("@idContrato", DbType.Int32, ParameterDirection.Input, idContrato);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 14));
                    }
                }
            }

            return Lista;
        }

        public static List<BaseEntity> ListaAreas(int idContrato)
        {
            var Lista = new List<BaseEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_AreaContrato_Consulta";
                db.AddParameter("@idContrato", DbType.Int32, ParameterDirection.Input, idContrato);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(GetItem(drlector, 15));
                    }
                }
            }

            return Lista;
        }

        #endregion
    }
}
