using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjetaARQ.Revit.Base;
using ProjetaARQ.Revit.UI;
using System;

namespace ProjetaARQ.Commands.FamiliesPane.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class FamiliesPaneButton : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
       {
            // 1. Criar o RevitContext
            RevitContext context = new RevitContext(commandData.Application);
            
            DockablePaneId paneId = DockablePaneManager.FamiliesPaneId;
            DockablePane pane = context.UiApp.GetDockablePane(paneId);

            if (pane != null)
            {
                if (pane.IsShown())
                    pane.Hide();
                else
                    pane.Show();
            }

            return Result.Succeeded;
        }
    }
}
