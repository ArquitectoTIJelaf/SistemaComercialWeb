﻿using SisComWeb.Entity;
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
                var lista = BaseRepository.ListaOficinas();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaOficinas, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaOficinas, false);
            }
        }

        public static Response<List<BaseEntity>> ListaPuntosVenta()
        {
            try
            {
                var lista = BaseRepository.ListaPuntosVenta();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaPuntosVenta, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaPuntosVenta, false);
            }
        }

        public static Response<List<BaseEntity>> ListaUsuarios(string value)
        {
            try
            {
                if (value == "null")
                    value = string.Empty;

                var lista = BaseRepository.ListaUsuariosAutocomplete(value);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaUsuarios, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuarios, false);
            }
        }

        public static Response<List<BaseEntity>> ListaServicios()
        {
            try
            {
                var lista = BaseRepository.ListaServicios();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaServicios, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaServicios, false);
            }
        }

        public static Response<List<BaseEntity>> ListaEmpresas()
        {
            try
            {
                var lista = BaseRepository.ListaEmpresas();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaEmpresas, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaEmpresas, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTiposDoc()
        {
            try
            {
                var lista = BaseRepository.ListaTiposDoc();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaTiposDoc, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTiposDoc, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTipoPago()
        {
            try
            {
                var lista = BaseRepository.ListaTipoPago();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaTipoPago, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTipoPago, false);
            }
        }

        public static Response<List<BaseEntity>> ListaTarjetaCredito()
        {
            try
            {
                var lista = BaseRepository.ListaTarjetaCredito();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaTarjetaCredito, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaTarjetaCredito, false);
            }
        }

        public static Response<List<BaseEntity>> ListaCiudad()
        {
            try
            {
                var lista = BaseRepository.ListaCiudad();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaCiudad, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaCiudad, false);
            }
        }

        public static Response<List<BaseEntity>> ListarParentesco()
        {
            try
            {
                var lista = BaseRepository.ListarParentesco();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListarParentesco, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarParentesco, false);
            }
        }

        public static Response<List<BaseEntity>> ListarGerente()
        {
            try
            {
                var lista = BaseRepository.ListarGerente();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListarGerente, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarGerente, false);
            }
        }

        public static Response<List<BaseEntity>> ListarSocio()
        {
            try
            {
                var lista = BaseRepository.ListarSocio();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListarSocio, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListarSocio, false);
            }
        }

        public static Response<List<BaseEntity>> ListaHospitales(int codiSucursal)
        {
            try
            {
                var lista = BaseRepository.ListaHospitales(codiSucursal);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListarSocio, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaHospitales, false);
            }
        }

        public static Response<List<BaseEntity>> ListaSecciones(int idContrato)
        {
            try
            {
                var lista = BaseRepository.ListaSecciones(idContrato);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaSecciones, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaSecciones, false);
            }
        }

        public static Response<List<BaseEntity>> ListaAreas(int idContrato)
        {
            try
            {
                var lista = BaseRepository.ListaAreas(idContrato);
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaAreas, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaAreas, false);
            }
        }

        public static Response<List<BaseEntity>> ListaUsuariosClaveAnuRei()
        {
            try
            {
                var lista = BaseRepository.ListaUsuariosClaveAnuRei();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaUsuariosClaveAnuRei, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuariosClaveAnuRei, false);
            }
        }

        public static Response<List<BaseEntity>> ListaUsuariosClaveControl()
        {
            try
            {
                var lista = BaseRepository.ListaUsuariosClaveControl();
                return new Response<List<BaseEntity>>(true, lista, Message.MsgCorrectoListaUsuariosClaveConrol, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<BaseEntity>>(false, null, Message.MsgExcListaUsuariosClaveConrol, false);
            }
        }
    }
}
