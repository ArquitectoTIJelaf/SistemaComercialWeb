using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;

namespace SisComWeb.Business
{
    public class PlanoLogic
    {
        private static readonly string colorVentaPuntosIntermedios = "#2271B3";

        public static Response<List<PlanoEntity>> MuestraPlano(PlanoRequest request)
        {
            try
            {
                string auxTipoLI = string.Empty;

                // Busca PlanoBus
                var buscarPlanoBus = PlanoRepository.BuscarPlanoBus(request.PlanoBus);
                if (buscarPlanoBus.Count == 0)
                    return new Response<List<PlanoEntity>>(false, buscarPlanoBus, Message.MsgValidaMuestraPlano, false);

                // Lista 'AsientosVendidos'
                var listarAsientosVendidos = PlanoRepository.ListarAsientosVendidos(request.CodiProgramacion, request.NroViaje);

                // Lista 'AsientosBloqueados'
                var listarAsientosBloqueados = PlanoRepository.ListarAsientosBloqueados(request.NroViaje, request.CodiProgramacion, request.FechaProgramacion);

                // Obtiene 'OrdenOrigenPasajero'
                var ordenOrigenPasajero = PlanoRepository.ObtenerOrdenOficinaRuta(request.NroViaje, request.CodiOrigen, request.CodiOrigen);
                // Obtiene 'OrdenDestinoPasajero'
                var ordenDestinoPasajero = PlanoRepository.ObtenerOrdenOficinaRuta(request.NroViaje, request.CodiDestino, request.CodiDestino);

                // Valida 'OrdenOrigenPasajero' y 'OrdenDestinoPasajero'
                if (string.IsNullOrEmpty(ordenOrigenPasajero) || string.IsNullOrEmpty(ordenDestinoPasajero))
                    return new Response<List<PlanoEntity>>(false, buscarPlanoBus, Message.MsgErrorMuestraPlano, false);

                // Recorre cada registro
                foreach (var entidad in buscarPlanoBus)
                {
                    bool auxBool = int.TryParse(entidad.Tipo, out int auxValue);
                    if (auxBool)
                    {
                        // Obtiene 'NivelAsiento'
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

                        foreach (PlanoEntity item in listarAsientosVendidos)
                        {
                            if (auxValue == item.NumeAsiento)
                            {
                                item.OrdenOrigen = PlanoRepository.ObtenerOrdenOficinaRuta(request.NroViaje, item.CodiOrigen, item.CodiOrigen);
                                var ordenDestino = PlanoRepository.ObtenerOrdenOficinaRuta(request.NroViaje, item.CodiDestino, item.CodiDestino);

                                // Valida 'OrdenOrigen' y 'ordenDestino'
                                if (string.IsNullOrEmpty(item.OrdenOrigen) || string.IsNullOrEmpty(ordenDestino))
                                    continue;

                                if (short.Parse(ordenDestino) > short.Parse(ordenOrigenPasajero))
                                {
                                    if (short.Parse(item.OrdenOrigen) > short.Parse(ordenOrigenPasajero))
                                    {
                                        if ((short.Parse(item.OrdenOrigen)) > short.Parse(ordenDestinoPasajero))
                                        {

                                        }
                                        else if (short.Parse(item.OrdenOrigen) == short.Parse(ordenDestinoPasajero))
                                        {

                                        }
                                        else
                                        {
                                            if (entidad.NumeAsiento == 0)
                                            {
                                                if (item.FlagVenta == "X" || item.FlagVenta == "AB")
                                                    item.Color = PlanoRepository.ObtenerColorDestino(request.CodiServicio, item.CodiDestino);

                                                item.FlagVenta = "VI";
                                                MatchPlano(entidad, item);
                                            }
                                            else
                                            {
                                                if (short.Parse(entidad.OrdenOrigen) > short.Parse(item.OrdenOrigen)) {
                                                    item.FlagVenta = "VI";
                                                    MatchPlano(entidad, item);
                                                }
                                            }
                                        }
                                    }
                                    else if (short.Parse(item.OrdenOrigen) == short.Parse(ordenOrigenPasajero))
                                    {
                                        MatchPlano(entidad, item);
                                    }
                                    else
                                    {
                                        MatchPlano(entidad, item);
                                    }
                                }
                                else if (short.Parse(ordenDestino) == short.Parse(ordenOrigenPasajero))
                                {
                                    if (short.Parse(item.OrdenOrigen) > short.Parse(ordenOrigenPasajero))
                                    {

                                    }
                                    else if (short.Parse(item.OrdenOrigen) == short.Parse(ordenOrigenPasajero))
                                    {

                                    }
                                    else
                                        entidad.Color = colorVentaPuntosIntermedios;
                                }
                                else
                                {
                                    if (short.Parse(item.OrdenOrigen) > short.Parse(ordenOrigenPasajero))
                                    {

                                    }
                                    else if (short.Parse(item.OrdenOrigen) == short.Parse(ordenOrigenPasajero))
                                    {

                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }

                        foreach (PlanoEntity item in listarAsientosBloqueados)
                        {
                            if (auxValue == item.NumeAsiento)
                            {
                                if (entidad.NumeAsiento == 0)
                                    entidad.NumeAsiento = item.NumeAsiento;
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

        public static void MatchPlano(PlanoEntity entidad, PlanoEntity item)
        {
            entidad.NumeAsiento = item.NumeAsiento;
            entidad.Nacionalidad = item.Nacionalidad;
            entidad.PrecioVenta = item.PrecioVenta;
            entidad.RecogeEn = item.RecogeEn;
            entidad.FlagVenta = item.FlagVenta;
            entidad.Sigla = item.Sigla;
            entidad.Boleto = item.Boleto;
            entidad.TipoBoleto = item.TipoBoleto;
            entidad.CodiOrigen = item.CodiOrigen;
            entidad.OrdenOrigen = item.OrdenOrigen;

            if (item.FlagVenta == "X" || item.FlagVenta == "AB")
            {
                entidad.IdVenta = string.Empty;
                entidad.Color = string.Empty;
                entidad.FechaVenta = string.Empty;
                entidad.FechaViaje = string.Empty;
            }
            else
            {
                entidad.IdVenta = item.IdVenta;
                entidad.Color = item.Color;
                entidad.FechaVenta = item.FechaVenta;
                entidad.FechaViaje = item.FechaViaje;
            }

            // Busca 'Pasajero'
            if (!string.IsNullOrEmpty(item.TipoDocumento) && !string.IsNullOrEmpty(item.NumeroDocumento))
            {
                var buscaPasajero = ClientePasajeRepository.BuscaPasajero(item.TipoDocumento, item.NumeroDocumento);
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
                else
                {
                    entidad.RazonSocial = string.Empty;
                    entidad.Direccion = string.Empty;
                }
            }
            else
            {
                entidad.TipoDocumento = string.Empty;
                entidad.NumeroDocumento = string.Empty;
                entidad.RucContacto = string.Empty;
                entidad.Nombres = string.Empty;
                entidad.ApellidoPaterno = string.Empty;
                entidad.ApellidoMaterno = string.Empty;
                entidad.FechaNacimiento = string.Empty;
                entidad.Edad = 0;
                entidad.Telefono = string.Empty;
                entidad.Sexo = string.Empty;
                entidad.RazonSocial = string.Empty;
                entidad.Direccion = string.Empty;
            }

            // Busca 'Acompaniante'
            if (!string.IsNullOrEmpty(item.IdVenta))
                entidad.ObjAcompaniante = PlanoRepository.BuscaAcompaniante(item.IdVenta);
            else
                entidad.ObjAcompaniante = null;
        }
    }
}
