using Autofac;
using SalesUpdater.Core;
using SalesUpdater.Interfaces.Core;

namespace SalesUpdater
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<File>().As<IFile>();
            
            return builder.Build();
        }
    }
}
