using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Entity.Peticiones.Response;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public class PaseLoteLogic
    {
        public static Response<List<PaseLoteResponse>> UpdatePostergacion(UpdatePostergacionRequest request)
        {
            try
            {
                var res = PaseLoteRepository.UpdatePostergacion(request.Lista.Replace('\'', '"'));

                if (res.Count > 0)
                {
                   foreach(var obj in res)
                    {
                        var objAuditoria = new AuditoriaEntity
                        {
                            CodiUsuario = Convert.ToInt16(request.CodiUsuario),
                            NomUsuario = request.NomUsuario,
                            Tabla = "VENTA",
                            TipoMovimiento = "POS-LOTE",
                            Boleto = obj.Boleto,
                            NumeAsiento = obj.NumeAsiento.PadLeft(2, '0'),
                            NomOficina = request.NomSucursal,
                            NomPuntoVenta = request.PuntoVenta.PadLeft(3, '0'),
                            Pasajero = obj.Pasajero,
                            FechaViaje = obj.FechaViaje,
                            HoraViaje = obj.HoraViaje,
                            NomDestino = string.Empty,
                            Precio = 0.00m,
                            Obs1 = "ID " + obj.IdVenta + " PROGRAMACION: " + obj.CodiProgramacion,
                            Obs2 = "TERMINAL: " + request.Terminal.PadLeft(3, '0'),
                            Obs3 = string.Empty,
                            Obs4 = string.Empty,
                            Obs5 = string.Empty
                        };
                        VentaRepository.GrabarAuditoria(objAuditoria);
                    }                    
                }

                return new Response<List<PaseLoteResponse>>(true, res, string.Empty, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<PaseLoteResponse>>(false, null, Message.MsgExcPaseLote, false);
            }
        }
    }
}
