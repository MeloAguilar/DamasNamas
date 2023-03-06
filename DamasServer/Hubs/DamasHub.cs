using Microsoft.AspNetCore.SignalR;
using Entities;
using DamasServer.Models;

namespace DamasServer.Hubs
{
	public class DamasHub : Hub
	{
        public async Task MandarMovimiento (ObjetoSignalR paquete)
        {
            await Clients.User(paquete.IdJugador).SendAsync("RecibirMovimiento", paquete);
        }
    }
}
