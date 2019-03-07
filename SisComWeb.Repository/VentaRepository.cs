using SisComWeb.Entity;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class VentaRepository
    {
        #region Métodos No Transaccionales

        public static Response<TerminalElectronicoEntity> ValidarTerminalElectronico(byte CodiEmpresa, short CodiSucursal, short CodiPuntoVenta, short CodiTerminal)
        {
            var response = new Response<TerminalElectronicoEntity>(false, null, "Error: ValidarTerminalElectronico.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarTerminalElectronico";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Codi_Terminal", DbType.Int16, ParameterDirection.Input, CodiTerminal);
                var entidad = new TerminalElectronicoEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Tipo = Reader.GetStringValue(drlector, "Tipo");
                        entidad.Imp = Reader.GetStringValue(drlector, "Imp");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: ValidarTerminalElectronico.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<CorrelativoEntity> BuscarCorrelativo(byte CodiEmpresa, string CodiDocumento, short CodiSucursal, short CodiPuntoVenta, string CodiTerminal, string Tipo)
        {
            var response = new Response<CorrelativoEntity>(false, null, "Error: BuscarCorrelativo.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarCorrelativo";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Documento", DbType.String, ParameterDirection.Input, CodiDocumento);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Terminal", DbType.String, ParameterDirection.Input, CodiTerminal);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, Tipo);
                var entidad = new CorrelativoEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.SerieBoleto = Reader.GetSmallIntValue(drlector, "Serie");
                        entidad.NumeBoleto = Reader.GetIntValue(drlector, "Numero");
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: BuscarCorrelativo.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<int> ValidarLiquidacionVentas(short CodiUsuario, string Fecha)
        {
            var response = new Response<int>(false, 0, "Error: ValidarLiquidacionVentas.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarLiquidacionVentas";
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, CodiUsuario);
                db.AddParameter("@Fecha", DbType.String, ParameterDirection.Input, Fecha);
                var Valor = new int();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Valor = Reader.GetIntValue(drlector, "NRO_LIQ");
                    }
                    response.EsCorrecto = true;
                    response.Valor = Valor;
                    response.Mensaje = "Correcto: ValidarLiquidacionVentas.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<string> BuscarRucEmpresa(byte CodiEmpresa)
        {
            var response = new Response<string>(false, null, "Error: BuscarRucEmpresa.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarRucEmpresa";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                var valor = string.Empty;
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "Ruc"); ;
                    }
                    response.EsCorrecto = true;
                    response.Valor = valor;
                    response.Mensaje = "Correcto: BuscarRucEmpresa.";
                    response.Estado = true;
                }
            }
            return response;
        }

        public static Response<List<BeneficiarioEntity>> ListaBeneficiarioPase(string Codi_Socio)
        {
            var response = new Response<List<BeneficiarioEntity>>(false, null, "Error: ListaBeneficiarioPase.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscaBeneficiariosPases";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, Codi_Socio);
                var entidad = new List<BeneficiarioEntity>();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Add(new BeneficiarioEntity
                        {
                            Nombre_Beneficiario = Reader.GetStringValue(drlector, "Nombre_Beneficiario"),
                            Tipo_Documento = Reader.GetStringValue(drlector, "Tipo_Documento"),
                            Documento = Reader.GetStringValue(drlector, "Documento"),
                            Numero_Documento = Reader.GetStringValue(drlector, "Numero_Documento"),
                            Sexo = Reader.GetStringValue(drlector, "Sexo")
                        });
                    }
                    response.EsCorrecto = true;
                    response.Valor = entidad;
                    response.Mensaje = "Correcto: ListaBeneficiarioPase.";
                    response.Estado = true;
                }
            }
            return response;
        }

        #endregion

        #region Métodos Transaccionales

        public static Response<int> GrabarVenta(VentaEntity entidad)
        {
            var response = new Response<int>(false, 0, "Error: GrabarVenta.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarVenta";
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
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Output, entidad.IdVenta);

                db.Execute();

                int _outputIdVenta = int.Parse(db.GetParameter("@Id_Venta").ToString());

                response.EsCorrecto = true;
                response.Valor = _outputIdVenta;
                response.Mensaje = "Correcto: GrabarVenta.";
                response.Estado = true;
            }
            return response;
        }

        public static Response<bool> ActualizarLiquidacionVentas(int NroLiq, string Hora)
        {
            var response = new Response<bool>(false, false, "Error: ActualizarLiquidacionVentas.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ActualizarLiquidacionVentas";
                db.AddParameter("@NRO_LIQ", DbType.Int32, ParameterDirection.Input, NroLiq);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: ActualizarLiquidacionVentas.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<string> GenerarCorrelativoAuxiliar(string Tabla, string Oficina, string FlagVenta, string Correlativo)
        {
            var response = new Response<string>(false, null, "Error: GenerarCorrelativoAuxiliar.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GenerarCorrelativoAuxiliar";
                db.AddParameter("@Tabla", DbType.String, ParameterDirection.Input, Tabla);
                db.AddParameter("@Oficina", DbType.String, ParameterDirection.Input, Oficina);
                db.AddParameter("@Flag_Venta", DbType.String, ParameterDirection.Input, FlagVenta);
                db.AddParameter("@Correlativo", DbType.String, ParameterDirection.Output, Correlativo);

                db.Execute();

                string _outputCorrelativo = db.GetParameter("@Correlativo").ToString();

                response.EsCorrecto = true;
                response.Valor = _outputCorrelativo;
                response.Mensaje = "Correcto: GenerarCorrelativoAuxiliar.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> GrabarLiquidacionVentas(int NroLiq, byte CodiEmpresa, short CodiSucursal, short CodiPuntoVenta, short CodiUsuario, decimal ImpTur)
        {
            var response = new Response<bool>(false, false, "Error: GrabarLiquidacionVentas.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarLiquidacionVentas";
                db.AddParameter("@NRO_LIQ", DbType.Int32, ParameterDirection.Input, NroLiq);
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, CodiUsuario);
                db.AddParameter("@IMP_TUR", DbType.Decimal, ParameterDirection.Input, ImpTur);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarLiquidacionVentas.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> GrabarAuditoria(AuditoriaEntity entidad)
        {
            var response = new Response<bool>(false, false, "Error: GrabarAuditoria.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarAuditoria";
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

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarAuditoria.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> GrabarProgramacion(ProgramacionEntity entidad)
        {
            var response = new Response<bool>(false, false, "Error: GrabarProgramacion.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarProgramacion";
                db.AddParameter("@Codi_programacion", DbType.Int32, ParameterDirection.Input, entidad.CodiProgramacion);
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, entidad.CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, entidad.CodiSucursal);
                db.AddParameter("@Codi_Ruta", DbType.Int16, ParameterDirection.Input, entidad.CodiRuta);
                db.AddParameter("@Codi_bus", DbType.String, ParameterDirection.Input, entidad.CodiBus);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, entidad.FechaProgramacion);
                db.AddParameter("@Hora_Programacion", DbType.String, ParameterDirection.Input, entidad.HoraProgramacion);
                db.AddParameter("@Codi_Servicio", DbType.Byte, ParameterDirection.Input, entidad.CodiServicio);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarProgramacion.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> GrabarViajeProgramacion(int NroViaje, int CodiProgramacion, string FechaProgramacion, string CodiBus)
        {
            var response = new Response<bool>(false, false, "Error: GrabarViajeProgramacion.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarViajeProgramacion";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                db.AddParameter("@Codi_Bus", DbType.String, ParameterDirection.Input, CodiBus);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarViajeProgramacion.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<int> GrabarCaja(CajaEntity entidad)
        {
            var response = new Response<int>(false, 0, "Error: GrabarCaja.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarCaja";
                db.AddParameter("@Nume_Caja", DbType.String, ParameterDirection.Input, entidad.NumeCaja);
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, entidad.CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, entidad.CodiSucursal);
                db.AddParameter("@Boleto", DbType.String, ParameterDirection.Input, entidad.Boleto);
                db.AddParameter("@Monto", DbType.Decimal, ParameterDirection.Input, entidad.Monto);
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, entidad.CodiUsuario);
                db.AddParameter("@Recibe", DbType.String, ParameterDirection.Input, entidad.Recibe);
                db.AddParameter("@Codi_Destino", DbType.String, ParameterDirection.Input, entidad.CodiDestino);
                db.AddParameter("@Fecha_Viaje", DbType.String, ParameterDirection.Input, entidad.FechaViaje);
                db.AddParameter("@Hora_Viaje", DbType.String, ParameterDirection.Input, entidad.HoraViaje);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, entidad.CodiPuntoVenta);
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, entidad.IdVenta);
                db.AddParameter("@Origen", DbType.String, ParameterDirection.Input, entidad.Origen);
                db.AddParameter("@Modulo", DbType.String, ParameterDirection.Input, entidad.Modulo);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, entidad.Tipo);
                db.AddParameter("@IdCaja", DbType.Int32, ParameterDirection.Output, entidad.IdCaja);

                db.Execute();

                int _outputIdCaja = int.Parse(db.GetParameter("@IdCaja").ToString());

                response.EsCorrecto = true;
                response.Valor = _outputIdCaja;
                response.Mensaje = "Correcto: GrabarCaja.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> GrabarPagoTarjetaCredito(TarjetaCreditoEntity entidad)
        {
            var response = new Response<bool>(false, false, "Error: GrabarPagoTarjetaCredito.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarPagoTarjetaCredito";
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, entidad.IdVenta);
                db.AddParameter("@Boleto", DbType.String, ParameterDirection.Input, entidad.Boleto);
                db.AddParameter("@Codi_TarjetaCredito", DbType.String, ParameterDirection.Input, entidad.CodiTarjetaCredito);
                db.AddParameter("@Nume_TarjetaCredito", DbType.String, ParameterDirection.Input, entidad.NumeTarjetaCredito);
                db.AddParameter("@Vale", DbType.String, ParameterDirection.Input, entidad.Vale);
                db.AddParameter("@IdCaja", DbType.Int32, ParameterDirection.Input, entidad.IdCaja);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, entidad.Tipo);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarPagoTarjetaCredito.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> GrabarPagoDelivery(int IdVenta, string CodiZona, string Direccion, string Observacion)
        {
            var response = new Response<bool>(false, false, "Error: GrabarPagoDelivery.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarPagoDelivery";
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@Codi_Zona", DbType.String, ParameterDirection.Input, CodiZona);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, Direccion);
                db.AddParameter("@Observacion", DbType.String, ParameterDirection.Input, Observacion);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarPagoDelivery.";
                response.Estado = true;
            }

            return response;
        }

        public static Response<bool> GrabarAcompañanteVenta(int IdVenta, AcompañanteEntity entidad)
        {
            var response = new Response<bool>(false, false, "Error: GrabarAcompañanteVenta.", false);
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarAcompañanteVenta";
                db.AddParameter("@IDVENTA", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@TIPO_DOC", DbType.String, ParameterDirection.Input, entidad.TipoDocumento);
                db.AddParameter("@DNI", DbType.String, ParameterDirection.Input, entidad.NumeroDocumento);
                db.AddParameter("@NOMBRE", DbType.String, ParameterDirection.Input, entidad.NombreCompleto);
                db.AddParameter("@FECHAN", DbType.String, ParameterDirection.Input, entidad.FechaNacimiento);
                db.AddParameter("@EDAD", DbType.String, ParameterDirection.Input, entidad.Edad);
                db.AddParameter("@SEXO", DbType.String, ParameterDirection.Input, entidad.Sexo);
                db.AddParameter("@PARENTESCO", DbType.String, ParameterDirection.Input, entidad.Parentesco);

                db.Execute();

                response.EsCorrecto = true;
                response.Valor = true;
                response.Mensaje = "Correcto: GrabarAcompañanteVenta.";
                response.Estado = true;
            }

            return response;
        }

        #endregion
    }
}
