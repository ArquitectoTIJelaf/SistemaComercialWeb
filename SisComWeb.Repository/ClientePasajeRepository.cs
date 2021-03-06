﻿using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ClientePasajeRepository
    {
        #region Métodos No Transaccionales

        public static ClientePasajeEntity BuscaPasajero(string TipoDoc, string NumeroDoc)
        {
            var entidad = new ClientePasajeEntity
            {
                IdCliente = 0,
                TipoDoc = string.Empty,
                NumeroDoc = string.Empty,
                NombreCliente = string.Empty,
                ApellidoPaterno = string.Empty,
                ApellidoMaterno = string.Empty,
                FechaNacimiento = string.Empty,
                Edad = 0,
                Direccion = string.Empty,
                Telefono = string.Empty,
                RucContacto = string.Empty,
                Sexo = string.Empty,
                RazonSocial = string.Empty,
                Especial = string.Empty,

                Correo = string.Empty
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarPasajero";
                db.AddParameter("@Tipo_Doc_Id", DbType.String, ParameterDirection.Input, TipoDoc);
                db.AddParameter("@Numero_Doc", DbType.String, ParameterDirection.Input, NumeroDoc);
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
                        entidad.FechaNacimiento = Reader.GetDateStringValue(drlector, "fec_nac");
                        entidad.Edad = Reader.GetByteValue(drlector, "edad");
                        entidad.Direccion = Reader.GetStringValue(drlector, "Direccion");
                        entidad.Telefono = Reader.GetStringValue(drlector, "telefono");
                        entidad.RucContacto = Reader.GetStringValue(drlector, "ruc_contacto");
                        entidad.Sexo = Reader.GetStringValue(drlector, "sexo");
                        entidad.Especial = Reader.GetStringValue(drlector, "ESPECIAL");

                        entidad.Correo = Reader.GetStringValue(drlector, "Email");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static RucEntity BuscarEmpresa(string RucCliente)
        {
            var entidad = new RucEntity
            {
                RucCliente = string.Empty,
                RazonSocial = string.Empty,
                Direccion = string.Empty,
                Telefono = string.Empty
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, RucCliente);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.RucCliente = Reader.GetStringValue(drlector, "Ruc_Cliente");
                        entidad.RazonSocial = Reader.GetStringValue(drlector, "Razon_Social");
                        entidad.Direccion = Reader.GetStringValue(drlector, "Direccion");
                        entidad.Telefono = Reader.GetStringValue(drlector, "Telefono");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static List<ClientePasajeEntity> BuscarClientesPasaje(string campo, string nombres, string paterno, string materno, string TipoDocId)
        {
            var Lista = new List<ClientePasajeEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Cliente_Pasajes_Buscar";
                db.AddParameter("@campo", DbType.String, ParameterDirection.Input, campo);
                db.AddParameter("@nombres", DbType.String, ParameterDirection.Input, nombres);
                db.AddParameter("@paterno", DbType.String, ParameterDirection.Input, paterno);
                db.AddParameter("@materno", DbType.String, ParameterDirection.Input, materno);
                db.AddParameter("@Tipo_Doc_id", DbType.String, ParameterDirection.Input, TipoDocId);

                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(new ClientePasajeEntity
                        {
                            NumeroDoc = Reader.GetStringValue(drlector, "Numero_Doc"),
                            NombreCliente = Reader.GetStringValue(drlector, "Nombre_Clientes"),
                            ApellidoPaterno = Reader.GetStringValue(drlector, "Apellido_p"),
                            ApellidoMaterno = Reader.GetStringValue(drlector, "Apellido_M")
                        });
                    }
                }
            }

            return Lista;
        }

        #endregion

        #region Métodos Transaccionales

        public static int GrabarPasajero(ClientePasajeEntity entidad)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarPasajero";
                db.AddParameter("@Tipo_Doc_id", DbType.String, ParameterDirection.Input, entidad.TipoDoc);
                db.AddParameter("@Numero_Doc", DbType.String, ParameterDirection.Input, entidad.NumeroDoc);
                db.AddParameter("@Nombre_Clientes", DbType.String, ParameterDirection.Input, entidad.NombreCliente);
                db.AddParameter("@Apellido_P", DbType.String, ParameterDirection.Input, entidad.ApellidoPaterno);
                db.AddParameter("@Apellido_M", DbType.String, ParameterDirection.Input, entidad.ApellidoMaterno);
                db.AddParameter("@fec_nac", DbType.String, ParameterDirection.Input, entidad.FechaNacimiento);
                db.AddParameter("@edad", DbType.Byte, ParameterDirection.Input, entidad.Edad);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, string.Empty);
                db.AddParameter("@telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.AddParameter("@ruc_contacto", DbType.String, ParameterDirection.Input, entidad.RucContacto);
                db.AddParameter("@sexo", DbType.String, ParameterDirection.Input, entidad.Sexo);

                db.AddParameter("@correo", DbType.String, ParameterDirection.Input, entidad.Correo);

                db.AddParameter("@Id_Clientes", DbType.Int32, ParameterDirection.Output, entidad.IdCliente);

                db.Execute();

                var auxIdClientes = db.GetParameter("@Id_Clientes").ToString();

                valor = int.Parse(string.IsNullOrEmpty(auxIdClientes) ? "0" : auxIdClientes);
            }

            return valor;
        }

        public static bool ModificarPasajero(ClientePasajeEntity entidad)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarPasajero";
                db.AddParameter("@Id_Clientes", DbType.Int32, ParameterDirection.Input, entidad.IdCliente);
                db.AddParameter("@Tipo_Doc_id", DbType.String, ParameterDirection.Input, entidad.TipoDoc);
                db.AddParameter("@Numero_Doc", DbType.String, ParameterDirection.Input, entidad.NumeroDoc);
                db.AddParameter("@Nombre_Clientes", DbType.String, ParameterDirection.Input, entidad.NombreCliente);
                db.AddParameter("@Apellido_P", DbType.String, ParameterDirection.Input, entidad.ApellidoPaterno);
                db.AddParameter("@Apellido_M", DbType.String, ParameterDirection.Input, entidad.ApellidoMaterno);
                db.AddParameter("@fec_nac", DbType.String, ParameterDirection.Input, entidad.FechaNacimiento);
                db.AddParameter("@edad", DbType.Byte, ParameterDirection.Input, entidad.Edad);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, string.Empty);
                db.AddParameter("@telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.AddParameter("@ruc_contacto", DbType.String, ParameterDirection.Input, entidad.RucContacto);
                db.AddParameter("@sexo", DbType.String, ParameterDirection.Input, entidad.Sexo);

                db.AddParameter("@correo", DbType.String, ParameterDirection.Input, entidad.Correo);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool GrabarEmpresa(RucEntity entidad)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, entidad.RucCliente);
                db.AddParameter("@Razon_Social", DbType.String, ParameterDirection.Input, entidad.RazonSocial);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, entidad.Direccion);
                db.AddParameter("@Telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ModificarEmpresa(RucEntity entidad)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarEmpresa";
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, entidad.RucCliente);
                db.AddParameter("@Razon_Social", DbType.String, ParameterDirection.Input, entidad.RazonSocial);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, entidad.Direccion);
                db.AddParameter("@Telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        #endregion
    }
}
