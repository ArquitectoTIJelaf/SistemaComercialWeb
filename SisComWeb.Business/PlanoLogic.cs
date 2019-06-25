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
                                                MatchPlano(entidad, item, request);
                                            }
                                            else
                                            {
                                                if (short.Parse(entidad.OrdenOrigen) > short.Parse(item.OrdenOrigen)) {
                                                    item.FlagVenta = "VI";
                                                    MatchPlano(entidad, item, request);
                                                }
                                            }
                                        }
                                    }
                                    else if (short.Parse(item.OrdenOrigen) == short.Parse(ordenOrigenPasajero))
                                    {
                                        MatchPlano(entidad, item, request);
                                    }
                                    else
                                    {
                                        MatchPlano(entidad, item, request);
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

        public static void MatchPlano(PlanoEntity entidad, PlanoEntity item, PlanoRequest request)
        {
            entidad.NumeAsiento = item.NumeAsiento;
            entidad.Nacionalidad = item.Nacionalidad;
            entidad.PrecioVenta = item.PrecioVenta;
            entidad.RecogeEn = item.RecogeEn;
            entidad.FlagVenta = item.FlagVenta;

            switch (entidad.FlagVenta) {
                case "7": // Pase de cortesía
                    entidad.Sigla = "PS";
                    break;
                case "1": // Crédito
                    entidad.Sigla = "VC";
                    break;
                default:
                    entidad.Sigla = item.Sigla;
                    break;
            }

            entidad.Boleto = item.Boleto;
            entidad.TipoBoleto = item.TipoBoleto;
            entidad.CodiOrigen = item.CodiOrigen;
            entidad.OrdenOrigen = item.OrdenOrigen;
            entidad.CodiDestino = item.CodiDestino;
            entidad.NomOrigen = item.NomOrigen;
            entidad.NomDestino = item.NomDestino;
            entidad.CodiPuntoVenta = item.CodiPuntoVenta;
            entidad.NomPuntoVenta = item.NomPuntoVenta;
            entidad.CodiUsuario = item.CodiUsuario;
            entidad.NomUsuario = item.NomUsuario;
            entidad.RucContacto = item.RucContacto;
            entidad.NumeSolicitud = item.NumeSolicitud;
            entidad.HoraVenta = item.HoraVenta;
            entidad.EmbarqueCod = item.EmbarqueCod;
            entidad.EmbarqueDir = item.EmbarqueDir;
            entidad.EmbarqueHora = item.EmbarqueHora;
            entidad.ImpManifiesto = item.ImpManifiesto;
            entidad.CodiSucursal = item.CodiSucursal;
            entidad.TipoDocumento = item.TipoDocumento.TrimStart('0'); // Formato de la vista: Combo 'tiposDoc' = {"1", "4", "7"}
            entidad.NumeroDocumento = item.NumeroDocumento;
            entidad.Nombres = item.SplitNombres[0];
            entidad.ApellidoPaterno = item.SplitNombres[1];
            entidad.ApellidoMaterno = item.SplitNombres[2];
            entidad.Sexo = item.Sexo;

            entidad.TipoPago = item.TipoPago;

            if (!string.IsNullOrEmpty(entidad.TipoDocumento) && !string.IsNullOrEmpty(entidad.NumeroDocumento))
            {
                // Busca 'Pasajero'
                var buscaPasajero = ClientePasajeRepository.BuscaPasajero(entidad.TipoDocumento, entidad.NumeroDocumento);
                entidad.FechaNacimiento = buscaPasajero.FechaNacimiento;
                entidad.Edad = buscaPasajero.Edad; // La tabla 'Tb_Boleto_Ruta' no contiene 'Edad'
                entidad.Telefono = buscaPasajero.Telefono; // La tabla 'Tb_Boleto_Ruta' no contiene 'Telefono'
            }

            switch (entidad.FlagVenta)
            {
                case "X": // Asiento bloqueado
                case "AB": // Asiento bloqueado por módulo
                    entidad.IdVenta = string.Empty;
                    entidad.Color = string.Empty;
                    entidad.FechaVenta = string.Empty;
                    entidad.FechaViaje = string.Empty;
                    break;
                case "VI": // Venta intermedia
                    entidad.IdVenta = item.IdVenta;
                    entidad.Color = item.Color;
                    entidad.FechaVenta = "07/07/1777";
                    entidad.FechaViaje = string.Empty;
                    break;
                default:
                    entidad.IdVenta = item.IdVenta;
                    entidad.Color = item.Color;
                    entidad.FechaVenta = item.FechaVenta;
                    entidad.FechaViaje = item.FechaViaje;
                    break;
            }

            // Busca 'Empresa'
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

            // Busca 'Acompaniante'
            if (!string.IsNullOrEmpty(entidad.IdVenta))
                entidad.ObjAcompaniante = PlanoRepository.BuscaAcompaniante(entidad.IdVenta);
            else
                entidad.ObjAcompaniante = null;

            // Consulta 'Reintegro'
            if (!string.IsNullOrEmpty(entidad.Boleto))
            {
                var consultarReintegro = PlanoRepository.ConsultarReintegro(short.Parse(entidad.Boleto.Substring(0, 3)), int.Parse(entidad.Boleto.Substring(4)), request.CodiEmpresa, entidad.TipoBoleto);
                entidad.ClavUsuarioReintegro = consultarReintegro.ClavUsuario;
                entidad.SucVentaReintegro = consultarReintegro.SucVenta;
                entidad.PrecVentaReintegro = consultarReintegro.PrecVenta;
            }
            
        }
    }
}
