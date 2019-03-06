using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public static class BaseLogic
    {
        public static Response<List<BaseEntity>> ListaOficinas()
        {
            try
            {
                var response = BaseRepository.ListaOficinas();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListOficina, false);
            }
        }

        public static Response<List<BaseEntity>> ListaPuntosVenta(short CodiSucursal)
        {
            try
            {
                var response = BaseRepository.ListaPuntosVenta(CodiSucursal);
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListPuntoVenta, false);
            }
        }

        public static Response<List<BaseEntity>> ListaUsuarios(string value)
        {
            try
            {
                if (value == "NULL") { value = string.Empty; }

                var response = BaseRepository.ListaUsuariosAutocomplete(value);
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListUsuario, false);
            }
        }

        public static Response<List<BaseEntity>> ListaServicios()
        {
            try
            {
                var response = BaseRepository.ListaServicios();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListServicio, false);
            }
        }

        public static Response<List<BaseEntity>> ListaEmpresas()
        {
            try
            {
                var response = BaseRepository.ListaEmpresas();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListEmpresa, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTiposDoc()
        {
            try
            {
                var response = BaseRepository.ListaTiposDoc();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListTipoDoc, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTipoPago()
        {
            try
            {
                var response = BaseRepository.ListaTipoPago();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListTipoPago, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTarjetaCredito()
        {
            try
            {
                var response = BaseRepository.ListaTarjetaCredito();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListTarjetaCredito, false);
            }
        }

        public static Response<List<BaseEntity>> ListaCiudad()
        {
            try
            {
                var response = BaseRepository.ListaCiudad();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListCiudad, false);
            }
        }

        public static Response<List<BaseEntity>> ListarParentesco()
        {
            try
            {
                var response = BaseRepository.ListarParentesco();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListCiudad, false);
            }
        }

        public static Response<List<BaseEntity>> ListarGerente()
        {
            try
            {
                var response = BaseRepository.ListarGerente();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListCiudad, false);
            }
        }

        public static Response<List<BaseEntity>> ListarSocio()
        {
            try
            {
                var response = BaseRepository.ListarSocio();
                return new Response<List<BaseEntity>>(response.EsCorrecto, response.Valor, response.Mensaje, response.Estado);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgErrExcListCiudad, false);
            }
        }
    }
}
