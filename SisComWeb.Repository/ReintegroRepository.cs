using SisComWeb.Entity;
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
                db.ProcedureName = "Usp_Tb_Venta_Conulta_F12_Elec";
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
    }
}
