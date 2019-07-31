﻿using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using System;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public class ReintegroRepository
    {
        public static ReintegroEntity VentaConsultaF12(ReintegroRequest filtro)
        {
            var objeto = new ReintegroEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_VentaConsultaF12Elect";
                db.AddParameter("@serie", DbType.Int32, ParameterDirection.Input, filtro.Serie);
                db.AddParameter("@numero", DbType.Int32, ParameterDirection.Input, filtro.Numero);
                db.AddParameter("@Empresa", DbType.Int32, ParameterDirection.Input, filtro.CodiEmpresa);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, filtro.Tipo);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        objeto = new ReintegroEntity
                        {
                            SerieBoleto = Reader.GetSmallIntValue(drlector, "SERIE_BOLETO"),
                            NumeBoleto = Reader.GetIntValue(drlector, "NUME_BOLETO"),
                            CodiEmpresa = Reader.GetByteValue(drlector, "CODI_EMPRESA"),
                            TipoDocumento = Reader.GetStringValue(drlector, "TIPO_DOC"),
                            CodiEsca = Reader.GetStringValue(drlector, "CODI_ESCA"),
                            FlagVenta = Reader.GetStringValue(drlector, "FLAG_VENTA"),
                            IndiAnulado = Reader.GetStringValue(drlector, "INDI_ANULADO"),
                            IdVenta = Reader.GetIntValue(drlector, "id_venta"),
                            Dni = Reader.GetStringValue(drlector, "DNI"),
                            Nombre = Reader.GetStringValue(drlector, "NOMBRE"),
                            RucCliente = Reader.GetStringValue(drlector, "NIT_CLIENTE"),
                            NumeAsiento = Reader.GetByteValue(drlector, "NUME_ASIENTO"),
                            PrecioVenta = Reader.GetDecimalValue(drlector, "PREC_VENTA"),
                            CodiDestino = Reader.GetSmallIntValue(drlector, "CODI_SUBRUTA"),
                            FechaViaje = Reader.GetDateStringValue(drlector, "Fecha_Viaje"),
                            HoraViaje = Reader.GetStringValue(drlector, "Hora_Viaje"),
                            CodiProgramacion = Reader.GetIntValue(drlector, "CODI_PROGRAMACION"),
                            CodiOrigen = Reader.GetSmallIntValue(drlector, "COD_ORIGEN"),
                            CodiEmbarque = Reader.GetSmallIntValue(drlector, "sube_en"),
                            CodiArribo = Reader.GetSmallIntValue(drlector, "baja_en"),
                            Edad = Reader.GetByteValue(drlector, "EDAD"),
                            Telefono = Reader.GetStringValue(drlector, "TELEFONO"),
                            Nacionalidad = Reader.GetStringValue(drlector, "nacionalidad"),
                            Tipo = Reader.GetStringValue(drlector, "TIPO")
                        };
                    }
                }
            }

            return objeto;
        }

        public static List<SelectReintegroEntity> ListaOpcionesModificacion()
        {
            var Lista = new List<SelectReintegroEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificacionConsultar";
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(new SelectReintegroEntity()
                        {
                            id = Reader.GetStringValue(drlector, "codigo"),
                            label = Reader.GetStringValue(drlector, "descripcion").ToUpper(),
                            monto = Reader.GetDecimalValue(drlector, "monto")
                        });
                    }
                }
            }

            return Lista;
        }

        public static ProgramacionEntity DatosProgramacion(int codiProgramacion)
        {
            var objeto = new ProgramacionEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_programacion_Datos";
                db.AddParameter("@codi_programacion", DbType.Int32, ParameterDirection.Input, codiProgramacion);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        objeto = new ProgramacionEntity()
                        {
                            CodiEmpresa = Reader.GetByteValue(drlector, "Codi_Empresa"),
                            CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal"),
                            CodiRuta = Reader.GetSmallIntValue(drlector, "Codi_ruta"),
                            CodiBus = Reader.GetStringValue(drlector, "Codi_Bus"),
                            FechaProgramacion = Reader.GetDateStringValue(drlector, "Fech_programacion"),
                            HoraProgramacion = Reader.GetStringValue(drlector, "Hora_programacion"),
                            CodiServicio = Reader.GetByteValue(drlector, "Codi_Servicio")
                        };
                    }
                }
            }
            return objeto;
        }

        //Valida si existe el DNI en consulta
        public static bool ValidaExDni(string documento)
        {
            bool response = false;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_DniExConsulta";
                db.AddParameter("@dni", DbType.String, ParameterDirection.Input, documento);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        response = Reader.GetBooleanValue(drlector, "Response");
                    }
                }
            }
            return response;
        }

        //Insertar Reintegro
        public static int SaveReintegro(ReintegroVentaRequest filtro)
        {
            int response = 0;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Venta_Insert_Derv_Act_Corr";
                db.AddParameter("@serie", DbType.Int16, ParameterDirection.Input, filtro.Serie);
                db.AddParameter("@NUME_BOLETO", DbType.Int32, ParameterDirection.Input, filtro.nume_boleto);
                db.AddParameter("@CODI_EMPRESA", DbType.Int32, ParameterDirection.Input, filtro.Codi_Empresa);
                db.AddParameter("@CODI_SUCURSAL", DbType.Int16, ParameterDirection.Input, filtro.CODI_SUCURSAL);
                db.AddParameter("@CODI_PROGRAMACION", DbType.Int32, ParameterDirection.Input, filtro.CODI_PROGRAMACION);
                db.AddParameter("@CODI_SUBRUTA", DbType.Int16, ParameterDirection.Input, filtro.CODI_SUBRUTA);
                db.AddParameter("@CODI_Cliente", DbType.String, ParameterDirection.Input, filtro.CODI_Cliente);
                db.AddParameter("@NIT_CLIENTE", DbType.String, ParameterDirection.Input, filtro.NIT_CLIENTE);
                db.AddParameter("@PREC_VENTA", DbType.Decimal, ParameterDirection.Input, filtro.PRECIO_VENTA);
                db.AddParameter("@NUME_ASIENTO", DbType.Int16, ParameterDirection.Input, filtro.NUMERO_ASIENTO);
                db.AddParameter("@FLAG_VENTA", DbType.String, ParameterDirection.Input, filtro.FLAG_VENTA);
                db.AddParameter("@FECH_VENTA", DbType.String, ParameterDirection.Input, filtro.FECH_VENTA);
                db.AddParameter("@RECO_VENTA", DbType.String, ParameterDirection.Input, filtro.Recoger);
                db.AddParameter("@CLAV_USUARIO", DbType.String, ParameterDirection.Input, filtro.Clav_Usuario);
                db.AddParameter("@DNI", DbType.String, ParameterDirection.Input, filtro.Dni);
                db.AddParameter("@EDAD", DbType.Int16, ParameterDirection.Input, filtro.EDAD);
                db.AddParameter("@TELEFONO", DbType.String, ParameterDirection.Input, filtro.TELEF);
                db.AddParameter("@NOMBRE", DbType.String, ParameterDirection.Input, filtro.NOMB);
                db.AddParameter("@CODI_ESCA", DbType.String, ParameterDirection.Input, filtro.Codi_Esca);
                db.AddParameter("@Punto_Venta", DbType.String, ParameterDirection.Input, filtro.Punto_Venta);
                db.AddParameter("@TIPO_DOC", DbType.String, ParameterDirection.Input, filtro.tipo_doc);
                db.AddParameter("@cod_origen", DbType.String, ParameterDirection.Input, filtro.codi_ori_psj);
                db.AddParameter("@tipo", DbType.String, ParameterDirection.Input, filtro.Tipo);
                db.AddParameter("@PER_AUTORIZA", DbType.String, ParameterDirection.Input, filtro.per_autoriza);
                db.AddParameter("@ESTADO_ASIENTO", DbType.String, ParameterDirection.Input, filtro.estado_asiento);
                db.AddParameter("@SEXO", DbType.String, ParameterDirection.Input, filtro.SEXO);
                db.AddParameter("@Tipo_Pago", DbType.String, ParameterDirection.Input, filtro.Tipo_Pago);
                db.AddParameter("@SUC_VENTA", DbType.Int16, ParameterDirection.Input, filtro.CODI_SUCURSAL);
                db.AddParameter("@Vale_Remoto", DbType.String, ParameterDirection.Input, filtro.Vale_Remoto);
                db.AddParameter("@TIPO_V", DbType.String, ParameterDirection.Input, filtro.Tipo_Venta);
                db.AddParameter("@FECHA_VIAJE", DbType.String, ParameterDirection.Input, filtro.Fecha_viaje);
                db.AddParameter("@HORA_V", DbType.String, ParameterDirection.Input, filtro.HORA_V);
                db.AddParameter("@na", DbType.String, ParameterDirection.Input, filtro.nacionalidad);
                db.AddParameter("@servicio", DbType.Int16, ParameterDirection.Input, filtro.servicio);
                db.AddParameter("@porcentaje", DbType.Decimal, ParameterDirection.Input, filtro.porcentaje);//str
                db.AddParameter("@tota_ruta1", DbType.Decimal, ParameterDirection.Input, filtro.tota_ruta1);//str
                db.AddParameter("@tota_ruta2", DbType.Decimal, ParameterDirection.Input, filtro.tota_ruta2);//str
                db.AddParameter("@sube_en", DbType.Int16, ParameterDirection.Input, filtro.Sube_en);
                db.AddParameter("@baja_en", DbType.Int16, ParameterDirection.Input, filtro.Baja_en);
                db.AddParameter("@hora_em", DbType.String, ParameterDirection.Input, filtro.Hora_Emb);
                db.AddParameter("@nivel", DbType.Int16, ParameterDirection.Input, filtro.nivel);
                db.AddParameter("@numero_C", DbType.String, ParameterDirection.Input, filtro.NUME_CORRELATIVO__);
                db.AddParameter("@pventa_C", DbType.String, ParameterDirection.Input, (filtro.Pventa__).PadLeft(3,'0'));
                db.AddParameter("@codi_Empresa_C", DbType.String, ParameterDirection.Input, filtro.Codi_Empresa__);
                db.AddParameter("@terminal_C", DbType.String, ParameterDirection.Input, filtro.CODI_TERMINAL__);
                db.AddParameter("@doc_C", DbType.String, ParameterDirection.Input, filtro.Codi_Documento__);
                db.AddParameter("@SERIE_C", DbType.String, ParameterDirection.Input, filtro.SERIE_BOLETO__);
                db.AddParameter("@TIPO_TRANS", DbType.String, ParameterDirection.Input, filtro.Sw_IngManual);
                db.AddParameter("@idContrato_cr", DbType.String, ParameterDirection.Input, "0");
                db.AddParameter("@solicitud_cr", DbType.String, ParameterDirection.Input, "0");
                db.AddParameter("@idarea_cr", DbType.String, ParameterDirection.Input, "0");
                db.AddParameter("@fechaViaje_cr", DbType.String, ParameterDirection.Input, "01/01/1900");
                db.AddParameter("@precio_cr", DbType.Decimal, ParameterDirection.Input, "0");
                db.AddParameter("@Condicion_cr", DbType.String, ParameterDirection.Input, "0");
                db.AddParameter("@idtabla_cr", DbType.Int32, ParameterDirection.Input, "0");
                db.AddParameter("@HoraViaje_cr", DbType.String, ParameterDirection.Input, "0");
                db.AddParameter("@Flg_Ida_cr", DbType.String, ParameterDirection.Input, "0");
                db.AddParameter("@Fecha_Cita", DbType.String, ParameterDirection.Input, "01/01/1900");
                db.AddParameter("@Id_hospital", DbType.String, ParameterDirection.Input, "0");
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        response = Reader.GetIntValue(drlector, "ID_VENTA");
                    }
                }
            }
            return response;
        }
    }
}