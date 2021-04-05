using Autofac;
using Warcraker.UrbanRivals.URManager.ViewModels;

namespace Warcraker.UrbanRivals.URManager.Bootstrap
{
    public static class ViewModelsBootstrap
    {
        public static ContainerBuilder BuildViewModels(this ContainerBuilder builder)
        {
            builder.RegisterSingleton<StartupVM>();
            builder.RegisterSingleton<LanguageVM>();
            builder.RegisterSingleton<ApiVM>();

            return builder;
        }

        private static ContainerBuilder RegisterSingleton<T>(this ContainerBuilder builder) where T : class
        {
            builder.RegisterType<T>().AsSelf().SingleInstance();

            return builder;
        }
    }
}
