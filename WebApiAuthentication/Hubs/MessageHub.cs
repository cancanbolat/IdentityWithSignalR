using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApiAuthentication.Hubs
{
    [Authorize]
    public class MessageHub : Hub<IMessageHub>
    {
        public async Task SendMessage(string message)
        {
            await Clients.Others.ReceiverMessage(message);
        }
    }
}
