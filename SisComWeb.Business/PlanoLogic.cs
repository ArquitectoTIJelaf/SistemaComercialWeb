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
                string auxTipoLI = "";

                // Busca PlanoBus
                var buscarPlanoBus = PlanoRepository.BuscarPlanoBus(request.PlanoBus);

                if (buscarPlanoBus.Count == 0)
                    return new Response<List<PlanoEntity>>(false, buscarPlanoBus, Message.MsgValidaMuestraPlano, false);

                // Lista 'AsientosOcupados'
                var listarAsientosOcupados = PlanoRepository.ListarAsientosOcupados(request.CodiProgramacion, request.FechaProgramacion, request.NroViaje, request.CodiOrigen, request.CodiDestino);

                // Recorre cada registro
                foreach (var entidad in buscarPlanoBus)
                {
                    // Obtiene 'NivelAsiento'
                    bool auxBool = int.TryParse(entidad.Tipo, out int auxValue);
                    if (auxBool)
                    {
                        var obtenerNivelAsiento = PlanoRepository.ObtenerNivelAsiento(request.CodiBus, auxValue);
                        if (!string.IsNullOrEmpty(obtenerNivelAsiento))
                            entidad.Nivel = int.Parse(obtenerNivelAsiento);

                        // Obtiene 'PrecioAsiento'
                        var obtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, request.HoraViaje, request.FechaViaje, request.CodiServicio, request.CodiEmpresa, entidad.Nivel.ToString());
                        
                        // En caso de no encontrar resultado
                        if (obtenerPrecioAsiento.PrecioNormal == 0 && obtenerPrecioAsiento.PrecioMinimo == 0 && obtenerPrecioAsiento.PrecioMaximo == 0)
                        {
                            obtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, string.Empty, request.FechaViaje, request.CodiServicio, request.CodiEmpresa, entidad.Nivel.ToString());
                            entidad.PrecioNormal = obtenerPrecioAsiento.PrecioNormal;
                            entidad.PrecioMinimo = obtenerPrecioAsiento.PrecioMinimo;
                            entidad.PrecioMaximo = obtenerPrecioAsiento.PrecioMaximo;
                        }
                        else
                        {
                            entidad.PrecioNormal = obtenerPrecioAsiento.PrecioNormal;
                            entidad.PrecioMinimo = obtenerPrecioAsiento.PrecioMinimo;
                            entidad.PrecioMaximo = obtenerPrecioAsiento.PrecioMaximo;
                        }

                        // Match entre los elementos del plano y los asientos ocupados
                        foreach (PlanoEntity ocu in listarAsientosOcupados)
                        {
                            if (auxValue == ocu.NumeAsiento)
                            {
                                entidad.NumeAsiento = ocu.NumeAsiento;
                                entidad.Nacionalidad = ocu.Nacionalidad;
                                entidad.PrecioVenta = ocu.PrecioVenta;
                                entidad.RecogeEn = ocu.RecogeEn;
                                entidad.FlagVenta = ocu.FlagVenta;
                                entidad.Sigla = ocu.Sigla;
                                entidad.Boleto = ocu.Boleto;
                                entidad.TipoBoleto = ocu.TipoBoleto;
                                // Para 'bgcVentaBus' y 'iconVentaBus'
                                if (ocu.IdVenta == "0") // Asiento ocupado pero no vendido(Bloqueado) -> scwsp_ListarAsientosOcupados
                                {
                                    entidad.IdVenta = string.Empty;
                                    entidad.Color = string.Empty;
                                    entidad.FechaVenta = string.Empty;
                                    entidad.FechaViaje = string.Empty;
                                }
                                else
                                {
                                    entidad.IdVenta = ocu.IdVenta;
                                    entidad.Color = ocu.Color;
                                    entidad.FechaVenta = ocu.FechaVenta;
                                    entidad.FechaViaje = ocu.FechaViaje;
                                }

                                // Busca 'Pasajero'
                                if (!string.IsNullOrEmpty(ocu.TipoDocumento) && !string.IsNullOrEmpty(ocu.NumeroDocumento))
                                {
                                    var buscaPasajero = ClientePasajeRepository.BuscaPasajero(ocu.TipoDocumento, ocu.NumeroDocumento);
                                    entidad.TipoDocumento = buscaPasajero.TipoDoc;
                                    entidad.NumeroDocumento = buscaPasajero.NumeroDoc;
                                    entidad.RucContacto = buscaPasajero.RucContacto;
                                    entidad.Nombres = buscaPasajero.NombreCliente;
                                    entidad.ApellidoPaterno = buscaPasajero.ApellidoPaterno;
                                    entidad.ApellidoMaterno = buscaPasajero.ApellidoMaterno;
                                    entidad.FechaNacimiento = buscaPasajero.FechaNacimiento;
                                    entidad.Edad = buscaPasajero.Edad;
                                    entidad.Telefono = buscaPasajero.Telefono;
                                    entidad.Sexo = buscaPasajero.Sexo;

                                    if (!string.IsNullOrEmpty(entidad.RucContacto))
                                    {
                                        var buscarEmpresa = ClientePasajeRepository.BuscarEmpresa(entidad.RucContacto);
                                        entidad.RazonSocial = buscarEmpresa.RazonSocial;
                                        entidad.Direccion = buscarEmpresa.Direccion;
                                    }
                                }

                                // Busca 'Acompaniante'
                                if (!string.IsNullOrEmpty(ocu.IdVenta) && ocu.IdVenta != "-1")
                                    entidad.ObjAcompaniante = PlanoRepository.BuscaAcompaniante(ocu.IdVenta);
                            }
                        }
                    }
                    else
                    {
                        if (entidad.Tipo == "LI")
                            auxTipoLI = entidad.Tipo;
                        else
                        {
                            if (string.IsNullOrEmpty(auxTipoLI))
                                entidad.Nivel = 1;
                            else
                                entidad.Nivel = 2;
                        }
                    }
                }

                return new Response<List<PlanoEntity>>(true, buscarPlanoBus, Message.MsgCorrectoMuestraPlano, true);
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(PlanoLogic)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return new Response<List<PlanoEntity>>(false, null, Message.MsgExcMuestraPlano, false);
            }
        }
    }
}
