using Core.Redis;
using Hangfire;
using Hangfire.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Services.Application
{
    public class JobServices : IJobServices
    {
        private readonly IRedisRepository redis;
        private readonly ILogger<JobServices> logger;
        private readonly IRecurringJobManager recurringJob;
        public JobServices(IRedisRepository redis, ILogger<JobServices> logger, IRecurringJobManager recurringJob)
        {
            this.redis = redis;
            this.logger = logger;
            this.recurringJob = recurringJob;
        }

        public void Execute()
        {
            recurringJob.AddOrUpdate(
                Guid.NewGuid().ToString(),
               new Job(typeof(JobServices).GetMethod("Print")),
                //Job.FromExpression(() => Console.WriteLine("==================> Now is time  ")),
                "0/10 * * * * ?",
                //redis.SetKeyValue(Guid.NewGuid().ToString(), DateTime.Now.ToString(), null)),
                // Cron.Minutely(),
                new RecurringJobOptions()
                {
                    //QueueName = "jservice",
                    //TimeZone = TimeZoneInfo.Utc
                });
        }

        //public void Print()
        //{
        //    Task.Run(() =>
        //    {
        //        var task = "==================> Now is time  " + DateTime.Now;
        //        Console.WriteLine(task);
        //    }).Wait();
        //}

        public void Print()
        {
            var task = "==================> Now is time  " + DateTime.Now;
            Console.WriteLine(task);
        }
    }
}
