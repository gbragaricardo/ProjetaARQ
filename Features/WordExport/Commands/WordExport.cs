using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ProjetaARQ.Features.WordExport.MVVM;
using ProjetaARQ.Services.Revit;

namespace ProjetaARQ.Features.WordExport.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class WordExport : IExternalCommand
    {
        internal WordViewModel ViewModel { get; private set; }
        internal WordView Window { get; private set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            
            RevitContext context = new RevitContext(commandData.Application);

            if (ViewModel == null)
                ViewModel = new WordViewModel();

            if (Window == null || Window.IsVisible == false)
            {
                Window = new WordView(ViewModel);
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

//    editor.ReplaceImage("Elab", "ProjetaARQ.Features.WordExport.Resources.diamante.png");
//    editor.ReplaceImage("consorcio", "ProjetaARQ.Features.WordExport.Resources.diamante.png");
//}


//fileHandler.OpenFile(savePath);

// 1. Criar o RevitContext