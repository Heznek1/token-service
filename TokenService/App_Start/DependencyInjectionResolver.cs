using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using TokenService.Helpers;
using TokenService.Helpers.Interfaces;
using TokenService.Interfaces;
using TokenService.Repositories;

namespace TokenService
{
    public class DependencyInjectionResolver
    {
        private static IContainer container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterService(new ContainerBuilder()));
        }

        private static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterService(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register components
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<SubscribersEvents>().As<ISubscribersEvents>();

            // Set dependency to Autofac
            container = builder.Build();

            return container;
        }
    }
}