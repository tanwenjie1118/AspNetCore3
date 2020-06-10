using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Infrastructure.Domain
{
    public class MyHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// when connected to do
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            //TODO..
            await Clients.All.SendAsync("ReceiveMessage", "SignalR", "connect ok");
            await base.OnConnectedAsync();
        }

        /// <summary>
        ///  when lose connection to do
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            //TODO..
            await Clients.All.SendAsync("ReceiveMessage", "SignalR", "disconnect ok");
            await base.OnDisconnectedAsync(ex);
        }
    }
}
