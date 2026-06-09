using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Core.DI;
using ProjetaARQ.Core.Ribbon;
using ProjetaARQ.Core.Services;
using System;
using System.IO;
using System.Reflection;

namespace ProjetaARQ.Core
{
    public class AddinApplication : IExternalApplication
    {
        public static IServiceProvider Provider { get; private set; }

        public Result OnStartup(UIControlledApplication application)
        {
            AssemblyResolver.Register(application, "ProjetaARQ");

            var services = new ServiceCollection();

            services.AddSingleton(application);
            services.AddSingleton<IRibbonManager, RibbonManager>();
            services.AddSingleton<IDockablePaneManager, DockablePaneManager>();
            services.AddSingleton<UIBuilder>();

            services.ConfigureAllModules();

            Provider = services.BuildServiceProvider();


            var uiBuilder = Provider.GetRequiredService<UIBuilder>();
            uiBuilder.Build();


            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication application)
            => Result.Succeeded;
    }
}

