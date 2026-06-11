using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Core.DI;
using ProjetaARQ.Core.Services;

namespace ProjetaARQ.Core.Commands
{
    public static class CommandRunner
    {
        public static Result Run<THandler>(ExternalCommandData commandData, ref string message, ElementSet elements) where THandler : ICommandHandler
        {
            using (var scope = AddinApplication.Provider.CreateScope())
            {
                var revitContext = scope.ServiceProvider.GetRequiredService<IRevitContext>() as RevitContext;
                revitContext?.Initialize(commandData);

                var telemetry = scope.ServiceProvider.GetRequiredService<IAppTelemetry>();
                string commandName = typeof(THandler).Name;

                using (telemetry.TrackActivity($"Execute_{commandName}"))
                {
                    try
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<THandler>();
                        var appResult = handler.Handle(commandData, elements);

                        if (appResult.IsFailure)
                        {
                            message = appResult.Error;
                            return Result.Failed;
                        }

                        return Result.Succeeded;
                    }
                    catch (Exception ex)
                    {
                        telemetry.LogError(ex, $"Falha fatal em {commandName}");
                        message = $"Erro crítico: {ex.Message}";
                        return Result.Failed;
                    }
                }
            }
        }
    }
}