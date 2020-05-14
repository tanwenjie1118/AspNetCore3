using Autofac;
using Infrastructure.Domain;
using Infrastructure.Singleton;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SignalR
{
    public static class SignalRLoggerExtension
    {
        //
        // 摘要:
        //     Formats and writes an informational log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   message:
        //     Format string of the log message in message template format. Example:
        //     "User {User} logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static async void LogSignalRInformation(this ILogger logger, string message, params object[] args)
        {
            if (AutofacContainer.Container != null)
            {
                var hubContext = AutofacContainer.Container.Resolve<IHubContext<MyHub>>();
                if (hubContext.Clients != null)
                {
                    logger.LogInformation(message, args);
                    await hubContext.Clients.All.SendAsync("ReceiveMessage", "System", message);
                }
            }
        }
    }
}
