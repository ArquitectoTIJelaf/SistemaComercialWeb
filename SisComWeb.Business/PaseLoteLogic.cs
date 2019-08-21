using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;

namespace SisComWeb.Business
{
    public class PaseLoteLogic
    {
        public static Response<bool> UpdatePostergacion(UpdatePostergacionRequest request)
        {
            try
            {
                var res = false;

                if (request.FlagVenta.Equals("R"))
                {
                    //Si es Reserva
                    res = PaseLoteRepository.UpdateProgramacion(request.CodiProgramacion, request.IdVenta);
                }
                else
                {
                    res = PaseLoteRepository.UpdatePostergacion(request);
                    FechaAbiertaRepository.VentaDerivadaUpdateViaje(request.IdVenta, request.FechaViaje, request.HoraViaje, request.CodiServicio);
                }

                if (res)
                {
                    var objAuditoria = new AuditoriaEntity
                    {
                        CodiUsuario = Convert.ToInt16(request.CodiUsuario),
                        NomUsuario = request.NomUsuario,
                        Tabla = "VENTA",
                        TipoMovimiento = "POS-LOTE",
                        Boleto = request.Boleto,
                        NumeAsiento = request.NumeAsiento.PadLeft(2, '0'),
                        NomOficina = request.NomSucursal,
                        NomPuntoVenta = request.PuntoVenta.PadLeft(3, '0'),
                        Pasajero = request.Pasajero,
                        FechaViaje = request.FechaViaje,
                        HoraViaje = request.HoraViaje,
                        NomDestino = string.Empty,
                        Precio = 0.00m,
                        Obs1 = "ID " + request.IdVenta + " PROGRAMACION: " + request.CodiProgramacion,
                        Obs2 = "TERMINAL: " + request.Terminal.PadLeft(3, '0'),
                        Obs3 = string.Empty,
                        Obs4 = string.Empty,
                        Obs5 = string.Empty
                    };
                    //Graba Auditoria
                    VentaRepository.GrabarAuditoria(objAuditoria);

                    if (!request.CodiEsca.Equals(""))
                    {
                        var filtro = new ReintegroRequest()
                        {
                            Serie = Convert.ToInt32(request.CodiEsca.Substring(1, 3)),
                            Numero = Convert.ToInt32(request.CodiEsca.Substring(5)),
                            CodiEmpresa = request.CodiEmpresa,
                            Tipo = request.CodiEsca.Substring(0, 1)
                        };
                        //Se obtiene objeto reintegro
                        var objReintegro = ReintegroRepository.VentaConsultaF12(filtro);

                        var objAuditoria2 = new AuditoriaEntity
                        {
                            CodiUsuario = Convert.ToInt16(request.CodiUsuario),//
                            NomUsuario = request.NomUsuario,
                            Tabla = "VENTA",
                            TipoMovimiento = "POS-LOTE",
                            Boleto = objReintegro.Tipo + objReintegro.SerieBoleto.ToString().PadLeft(3, '0') + "-" + objReintegro.NumeBoleto.ToString().PadLeft(7, '0'),
                            NumeAsiento = objReintegro.NumeAsiento.ToString().PadLeft(2, '0'),
                            NomOficina = request.NomSucursal,//
                            NomPuntoVenta = request.PuntoVenta.PadLeft(3, '0'),//
                            Pasajero = objReintegro.Nombre,
                            FechaViaje = request.FechaViaje,
                            HoraViaje = request.HoraViaje,
                            NomDestino = string.Empty,
                            Precio = 0.00m,
                            Obs1 = "ID " + objReintegro.IdVenta + " PROGRAMACION: " + objReintegro.CodiProgramacion,
                            Obs2 = "TERMINAL: " + request.Terminal.PadLeft(3, '0'),//
                            Obs3 = string.Empty,
                            Obs4 = string.Empty,
                            Obs5 = string.Empty
                        };
                        //Graba Auditoria Reintegro
                        VentaRepository.GrabarAuditoria(objAuditoria2);
                    }
                }

                return new Response<bool>(true, res, string.Empty, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcConsultaIgv, false);
            }
        }
    }
}
