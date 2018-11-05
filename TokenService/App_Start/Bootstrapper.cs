using System.Web.Http;

namespace TokenService
{
    public class Bootstrapper
    {
        public static void Run()
        {
            // Configure Autofac Dependency Resolver
            DependencyInjectionResolver.Initialize(GlobalConfiguration.Configuration);
        }
    }
}