﻿using Consul;
using Hal.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;

namespace Hal.Core.Extensions
{
    public static class ConsulBuilderExtension
    {
        public static IApplicationBuilder RegisterConsul<TOption>(this IApplicationBuilder app, IHostApplicationLifetime lifetime, TOption option)
            where TOption : ConsulServiceOption
        {
            var consulClient = new ConsulClient(x =>
            {
                // consul server host
                x.Address = new Uri(option.ServerHost);
            });

            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = option.ClientName,// service name
                Address = option.ClientIp, // ip
                Port = option.ClientPort, // port
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),// 
                    Interval = TimeSpan.FromSeconds(10),// healthcheck interval
                    HTTP = option.HealthCheckHttp,// address of healthcheck
                    Timeout = TimeSpan.FromSeconds(5),
                }
            };

            try
            {
                // register
                consulClient.Agent.ServiceRegister(registration).Wait();

                // deregister
                lifetime.ApplicationStopping.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                });
            }
            catch (Exception)
            {
            }

            return app;
        }
    }
}
