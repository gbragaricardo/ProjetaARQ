using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Commands.DetailDoor;

namespace ProjetaARQ.Core.DI
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection ConfigureAllModules(this IServiceCollection services)
        {
            
            new DetailDoorModule().RegisterServices(services);

            return services;
        }
    }
}
