using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public class PlanoLogic
    {
        public static Response<List<PlanoEntity>> MuestraPlano(PlanoRequest request)
        {
            try
            {
                var response = new Response<List<PlanoEntity>>(false, null, "", false);
                Response<string> resObtenerNivelAsiento = new Response<string>(false, null, "", false);

                // Busca PlanoBus
                var resBuscarPlanoBus = PlanoRepository.BuscarPlanoBus(request.PlanoBus);
                if (resBuscarPlanoBus.Estado)
                    response.Mensaje += resBuscarPlanoBus.Mensaje;
                else
                {
                    response.Mensaje += "Error: BuscarPlanoBus. ";
                    return response;
                }

                // Recorre cada registro
                for (int i = 0; i < resBuscarPlanoBus.Valor.Count; i++)
                {
                    // Obtiene 'NivelAsiento'
                    bool auxBool = int.TryParse(resBuscarPlanoBus.Valor[i].Tipo, out int auxValue);
                    if (auxBool)
                    {
                        resObtenerNivelAsiento = PlanoRepository.ObtenerNivelAsiento(request.CodiBus, auxValue);
                        if (resObtenerNivelAsiento.Estado)
                            resBuscarPlanoBus.Valor[i].Nivel = int.Parse(resObtenerNivelAsiento.Valor);
                        else
                        {
                            response.Mensaje += "Error: ObtenerNivelAsiento. ";
                            return response;
                        }

                        // Obtiene 'PrecioAsiento'
                        var resObtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, request.HoraViaje, request.FechaViaje, request.CodiServicio, request.CodiEmpresa, resObtenerNivelAsiento.Valor);
                        if (resObtenerPrecioAsiento.Estado)
                        {
                            resBuscarPlanoBus.Valor[i].PrecioNormal = resObtenerPrecioAsiento.Valor.PrecioNormal;
                            resBuscarPlanoBus.Valor[i].PrecioMinimo = resObtenerPrecioAsiento.Valor.PrecioMinimo;
                            resBuscarPlanoBus.Valor[i].PrecioMaximo = resObtenerPrecioAsiento.Valor.PrecioMaximo;
                        }
                        else
                        {
                            response.Mensaje += "Error: ObtenerPrecioAsiento. ";
                            return response;
                        }
                    }
                }

                // Lista 'AsientosOcupados'
                var resListarAsientosOcupados = PlanoRepository.ListarAsientosOcupados(request.CodiProgramacion, request.FechaProgramacion, request.NroViaje, request.CodiOrigen, request.CodiDestino);
                if (resListarAsientosOcupados.Estado)
                {
                    response.Mensaje += resListarAsientosOcupados.Mensaje;
                }
                else
                {
                    response.Mensaje += "Error: ListarAsientosOcupados. ";
                    return response;
                }

                // Recorre cada registro
                for (int i = 0; i < resListarAsientosOcupados.Valor.Count; i++)
                {
                    // Busca 'Pasajero'
                    var resBuscaPasajero = ClientePasajeRepository.BuscaPasajero(resListarAsientosOcupados.Valor[i].TipoDocumento, resListarAsientosOcupados.Valor[i].NumeroDocumento);
                    if (resBuscaPasajero.Estado)
                    {
                        resListarAsientosOcupados.Valor[i].TipoDocumento = resBuscaPasajero.Valor.TipoDoc;
                        resListarAsientosOcupados.Valor[i].NumeroDocumento = resBuscaPasajero.Valor.NumeroDoc;
                        resListarAsientosOcupados.Valor[i].RucContacto = resBuscaPasajero.Valor.RucContacto;
                        resListarAsientosOcupados.Valor[i].Nombres = resBuscaPasajero.Valor.NombreCliente;
                        resListarAsientosOcupados.Valor[i].ApellidoPaterno = resBuscaPasajero.Valor.ApellidoPaterno;
                        resListarAsientosOcupados.Valor[i].ApellidoMaterno = resBuscaPasajero.Valor.ApellidoMaterno;
                        resListarAsientosOcupados.Valor[i].FechaNacimiento = resBuscaPasajero.Valor.FechaNacimiento;
                        resListarAsientosOcupados.Valor[i].Edad = resBuscaPasajero.Valor.Edad;
                        resListarAsientosOcupados.Valor[i].Telefono = resBuscaPasajero.Valor.Telefono;
                    }
                    else
                    {
                        response.Mensaje += "Error: BuscaPasajero. ";
                        return response;
                    }
                }

                // Match entre los elementos del plano y los asientos ocupados
                foreach (PlanoEntity ele in resBuscarPlanoBus.Valor)
                {
                    bool auxBool = int.TryParse(ele.Tipo, out int auxValue);
                    if (auxBool)
                    {
                        foreach (PlanoEntity ocu in resListarAsientosOcupados.Valor)
                        {
                            if (ocu.NumeAsiento == auxValue)
                            {
                                ele.NumeAsiento = ocu.NumeAsiento;
                                ele.TipoDocumento = ocu.TipoDocumento;
                                ele.NumeroDocumento = ocu.NumeroDocumento;
                                ele.RucContacto = ocu.RucContacto;
                                ele.FechaViaje = ocu.FechaViaje;
                                ele.FechaVenta = ocu.FechaVenta;
                                ele.Nacionalidad = ocu.Nacionalidad;
                                ele.PrecioVenta = ocu.PrecioVenta;
                                ele.RecogeEn = ocu.RecogeEn;
                                ele.Color = ocu.Color;
                                ele.FlagVenta = ocu.FlagVenta;
                                ele.Nombres = ocu.Nombres;
                                ele.ApellidoPaterno = ocu.ApellidoPaterno;
                                ele.ApellidoMaterno = ocu.ApellidoMaterno;
                                ele.FechaNacimiento = ocu.FechaNacimiento;
                                ele.Edad = ocu.Edad;
                                ele.Telefono = ocu.Telefono;
                            }
                        }
                    }
                }

                response.EsCorrecto = true;
                response.Valor = resBuscarPlanoBus.Valor;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PlanoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<PlanoEntity>>(false, null, Message.MsgErrExcListPlano, false);
            }
        }
    }
}
