﻿using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using System;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public class FechaAbiertaRepository
    {
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
                db.AddParameter("@subruta", DbType.Int32, ParameterDirection.Input, Convert.ToInt32(filtro.CodiDestino));
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
                            IdVenta = Reader.GetStringValue(drlector, "id_venta"),
                            StRemoto = Reader.GetStringValue(drlector, "st_remoto"),
                            Dni = Reader.GetStringValue(drlector, "DNI"),
                            TipoDoc = Reader.GetStringValue(drlector, "TIPO_DOC"),
                            CodiEsca = Reader.GetStringValue(drlector, "CODI_ESCA")
                        });
                    }
                }
            }

            return lista;
        }

        //Nivel de asiento del boleto vendido
        public static int NivelAsientoVentaDerivada(int IdVenta)
        {
            int NivelAsiento = 0;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Venta_Derivada_Nivel_asi";
                db.AddParameter("@id", DbType.Int32, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        NivelAsiento = Reader.GetIntValue(drlector, "nivel_asi");
                        break;
                    }
                }
            }
            return NivelAsiento;
        }

        public static int NivelDelAsiento(string CodiBus, string Asiento)
        {
            int Nivel = 0;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_asientonivel";
                db.AddParameter("@bus", DbType.String, ParameterDirection.Input, CodiBus);
                db.AddParameter("@as", DbType.String, ParameterDirection.Input, Asiento);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Nivel = Reader.GetIntValue(drlector, "nivel");
                        break;
                    }
                }
            }
            return Nivel;
        }

        public static int TablasPnpConsulta(string Tabla)
        {
            int Codigo = 0;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_TB_Tablas_Pnp_Consulta";
                db.AddParameter("@tabla", DbType.String, ParameterDirection.Input, Tabla);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Codigo = Reader.GetIntValue(drlector, "cod_tip");
                        break;
                    }
                }
            }
            return Codigo;
        }

        public static bool VentaUpdatePostergacionEle(FechaAbiertaRequest filtro)
        {
            bool Response = false;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Venta_Update_Postergacion_Ele";
                db.AddParameter("@Numero_reint", DbType.String, ParameterDirection.Input, filtro.CodiEsca);
                db.AddParameter("@programacion", DbType.String, ParameterDirection.Input, filtro.CodiProgramacion);
                db.AddParameter("@origen", DbType.String, ParameterDirection.Input, filtro.CodiOrigen);
                db.AddParameter("@id_Venta", DbType.Int32, ParameterDirection.Input, filtro.IdVenta);
                db.AddParameter("@asiento", DbType.String, ParameterDirection.Input, filtro.NumeAsiento);
                db.AddParameter("@ruta", DbType.String, ParameterDirection.Input, "");
                db.AddParameter("@servicio", DbType.String, ParameterDirection.Input, filtro.CodiServicio);
                db.AddParameter("@TipoDoc", DbType.String, ParameterDirection.Input, filtro.Tipo);

                db.Execute();

                Response = true;
            }

            return Response;
        }

        public static void VentaUpdateImpManifiesto(int IdVenta)
        {
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_UpdateVentaImpManifiesto";
                db.AddParameter("@Id_Venta", DbType.String, ParameterDirection.Input, IdVenta);
                db.Execute();
            }
        }
        
        public static void VentaDerivadaUpdateViaje(int IdVenta, string FechaViaje, string HoraViaje, string CodServicio)
        {
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_TB_Venta_Derivada_Update_Viaje";
                db.AddParameter("@id", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@fecha", DbType.String, ParameterDirection.Input, FechaViaje);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, HoraViaje);
                db.AddParameter("@servicio", DbType.String, ParameterDirection.Input, CodServicio);
                db.Execute();
            }
        }

        public static void VentaUpdatePostergacion(decimal IdVenta, int CodiOrigen, int CodiDestino)
        {
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Venta_Update_Postergacion_2";
                db.AddParameter("@id_Venta", DbType.Decimal, ParameterDirection.Input, IdVenta);
                db.AddParameter("@cod_origen", DbType.Int32, ParameterDirection.Input, CodiOrigen);
                db.AddParameter("@codiRuta", DbType.Int32, ParameterDirection.Input, CodiDestino);
                db.Execute();
            }
        }
    }
}
