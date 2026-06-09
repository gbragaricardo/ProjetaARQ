using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Core.DI
{
    public interface ICommandModule
    {
        void RegisterServices(IServiceCollection services);
    }   
}
