using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class PaseRepository
    {
        #region Métodos No Transaccionales

        public static decimal ValidarSaldoPaseCortesia(string CodiSocio, string Mes, string Anno)
        {
            var valor = new decimal();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarSaldoPaseCortesia";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Mes", DbType.String, ParameterDirection.Input, Mes);
                db.AddParameter("@Anno", DbType.String, ParameterDirection.Input, Anno);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetDecimalValue(drlector, "saldo");
                    }
                }
            }

            return valor;
        }

        #endregion

        #region Métodos Transaccionales

        public static int GrabarVentaFechaAbierta(VentaEntity entidad)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarVentaFechaAbierta";
                db.AddParameter("@Serie_Boleto", DbType.Int16, ParameterDirection.Input, entidad.SerieBoleto);
                db.AddParameter("@Nume_Boleto", DbType.Int32, ParameterDirection.Input, entidad.NumeBoleto);
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, entidad.CodiEmpresa);
                db.AddParameter("@Codi_Oficina", DbType.Int16, ParameterDirection.Input, entidad.CodiOficina);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, entidad.CodiPuntoVenta);
                db.AddParameter("@Codi_Origen", DbType.Int16, ParameterDirection.Input, entidad.CodiOrigen);
                db.AddParameter("@Codi_Destino", DbType.Int16, ParameterDirection.Input, entidad.CodiDestino);
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, entidad.CodiProgramacion);
                db.AddParameter("@Ruc_Cliente", DbType.String, ParameterDirection.Input, entidad.RucCliente);
                db.AddParameter("@Nume_Asiento", DbType.Byte, ParameterDirection.Input, entidad.NumeAsiento);
                db.AddParameter("@Flag_Venta", DbType.String, ParameterDirection.Input, entidad.FlagVenta);
                db.AddParameter("@Precio_Venta", DbType.Decimal, ParameterDirection.Input, entidad.PrecioVenta);
                db.AddParameter("@Nombre", DbType.String, ParameterDirection.Input, entidad.Nombre);
                db.AddParameter("@Edad", DbType.Byte, ParameterDirection.Input, entidad.Edad);
                db.AddParameter("@Telefono", DbType.String, ParameterDirection.Input, entidad.Telefono);
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, entidad.CodiUsuario);
                db.AddParameter("@Dni", DbType.String, ParameterDirection.Input, entidad.Dni);
                db.AddParameter("@Tipo_Documento", DbType.String, ParameterDirection.Input, entidad.TipoDocumento);
                db.AddParameter("@Codi_Documento", DbType.String, ParameterDirection.Input, entidad.AuxCodigoBF_Interno);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, entidad.Tipo);
                db.AddParameter("@Sexo", DbType.String, ParameterDirection.Input, entidad.Sexo);
                db.AddParameter("@Tipo_Pago", DbType.String, ParameterDirection.Input, entidad.TipoPago);
                db.AddParameter("@Fecha_Viaje", DbType.String, ParameterDirection.Input, entidad.FechaViaje);
                db.AddParameter("@Hora_Viaje", DbType.String, ParameterDirection.Input, entidad.HoraViaje);
                db.AddParameter("@Nacionalidad", DbType.String, ParameterDirection.Input, entidad.Nacionalidad);
                db.AddParameter("@Codi_Servicio", DbType.Byte, ParameterDirection.Input, entidad.CodiServicio);
                db.AddParameter("@Codi_Embarque", DbType.Int16, ParameterDirection.Input, entidad.CodiEmbarque);
                db.AddParameter("@Codi_Arribo", DbType.Int16, ParameterDirection.Input, entidad.CodiArribo);
                db.AddParameter("@Hora_Embarque", DbType.String, ParameterDirection.Input, entidad.HoraEmbarque);
                db.AddParameter("@Nivel_Asiento", DbType.Byte, ParameterDirection.Input, entidad.NivelAsiento);
                db.AddParameter("@Codi_Terminal", DbType.Int16, ParameterDirection.Input, entidad.CodiTerminal);
                db.AddParameter("@Credito", DbType.Decimal, ParameterDirection.Input, entidad.Credito);
                db.AddParameter("@Reco_Venta", DbType.String, ParameterDirection.Input, entidad.Concepto);
                db.AddParameter("@Codi_Ruta", DbType.String, ParameterDirection.Input, entidad.CodiRuta);

                db.AddParameter("@IdContrato", DbType.Int32, ParameterDirection.Input, entidad.IdContrato);
                db.AddParameter("@NroSolicitud", DbType.String, ParameterDirection.Input, entidad.NroSolicitud ?? "");
                db.AddParameter("@IdAreaContrato", DbType.Int32, ParameterDirection.Input, entidad.IdArea);
                db.AddParameter("@Flg_Ida", DbType.String, ParameterDirection.Input, entidad.FlgIda ?? "");
                db.AddParameter("@Fecha_Cita", DbType.String, ParameterDirection.Input, entidad.FechaCita ?? "");
                db.AddParameter("@Id_hospital", DbType.Int32, ParameterDirection.Input, entidad.IdHospital);
                db.AddParameter("@IdTabla", DbType.Int32, ParameterDirection.Input, entidad.IdPrecio);

                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Output, entidad.IdVenta);

                db.Execute();

                valor = int.Parse(db.GetParameter("@Id_Venta").ToString());
            }

            return valor;
        }

        public static bool ModificarSaldoPaseCortesia(string CodiSocio, string Mes, string Anno)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarSaldoPaseCortesia";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Mes", DbType.String, ParameterDirection.Input, Mes);
                db.AddParameter("@Anno", DbType.String, ParameterDirection.Input, Anno);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool GrabarPaseSocio(int IdVenta, string CodiGerente, string CodiSocio, string Observacion)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarPaseSocio";
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@Codi_Gerente", DbType.String, ParameterDirection.Input, CodiGerente);
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Observacion", DbType.String, ParameterDirection.Input, Observacion);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        #endregion
    }
}
