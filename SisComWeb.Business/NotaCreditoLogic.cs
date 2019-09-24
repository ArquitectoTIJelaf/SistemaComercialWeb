using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public static class NotaCreditoLogic
    {
        public static Response<string>ConsultaTipoTerminalElectronico(int CodiTerminal, int CodiEmpresa)
        {
            try
            {
                var consultaTipoTerminalElectronico = NotaCreditoRepository.ConsultaTipoTerminalElectronico(CodiTerminal, CodiEmpresa);

                return new Response<string>(true, consultaTipoTerminalElectronico, Message.MsgCorrectoConsultaTipoTerminalElectronico, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(VentaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<string>(false, null, Message.MsgExcConsultaTipoTerminalElectronico, false);
            }
        }

        public static Response<List<BaseEntity>> ListaClientesNC_Autocomplete(string TipoDocumento, string Value)
        {
            try
            {
                if (Value == "null")
                    Value = string.Empty;

                if (TipoDocumento != "06") // Caso: RUC.
                    TipoDocumento = VentaLogic.TipoDocumentoHomologado(TipoDocumento).ToString();

                var lista = NotaCreditoRepository.ListaClientesNC_Autocomplete(TipoDocumento, Value);

                if (lista.Count > 0)
                    return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaClientesNC_Autocomplete, true);
                else
                    return new Response<List<BaseEntity>>(false, lista, Message.MsgErrorListaClientesNC_Autocomplete, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaClientesNC_Autocomplete, false);
            }
        }

        public static Response<List<DocumentoEmitidoNCEntity>> ListaDocumentosEmitidos(DocumentosEmitidosRequest request)
        {
            try
            {
                var ListaDocumentosEmitidos = NotaCreditoRepository.ListaDocumentosEmitidos(request);

                for (int i = 0; i < ListaDocumentosEmitidos.Count; i++)
                {
                    if (ListaDocumentosEmitidos[i].Total > 0 && (request.TipoDocumento == "06" ? "F" : "B") == request.TipoNumDoc)
                    {
                        if (request.TipoPasEnc == "P" && request.Tipo == "M")
                            ListaDocumentosEmitidos[i].ColumnTipo = "16";
                        else
                            ListaDocumentosEmitidos[i].ColumnTipo = (request.TipoDocumento == "06" ? "01" : "03");

                        if (ListaDocumentosEmitidos[i].Tipo == "F" || ListaDocumentosEmitidos[i].Tipo == "B")
                            ListaDocumentosEmitidos[i].ColumnNroDocumento = (request.TipoDocumento == "06" ? "F" : "B") + ListaDocumentosEmitidos[i].Serie.ToString("D3") + "-" + ListaDocumentosEmitidos[i].Numero.ToString("D8");
                        else
                            ListaDocumentosEmitidos[i].ColumnNroDocumento = ListaDocumentosEmitidos[i].Serie.ToString("D4") + "-" + ListaDocumentosEmitidos[i].Numero.ToString("D8");
                    }
                    else
                    {
                        ListaDocumentosEmitidos.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if (ListaDocumentosEmitidos.Count > 0)
                    return new Response<List<DocumentoEmitidoNCEntity>>(true, ListaDocumentosEmitidos, Message.MsgCorrectoListaDocumentosEmitidos, true);
                else
                    return new Response<List<DocumentoEmitidoNCEntity>>(false, ListaDocumentosEmitidos, Message.MsgErrorListaDocumentosEmitidos, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<DocumentoEmitidoNCEntity>>(false, null, Message.MsgExcListaDocumentosEmitidos, false);
            }
        }
    }
}
