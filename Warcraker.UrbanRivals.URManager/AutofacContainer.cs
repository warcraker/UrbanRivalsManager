using Autofac;
using Warcraker.UrbanRivals.URManager.Bootstrap;

namespace Warcraker.UrbanRivals.URManager
{
    public static class AutofacContainer
    {
        public static readonly IContainer INSTANCE;

        static AutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.BuildTextFileLogger();
            builder.BuildViewModels();
            builder.BuildConfigurationManager();

            INSTANCE = builder.Build();
        }
    }
}
