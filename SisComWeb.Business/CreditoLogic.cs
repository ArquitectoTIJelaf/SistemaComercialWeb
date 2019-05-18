using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public static class CreditoLogic
    {
        public static Response<List<ClienteCreditoEntity>> ListaClientesContrato(CreditoRequest request)
        {
            try
            {
                // Lista 'ClientesContrato'
                var listarClientesContrato = CreditoRepository.ListarClientesContrato(0);

                for (int i = 0; i < listarClientesContrato.Count; i++)
                {
                    // Consulta 'Contrato'
                    var consultarContrato = CreditoRepository.ConsultarContrato(listarClientesContrato[i].IdContrato);

                    if (consultarContrato.Marcador == "1")
                    {
                        if (consultarContrato.Saldo - request.Precio <= 0)
                        {
                            listarClientesContrato.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }

                    // Verifica 'ContratoPasajes'
                    var verificarContratoPasajes = CreditoRepository.VerificarContratoPasajes(listarClientesContrato[i].RucCliente, request.FechaViaje, request.FechaViaje, request.CodiOficina.ToString(), request.CodiRuta.ToString(), request.CodiServicio.ToString(), listarClientesContrato[i].IdRuc);

                    if (verificarContratoPasajes.IdContrato <= 0)
                    {
                        // Valida 'ClientePrecioNormal'
                        var validarClientePrecioNormal = CreditoRepository.ValidarClientePrecioNormal(listarClientesContrato[i].IdRuc, request.FechaViaje, request.FechaViaje);
                        if (!validarClientePrecioNormal)
                        {
                            listarClientesContrato.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }

                    // Verificar 'PrecioNormal'
                    var verificarPrecioNormal = CreditoRepository.VerificarPrecioNormal();
                    // Si encontró datos (IdNormal toma desde el 0)
                    if (verificarPrecioNormal.IdNormal >= 0)
                    {
                        if (verificarPrecioNormal.Saldo < 1)
                        {
                            listarClientesContrato.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                    // Sino
                    else
                    {
                        // Consulta 'AsientoNivel'
                        //var consultarAsientoNivel = CreditoRepository.ConsultarAsientoNivel(request.CodiBus, request.NumeAsiento.ToString());

                        // Busca 'Precio'
                        //var buscarPrecio = CreditoRepository.BuscarPrecio(request.FechaViaje, consultarAsientoNivel, request.HoraViaje, verificarContratoPasajes.IdPrecio.ToString());
                        //if (buscarPrecio <= 0)
                        //{
                        //    listarClientesContrato.RemoveAt(i);
                        //    i--;
                        //    continue;
                        //}

                        if (verificarContratoPasajes.SaldoBoletos <= 0)
                        {
                            listarClientesContrato.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }
                }

                return new Response<List<ClienteCreditoEntity>>(true, listarClientesContrato, Message.MsgCorrectoListaClientesContrato, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ClienteCreditoEntity>>(false, null, Message.MsgExcListaClientesContrato, false);
            }
        }
    }
}
