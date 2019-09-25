using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public static class BaseLogic
    {
        public static Response<List<BaseEntity>> ListaOficinas()
        {
            try
            {
                var lista = BaseRepository.ListaOficinas();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaOficinas, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaOficinas, false);
            }
        }

        public static Response<List<BaseEntity>> ListaPuntosVenta()
        {
            try
            {
                var lista = BaseRepository.ListaPuntosVenta();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaPuntosVenta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaPuntosVenta, false);
            }
        }

        public static Response<List<BaseEntity>> ListaUsuarios(string value)
        {
            try
            {
                if (value == "null")
                    value = string.Empty;

                var lista = BaseRepository.ListaUsuariosAutocomplete(value);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaUsuarios, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuarios, false);
            }
        }

        public static Response<List<BaseEntity>> ListaServicios()
        {
            try
            {
                var lista = BaseRepository.ListaServicios();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaServicios, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaServicios, false);
            }
        }

        public static Response<List<BaseEntity>> ListaEmpresas()
        {
            try
            {
                var lista = BaseRepository.ListaEmpresas();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaEmpresas, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaEmpresas, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTiposDoc()
        {
            try
            {
                var lista = BaseRepository.ListaTiposDoc();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaTiposDoc, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTiposDoc, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTipoPago()
        {
            try
            {
                var lista = BaseRepository.ListaTipoPago();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaTipoPago, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTipoPago, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTarjetaCredito()
        {
            try
            {
                var lista = BaseRepository.ListaTarjetaCredito();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaTarjetaCredito, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTarjetaCredito, false);
            }
        }

        public static Response<List<BaseEntity>> ListaCiudad()
        {
            try
            {
                var lista = BaseRepository.ListaCiudad();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaCiudad, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaCiudad, false);
            }
        }

        public static Response<List<BaseEntity>> ListarParentesco()
        {
            try
            {
                var lista = BaseRepository.ListarParentesco();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListarParentesco, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarParentesco, false);
            }
        }

        public static Response<List<BaseEntity>> ListarGerente()
        {
            try
            {
                var lista = BaseRepository.ListarGerente();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListarGerente, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarGerente, false);
            }
        }

        public static Response<List<BaseEntity>> ListarSocio()
        {
            try
            {
                var lista = BaseRepository.ListarSocio();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListarSocio, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarSocio, false);
            }
        }

        public static Response<List<BaseEntity>> ListaHospitales(int codiSucursal)
        {
            try
            {
                var lista = BaseRepository.ListaHospitales(codiSucursal);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListarSocio, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaHospitales, false);
            }
        }

        public static Response<List<BaseEntity>> ListaSecciones(int idContrato)
        {
            try
            {
                var lista = BaseRepository.ListaSecciones(idContrato);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaSecciones, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaSecciones, false);
            }
        }

        public static Response<List<BaseEntity>> ListaAreas(int idContrato)
        {
            try
            {
                var lista = BaseRepository.ListaAreas(idContrato);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaAreas, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaAreas, false);
            }
        }

        public static Response<List<BaseEntity>> ListaUsuariosClaveAnuRei(string Value)
        {
            try
            {
                if (Value == "null")
                    Value = string.Empty;

                var lista = BaseRepository.ListaUsuariosClaveAnuRei(Value);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaUsuariosClaveAnuRei, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuariosClaveAnuRei, false);
            }
        }

        public static Response<List<BaseEntity>> ListaOpcionesModificacion()
        {
            try
            {
                var lista = BaseRepository.ListaOpcionesModificacion();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaOpcionesModificacion, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaOpcionesModificacion, false);
            }
        }

        public static Response<List<BaseEntity>> ListaUsuariosHC(string Descripcion, short Suc, short Pv)
        {
            try
            {
                if (Descripcion == "null")
                    Descripcion = string.Empty;

                var lista = BaseRepository.ListaUsuariosHC(Descripcion, Suc, Pv);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaUsuarios, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuarios, false);
            }
        }

        public static Response<List<BaseEntity>> ListaUsuarioControlPwd(string Value)
        {
            try
            {
                if (Value == "null")
                    Value = string.Empty;

                var lista = BaseRepository.ListaUsuarioControlPwd(Value);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaUsuarios, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuarios, false);
            }
        }

        public static Response<MensajeriaEntity> ObtenerMensaje(int CodiUsuario, string Fecha, string Tipo, int CodiSucursal, int CodiPventa)
        {
            try
            {
                var obtenerMensaje = BaseRepository.ObtenerMensaje(CodiUsuario, Fecha, Tipo, CodiSucursal, CodiPventa);

                if (obtenerMensaje.IdMensaje > 0)
                    return new Response<MensajeriaEntity>(true, obtenerMensaje, Message.MsgCorrectoObtenerMensaje, true);
                else
                    return new Response<MensajeriaEntity>(false, obtenerMensaje, Message.MsgValidaObtenerMensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<MensajeriaEntity>(false, null, Message.MsgExcObtenerMensaje, false);
            }
        }

        public static Response<bool> EliminarMensaje(MensajeriaRequest request)
        {
            try
            {
                BaseRepository.EliminarMensaje(request.IdMensaje);

                var objAuditoria = new AuditoriaEntity
                {
                    CodiUsuario = request.CajeroCod,
                    NomUsuario = request.CajeroNom,
                    Tabla = "MENSAJES",
                    TipoMovimiento = "MENSAJE",
                    Boleto = string.Empty,
                    NumeAsiento = "0",
                    NomOficina = request.CajeroNomSuc,
                    NomPuntoVenta = request.CajeroCodPven.ToString(),
                    Pasajero = string.Empty,
                    FechaViaje = "01/01/1900",
                    HoraViaje = string.Empty,
                    NomDestino = string.Empty,
                    Precio = 0,
                    Obs1 = "USR ENVIA " + request.CodiUsuario,
                    Obs2 = "SUCURSAL ENVIA " + request.CodiSucursal,
                    Obs3 = "TERMINAL QUE ENVIA : " + request.Terminal,
                    Obs4 = "TERMINAL " + request.CajeroTer,
                    Obs5 = string.Empty
                };
                VentaRepository.GrabarAuditoria(objAuditoria);

                return new Response<bool>(true, true, Message.MsgCorrectoEliminarMensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcEliminarMensaje, false);
            }
        }

        public static Response<SucursalControlEntity> GetSucursalControl(string CodiPuntoVenta)
        {
            try
            {
                var obtenerMensaje = BaseRepository.GetSucursalControl(CodiPuntoVenta);

                return new Response<SucursalControlEntity>(true, obtenerMensaje, Message.MsgCorrectoGetSucursalControl, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<SucursalControlEntity>(false, null, Message.MsgExcGetSucursalControl, false);
            }
        }

        public static Response<List<BaseEntity>> ListaConceptosNC(string RucEmpresa, string TipoTerminalElectronico, string CodDoc, string ElectronicoEmpresa)
        {
            try
            {
                var lista = new List<BaseEntity>();

                if (TipoTerminalElectronico == "E" && ElectronicoEmpresa == "1")
                {
                    var obtenerRparametro = VentaLogic.ObtenerRparametro(RucEmpresa);

                    foreach (var objeto in obtenerRparametro.RDocumentNote)
                    {
                        if (objeto.CodDoc == CodDoc) // Motivo: Nota de crédito.
                        {
                            var entidad = new BaseEntity()
                            {
                                id = objeto.CodMotivo,
                                label = objeto.Descripcion
                            };

                            lista.Add(entidad);
                        }
                    }
                }
                else
                    lista = BaseRepository.ListaConceptosNC(CodDoc);

                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaConceptosNC, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaConceptosNC, false);
            }
        }
    }
}
