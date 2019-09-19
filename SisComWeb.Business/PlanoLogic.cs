using SisComWeb.Entity;
using SisComWeb.Repository;
using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SisComWeb.Business
{
    public class PlanoLogic
    {
        private static readonly string ColorVentaPuntosIntermedios = ConfigurationManager.AppSettings["colorVentaPuntosIntermedios"];
        private static readonly string ColorVentaReserva = ConfigurationManager.AppSettings["colorVentaReserva"];
        private static readonly string ColorVentaPaseCortesia = ConfigurationManager.AppSettings["colorVentaPaseCortesia"];

        public static Response<List<PlanoEntity>> MuestraPlano(PlanoRequest request)
        {
            try
            {
                string auxTipoLI = string.Empty;

                // Busca PlanoBus
                var buscarPlanoBus = PlanoRepository.BuscarPlanoBus(request.PlanoBus, request.CodiBus);
                if (buscarPlanoBus.Count == 0)
                    return new Response<List<PlanoEntity>>(false, buscarPlanoBus, Message.MsgValidaMuestraPlano, true);

                // Lista 'AsientosVendidos'
                var listarAsientosVendidos = PlanoRepository.ListarAsientosVendidos(request.NroViaje, request.CodiProgramacion, request.FechaProgramacion);

                // Lista 'AsientosBloqueados'
                var listarAsientosBloqueados = PlanoRepository.ListarAsientosBloqueados(request.NroViaje, request.CodiProgramacion, request.FechaProgramacion);

                // Obtiene 'OrdenOrigenPasajero'
                var ordenOrigenPasajero = PlanoRepository.ObtenerOrdenOficinaRuta(request.NroViaje, request.CodiOrigen);

                // Obtiene 'OrdenDestinoPasajero'
                var ordenDestinoPasajero = PlanoRepository.ObtenerOrdenOficinaRuta(request.NroViaje, request.CodiDestino);

                // Recorre cada registro
                for (int i = 0; i < buscarPlanoBus.Count; i++)
                {
                    bool auxBool = int.TryParse(buscarPlanoBus[i].Tipo, out int auxValue);
                    if (auxBool)
                    {
                        // Obtiene 'PrecioAsiento'
                        var obtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, request.HoraViaje, request.FechaViaje, request.CodiServicio, request.CodiEmpresa, buscarPlanoBus[i].Nivel.ToString());

                        // En caso de no encontrar resultado
                        if (obtenerPrecioAsiento.PrecioNormal == 0 && obtenerPrecioAsiento.PrecioMinimo == 0 && obtenerPrecioAsiento.PrecioMaximo == 0)
                        {
                            obtenerPrecioAsiento = PlanoRepository.ObtenerPrecioAsiento(request.CodiOrigen, request.CodiDestino, string.Empty, request.FechaViaje, request.CodiServicio, request.CodiEmpresa, buscarPlanoBus[i].Nivel.ToString());
                            buscarPlanoBus[i].PrecioNormal = obtenerPrecioAsiento.PrecioNormal;
                            buscarPlanoBus[i].PrecioMinimo = obtenerPrecioAsiento.PrecioMinimo;
                            buscarPlanoBus[i].PrecioMaximo = obtenerPrecioAsiento.PrecioMaximo;
                        }
                        else
                        {
                            buscarPlanoBus[i].PrecioNormal = obtenerPrecioAsiento.PrecioNormal;
                            buscarPlanoBus[i].PrecioMinimo = obtenerPrecioAsiento.PrecioMinimo;
                            buscarPlanoBus[i].PrecioMaximo = obtenerPrecioAsiento.PrecioMaximo;
                        }

                        foreach (PlanoEntity item in listarAsientosVendidos)
                        {
                            if (auxValue == item.NumeAsiento)
                            {
                                item.OrdenOrigen = PlanoRepository.ObtenerOrdenOficinaRuta(request.NroViaje, item.CodiOrigen);
                                var ordenDestino = PlanoRepository.ObtenerOrdenOficinaRuta(request.NroViaje, item.CodiDestino);

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
                                            if (buscarPlanoBus[i].NumeAsiento == 0)
                                            {
                                                if (item.FlagVenta == "X" || item.FlagVenta == "AB")
                                                    item.Color = PlanoRepository.ObtenerColorDestino(request.CodiServicio, item.CodiDestino);

                                                item.FlagVenta = "VI";
                                                MatchPlano(buscarPlanoBus[i], item, request);
                                            }
                                            else
                                            {
                                                if (short.Parse(buscarPlanoBus[i].OrdenOrigen) > short.Parse(item.OrdenOrigen))
                                                {
                                                    item.FlagVenta = "VI";
                                                    MatchPlano(buscarPlanoBus[i], item, request);
                                                }
                                            }
                                        }
                                    }
                                    else if (short.Parse(item.OrdenOrigen) == short.Parse(ordenOrigenPasajero))
                                    {
                                        MatchPlano(buscarPlanoBus[i], item, request);
                                    }
                                    else
                                    {
                                        MatchPlano(buscarPlanoBus[i], item, request);
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
                                        buscarPlanoBus[i].Color = ColorVentaPuntosIntermedios;
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
                                if (buscarPlanoBus[i].NumeAsiento == 0)
                                    buscarPlanoBus[i].NumeAsiento = item.NumeAsiento;
                            }
                        }
                    }
                    else
                    {
                        if (buscarPlanoBus[i].Tipo == "LI")
                            auxTipoLI = buscarPlanoBus[i].Tipo;
                        else
                        {
                            // Caso: "LI LI 'VA' LI LI"
                            if (buscarPlanoBus[i].Tipo == "VA"
                                && (i - 1) >= 0 && (i + 1) <= buscarPlanoBus.Count
                                && buscarPlanoBus[i - 1].Tipo == "LI" && buscarPlanoBus[i + 1].Tipo == "LI")

                                buscarPlanoBus[i].Tipo = "LI";
                            else
                            {
                                if (string.IsNullOrEmpty(auxTipoLI))
                                    buscarPlanoBus[i].Nivel = 1;
                                else
                                    buscarPlanoBus[i].Nivel = 2;
                            }
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
            entidad.Sigla = item.Sigla;
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
            entidad.TipoDocumento = item.TipoDocumento;
            entidad.NumeroDocumento = item.NumeroDocumento;
            entidad.Nombres = item.SplitNombres[0];
            entidad.ApellidoPaterno = item.SplitNombres[1];
            entidad.ApellidoMaterno = item.SplitNombres[2];
            entidad.Sexo = item.Sexo;
            entidad.TipoPago = item.TipoPago;
            entidad.Edad = item.Edad;
            entidad.Telefono = item.Telefono;
            entidad.ValeRemoto = item.ValeRemoto;
            entidad.CodiEsca = item.CodiEsca;
            entidad.CodiEmpresa = item.CodiEmpresa;

            // Busca 'Pasajero'
            if (!string.IsNullOrEmpty(entidad.TipoDocumento) && !string.IsNullOrEmpty(entidad.NumeroDocumento))
            {
                var buscaPasajero = ClientePasajeRepository.BuscaPasajero(VentaLogic.TipoDocumentoHomologado(entidad.TipoDocumento).ToString(), entidad.NumeroDocumento);
                entidad.FechaNacimiento = buscaPasajero.FechaNacimiento;
                entidad.Especial = buscaPasajero.Especial;

                entidad.Correo = buscaPasajero.Correo;

                if (entidad.Edad == 0 && string.IsNullOrEmpty(entidad.Telefono))
                {
                    entidad.Edad = buscaPasajero.Edad; // La tabla 'Tb_Boleto_Ruta' no contiene 'Edad'
                    entidad.Telefono = buscaPasajero.Telefono; // La tabla 'Tb_Boleto_Ruta' no contiene 'Telefono'
                }
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
                default:
                    {
                        entidad.IdVenta = item.IdVenta;
                        entidad.Color = item.Color;
                        entidad.FechaVenta = item.FechaVenta;
                        entidad.FechaViaje = item.FechaViaje;

                        switch (entidad.FlagVenta)
                        {
                            case "VI": // Venta intermedia
                                entidad.FechaVenta = "07/07/1777";
                                entidad.FechaViaje = string.Empty;
                                break;
                            case "7": // Pase de cortesía
                                entidad.Sigla = "PS";
                                entidad.Color = ColorVentaPaseCortesia;
                                break;
                            case "1": // Crédito
                                entidad.Sigla = "VC";
                                break;
                            case "R": // Reserva
                                entidad.Color = ColorVentaReserva;
                                break;
                        };
                    };
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

            // Consulta 'Reintegro'
            if (!string.IsNullOrEmpty(entidad.CodiEsca))
            {
                var consultaVentaReintegro = VentaRepository.ConsultaVentaReintegro(entidad.CodiEsca.Substring(1, 3), entidad.CodiEsca.Substring(5), entidad.CodiEmpresa.ToString(), entidad.CodiEsca.Substring(0, 1));
                entidad.ClavUsuarioReintegro = consultaVentaReintegro.CodiUsuario;
                entidad.SucVentaReintegro = consultaVentaReintegro.SucVenta;
                entidad.PrecVentaReintegro = consultaVentaReintegro.PrecioVenta;
                entidad.Nombres = consultaVentaReintegro.SplitNombre[0];
                entidad.ApellidoPaterno = consultaVentaReintegro.SplitNombre[1];
                entidad.ApellidoMaterno = consultaVentaReintegro.SplitNombre[2];
                entidad.TipoDocumento = consultaVentaReintegro.TipoDocumento;
                entidad.NumeroDocumento = consultaVentaReintegro.Dni;
                entidad.Edad = consultaVentaReintegro.Edad;
                entidad.RucContacto = consultaVentaReintegro.RucCliente;
                entidad.Telefono = consultaVentaReintegro.Telefono;

                // Busca 'Pasajero'
                if (!string.IsNullOrEmpty(entidad.TipoDocumento) && !string.IsNullOrEmpty(entidad.NumeroDocumento))
                {
                    var buscaPasajero = ClientePasajeRepository.BuscaPasajero(VentaLogic.TipoDocumentoHomologado(entidad.TipoDocumento).ToString(), entidad.NumeroDocumento);
                    entidad.FechaNacimiento = buscaPasajero.FechaNacimiento;
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
            }

            // Consulta 'FechaHoraReservacion'
            if (entidad.FlagVenta == "R")
            {
                var consultarFechaHoraReservacion = VentaRepository.ConsultarFechaHoraReservacion(int.Parse(entidad.IdVenta));
                entidad.FechaReservacion = consultarFechaHoraReservacion.FechaReservacion;
                entidad.HoraReservacion = consultarFechaHoraReservacion.HoraReservacion;
            }

            // Seteo 'Info'
            entidad.Info = entidad.CodiUsuario + " " + entidad.NomUsuario + " - " + entidad.NomPuntoVenta + " - ";
            switch (entidad.FlagVenta)
            {
                case "7":
                    entidad.Info += "(PS)" + " - ";
                    break;
                case "1":
                    entidad.Info += "(VC)" + " - ";
                    break;
            }
            entidad.Info += entidad.NomOrigen + " - " + entidad.NomDestino;

            // Seteo 'Observacion'
            if (entidad.FlagVenta == "I")
            {
                var consultaUsrValeR = VentaRepository.ConsultaUsrValeR(int.Parse(entidad.IdVenta));

                entidad.Observacion = "USR VALE: " + consultaUsrValeR;
            }
            entidad.Observacion += entidad.CodiEsca;
        }
    }
}
