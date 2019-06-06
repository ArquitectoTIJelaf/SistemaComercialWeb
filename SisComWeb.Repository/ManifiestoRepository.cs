using SisComWeb.Entity;
using System.Data;

namespace SisComWeb.Repository
{
    public static class ManifiestoRepository
    {
        #region Métodos Transaccionales

        public static bool ActualizarManifiestoProgramacion(int CodiProgramacion, short CodiSucursal, bool TipoApertura)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Manifiesto_Programacion_Update";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@TipoApertura", DbType.Boolean, ParameterDirection.Input, TipoApertura);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ActualizarProgramacion(byte CodiEmpresa, int CodiProgramacion, short CodiSucursal, bool TipoApertura)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Programacion_Update_Manifiesto";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@TipoApertura", DbType.Boolean, ParameterDirection.Input, TipoApertura);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool GrabarAuditoriaProg(AuditoriaEntity entidad)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarAuditoria_prog";
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, entidad.CodiUsuario);
                db.AddParameter("@Nom_Usuario", DbType.String, ParameterDirection.Input, entidad.NomUsuario);
                db.AddParameter("@Tabla", DbType.String, ParameterDirection.Input, entidad.Tabla);
                db.AddParameter("@Tipo_Movimiento", DbType.String, ParameterDirection.Input, entidad.TipoMovimiento);
                db.AddParameter("@Boleto", DbType.String, ParameterDirection.Input, entidad.Boleto);
                db.AddParameter("@Nume_Asiento", DbType.String, ParameterDirection.Input, entidad.NumeAsiento);
                db.AddParameter("@Nom_Oficina", DbType.String, ParameterDirection.Input, entidad.NomOficina);
                db.AddParameter("@Nom_PuntoVenta", DbType.String, ParameterDirection.Input, entidad.NomPuntoVenta);
                db.AddParameter("@Pasajero", DbType.String, ParameterDirection.Input, entidad.Pasajero);
                db.AddParameter("@Fecha_Viaje", DbType.String, ParameterDirection.Input, entidad.FechaViaje);
                db.AddParameter("@Hora_Viaje", DbType.String, ParameterDirection.Input, entidad.HoraViaje);
                db.AddParameter("@Nom_Destino", DbType.String, ParameterDirection.Input, entidad.NomDestino);
                db.AddParameter("@Precio", DbType.Decimal, ParameterDirection.Input, entidad.Precio);
                db.AddParameter("@Obs1", DbType.String, ParameterDirection.Input, entidad.Obs1);
                db.AddParameter("@Obs2", DbType.String, ParameterDirection.Input, entidad.Obs2);
                db.AddParameter("@Obs3", DbType.String, ParameterDirection.Input, entidad.Obs3);
                db.AddParameter("@Obs4", DbType.String, ParameterDirection.Input, entidad.Obs4);
                db.AddParameter("@Obs5", DbType.String, ParameterDirection.Input, entidad.Obs5);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ActualizarVentaManifiesto(int CodiProgramacion)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Manifiesto_Programacion_Update";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        #endregion
    }
}
