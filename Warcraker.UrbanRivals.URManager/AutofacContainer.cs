using Autofac;
using Warcraker.UrbanRivals.URManager.Bootstrap;

namespace Warcraker.UrbanRivals.URManager
{
    public static class AutofacContainer
    {
        public static readonly IContainer INSTANCE = GetInstance();

        private static IContainer GetInstance()
        {
            IContainer container;
            var builder = new ContainerBuilder();
            builder.BuildSerilogTextFileLogger();
            builder.BuildViewModels();
            builder.BuildWindowsSettingsManager();
            container = builder.Build();

            return container;
        }
    }
}
