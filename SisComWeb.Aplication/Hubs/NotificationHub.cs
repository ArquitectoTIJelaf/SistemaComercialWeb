using Microsoft.AspNet.SignalR;

namespace SisComWeb.Aplication.Hubs
{
    public class NotificationHub : Hub
    {

        public void Bloquear(string nroViaje, string fechaViaje, string nroAsiento)
        {
            Clients.All.bloquearBus(nroViaje, fechaViaje, nroAsiento);
        }

        public void Liberar(string nroViaje, string fechaViaje, string nroAsiento)
        {
            Clients.All.liberarBus(nroViaje, fechaViaje, nroAsiento);
        }
    }
}