using Core.Redis;
using Hangfire;
using Hangfire.Common;
using System;

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
                Guid.NewGuid().ToString(), Job.FromExpression(() => redis.SetKeyValue(Guid.NewGuid().ToString(), DateTime.Now.ToString(), null)),
                Cron.Minutely(),
                new RecurringJobOptions() { QueueName = "jservice", TimeZone = TimeZoneInfo.Local });
        }
    }
}
