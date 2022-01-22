using Autofac;
using Warcraker.UrbanRivals.Interfaces;
using Warcraker.UrbanRivals.URManager.ViewModels;

namespace Warcraker.UrbanRivals.URManager.Bootstrap
{
    public static class ViewModelsBootstrap
    {
        public static ContainerBuilder BuildViewModels(this ContainerBuilder builder)
        {
            builder.RegisterType<WindowsStartupVm>().AsSelf().SingleInstance();
            builder.RegisterSingleton<ILanguageVm, WindowsLanguageVm>();
            builder.RegisterType<ApiVm>().AsSelf().SingleInstance(); // TODO define interface for ApiVM
            builder.RegisterType<MailExceptionHandlerVm>().As<ExceptionHandlerVmBase>(); 

            return builder;
        }

        private static ContainerBuilder RegisterSingleton<TContract, TImplementation>(this ContainerBuilder builder)
            where TContract : class 
            where TImplementation : class
        {
            builder.RegisterType<TImplementation>().As<TContract>().SingleInstance();

            return builder;
        }
    }
}
