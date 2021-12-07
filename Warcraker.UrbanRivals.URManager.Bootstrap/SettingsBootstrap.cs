using Autofac;
using Warcraker.UrbanRivals.Interfaces;
using Warcraker.UrbanRivals.URManager.Configuration;

namespace Warcraker.UrbanRivals.URManager.Bootstrap
{
    public static class SettingsBootstrap
    {
        public static ContainerBuilder BuildWindowsSettingsManager(this ContainerBuilder builder)
        {
            builder.RegisterType<WindowsSettingsManager>().As<ISettingsManager>().SingleInstance();

            return builder;
        }
    }
}
