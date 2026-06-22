using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Commands.DebugFluentUi;
using ProjetaARQ.Commands.DetailDoor;
using ProjetaARQ.Commands.FamiliesPanel.MVVM;
using ProjetaARQ.Core.Services;
using System.Diagnostics;

namespace ProjetaARQ.Core.DI
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection ConfigureAllModules(this IServiceCollection services)
        {
            services.AddSingleton<IAppTelemetry, AppTelemetry>();
            services.AddScoped<IRevitContext, RevitContext>();

            services.AddSingleton<FamiliesViewModel>();
            services.AddSingleton<FamiliesView>();
            new DetailDoorModule().RegisterServices(services);
            new DebugModule().RegisterServices(services);

            return services;
        }
    }
}
