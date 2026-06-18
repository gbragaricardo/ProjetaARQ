using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Microsoft.Extensions.DependencyInjection;
using ProjetaARQ.Core.DI;
using ProjetaARQ.Core.Services;
using ProjetaARQ.Core.Results;

namespace ProjetaARQ.Core.Commands
{
    public static class CommandRunner
    {
        public static Autodesk.Revit.UI.Result Run<THandler>(ExternalCommandData commandData, ref string message, ElementSet elements) where THandler : ICommandHandler
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

                        switch (appResult.Status)
                        {
                            case ResultStatus.Success:
                                if (!string.IsNullOrWhiteSpace(appResult.Message))
                                    TaskDialog.Show("Sucesso", appResult.Message);

                                return Autodesk.Revit.UI.Result.Succeeded;

                            case ResultStatus.Warning:
                                TaskDialog.Show("Aviso", appResult.Message);

                                return Autodesk.Revit.UI.Result.Cancelled;

                            case ResultStatus.Cancelled:
                                return Autodesk.Revit.UI.Result.Cancelled;

                            case ResultStatus.FatalError:

                            default:
                                message = appResult.Message;
                                return Autodesk.Revit.UI.Result.Failed;
                        }
                    }
                    catch (Exception ex)
                    {
                        telemetry.LogError(ex, $"Falha fatal em {commandName}");
                        message = $"Erro crítico inesperado: {ex.Message}";
                        return Autodesk.Revit.UI.Result.Failed;
                    }
                }
            }
        }
    }
}