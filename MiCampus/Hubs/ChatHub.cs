using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MiCampus.Hubs
{
    // El ChatHub será el punto central para la comunicación del chat.
    public class ChatHub : Hub
    {
        // Este método puede ser llamado por un cliente (ej. el emisor)
        // para enviar un mensaje a todos los demás clientes.
        // También puedes guardar el mensaje en la base de datos aquí.
        public async Task SendMessage(string emisorId, string receptorId, string texto)
        {
            // Podrías guardar el mensaje en la base de datos aquí
            // ...

            // Luego, envía el mensaje al cliente receptor.
            // Los clientes deben usar "ReceiveMessage" para escuchar los mensajes.
            await Clients.User(receptorId).SendAsync("ReceiveMessage", emisorId, receptorId, texto);

            // Si quisieras enviarlo a todos los clientes conectados, usarías:
            // await Clients.All.SendAsync("ReceiveMessage", emisorId, receptorId, texto);
        }
    }
}