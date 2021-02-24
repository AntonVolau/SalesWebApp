using SalesUpdater.Web.Data.Contracts.Services;
using SalesUpdater.Web.Data.Contracts.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;

namespace SalesUpdater.Web
{
    public static class IocConfigurator
    {
        public static void ConfigureIocUnityContainer()
        {
            IUnityContainer unityContainer = new UnityContainer();

            RegisterServices(unityContainer);

            DependencyResolver.SetResolver(new UnitDependencyResolver(unityContainer));
        }

        private static void RegisterServices(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<IProductService, ProductService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IClientService, ClientService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ISaleService, SaleService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IManagerService, ManagerService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterInstance(AutoMapper.CreateConfiguration().CreateMapper());

        }
    }

    public class UnitDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _unityContainer;
        private readonly HashSet<Type> excludedTypes;
        public UnitDependencyResolver(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            excludedTypes = new HashSet<Type>();
            StandardExclusions();
        }

        public object GetService(Type serviceType)
        {
          //  if (typeof(IController).IsAssignableFrom(serviceType))
          //  {
          //      return _unityContainer.Resolve(serviceType);
          //  }
          //
          //  try
          //  {
          //      return excludedTypes.Contains(serviceType) ? null : _unityContainer.Resolve(serviceType);
          //  }
          //  catch (ResolutionFailedException)
          //  {
          //      return null;
          //  }

           try
           {
               return _unityContainer.Resolve(serviceType);
           }
           catch (Exception e)
           {
               return null;
           }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _unityContainer.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

        public void Exclude<T>()
        {
            Exclude(typeof(T));
        }

        public void Exclude(Type type)
        {
            excludedTypes.Add(type);
        }

        private void StandardExclusions()
        {
            Exclude<System.Web.Mvc.IControllerFactory>();
            Exclude<System.Web.Mvc.IControllerActivator>();
            Exclude<System.Web.Mvc.ITempDataProviderFactory>();
            Exclude<System.Web.Mvc.ITempDataProvider>();
            Exclude<System.Web.Mvc.Async.IAsyncActionInvokerFactory>();
            Exclude<System.Web.Mvc.IActionInvokerFactory>();
            Exclude<System.Web.Mvc.Async.IAsyncActionInvoker>();
            Exclude<System.Web.Mvc.IActionInvoker>();
            Exclude<System.Web.Mvc.IViewPageActivator>();
            Exclude<System.Web.Mvc.ModelMetadataProvider>();
        }
    }
}