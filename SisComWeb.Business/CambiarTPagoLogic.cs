using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Business
{
    public class CambiarTPagoLogic
    {
        public static Response<string> CambiarTipoPago(CambiarTPagoRequest request)
        {
            try
            {
                var response = "";

                if(request.NewTipoPago == "01")
                {
                    request.CodiTarjetaCredito = "";
                    request.NumeTarjetaCredito = "";
                    request.NomTarjetaCredito = "";
                }

                // Resta al Vale de Caja el crédito ingresado, Elimina el Pago con Tarjeta 
                // y Modificar el Tipo de Pago
                var NumeroCaja = CambiarTPagoRepository.CambiarTipoPago(request);

                var auxBoletoCompleto = string.Format("{0}{1}-{2}", request.Tipo, request.Serie.ToString().PadLeft(3, '0'), request.Numero.ToString().PadLeft(7, '0'));

                if (request.NewTipoPago == "03")
                {
                    //  Genera 'CorrelativoAuxiliar'
                    var generarCorrelativoAuxiliar = VentaRepository.GenerarCorrelativoAuxiliar("CAJA", request.CodiOficina.ToString(), request.CodiPuntoVenta.ToString(), string.Empty);
                    if (string.IsNullOrEmpty(generarCorrelativoAuxiliar))
                        return new Response<string>(false, string.Empty, string.Empty, false);

                    // Graba 'Caja'
                    var objCajaEntity = new CajaEntity
                    {
                        NumeCaja = generarCorrelativoAuxiliar.PadLeft(7, '0'),
                        CodiEmpresa = Convert.ToByte(request.CodiEmpresa),
                        CodiSucursal = request.CodiOficina,
                        FechaCaja = DataUtility.ObtenerFechaDelSistema(),
                        TipoVale = "S",
                        Boleto = auxBoletoCompleto.Substring(1),
                        NomUsuario = request.NomUsuario,
                        CodiBus = string.Empty,
                        CodiChofer = string.Empty,
                        CodiGasto = string.Empty,
                        ConcCaja = auxBoletoCompleto.Substring(1),
                        Monto = request.PrecioVenta,
                        CodiUsuario = request.CodiUsuario,
                        IndiAnulado = "F",
                        TipoDescuento = string.Empty,
                        TipoDoc = "XX",
                        TipoGasto = "P",
                        Liqui = 0M,
                        Diferencia = 0M,
                        Recibe = string.Empty,
                        CodiDestino = request.CodiDestino,
                        FechaViaje = request.FechaViaje,
                        HoraViaje = request.HoraViaje,
                        CodiPuntoVenta = request.CodiPuntoVenta,
                        Voucher = "PA",
                        Asiento = string.Empty,
                        Ruc = "N",
                        IdVenta = request.IdVenta,
                        Origen = "MT",
                        Modulo = "PM",
                        Tipo = request.Tipo,
                        IdCaja = 0
                    };

                    var grabarCaja = VentaRepository.GrabarCaja(objCajaEntity);
                    if (grabarCaja > 0)
                    {
                        // Seteo 'NumeCaja'
                        var auxNumeCaja = request.CodiOficina.ToString("D3") + request.CodiPuntoVenta.ToString("D3") + generarCorrelativoAuxiliar.PadLeft(7, '0');

                        response = auxNumeCaja;

                        // Graba 'PagoTarjetaCredito'
                        var objTarjetaCreditoEntity = new TarjetaCreditoEntity
                        {
                            IdVenta = request.IdVenta,
                            Boleto = auxBoletoCompleto.Substring(1),
                            CodiTarjetaCredito = request.CodiTarjetaCredito,
                            NumeTarjetaCredito = request.NumeTarjetaCredito,
                            Vale = auxNumeCaja,
                            IdCaja = grabarCaja,
                            Tipo = request.Tipo
                        };
                        var grabarPagoTarjetaCredito = VentaRepository.GrabarPagoTarjetaCredito(objTarjetaCreditoEntity);
                        if (!grabarPagoTarjetaCredito)
                            return new Response<string>(false, string.Empty, string.Empty, false);
                    }
                    else
                        return new Response<string>(false, string.Empty, string.Empty, false);
                }

                var objAuditoria = new AuditoriaEntity
                {
                    CodiUsuario = Convert.ToInt16(request.CodiUsuario),
                    NomUsuario = request.NomUsuario,
                    Tabla = "VENTA",
                    TipoMovimiento = "MODIFICACION DE TIPO DE PAGO",
                    Boleto = auxBoletoCompleto.Substring(1),
                    NumeAsiento = request.NumeAsiento,
                    NomOficina = request.NomSucursal,
                    NomPuntoVenta = request.CodiPuntoVenta.ToString().PadLeft(3, '0'),
                    Pasajero = request.Nombre,
                    FechaViaje = request.FechaViaje,
                    HoraViaje = request.HoraViaje,
                    NomDestino = request.NombDestino,
                    Precio = request.PrecioVenta,
                    Obs1 = "MODIFICACION T PAGO",
                    Obs2 = string.Format("{0} {1}", request.NewTipoPago, request.NomNewTipoPago),
                    Obs3 = request.NomTarjetaCredito,
                    Obs4 = request.NumeTarjetaCredito,
                    Obs5 = string.Empty
                };

                VentaRepository.GrabarAuditoria(objAuditoria);

                return new Response<string>(true, response, string.Empty, true); ;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CambiarTPagoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, string.Empty, "Error", false);
            }
        }
    }
}
