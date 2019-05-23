using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public static class CreditoLogic
    {
        public static Response<List<ClienteCreditoEntity>> ListarClientesContrato(CreditoRequest request)
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
                    var verificarPrecioNormal = CreditoRepository.VerificarPrecioNormal(listarClientesContrato[i].IdContrato);
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
                        if (verificarContratoPasajes.SaldoBoletos <= 0)
                        {
                            listarClientesContrato.RemoveAt(i);
                            i--;
                            continue;
                        }
                    }

                    // Seteo variables auxiliares desde 'verificarContratoPasajes'
                    listarClientesContrato[i].CntBoletos = verificarContratoPasajes.CntBoletos;
                    listarClientesContrato[i].SaldoBoletos = verificarContratoPasajes.SaldoBoletos;
                    listarClientesContrato[i].IdPrecio = verificarContratoPasajes.IdPrecio;
                    listarClientesContrato[i].Precio = verificarContratoPasajes.Precio;
                }

                return new Response<List<ClienteCreditoEntity>>(true, listarClientesContrato, Message.MsgCorrectoListarClientesContrato, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ClienteCreditoEntity>>(false, null, Message.MsgExcListarClientesContrato, false);
            }
        }

        public static Response<List<PanelControlEntity>> ListarPanelControl()
        {
            try
            {
                // Lista 'PanelControl'
                var listarPanelControl = CreditoRepository.ListarPanelControl();

                return new Response<List<PanelControlEntity>>(true, listarPanelControl, Message.MsgCorrectoListarPanelControl, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<PanelControlEntity>>(false, null, Message.MsgExcListarPanelControl, false);
            }
        }

        public static Response<ContratoEntity> ConsultarContrato(int idContrato)
        {
            try
            {
                // Consulta 'Contrato'
                var consultarContrato = CreditoRepository.ConsultarContrato(idContrato);

                return new Response<ContratoEntity>(true, consultarContrato, Message.MsgCorrectoConsultarContrato, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ContratoEntity>(false, null, Message.MsgExcConsultarContrato, false);
            }
        }
    }
}
