using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using System.Collections.Generic;
using System.Data;

namespace SisComWeb.Repository
{
    public static class VentaRepository
    {
        #region Métodos No Transaccionales

        public static TerminalElectronicoEntity ValidarTerminalElectronico(byte CodiEmpresa, short CodiSucursal, short CodiPuntoVenta, short CodiTerminal)
        {
            var entidad = new TerminalElectronicoEntity()
            {
                Tipo = "M",
                Imp = "3" // Por mientras se quemará el valor "3".
            };

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
                        break;
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
                        break;
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
                        break;
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
                        break;
                    }
                }
            }

            return entidad;
        }

        public static AgenciaEntity BuscarAgenciaEmpresa(byte CodiEmpresa, int CodiSucursal)
        {
            var entidad = new AgenciaEntity()
            {
                Direccion = string.Empty,
                Telefono1 = string.Empty,
                Telefono2 = string.Empty
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_BuscarAgenciaEmpresa";
                db.AddParameter("@Codi_Empresa", DbType.Byte, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@Codi_Sucursal", DbType.Int32, ParameterDirection.Input, CodiSucursal);

                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Direccion = Reader.GetStringValue(drlector, "direccion") ?? string.Empty;
                        entidad.Telefono1 = Reader.GetStringValue(drlector, "telefono1") ?? string.Empty;
                        entidad.Telefono2 = Reader.GetStringValue(drlector, "telefono2") ?? string.Empty;
                        break;
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
                        break;
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
                        break;
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
                        entidad.SerieBoleto = Reader.GetSmallIntValue(drlector, "SERIE_BOLETO");
                        entidad.NumeBoleto = Reader.GetIntValue(drlector, "NUME_BOLETO");
                        entidad.IdPrecio = Reader.GetIntValue(drlector, "idtabla");
                        entidad.PerAutoriza = Reader.GetStringValue(drlector, "per_autoriza");
                        entidad.FechaAnulacion = Reader.GetDateStringValue(drlector, "FECH_ANULACION");
                        entidad.CodiProgramacion = Reader.GetIntValue(drlector, "CODI_PROGRAMACION");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static string VerificaManifiestoPorPVenta(int CodiProgramacion, short Pvta)
        {
            var valor = "0";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_VerificarManiXPvta";
                db.AddParameter("@Codi_programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Pvta", DbType.Int16, ParameterDirection.Input, Pvta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = "1";
                        break;
                    }
                }
            }

            return valor;
        }

        public static string ConsultaConfigManifiestoPorHora(short CodiEmpresa, short CodiSucursal, short CodiPuntoVenta)
        {
            var valor = "0";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_ConfManiXHora";
                db.AddParameter("@codi_empresa", DbType.Int16, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@codi_sucursal", DbType.Int16, ParameterDirection.Input, CodiSucursal);
                db.AddParameter("@Codi_puntoVenta", DbType.Int16, ParameterDirection.Input, CodiPuntoVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "") ?? "0";
                        break;
                    }
                }
            }

            return valor;
        }

        public static PolizaEntity ConsultaNroPoliza(int CodiEmpresa, string CodiBus, string Fecha)
        {
            var entidad = new PolizaEntity()
            {
                NroPoliza = string.Empty,
                FechaReg = "01/01/1900",
                FechaVen = "01/01/1900"
            };

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_NroPoliza";
                db.AddParameter("@codi_Empresa", DbType.Int32, ParameterDirection.Input, CodiEmpresa);
                db.AddParameter("@codi_Bus", DbType.String, ParameterDirection.Input, CodiBus);
                db.AddParameter("@fecha", DbType.String, ParameterDirection.Input, Fecha);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.NroPoliza = Reader.GetStringValue(drlector, "Nro_Poliza") ?? "";
                        entidad.FechaReg = Reader.GetDateStringValue(drlector, "Fecha_Reg") ?? "01/01/1900";
                        entidad.FechaVen = Reader.GetDateStringValue(drlector, "Fecha_Ven") ?? "01/01/1900";
                        break;
                    }
                }
            }

            return entidad;
        }

        public static int VerificaNC(int IdVenta)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Venta_VeriNc";
                db.AddParameter("@ID_VENTA", DbType.Int32, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "ID_VENTA");
                        break;
                    }
                }
            }

            return valor;
        }

        public static decimal ConsultaControlTiempo(string tipo)
        {
            var valor = new decimal();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Control_Tiempo";
                db.AddParameter("@tipo", DbType.String, ParameterDirection.Input, tipo);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetDecimalValue(drlector, "tiempo");
                        break;
                    }
                }
            }

            return valor;
        }

        public static string ConsultaPanelNiveles(int codigo, int Nivel)
        {
            var valor = "0";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_panel_Niveles_Trae";
                db.AddParameter("@codigo", DbType.Int32, ParameterDirection.Input, codigo);
                db.AddParameter("@Nivel", DbType.Int32, ParameterDirection.Input, Nivel);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = "1";
                        break;
                    }
                }
            }

            return valor;
        }

        public static int VerificaLiquidacionComiDet(int IdVenta)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_liquidacionComi_Det_Verifica";
                db.AddParameter("@id_venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "Id_lquidaciondet");
                    }
                }
            }

            return valor;
        }

        public static string VerificaLiquidacionComi(int CodiProgramacion, int Pvta)
        {
            var valor = string.Empty;

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Cierre_LiquiComi_Verifica";
                db.AddParameter("@Codi_Programacion", DbType.Int32, ParameterDirection.Input, CodiProgramacion);
                db.AddParameter("@Pvta", DbType.Int32, ParameterDirection.Input, Pvta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "estado");
                    }
                }
            }

            return valor;
        }

        public static bool ConsultaVentaIdaV(int IdVenta)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_TB_VENTA_IDA_V_Consulta";
                db.AddParameter("@id_venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = true;
                    }
                }
            }

            return valor;
        }

        public static bool ConsultaClaveAnuRei(int CodiUsuario, string Clave)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_tb_Mantenimiento_ClaveAnuRei_Consulta";
                db.AddParameter("@Codi_Usuario", DbType.Int32, ParameterDirection.Input, CodiUsuario);
                db.AddParameter("@Clave", DbType.String, ParameterDirection.Input, Clave);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = true;
                    }
                }
            }

            return valor;
        }

        public static bool ConsultaClaveControl(short Usuario, string Pwd)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_tb_control_Pwd_Consulta";
                db.AddParameter("@USUARIO", DbType.Int16, ParameterDirection.Input, Usuario);
                db.AddParameter("@PWD", DbType.String, ParameterDirection.Input, Pwd);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = true;
                    }
                }
            }

            return valor;
        }

        public static BuscaEntity ConsultaF9Elec(int Serie, int Numero, string Tipo, int CodEmpresa)
        {
            var entidad = new BuscaEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_VentaConsultaF9Elec";
                db.AddParameter("@serie", DbType.Int32, ParameterDirection.Input, Serie);
                db.AddParameter("@numero", DbType.Int32, ParameterDirection.Input, Numero);
                db.AddParameter("@tipo", DbType.String, ParameterDirection.Input, Tipo);
                db.AddParameter("@empresa", DbType.Int32, ParameterDirection.Input, CodEmpresa);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.FechaProgramacion = Reader.GetDateStringValue(drlector, "Fech_programacion");
                        entidad.Tipo = Reader.GetStringValue(drlector, "tipo");
                        entidad.SerieBoleto = Reader.GetSmallIntValue(drlector, "SERIE_BOLETO");
                        entidad.NumeBoleto = Reader.GetIntValue(drlector, "NUME_BOLETO");
                        entidad.CodiEmpresa = Reader.GetByteValue(drlector, "CODI_EMPRESA");
                        entidad.CodiSucursal = Reader.GetSmallIntValue(drlector, "CODI_SUCURSAL");
                        entidad.CodiProgramacion = Reader.GetIntValue(drlector, "CODI_PROGRAMACION");
                        entidad.CodiSubruta = Reader.GetIntValue(drlector, "CODI_SUBRUTA");
                        entidad.CodiCliente = Reader.GetIntValue(drlector, "CODI_Cliente");
                        entidad.RucCliente = Reader.GetStringValue(drlector, "NIT_CLIENTE");
                        entidad.PrecioVenta = Reader.GetDecimalValue(drlector, "PREC_VENTA");
                        entidad.NumeAsiento = Reader.GetByteValue(drlector, "NUME_ASIENTO");
                        entidad.FlagVenta = Reader.GetStringValue(drlector, "FLAG_VENTA");
                        entidad.FechaVenta = Reader.GetDateStringValue(drlector, "FECH_VENTA");
                        entidad.RecoVenta = Reader.GetStringValue(drlector, "RECO_VENTA");
                        entidad.ClavUsuario = Reader.GetIntValue(drlector, "CLAV_USUARIO");
                        entidad.IndiAnulado = Reader.GetStringValue(drlector, "INDI_ANULADO");
                        entidad.FechaAnulacion = Reader.GetDateStringValue(drlector, "FECH_ANULACION");
                        entidad.Dni = Reader.GetStringValue(drlector, "DNI");
                        entidad.Edad = Reader.GetByteValue(drlector, "EDAD");
                        entidad.Telefono = Reader.GetStringValue(drlector, "TELEFONO");
                        entidad.Nombre = Reader.GetStringValue(drlector, "NOMBRE");
                        entidad.CodiEsca = Reader.GetStringValue(drlector, "CODI_ESCA");
                        entidad.CodiPuntoVenta = Reader.GetSmallIntValue(drlector, "Punto_Venta");
                        entidad.TipoDocumento = Reader.GetStringValue(drlector, "TIPO_DOC");
                        entidad.CodiOrigen = Reader.GetSmallIntValue(drlector, "cod_origen");
                        entidad.PerAutoriza = Reader.GetStringValue(drlector, "per_autoriza");
                        entidad.ClavUsuario1 = Reader.GetIntValue(drlector, "clav_usuario1");
                        entidad.EstadoAsiento = Reader.GetStringValue(drlector, "ESTADO_ASIENTO");
                        entidad.Sexo = Reader.GetStringValue(drlector, "SEXO");
                        entidad.TipoPago = Reader.GetStringValue(drlector, "tipo_pago");
                        entidad.CodiSucursalVenta = Reader.GetIntValue(drlector, "SUC_VENTA");
                        entidad.IdVenta = Reader.GetIntValue(drlector, "id_venta");
                        entidad.ValeRemoto = Reader.GetStringValue(drlector, "vale_remoto");
                        entidad.IdVentaWeb = Reader.GetIntValue(drlector, "Id_VentaWeb");
                        entidad.ImpManifiesto = Reader.GetDecimalValue(drlector, "imp_manifiesto");
                        entidad.TipoVenta = Reader.GetStringValue(drlector, "TIPO_V");
                        entidad.CodiRuta = Reader.GetSmallIntValue(drlector, "Codi_ruta");
                        entidad.HoraProgramacion = Reader.GetStringValue(drlector, "Hora_programacion");
                        entidad.CodiServicio = Reader.GetByteValue(drlector, "Codi_Servicio");
                        entidad.Nacionalidad = Reader.GetStringValue(drlector, "nacionalidad");
                        break;
                    }
                }
            }

            return entidad;
        }

        public static string ConsultaPos(string CodTab, string CodEmp)
        {
            var valor = "0";

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Tabla_Consulta_Pos";
                db.AddParameter("@COD_TAB", DbType.String, ParameterDirection.Input, CodTab);
                db.AddParameter("@COD_EMP", DbType.String, ParameterDirection.Input, CodEmp);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetStringValue(drlector, "MES_CON") ?? "0";
                        break;
                    }
                }
            }

            return valor;
        }

        public static int ConsultaSumaBoletosPostergados(string Tipo, string Numero, string Emp)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Bol_Postergados_Consulta_Suma_Ele";
                db.AddParameter("@tipo", DbType.String, ParameterDirection.Input, Tipo);
                db.AddParameter("@numero", DbType.String, ParameterDirection.Input, Numero);
                db.AddParameter("@emp", DbType.String, ParameterDirection.Input, Emp);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "");
                    }
                }
            }

            return valor;
        }

        public static bool VerificaVentaPromoKmt(int IdVenta)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_venta_promo_kmt_Verifica";
                db.AddParameter("@id_venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = true;
                    }
                }
            }

            return valor;
        }

        public static int ConsultaPagoTarjetaVenta(int IdVenta)
        {
            var valor = new int();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_PagoTarjetaVenta_Trae_Id_Caja";
                db.AddParameter("@id_venta", DbType.String, ParameterDirection.Input, IdVenta);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        valor = Reader.GetIntValue(drlector, "idcaja");
                        break;
                    }
                }
            }

            return valor;
        }

        public static VentaEntity ConsultaVentaReintegro(string Ser, string Bol, string Emp, string Tipo)
        {
            var entidad = new VentaEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Venta_Reintegro_Consulta_Anul_Ele";
                db.AddParameter("@Ser", DbType.String, ParameterDirection.Input, Ser);
                db.AddParameter("@Bol", DbType.String, ParameterDirection.Input, Bol);
                db.AddParameter("@Emp", DbType.String, ParameterDirection.Input, Emp);
                db.AddParameter("@Tipo", DbType.String, ParameterDirection.Input, Tipo);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.IdVenta = Reader.GetIntValue(drlector, "id_venta");
                        entidad.CodiEsca = Reader.GetStringValue(drlector, "codi_esca");
                        entidad.CodiSucursal = Reader.GetSmallIntValue(drlector, "Codi_Sucursal");
                        entidad.PrecioVenta = Reader.GetDecimalValue(drlector, "PREC_VENTA");
                        entidad.TipoPago = Reader.GetStringValue(drlector, "tipo_pago");
                        entidad.CodiUsuario = Reader.GetSmallIntValue(drlector, "clav_usuario");
                        entidad.Tipo = Reader.GetStringValue(drlector, "tipo");
                        entidad.RucCliente = Reader.GetStringValue(drlector, "NIT_CLIENTE");
                        entidad.FechaVenta = Reader.GetDateStringValue(drlector, "FECH_VENTA");
                        entidad.CodiEmpresa = Reader.GetByteValue(drlector, "CODI_EMPRESA");

                        entidad.SucVenta = Reader.GetSmallIntValue(drlector, "SUC_VENTA");

                        break;
                    }
                }
            }

            return entidad;
        }

        public static CajaEntity ConsultaCajaPase(string Nume)
        {
            var entidad = new CajaEntity();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_caja_COnsulta_Pase";
                db.AddParameter("@nume", DbType.String, ParameterDirection.Input, Nume);
                using (IDataReader drlector = db.GetDataReader())
                {
                    while (drlector.Read())
                    {
                        entidad.Monto = Reader.GetDecimalValue(drlector, "MONT_CAJA");
                        entidad.IdCaja = Reader.GetIntValue(drlector, "idcaja");

                        break;
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
                // Condición especial 'entidad.TipoPago': (Multipago = "02")
                db.AddParameter("@Tipo_Pago", DbType.String, ParameterDirection.Input, (entidad.TipoPago == "02" ? "03" : entidad.TipoPago));
                // -----------------------------------------------
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
                db.AddParameter("@Precio_Normal", DbType.Decimal, ParameterDirection.Input, entidad.PrecioNormal);

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
                db.AddParameter("@FECH_CAJA", DbType.String, ParameterDirection.Input, entidad.FechaCaja);
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

                db.AddParameter("@Nom_Usario", DbType.String, ParameterDirection.Input, entidad.NomUsuario);
                db.AddParameter("@CONC_CAJA", DbType.String, ParameterDirection.Input, entidad.ConcCaja);
                db.AddParameter("@TIPO_VALE", DbType.String, ParameterDirection.Input, entidad.TipoVale);
                db.AddParameter("@CODI_BUS", DbType.String, ParameterDirection.Input, entidad.CodiBus);
                db.AddParameter("@CODI_CHOFER", DbType.String, ParameterDirection.Input, entidad.CodiChofer);
                db.AddParameter("@CODI_GASTO", DbType.String, ParameterDirection.Input, entidad.CodiGasto);
                db.AddParameter("@INDI_ANULADO", DbType.String, ParameterDirection.Input, entidad.IndiAnulado);
                db.AddParameter("@TIPO_DESCUENTO", DbType.String, ParameterDirection.Input, entidad.TipoDescuento);
                db.AddParameter("@TIPO_DOC", DbType.String, ParameterDirection.Input, entidad.TipoDoc);
                db.AddParameter("@TIPO_GASTO", DbType.String, ParameterDirection.Input, entidad.TipoGasto);
                db.AddParameter("@LIQUI", DbType.Decimal, ParameterDirection.Input, entidad.Liqui);
                db.AddParameter("@DIFERENCIA", DbType.Decimal, ParameterDirection.Input, entidad.Diferencia);
                db.AddParameter("@VOUCHER", DbType.String, ParameterDirection.Input, entidad.Voucher);
                db.AddParameter("@ASIENTO", DbType.String, ParameterDirection.Input, entidad.Asiento);
                db.AddParameter("@RUC", DbType.String, ParameterDirection.Input, entidad.Ruc);

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

        public static bool AcompanianteVentaCRUD(AcompanianteRequest request)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_CRUD_AcompañanteVenta";
                db.AddParameter("@IDVENTA", DbType.Int32, ParameterDirection.Input, request.IdVenta);
                db.AddParameter("@TIPO_DOC", DbType.String, ParameterDirection.Input, request.Acompaniante.TipoDocumento);
                db.AddParameter("@DNI", DbType.String, ParameterDirection.Input, request.Acompaniante.NumeroDocumento);
                db.AddParameter("@NOMBRE", DbType.String, ParameterDirection.Input, request.Acompaniante.NombreCompleto);
                db.AddParameter("@FECHAN", DbType.String, ParameterDirection.Input, request.Acompaniante.FechaNacimiento);
                db.AddParameter("@EDAD", DbType.String, ParameterDirection.Input, request.Acompaniante.Edad);
                db.AddParameter("@SEXO", DbType.String, ParameterDirection.Input, request.Acompaniante.Sexo);
                db.AddParameter("@PARENTESCO", DbType.String, ParameterDirection.Input, request.Acompaniante.Parentesco);

                db.AddParameter("@ACTION_TYPE", DbType.Byte, ParameterDirection.Input, request.ActionType);
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

        public static bool InsertarDescuentoBoleto(DescuentoBoletoEntity entidad)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_Tb_Descuentos_Boleto_Insertar";
                db.AddParameter("@Usuario", DbType.Int16, ParameterDirection.Input, entidad.Usuario);
                db.AddParameter("@Oficina", DbType.Int16, ParameterDirection.Input, entidad.Oficina);
                db.AddParameter("@motivo", DbType.String, ParameterDirection.Input, entidad.Motivo);
                db.AddParameter("@Boleto", DbType.String, ParameterDirection.Input, entidad.Boleto);
                db.AddParameter("@Imp_Teorico", DbType.Decimal, ParameterDirection.Input, entidad.ImpTeorico);
                db.AddParameter("@Imp_Real", DbType.Decimal, ParameterDirection.Input, entidad.ImpReal);
                db.AddParameter("@Servicio", DbType.Byte, ParameterDirection.Input, entidad.Servicio);
                db.AddParameter("@Origen", DbType.Int16, ParameterDirection.Input, entidad.Origen);
                db.AddParameter("@Destino", DbType.Int16, ParameterDirection.Input, entidad.Destino);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool InsertarDescuentoVenta(int IdVenta, string Tipo, decimal CamDes, decimal ImpDes, string Obs)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "UPS_TB_DESCUENTO_VENTA_INSERT";
                db.AddParameter("@ID_VENTA", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@TIPO", DbType.String, ParameterDirection.Input, Tipo);
                db.AddParameter("@CamDes", DbType.Decimal, ParameterDirection.Input, CamDes);
                db.AddParameter("@ImpDes", DbType.Decimal, ParameterDirection.Input, ImpDes);
                db.AddParameter("@obs", DbType.String, ParameterDirection.Input, Obs);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool InsertarUsuarioPorVenta(string Usuario, string Accion, decimal IdVenta, string Motivo)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Usuario_X_Venta_Insert";
                db.AddParameter("@usuario", DbType.String, ParameterDirection.Input, Usuario);
                db.AddParameter("@accion", DbType.String, ParameterDirection.Input, Accion);
                db.AddParameter("@id_venta", DbType.Decimal, ParameterDirection.Input, IdVenta);
                db.AddParameter("@motivo", DbType.String, ParameterDirection.Input, Motivo);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static void ActualizaF9Elec(int IdVenta, string Dni, string Nombre, string Ruc, int Edad, string Telefono, string RecoVenta, string TipoDoc, string Nacionalidad)
        {
            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Venta_Update_F9_2";
                db.AddParameter("@Id_venta", DbType.Int32, ParameterDirection.Input, IdVenta);
                db.AddParameter("@dni", DbType.String, ParameterDirection.Input, Dni);
                db.AddParameter("@NOMBRE", DbType.String, ParameterDirection.Input, Nombre);
                db.AddParameter("@Nit_Cliente", DbType.String, ParameterDirection.Input, Ruc);
                db.AddParameter("@EDAD", DbType.Int16, ParameterDirection.Input, Edad);
                db.AddParameter("@TELEFONO", DbType.String, ParameterDirection.Input, Telefono);
                db.AddParameter("@RECO_VENTA", DbType.String, ParameterDirection.Input, RecoVenta);
                db.AddParameter("@tipo_doc", DbType.String, ParameterDirection.Input, TipoDoc);
                db.AddParameter("@Nacionidad", DbType.String, ParameterDirection.Input, Nacionalidad);
                db.Execute();
            }
        }

        public static bool ActualizarVentaPromokmt(int IdVentaCan)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_venta_promo_kmt_Update";
                db.AddParameter("@id_venta_can", DbType.Int32, ParameterDirection.Input, IdVentaCan);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool EliminarVentaPromokmt(int IdVenta)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "scwsp_venta_promo_kmt_Update";
                db.AddParameter("@id_venta", DbType.Int32, ParameterDirection.Input, IdVenta);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ActualizarCajaAnulacion(int IdCaja)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_caja_Update_Anula_id";
                db.AddParameter("@idcaja", DbType.Int32, ParameterDirection.Input, IdCaja);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool EliminarPoliza(int IdVenta)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Poliza_Delete";
                db.AddParameter("@idventa", DbType.Int32, ParameterDirection.Input, IdVenta);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool InsertarAnulacionPorDia(string Fecha, int Pventa, int Cnt)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Anulacion_por_Dia_Insert";
                db.AddParameter("@fecha", DbType.String, ParameterDirection.Input, Fecha);
                db.AddParameter("@pventa", DbType.Int32, ParameterDirection.Input, Pventa);
                db.AddParameter("@cnt", DbType.Int32, ParameterDirection.Input, Cnt);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ActualizarAnulacionPorDia(string Fecha, int Pventa)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_Anulacion_por_Dia_Update";
                db.AddParameter("@fecha", DbType.String, ParameterDirection.Input, Fecha);
                db.AddParameter("@pventa", DbType.Int32, ParameterDirection.Input, Pventa);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ActualizarBoletosPorSocio(string Socio, string Mes, string Ann)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_boletos_x_socio_Update";
                db.AddParameter("@socio", DbType.String, ParameterDirection.Input, Socio);
                db.AddParameter("@mes", DbType.String, ParameterDirection.Input, Mes);
                db.AddParameter("@ann", DbType.String, ParameterDirection.Input, Ann);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        public static bool ActualizarBoletosPorSocioV(string Socio, string Mes, string Ann)
        {
            var valor = new bool();

            using (IDatabase db = DatabaseHelper.GetDatabase())
            {
                db.ProcedureName = "Usp_Tb_boletos_x_socio_Update_V";
                db.AddParameter("@socio", DbType.String, ParameterDirection.Input, Socio);
                db.AddParameter("@mes", DbType.String, ParameterDirection.Input, Mes);
                db.AddParameter("@ann", DbType.String, ParameterDirection.Input, Ann);

                db.Execute();

                valor = true;
            }

            return valor;
        }

        #endregion
    }
}
