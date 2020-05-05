using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
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
            logger.LogInformation("do hosted task at : " + DateTime.Now);
        }
    }
}
