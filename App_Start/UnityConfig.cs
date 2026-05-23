using MicroProjectApplication.Services;
using MicroProjectApplication.Services.Interfaces;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace MicroProjectApplication
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register components here
            container.RegisterType<ISsoService, SsoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuditLogger, AuditLogger>(new HierarchicalLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
