using Autofac;
using Hal.Core.SignalR;
using Hal.Infrastructure.Domain;
using Hal.Infrastructure.Singleton;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hal.Tasks
{
    public class SimpleHostedTask : IHostedService
    {
        private Timer timer;
        private readonly ILogger<SimpleHostedTask> logger;

        public SimpleHostedTask(ILogger<SimpleHostedTask> logger)
        {
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(RealWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            return Task.CompletedTask;
        }

        private void RealWork(object x)
        {
            var task = "self hosted timer job was finished at " + DateTime.Now;
            logger.LogSignalRInformation(task);
        }
    }
}
