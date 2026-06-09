using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjetaARQ.Core.DI;
using ProjetaARQ.Core.Services;
using Result = ProjetaARQ.Core.Results.Result;
namespace ProjetaARQ.Commands.DetailDoor
{
    public class DetailDoorHandler : ICommandHandler
    {
        private readonly IAppTelemetry _telemetry;
        public DetailDoorHandler(IAppTelemetry telemetry)
        {
             _telemetry = telemetry;
        }
        public Result Handle(ExternalCommandData commandData, ElementSet elements)
        {
            _telemetry.LogInfo("Iniciando exportação do Word...");

            // Se algo der errado, você não usa exception, usa o Result
            if (commandData.Application.ActiveUIDocument == null)
                return Result.Failure("Nenhum projeto aberto.");

            TaskDialog.Show("TESTE", "TESTADO");
                
            // Abre a janela...
            return Result.Success();
        }
    }
}
