using Microsoft.AspNet.SignalR;

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

        public void ActualizarCodiProgramacion(string nroViaje, string FechaProgramacion, string codiProgramacion)
        {
            Clients.All.actualizarCodiProgramacion(nroViaje, FechaProgramacion, codiProgramacion);
        }
    }
}