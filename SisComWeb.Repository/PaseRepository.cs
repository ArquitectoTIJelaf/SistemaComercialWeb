using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class PaseRepository
    {
        #region Métodos No Transaccionales

        public static Response<int> ValidarSaldoPaseCortesia(string CodiSocio, string Mes, string Año)
        {
            var response = new Response<int>(false, 0, "Error: ValidarSaldoPaseCortesia.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarSaldoPaseCortesia";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Mes", DbType.String, ParameterDirection.Input, Mes);
                db.AddParameter("@Anno", DbType.String, ParameterDirection.Input, Año);
                var Valor = new int();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Valor = 1;
                        break;
                    }
                    response.EsCorrecto = true;
                    response.Valor = Valor;
                    response.Mensaje = "Correcto: ValidarSaldoPaseCortesia.";
                    response.Estado = true;
                }
            }
            return response;
        }        

        #endregion

        #region Métodos Transaccionales

        public static Response<int> GrabarVentaFechaAbierta(PaseEntity entidad)
        {
            var response = new Response<int>(false, 0, "Error: GrabarVentaFechaAbierta.", false);
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
                db.AddParameter("@Codi_Documento", DbType.String, ParameterDirection.Input, entidad.CodiDocumento);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, entidad.Tipo);
                db.AddParameter("@Sexo", DbType.String, ParameterDirection.Input, entidad.Sexo);
                db.AddParameter("@Tipo_Pago", DbType.String, ParameterDirection.Input, entidad.TipoPago);
                db.AddParameter("@Fecha_Viaje", DbType.String, ParameterDirection.Input, entidad.FechaViaje);
                db.AddParameter("@Hora_Viaje", DbType.String, ParameterDirection.Input, entidad.HoraViaje);
                db.AddParameter("@Nacionalidad", DbType.String, ParameterDirection.Input, entidad.Nacionalidad);
                db.AddParameter("@Codi_Servicio", DbType.Byte, ParameterDirection.Input, entidad.CodiServicio);
                db.AddParameter("@Codi_Embarque", DbType.Int16, ParameterDirection.Input, entidad.CodiEmbarque);
                db.AddParameter("@Codi_Arribo", DbType.Int16, ParameterDirection.Input, entidad.CodiArribo);
                db.AddParameter("@Hora_Embarque", DbType.String, ParameterDirection.Input, entidad.Hora_Embarque);
                db.AddParameter("@Nivel_Asiento", DbType.Byte, ParameterDirection.Input, entidad.NivelAsiento);
                db.AddParameter("@Codi_Terminal", DbType.Int16, ParameterDirection.Input, entidad.CodiTerminal);
                db.AddParameter("@Credito", DbType.Decimal, ParameterDirection.Input, entidad.Credito);
                db.AddParameter("@Reco_Venta", DbType.String, ParameterDirection.Input, entidad.RecoVenta);
                db.AddParameter("@Codi_Ruta", DbType.String, ParameterDirection.Input, entidad.CodiRuta);
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Output, entidad.IdVenta);

                db.Execute();

                int _outputIdVenta = int.Parse(db.GetParameter("@Id_Venta").ToString());

                response.EsCorrecto = true;
                response.Valor = _outputIdVenta;
                response.Mensaje = "Correcto: GrabarVentaFechaAbierta.";
                response.Estado = true;
            }
            return response;
        }

        public static Response<bool> ModificarSaldoPaseCortesia(string CodiSocio, string Mes, string Año)
        {
            var response = new Response<bool>(false, false, "Error: ModificarSaldoPaseCortesia.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarSaldoPaseCortesia";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Mes", DbType.String, ParameterDirection.Input, Mes);
                db.AddParameter("@Anno", DbType.String, ParameterDirection.Input, Año);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: ModificarSaldoPaseCortesia.";
                response.Estado = true;
            }
            return response;
        }

        public static Response<bool> GrabarPaseSocio(int IdVenta, string CodiGerente, string CodiSocio, string Observacion)
        {
            var response = new Response<bool>(false, false, "Error: GrabarPaseSocio.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarPaseSocio";
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@Codi_Gerente", DbType.String, ParameterDirection.Input, CodiGerente);
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Observacion", DbType.String, ParameterDirection.Input, Observacion);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarPaseSocio.";
                response.Estado = true;
            }
            return response;
        }

        #endregion
    }
}
