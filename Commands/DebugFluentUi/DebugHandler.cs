using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjetaARQ.Commands.DetailDoor.Services;
using ProjetaARQ.Core.DI;
using ProjetaARQ.Core.Services;
using ProjetaARQ.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Result = ProjetaARQ.Core.Results.Result;
namespace ProjetaARQ.Commands.DebugFluentUi
{
    public class DebugHandler : ICommandHandler
    {
        private readonly IAppTelemetry _telemetry;
        private readonly IRevitContext _revitContext;

        public DebugHandler(IAppTelemetry telemetry, IRevitContext revitContext)
        {
            _telemetry = telemetry;
            _revitContext = revitContext;
        }
        public Result Handle(ExternalCommandData commandData, ElementSet elements)
        {
            var view = new DebugView();

            view.ShowDialog();
            return Result.Success();
        }
    }
}
