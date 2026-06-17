using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Commands.DetailDoor.Services;
using ProjetaARQ.Commands.DetailDoor.ViewModels;
using ProjetaARQ.Commands.DetailDoor.Views;
using ProjetaARQ.Commands.Shared.Services;
using ProjetaARQ.Core.DI;
using ProjetaARQ.Core.UI;

namespace ProjetaARQ.Commands.DetailDoor
{
    public class DetailDoorModule : ICommandModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<DetailDoorViewModel>();
            services.AddScoped<DetailDoorView>();
            services.AddScoped<DetailDoorHandler>();

            services.AddTransient<IUIService<IDetailDoorResult>, DetailDoorUIService>();
            services.AddTransient<IViewTemplateService, ViewTemplateService>();
            services.AddTransient<IDoorService, DoorService>();
        }
    }
}
