using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Core.DI;

namespace ProjetaARQ.Commands.DebugFluentUi
{
    public class DebugModule : ICommandModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<DebugViewModel>();
            services.AddScoped<DebugView>();
            services.AddScoped<DebugHandler>();

        }
    }
}
