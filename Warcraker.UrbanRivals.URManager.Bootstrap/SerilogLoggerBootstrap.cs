using System;
using Autofac;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace Warcraker.UrbanRivals.URManager.Bootstrap
{
    public static class SerilogLoggerBootstrap
    {
        public static ContainerBuilder BuildSerilogTextFileLogger(this ContainerBuilder builder)
        {
            DateTime today = DateTime.Today;

            LoggerConfiguration config = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File($@"logs\{today.Year}\{today.Month}\log.txt", rollingInterval: RollingInterval.Day);

            builder.RegisterSerilog(config);

            return builder;
        }
    }
}
