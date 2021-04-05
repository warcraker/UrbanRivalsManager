using Autofac;
using Warcraker.UrbanRivals.URManager.Configuration;

namespace Warcraker.UrbanRivals.URManager.Bootstrap
{
    public static class SettingsBootstrap
    {
        public static ContainerBuilder BuildConfigurationManager(this ContainerBuilder builder)
        {
            builder.RegisterType<SettingsManager>().AsSelf();

            return builder;
        }
    }
}
