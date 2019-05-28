using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class VentaRepository
    {
        #region Métodos No Transaccionales

        public static TerminalElectronicoEntity ValidarTerminalElectronico(byte CodiEmpresa, short CodiSucursal, short CodiPuntoVenta, short CodiTerminal)
        {
            var entidad = new TerminalElectronicoEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarTerminalElectronico";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Codi_Terminal", DbType.Int16, ParameterDirection.Input, CodiTerminal);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Tipo = Reader.GetStringValue(drlector, "Tipo");
                        entidad.Imp = Reader.GetStringValue(drlector, "Imp");
                    }
                }
            }

            return entidad;
        }

        public static CorrelativoEntity BuscarCorrelativo(byte CodiEmpresa, string CodiDocumento, short CodiSucursal, short CodiPuntoVenta, string CodiTerminal, string Tipo)
        {
            var entidad = new CorrelativoEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarCorrelativo";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Documento", DbType.String, ParameterDirection.Input, CodiDocumento);
                db.AddParameter("@Codi_Sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_PuntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Terminal", DbType.String, ParameterDirection.Input, CodiTerminal);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, Tipo);
                
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.SerieBoleto = Reader.GetSmallIntValue(drlector, "Serie");
                        entidad.NumeBoleto = Reader.GetIntValue(drlector, "Numero");
                    }
                }
            }

            return entidad;
        }

        public static int ValidarLiquidacionVentas(short CodiUsuario, string Fecha)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ValidarLiquidacionVentas";
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, CodiUsuario);
                db.AddParameter("@Fecha", DbType.String, ParameterDirection.Input, Fecha);
                
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "NRO_LIQ");
                    }
                }
            }

            return valor;
        }

        public static EmpresaEntity BuscarEmpresaEmisor(byte CodiEmpresa)
        {
            var entidad = new EmpresaEntity
            {
                Ruc = string.Empty,
                RazonSocial = string.Empty,
                Direccion = string.Empty,
                Electronico = string.Empty,
                Contingencia = string.Empty
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarEmpresaEmisor";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Ruc = Reader.GetStringValue(drlector, "Ruc") ?? string.Empty;
                        entidad.RazonSocial = Reader.GetStringValue(drlector, "Razon_Social") ?? string.Empty;
                        entidad.Direccion = Reader.GetStringValue(drlector, "DIRECCION") ?? string.Empty;
                        entidad.Electronico = Reader.GetStringValue(drlector, "electronico") ?? string.Empty;
                        entidad.Contingencia = Reader.GetStringValue(drlector, "contingencia") ?? string.Empty;
                    }
                }
            }

            return entidad;
        }

        public static AgenciaEntity BuscarAgenciaEmpresa(byte CodiEmpresa, int CodiSucursal)
        {
            var entidad = new AgenciaEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarAgenciaEmpresa";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int32, ParameterDirection.Input, CodiSucursal);

                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Direccion = Reader.GetStringValue(drlector, "direccion");
                        entidad.Telefono1 = Reader.GetStringValue(drlector, "telefono1");
                        entidad.Telefono2 = Reader.GetStringValue(drlector, "telefono2");
                    }
                }
            }

            return entidad;
        }

        public static List<BeneficiarioEntity> ListaBeneficiarios(string CodiSocio)
        {
            var Lista = new List<BeneficiarioEntity>();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscaBeneficiariosPases";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        Lista.Add(new BeneficiarioEntity
                        {
                            NombreBeneficiario = Reader.GetStringValue(drlector, "Nombre_Beneficiario") ?? string.Empty,
                            TipoDocumento = Reader.GetStringValue(drlector, "Tipo_Documento") ?? string.Empty,
                            Documento = Reader.GetStringValue(drlector, "Documento") ?? string.Empty,
                            NumeroDocumento = Reader.GetStringValue(drlector, "Numero_Documento") ?? string.Empty,
                            Sexo = Reader.GetStringValue(drlector, "Sexo") ?? string.Empty
                        });
                    }
                }
            }

            return Lista;
        }

        public static BoletosCortesiaEntity ObjetoBoletosCortesia(string CodiSocio, string Anno, string Mes)
        {
            var entidad = new BoletosCortesiaEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ListarSaldoBoletosCortesia";
                db.AddParameter("@Codi_Socio", DbType.String, ParameterDirection.Input, CodiSocio);
                db.AddParameter("@Anno", DbType.String, ParameterDirection.Input, Anno);
                db.AddParameter("@Mes", DbType.String, ParameterDirection.Input, Mes);
                entidad = new BoletosCortesiaEntity();
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.TotalBoletos = Reader.GetDecimalValue(drlector, "Total_Boletos");
                        entidad.BoletosLibres = Reader.GetDecimalValue(drlector, "Boletos_Libres");
                        entidad.BoletosPrecio = Reader.GetDecimalValue(drlector, "Boletos_Precio");
                    }
                }
            }

            return entidad;
        }

        public static VentaBeneficiarioEntity BuscarVentaxBoleto(string Tipo, short Serie, int Numero, short CodiEmpresa)
        {
            var entidad = new VentaBeneficiarioEntity()
            {
                IdVenta = 0,
                NombresConcat = string.Empty,
                CodiOrigen = 0,
                NombOrigen = string.Empty,
                CodiDestino = 0,
                NombDestino = string.Empty,
                NombServicio = string.Empty,
                FechViaje = string.Empty,
                HoraViaje = string.Empty,
                NumeAsiento = 0
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarVentaxBoleto";
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, Tipo);
                db.AddParameter("@Serie_Boleto", DbType.String, ParameterDirection.Input, Serie);
                db.AddParameter("@Nume_Boleto", DbType.String, ParameterDirection.Input, Numero);
                db.AddParameter("@Codi_Empresa", DbType.String, ParameterDirection.Input, CodiEmpresa);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {

                        entidad.IdVenta = Reader.GetBigIntValue(drlector, "Id_Venta");
                        entidad.NombresConcat = Reader.GetStringValue(drlector, "NOMBRE") ?? string.Empty;
                        entidad.CodiOrigen = Reader.GetIntValue(drlector, "Codi_Origen");
                        entidad.NombOrigen = Reader.GetStringValue(drlector, "Nom_Origen") ?? string.Empty;
                        entidad.CodiDestino = Reader.GetIntValue(drlector, "Codi_Destino");
                        entidad.NombDestino = Reader.GetStringValue(drlector, "Nom_Destino") ?? string.Empty;
                        entidad.NombServicio = Reader.GetStringValue(drlector, "Nom_Servicio") ?? string.Empty;
                        entidad.FechViaje = Reader.GetDateStringValue(drlector, "Fecha_Viaje");
                        entidad.HoraViaje = Reader.GetStringValue(drlector, "Hora_Viaje") ?? string.Empty;
                        entidad.NumeAsiento = Reader.GetSmallIntValue(drlector, "NUME_ASIENTO");
                    }
                }
            }

            return entidad;
        }

        public static VentaEntity BuscarVentaById(int IdVenta)
        {
            var entidad = new VentaEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarVentaxId";
                db.AddParameter("@Id_venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.CodiEmpresa = Reader.GetByteValue(drlector, "CODI_EMPRESA");
                        entidad.PrecioVenta = Reader.GetDecimalValue(drlector, "Precio_Venta");
                        entidad.CodiRuta = Reader.GetSmallIntValue(drlector, "Codi_ruta");
                        entidad.FechaViaje = Reader.GetDateStringValue(drlector, "Fecha_Viaje");
                        entidad.SerieBoleto = Reader.GetSmallIntValue(drlector, "SERIE_BOLETO");
                        entidad.NumeBoleto = Reader.GetIntValue(drlector, "NUME_BOLETO");
                        entidad.FechaVenta = Reader.GetDateStringValue(drlector, "Fecha_Venta");
                        entidad.Tipo = Reader.GetStringValue(drlector, "Tipo");
                        entidad.IdPrecio = Reader.GetIntValue(drlector, "idtabla");
                    }
                }
            }

            return entidad;
        }

        public static PolizaEntity ConsultaPoliza(int CodiEmpresa, string CodiBus)
        {
            var entidad = new PolizaEntity
            {
                NroPoliza = string.Empty,
                FechaReg = string.Empty,
                FechaVen = string.Empty
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Bus_Poliza_consulta";
                db.AddParameter("@codi_Empresa", DbType.Int32, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@codi_Bus", DbType.String, ParameterDirection.Input, CodiBus);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.NroPoliza = Reader.GetStringValue(drlector, "Nro_Poliza") ?? string.Empty;
                        entidad.FechaReg = Reader.GetStringValue(drlector, "Fecha_Reg") ?? string.Empty;
                        entidad.FechaVen = Reader.GetStringValue(drlector, "Fecha_Ven") ?? string.Empty;
                    }
                }
            }

            return entidad;
        }

        #endregion

        #region Métodos Transaccionales

        public static int GrabarVenta(VentaEntity entidad)
        {
            var valor = new int();

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
                db.AddParameter("@Reco_Venta", DbType.String, ParameterDirection.Input, entidad.Concepto ?? "");

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

        public static bool ActualizarLiquidacionVentas(int NroLiq, string Hora)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ActualizarLiquidacionVentas";
                db.AddParameter("@NRO_LIQ", DbType.Int32, ParameterDirection.Input, NroLiq);
                db.AddParameter("@Hora", DbType.String, ParameterDirection.Input, Hora);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static string GenerarCorrelativoAuxiliar(string Tabla, string CodiOficina, string CodiPuntoVenta, string Correlativo)
        {
            var valor = string.Empty;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GenerarCorrelativoAuxiliar";
                db.AddParameter("@Tabla", DbType.String, ParameterDirection.Input, Tabla);
                db.AddParameter("@Oficina", DbType.String, ParameterDirection.Input, CodiOficina);
                db.AddParameter("@Flag_Venta", DbType.String, ParameterDirection.Input, CodiPuntoVenta);
                db.AddParameter("@Correlativo", DbType.String, ParameterDirection.Output, Correlativo);

                db.Execute();

                valor = db.GetParameter("@Correlativo").ToString();
            }

            return valor;
        }

        public static bool GrabarLiquidacionVentas(int NroLiq, byte CodiEmpresa, short CodiSucursal, short CodiPuntoVenta, short CodiUsuario, decimal ImpTur)
        {
            var valor = new bool();

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

                valor = true;
            }

            return valor;
        }

        public static bool GrabarAuditoria(AuditoriaEntity entidad)
        {
            var valor = new bool();

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

                valor = true;
            }

            return valor;
        }

        public static bool GrabarProgramacion(ProgramacionEntity entidad)
        {
            var valor = new bool();

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

                valor = true;
            }

            return valor;
        }

        public static bool GrabarViajeProgramacion(int NroViaje, int CodiProgramacion, string FechaProgramacion, string CodiBus)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarViajeProgramacion";
                db.AddParameter("@Nro_Viaje", DbType.Int32, ParameterDirection.Input, NroViaje);
                db.AddParameter("@Codi_programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Fecha_Programacion", DbType.String, ParameterDirection.Input, FechaProgramacion);
                db.AddParameter("@Codi_Bus", DbType.String, ParameterDirection.Input, CodiBus);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static int GrabarCaja(CajaEntity entidad)
        {
            var valor = new int();

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

                valor = int.Parse(db.GetParameter("@IdCaja").ToString());
            }

            return valor;
        }

        public static bool GrabarPagoTarjetaCredito(TarjetaCreditoEntity entidad)
        {
            var valor = new bool();

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

                valor = true;
            }

            return valor;
        }

        public static bool GrabarPagoDelivery(int IdVenta, string CodiZona, string Direccion, string Observacion)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_GrabarPagoDelivery";
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@Codi_Zona", DbType.String, ParameterDirection.Input, CodiZona);
                db.AddParameter("@Direccion", DbType.String, ParameterDirection.Input, Direccion);
                db.AddParameter("@Observacion", DbType.String, ParameterDirection.Input, Observacion);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool GrabarAcompanianteVenta(int IdVenta, AcompanianteEntity entidad)
        {
            var valor = new bool();

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

                valor = true;
            }

            return valor;
        }

        public static byte AnularVenta(int IdVenta, int CodiUsuario)
        {
            var valor = new byte();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_AnularVenta";
                db.AddParameter("@Id_Venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@Codi_Usuario", DbType.Int16, ParameterDirection.Input, CodiUsuario);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetByteValue(drlector, "Validator");
                        break;
                    }
                }
            }

            return valor;
        }

        public static bool PostergarVenta(int IdVenta, int CodiProgramacion, int NumeAsiento, int CodiServicio, string FechaViaje, string HoraViaje)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_PostergarVenta";
                db.AddParameter("@Id_Venta", DbType.String, ParameterDirection.Input, IdVenta);
                db.AddParameter("@Codi_programacion", DbType.String, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Nume_Asiento", DbType.String, ParameterDirection.Input, NumeAsiento);
                db.AddParameter("@Codi_Servicio", DbType.String, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Fecha_Viaje", DbType.String, ParameterDirection.Input, FechaViaje);
                db.AddParameter("@Hora_Viaje", DbType.String, ParameterDirection.Input, HoraViaje);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static byte EliminarReserva(int IdVenta)
        {
            var valor = new byte();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_EliminarReserva";
                db.AddParameter("@IdVenta", DbType.Int32, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetByteValue(drlector, "Validator");
                        break;
                    }
                }
            }

            return valor;
        }

        public static byte ModificarVentaAFechaAbierta(int IdVenta, int CodiServicio, int CodiRuta)
        {
            var valor = new byte();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_ModificarVentaAFechaAbierta";
                db.AddParameter("@Id_Venta", DbType.String, ParameterDirection.Input, IdVenta);
                db.AddParameter("@Codi_Servicio", DbType.String, ParameterDirection.Input, CodiServicio);
                db.AddParameter("@Codi_Ruta", DbType.String, ParameterDirection.Input, CodiRuta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetByteValue(drlector, "Validator");
                        break;
                    }
                }
            }

            return valor;
        }

        #endregion
    }
}
