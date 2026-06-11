using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Commands.DetailDoor;
using ProjetaARQ.Commands.FamiliesPanel.MVVM;
using ProjetaARQ.Core.Services;

namespace ProjetaARQ.Core.DI
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection ConfigureAllModules(this IServiceCollection services)
        {
            services.AddSingleton<IAppTelemetry, AppTelemetry>();

            services.AddSingleton<FamiliesViewModel>();
            services.AddSingleton<FamiliesView>();
            new DetailDoorModule().RegisterServices(services);

            return services;
        }
    }
}
