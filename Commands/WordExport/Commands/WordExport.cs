using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjetaARQ.Commands.WordExport.MVVM.ViewModels;
using ProjetaARQ.Commands.WordExport.MVVM.Views;
using ProjetaARQ.Services.Revit;
using System;
using System.Windows.Interop;

namespace ProjetaARQ.Commands.WordExport.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class WordExport : IExternalCommand
    {
        internal WordConfigViewModel ViewModel { get; private set; }
        internal WordConfigView Window { get; private set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            
            RevitContext context = new RevitContext(commandData.Application);

            IntPtr revitHandle = context.UiApp.MainWindowHandle;

            if (ViewModel == null)
                ViewModel = new WordConfigViewModel();

            if (Window == null || Window.IsVisible == false)
            {
                Window = new WordConfigView(ViewModel);
                WindowInteropHelper helper = new WindowInteropHelper(Window) { Owner = revitHandle };
                Window.Show();
            }
            else
            {
                Window.Focus();
            }

            return Result.Succeeded;
        }
    }
}




//FileHandler fileHandler = new FileHandler();

//string savePath = fileHandler.GetSavePath();
//fileHandler.CreateNewFile(savePath);

//using (WordEditor editor = new WordEditor(savePath))
//{
//    editor.ReplaceTextInContentControl("Nome Do Projeto", "ESCOLA CEMPA");
//    editor.ReplaceTextInContentControl("Projetista", "Ricardo Braga");
//    editor.ReplaceTextInContentControl("Lista De Desenhos", "TITULO TESTE");
//    editor.ReplaceTextInContentControl("TITULO DESENHO", "PLANTA BAIXA TESTE");
//    editor.ReplaceTextInContentControl("MES ANO", "JUNHO/2025");
//    editor.ReplaceTextInContentControl("Nome Obra", "NOME DA OBRA MODIFICADO");
//    editor.DeleteParagraphByContentControlTag("AdotadasDois");
//    editor.DeleteSectionByTag("AdotadasUm");

//    editor.ReplaceImage("Elab", "ProjetaARQ.Commands.WordExport.Resources.diamante.png");
//    editor.ReplaceImage("consorcio", "ProjetaARQ.Commands.WordExport.Resources.diamante.png");
//}


//fileHandler.OpenFile(savePath);

// 1. Criar o RevitContext