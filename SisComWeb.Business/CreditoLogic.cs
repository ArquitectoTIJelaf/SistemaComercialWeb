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
                    ContratoEntity consultarContrato = CreditoRepository.ConsultarContrato(listarClientesContrato[i].IdContrato);
                    if (consultarContrato != null)
                    {
                        if (consultarContrato.Marcador == "1")
                        {
                            if (consultarContrato.Saldo <= 0)
                            {
                                listarClientesContrato.RemoveAt(i);
                                i--;
                                continue;
                            }
                        }
                    }
                    else {
                        listarClientesContrato.RemoveAt(i);
                        i--;
                        continue;
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
                    var verificarPrecioNormal = CreditoRepository.VerificarPrecioNormal(verificarContratoPasajes.IdContrato);
                    // Si encontró datos (IdNormal toma desde el 0)
                    if (verificarPrecioNormal.IdNormal >= 0)
                    {
                        if (verificarPrecioNormal.Saldo <= 0)
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
                    listarClientesContrato[i].CntBoletos = verificarContratoPasajes.CntBoletos; // Cnt
                    listarClientesContrato[i].SaldoBoletos = verificarContratoPasajes.SaldoBoletos; // SALDO
                    listarClientesContrato[i].IdPrecio = verificarContratoPasajes.IdPrecio; // IdServicioContrato
                    listarClientesContrato[i].Precio = verificarContratoPasajes.Precio; // Precio
                }

                if (listarClientesContrato.Count == 0)
                    return new Response<List<ClienteCreditoEntity>>(false, listarClientesContrato, Message.MsgValidaListarClientesContrato, true);
                else
                    return new Response<List<ClienteCreditoEntity>>(true, listarClientesContrato, Message.MsgCorrectoListarClientesContrato, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<ClienteCreditoEntity>>(false, null, Message.MsgExcListarClientesContrato, false);
            }
        }

        public static Response<PanelControlResponse> ListarPanelesControl()
        {
            try
            {
                // Lista 'PanelControl'
                var listarPanelControl = CreditoRepository.ListarPanelControl();

                // Lista 'PanelControlClave'
                var listarPanelControlCalve = CreditoRepository.ListarPanelControlClave();

                var panelControlResponse = new PanelControlResponse()
                {
                    ListarPanelControl = listarPanelControl,
                    ListarPanelControlClave = listarPanelControlCalve
                };

                return new Response<PanelControlResponse>(true, panelControlResponse, Message.MsgCorrectoListarPanelesControl, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PanelControlResponse>(false, null, Message.MsgExcListarPanelesControl, false);
            }
        }

        public static Response<ContratoEntity> ConsultarContrato(int idContrato)
        {
            try
            {
                // Consulta 'Contrato'
                ContratoEntity consultarContrato = CreditoRepository.ConsultarContrato(idContrato);
                if (consultarContrato != null)
                    return new Response<ContratoEntity>(true, consultarContrato, Message.MsgCorrectoConsultarContrato, true);
                else
                    return new Response<ContratoEntity>(false, consultarContrato, Message.MsgValidaNullConsultarContrato, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ContratoEntity>(false, null, Message.MsgExcConsultarContrato, false);
            }
        }

        public static Response<PrecioNormalEntity> VerificarPrecioNormal(int idContrato)
        {
            try
            {
                // Verificar 'PrecioNormal'
                var verificarPrecioNormal = CreditoRepository.VerificarPrecioNormal(idContrato);

                return new Response<PrecioNormalEntity>(true, verificarPrecioNormal, Message.MsgCorrectoVerificarPrecioNormal, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<PrecioNormalEntity>(false, null, Message.MsgExcVerificarPrecioNormal, false);
            }
        }

        public static Response<decimal> BuscarPrecio(string fechaViaje, string nivel, string hora, string idPrecio)
        {
            try
            {
                // Buscar 'Precio'
                var buscarPrecio = CreditoRepository.BuscarPrecio(fechaViaje, nivel, hora, idPrecio);

                return new Response<decimal>(true, buscarPrecio, Message.MsgCorrectoBuscarPrecio, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(CreditoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<decimal>(false, 0, Message.MsgExcBuscarPrecio, false);
            }
        }
    }
}
