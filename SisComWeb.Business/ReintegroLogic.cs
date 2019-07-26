using SisComWeb.Entity;
using SisComWeb.Entity.Peticiones.Request;
using SisComWeb.Entity.Peticiones.Response;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public class ReintegroLogic
    {
        public static Response<ReintegroEntity> VentaConsultaF12(ReintegroRequest request)
        {
            try
            {
                var valor = ReintegroRepository.VentaConsultaF12(request);
                //Datos adicionales: 'FechaNacimiento'
                var clientePasaje = ClientePasajeRepository.BuscaPasajero(valor.TipoDocumento, valor.Dni);
                valor.FechaNac = clientePasaje.FechaNacimiento;

                if (valor.IdVenta == 0)
                {
                    return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12NoExiste, true);
                }
                //Setea Razón Social y Dirección con el RUC
                var buscarEmpresa = ClientePasajeRepository.BuscarEmpresa(valor.RucCliente);
                valor.RazonSocial = buscarEmpresa.RazonSocial;
                valor.Direccion = buscarEmpresa.Direccion;
                //Verifica si el boleto está en Fecha Abierta
                if (valor.CodiProgramacion != 0)
                {
                    var programacion = ReintegroRepository.DatosProgramacion(valor.CodiProgramacion);
                    //Verifica si no tiene Programación, caso contrario setea Ruta y Servicio
                    if(programacion.CodiRuta != 0 && programacion.CodiServicio != 00)
                    {
                        valor.CodiRuta = programacion.CodiRuta;
                        valor.CodiServicio = programacion.CodiServicio;
                    } else
                    {
                        valor.CodiError = 2;
                        return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12SinProgramacion, true);
                    }
                    //Verfica si tiene Nota de Crédito
                    var notaCredito = VentaRepository.VerificaNC(valor.IdVenta);
                    if (notaCredito != 0)
                    {
                        valor.CodiError = 3;
                        return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12NotaCredito + " " + notaCredito, true);
                    }
                    //Verfica si esta como Reintegro
                    if (valor.FlagVenta == "O")
                    {
                        valor.CodiError = 4;
                        return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12EsReintegro, true);
                    }
                    //Verfica si ya tiene adjunto un Reintegro
                    if (valor.CodiEsca != "")
                    {
                        valor.CodiError = 5;
                        return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12TieneReintegro, true);
                    }
                }
                else
                {
                    valor.CodiError = 1;
                    return new Response<ReintegroEntity>(false, valor, Message.MsgExcF12EsFechaAbierta, true);
                }
                return new Response<ReintegroEntity>(true, valor, Message.MsgCorrectoVentaConsultaF12, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(FechaAbiertaLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<ReintegroEntity>(false, null, Message.MsgExcVentaConsultaF12, false);
            }
        }

        public static Response<List<SelectReintegroEntity>> ListaOpcionesModificacion()
        {
            try
            {
                var lista = ReintegroRepository.ListaOpcionesModificacion();
                return new Response<List<SelectReintegroEntity>>(true, lista, Message.MsgCorrectoListaOpcionesModificacion, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<SelectReintegroEntity>>(false, null, Message.MsgExcListaOpcionesModificacion, false);
            }
        }
        
        public static Response<bool> ValidaExDni(string documento)
        {
            try
            {
                var res = ReintegroRepository.ValidaExDni(documento);
                var mensaje = (res) ? Message.MsgValidaExDni : string.Empty;
                return new Response<bool>(true, res, mensaje, false);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<bool>(false, false, Message.MsgExcValidaExDni, false);
            }
        }
                
        public static Response<int> SaveReintegro(ReintegroVentaRequest filtro)
        {
            try
            {
                var res = ReintegroRepository.SaveReintegro(filtro); 
                return (res == 0) ? new Response<int>(true, 0, Message.MsgNoVentaReintegro, true) : new Response<int>(true, res, string.Empty, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(BaseLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<int>(false, 0, Message.MsgExcVentaReintegro, false);
            }
        }
    }
}
