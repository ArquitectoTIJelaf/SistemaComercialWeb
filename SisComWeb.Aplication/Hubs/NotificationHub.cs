using Microsoft.AspNet.SignalR;
using SisComWeb.Aplication.Models;

namespace SisComWeb.Aplication.Hubs
{
    public class NotificationHub : Hub
    {

        public void BloquearAsiento(string nroViaje, string FechaProgramacion, string nroAsiento)
        {
            Clients.All.bloquearAsiento(nroViaje, FechaProgramacion, nroAsiento);
        }

        public void LiberarAsiento(string nroViaje, string FechaProgramacion, string nroAsiento)
        {
            Clients.All.liberarAsiento(nroViaje, FechaProgramacion, nroAsiento);
        }

        public void LiberarArregloAsientos(string nroViaje, string FechaProgramacion, int[] ArregloNroAsientos)
        {
            Clients.All.liberarArregloAsientos(nroViaje, FechaProgramacion, ArregloNroAsientos);
        }

        public void ActualizarTurnoPlano(string nroViaje, string FechaProgramacion, VentaResponse ventaResponse)
        {
            Clients.All.actualizarTurnoPlano(nroViaje, FechaProgramacion, ventaResponse);
        }
    }
}