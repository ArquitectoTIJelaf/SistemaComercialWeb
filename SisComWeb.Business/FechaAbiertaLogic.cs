using SisComWeb.Entity;
using SisComWeb.Entity.Objects.Entities;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Entity.Peticiones.Response;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SisComWeb.Business
{
    public class FechaAbiertaLogic
    {
        public static Response<List<FechaAbiertaEntity>> VentaConsultaF6(FechaAbiertaRequest request)
        {
            try
            {
                var result = FechaAbiertaRepository.VentaConsultaF6(request);

                return new Response<List<FechaAbiertaEntity>>(true, result, Message.MsgCorrectoVentaConsultaF6, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<FechaAbiertaEntity>>(false, null, Message.MsgExcVentaConsultaF6, false);
            }
        }

        public static Response<bool> ValidateNivelAsiento(int IdVenta, string CodiBus, string Asiento)
        {
            try
            {
                var asientoBoleto = FechaAbiertaRepository.NivelAsientoVentaDerivada(IdVenta);
                var asientoTarget = FechaAbiertaRepository.NivelDelAsiento(CodiBus, Asiento);

                var response = false;
                var mensaje = "";

                if (asientoBoleto == asientoTarget)
                {
                    response = true;
                }
                else
                {
                    response = false;
                    mensaje = "El asiento fue vendido en el nivel " + asientoBoleto;
                }

                return new Response<bool>(true, response, mensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcValidateNivelAsiento, false);
            }
        }

        public static Response<int> ValidateNumDias(string FechaVenta, string CodTab)
        {
            try
            {
                var CantidadPerimita = FechaAbiertaRepository.TablasPnpConsulta(CodTab);

                var response = 0;
                var mensaje = "";

                //var DayFechaVenta = Convert.ToDateTime(FechaVenta);
                var DayFechaVenta = DateTime.ParseExact(FechaVenta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                int DiffDays = (DateTime.Now - DayFechaVenta).Days;

                if (CantidadPerimita > 0 && DiffDays >= CantidadPerimita)
                {
                    response = CantidadPerimita;
                    mensaje = string.Format("La diferencia de días es {0} y la cantidad de días permitidos es de {1}, ingrese clave de autorización", DiffDays, CantidadPerimita);
                }

                return new Response<int>(true, response, mensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcValidateNumDias, false);
            }
        }

        public static Response<int> VerificaNotaCredito(int IdVenta)
        {
            try
            {
                var Response = VentaRepository.VerificaNC(IdVenta);
                var mensaje = (Response > 0) ? Message.MsgVerificaNotaCredito : string.Empty;
                return new Response<int>(true, Response, mensaje, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcVerificaNotaCredito, false);
            }
        }

        public static Response<VentaResponse> VentaUpdatePostergacionEle(FechaAbiertaRequest filtro)
        {
            try
            {
                var valor = new VentaResponse();
                var listaVentasRealizadas = new List<VentaRealizadaEntity>();

                // Verifica 'CodiProgramacion'
                var objProgramacion = new ProgramacionEntity()
                {
                    NroViaje = filtro.NroViaje,
                    FechaProgramacion = filtro.FechaProgramacion,
                    CodiProgramacion = filtro.CodiProgramacion,
                    CodiEmpresa = filtro.CodiEmpresa,
                    CodiSucursal = filtro.CodiSucursal,
                    CodiRuta = filtro.CodiRutaBus,
                    CodiBus = filtro.CodiBus,
                    HoraProgramacion = filtro.HoraProgramacion,
                    CodiServicio = byte.Parse(filtro.CodiServicio.ToString()),

                    CodiUsuario = filtro.CodiUsuario.ToString(),
                    NomUsuario = filtro.NomUsuario,
                    CodiPuntoVenta = filtro.CodiPuntoVenta,
                    Terminal = filtro.Terminal,
                    CodiOrigen = filtro.CodiOrigen,
                    CodiDestino = filtro.CodiDestino,
                    NomOrigen = filtro.NomSucursal
                };

                var verificaCodiProgramacion = VentaLogic.VerificaCodiProgramacion(objProgramacion);
                if (verificaCodiProgramacion == 0)
                    return new Response<VentaResponse>(false, valor, Message.MsgErrorVerificaCodiProgramacion, false);
                else
                    filtro.CodiProgramacion = verificaCodiProgramacion;

                valor.CodiProgramacion = filtro.CodiProgramacion;

                var ventaRealizada = new VentaRealizadaEntity
                {
                    NumeAsiento = filtro.NumeAsiento
                };
                listaVentasRealizadas.Add(ventaRealizada);

                valor.ListaVentasRealizadas = listaVentasRealizadas;

                var Response = FechaAbiertaRepository.VentaUpdatePostergacionEle(filtro);
                FechaAbiertaRepository.VentaUpdateImpManifiesto(filtro.IdVenta);
                FechaAbiertaRepository.VentaDerivadaUpdateViaje(filtro.IdVenta, filtro.FechaViaje, filtro.HoraViaje, filtro.CodiServicio);
                FechaAbiertaRepository.VentaUpdateCnt(0, filtro.CodiProgramacion, filtro.Oficina, filtro.Oficina);

                var objAuditoria = new AuditoriaEntity
                {
                    CodiUsuario = filtro.CodiUsuario,//
                    NomUsuario = filtro.NomUsuario,//
                    Tabla = "VENTA",
                    TipoMovimiento = "CONFIRMA FECHA ABIERTA",
                    Boleto = filtro.Serie.PadLeft(3, '0') + "-" + filtro.Numero.PadLeft(8, '0'),
                    NumeAsiento = filtro.NumeAsiento.PadLeft(2, '0'),
                    NomOficina = filtro.NomSucursal,//
                    NomPuntoVenta = filtro.CodiPuntoVenta.PadLeft(3, '0'),//
                    Pasajero = filtro.Nombre,
                    FechaViaje = filtro.FechaViaje,
                    HoraViaje = filtro.HoraViaje,
                    NomDestino = filtro.NombDestino,
                    Precio = decimal.Parse(filtro.Precio),
                    Obs1 = "CONFIRMACION FECHA ABIERTA",
                    Obs2 = string.Empty,
                    Obs3 = string.Empty,
                    Obs4 = string.Empty,
                    Obs5 = string.Empty
                };
                VentaRepository.GrabarAuditoria(objAuditoria);

                return new Response<VentaResponse>(true, valor, Message.MsgCorrectoVentaUpdatePostergacionEle, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<VentaResponse>(false, null, Message.MsgExcVentaUpdatePostergacionEle, false);
            }
        }
    }
}
