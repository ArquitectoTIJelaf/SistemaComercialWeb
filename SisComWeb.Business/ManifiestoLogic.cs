using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public static class ManifiestoLogic
    {
        public static Response<bool> ActualizarProgramacionManifiesto(ManifiestoRequest request)
        {
            try
            {
                // Actualiza 'ManifiestoProgramacion'
                var actualizarManifiestoProgramacion = ManifiestoRepository.ActualizarManifiestoProgramacion(request.CodiProgramacion, request.CodiSucursal, request.TipoApertura);

                if (actualizarManifiestoProgramacion)
                {
                    if (request.CodiSucursal == request.CodiSucursalBus)
                    {
                        // Actualiza 'Programacion'
                        var actualizarProgramacion = ManifiestoRepository.ActualizarProgramacion(request.CodiEmpresa, request.CodiProgramacion, request.CodiSucursal, request.TipoApertura);
                        if (!actualizarProgramacion)
                            return new Response<bool>(false, actualizarProgramacion, Message.MsgErrorActualizarProgramacion, false);
                    }

                    var objAuditoriaEntity = new AuditoriaEntity
                    {
                        CodiUsuario = request.CodiUsuario,
                        NomUsuario = request.NomUsuario,
                        Tabla = "PROGRAMACION",
                        TipoMovimiento = "EDICION",
                        Boleto = "CODIGO= " + request.CodiProgramacion.ToString(),
                        NumeAsiento = string.Empty,
                        NomOficina = request.CodiSucursal.ToString(),
                        NomPuntoVenta = request.CodiPuntoVenta,
                        Pasajero = string.Empty,
                        FechaViaje = string.Empty,
                        HoraViaje = string.Empty,
                        NomDestino = string.Empty,
                        Precio = 0,
                        Obs1 = request.TipoApertura ? "APERTURA DE PROGRAMACION PARA SEGUIR VENDIENDO" : "APERTURA DE PROGRAMACION PARA VOLVER A IMPRIMIR EL MANIFIESTO",
                        Obs2 = string.Empty,
                        Obs3 = string.Empty,
                        Obs4 = string.Empty,
                        Obs5 = string.Empty
                    };

                    // Graba 'AuditoriaProg'
                    var grabarAuditoriaProg = ManifiestoRepository.GrabarAuditoriaProg(objAuditoriaEntity);
                    if (grabarAuditoriaProg)
                    {
                        // Actualiza 'VentaManifiesto'
                        var actualizarVentaManifiesto = ManifiestoRepository.ActualizarVentaManifiesto(request.CodiProgramacion);
                        if (actualizarVentaManifiesto)
                            return new Response<bool>(true, actualizarVentaManifiesto, Message.MsgCorrectoActualizarProgramacionManifiesto, true);
                        else
                            return new Response<bool>(false, actualizarVentaManifiesto, Message.MsgErrorActualizarVentaManifiesto, false);
                    }
                    else
                        return new Response<bool>(false, grabarAuditoriaProg, Message.MsgErrorGrabarAuditoriaProg, false);
                }
                else
                    return new Response<bool>(false, actualizarManifiestoProgramacion, Message.MsgExcActualizarProgramacionManifiesto, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcActualizarProgramacionManifiesto, false);
            }
        }
    }
}
