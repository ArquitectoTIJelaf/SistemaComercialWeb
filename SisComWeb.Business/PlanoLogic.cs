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
                var response = new Response<List<PlanoEntity>>(false, null, "Error: MuestraPlano.", false);
                Response<string> resObtenerNivelAsiento;
                string auxTipoLI = "";

                // Busca PlanoBus
                var resBuscarPlanoBus = PlanoRepository.BuscarPlanoBus(request.PlanoBus);
                if (!resBuscarPlanoBus.Estado)
                {
                    response.Mensaje = resBuscarPlanoBus.Mensaje;
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
                        {
                            if (!string.IsNullOrEmpty(resObtenerNivelAsiento.Valor))
                                resBuscarPlanoBus.Valor[i].Nivel = int.Parse(resObtenerNivelAsiento.Valor);
                        }
                        else
                        {
                            response.Mensaje = resObtenerNivelAsiento.Mensaje;
                            return response;
                        }

                        // Obtiene 'PrecioAsiento'
                        var resObtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, request.HoraViaje, request.FechaViaje, request.CodiServicio, request.CodiEmpresa, resBuscarPlanoBus.Valor[i].Nivel.ToString());
                        if (resObtenerPrecioAsiento.Estado)
                        {
                            // En caso de no encontrar resultado
                            if (resObtenerPrecioAsiento.Valor.PrecioNormal == 0 && resObtenerPrecioAsiento.Valor.PrecioMinimo == 0 && resObtenerPrecioAsiento.Valor.PrecioMaximo == 0)
                            {
                                resObtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, "", request.FechaViaje, request.CodiServicio, request.CodiEmpresa, resBuscarPlanoBus.Valor[i].Nivel.ToString());
                                if (resObtenerPrecioAsiento.Estado)
                                {
                                    resBuscarPlanoBus.Valor[i].PrecioNormal = resObtenerPrecioAsiento.Valor.PrecioNormal;
                                    resBuscarPlanoBus.Valor[i].PrecioMinimo = resObtenerPrecioAsiento.Valor.PrecioMinimo;
                                    resBuscarPlanoBus.Valor[i].PrecioMaximo = resObtenerPrecioAsiento.Valor.PrecioMaximo;
                                }
                                else
                                {
                                    response.Mensaje = "Error: ObtenerPrecioAsiento sin Hora.";
                                    return response;
                                }
                            }
                            else
                            {
                                resBuscarPlanoBus.Valor[i].PrecioNormal = resObtenerPrecioAsiento.Valor.PrecioNormal;
                                resBuscarPlanoBus.Valor[i].PrecioMinimo = resObtenerPrecioAsiento.Valor.PrecioMinimo;
                                resBuscarPlanoBus.Valor[i].PrecioMaximo = resObtenerPrecioAsiento.Valor.PrecioMaximo;
                            }
                        }
                        else
                        {
                            response.Mensaje = resObtenerPrecioAsiento.Mensaje;
                            return response;
                        }
                    }
                    else
                    {
                        if (resBuscarPlanoBus.Valor[i].Tipo == "LI")
                        {
                            auxTipoLI = resBuscarPlanoBus.Valor[i].Tipo;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(auxTipoLI))
                                resBuscarPlanoBus.Valor[i].Nivel = 1;
                            else
                                resBuscarPlanoBus.Valor[i].Nivel = 2;
                        }
                    }
                }

                // Lista 'AsientosOcupados'
                var resListarAsientosOcupados = PlanoRepository.ListarAsientosOcupados(request.CodiProgramacion, request.FechaProgramacion, request.NroViaje, request.CodiOrigen, request.CodiDestino);
                if (!resListarAsientosOcupados.Estado)
                {
                    response.Mensaje = resListarAsientosOcupados.Mensaje;
                    return response;
                }

                // Recorre cada registro
                for (int i = 0; i < resListarAsientosOcupados.Valor.Count; i++)
                {
                    // Busca 'Pasajero'
                    if (!string.IsNullOrEmpty(resListarAsientosOcupados.Valor[i].TipoDocumento) && !string.IsNullOrEmpty(resListarAsientosOcupados.Valor[i].NumeroDocumento)) {
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
                            resListarAsientosOcupados.Valor[i].Sexo = resBuscaPasajero.Valor.Sexo;

                            if (!string.IsNullOrEmpty(resListarAsientosOcupados.Valor[i].RucContacto))
                            {
                                var resBuscarEmpresa = ClientePasajeRepository.BuscarEmpresa(resListarAsientosOcupados.Valor[i].RucContacto);
                                if (resBuscarEmpresa.Estado)
                                {
                                    resListarAsientosOcupados.Valor[i].RazonSocial = resBuscarEmpresa.Valor.RazonSocial;
                                    resListarAsientosOcupados.Valor[i].Direccion = resBuscarEmpresa.Valor.Direccion;
                                }
                            }
                        }
                        else
                        {
                            response.Mensaje = resBuscaPasajero.Mensaje;
                            return response;
                        }
                    }

                    // Busca 'Acompaniante'
                    if (!string.IsNullOrEmpty(resListarAsientosOcupados.Valor[i].IdVenta) && resListarAsientosOcupados.Valor[i].IdVenta != "-1")
                    {
                        var resBuscaAcompaniante = PlanoRepository.BuscaAcompaniante(resListarAsientosOcupados.Valor[i].IdVenta);
                        if (resBuscaAcompaniante.Estado)
                            resListarAsientosOcupados.Valor[i].ObjAcompanianate = resBuscaAcompaniante.Valor;
                        else
                        {
                            response.Mensaje = resBuscaAcompaniante.Mensaje;
                            return response;
                        }
                    }
                }

                // Match entre los elementos del plano y los asientos ocupados
                foreach (PlanoEntity ele in resBuscarPlanoBus.Valor)
                {
                    bool auxBool = byte.TryParse(ele.Tipo, out byte auxValue);
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
                                // Para 'bgcVentaBus', 'iconVentaBus' y 'showNombrePasajero'
                                if (ocu.FlagVenta == "AB" || ocu.IdVenta == "0") // Asiento ocupado pero no vendido (bloqueado).
                                    ele.FechaVenta = string.Empty;
                                else
                                    ele.FechaVenta = ocu.FechaVenta;
                                // ------------------
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
                                ele.Sexo = ocu.Sexo;
                                ele.Sigla = ocu.Sigla;
                                ele.RazonSocial = ocu.RazonSocial;
                                ele.Direccion = ocu.Direccion;
                                ele.Boleto = ocu.Boleto;
                                ele.TipoBoleto = ocu.TipoBoleto;
                                ele.IdVenta = ocu.IdVenta;
                                ele.ObjAcompanianate = ocu.ObjAcompanianate;
                            }
                        }
                    }
                }

                response.EsCorrecto = true;
                response.Valor = resBuscarPlanoBus.Valor;
                response.Mensaje = Message.MsgCorrectoMuestraPlano;
                response.Estado = true;

                return response;
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PlanoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<PlanoEntity>>(false, null, Message.MsgErrExcMuestraPlano, false);
            }
        }
    }
}
