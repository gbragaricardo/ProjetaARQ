using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Commands.DetailDoor.Services;
using ProjetaARQ.Commands.DetailDoor.ViewModels;
using ProjetaARQ.Commands.DetailDoor.Views;
using ProjetaARQ.Commands.Shared.Services;
using ProjetaARQ.Core.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.DetailDoor
{
    public class DetailDoorModule : ICommandModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<DetailDoorViewModel>();
            services.AddTransient<DetailDoorView>();
            services.AddTransient<DetailDoorHandler>();
            services.AddTransient<IViewTemplateService, ViewTemplateService>();
            services.AddTransient<IDoorService, DoorService>();
        }
    }
}
