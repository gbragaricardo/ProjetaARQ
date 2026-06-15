using Microsoft.Extensions.DependencyInjection;
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
        }
    }
}
