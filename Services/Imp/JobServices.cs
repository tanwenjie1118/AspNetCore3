using Core.Redis;
using Hangfire;
using Hangfire.Common;
using System;
using System.Threading.Tasks;

namespace Services.Application
{
    public class JobServices : IJobServices
    {
        private readonly IRedisRepository redis;
        private readonly IRecurringJobManager recurringJob;
        public JobServices(IRedisRepository redis, IRecurringJobManager recurringJob)
        {
            this.redis = redis;
            this.recurringJob = recurringJob;
        }

        public void Execute()
        {
            recurringJob.AddOrUpdate(
                Guid.NewGuid().ToString(),
                Job.FromExpression(
                () => DoSomething().Wait()
                // redis.SetKeyValue(Guid.NewGuid().ToString(), DateTime.Now.ToString(), null)
                ),
               "*/5 * * * * ?",//   Cron.Minutely(),
                new RecurringJobOptions()
                {
                    QueueName = "jservice",
                    TimeZone = TimeZoneInfo.Utc
                });
        }


        public Task DoSomething()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("============================== Now is : " + DateTime.Now.ToLongTimeString());
            });
        }
    }
}
