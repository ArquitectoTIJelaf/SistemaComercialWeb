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

                if (TipoDocumento != "77") // Caso: RUC.
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
    }
}
