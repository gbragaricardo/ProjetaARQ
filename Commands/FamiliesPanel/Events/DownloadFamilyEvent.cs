using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Data;
using System.Linq;
using Autodesk.Revit.Attributes;
using ProjetaARQ.Commands.FamiliesPanel.Services;
using ProjetaARQ.Services.Revit;

namespace ProjetaARQ.Commands.FamiliesPanel.Events
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class DownloadFamilyEvent : IExternalEventHandler
    {
        public string FamilyPath { get; set; }
        public string FamilyName { get; set; }

        public DownloadFamilyEvent()
        {
            
        }

        public string GetName() => "Download de familia";

        public void Execute(UIApplication app)
        {
            RevitContext context = new RevitContext(app);
            Family family = null;

            try
            {

                using (Transaction transaction = new Transaction(context.Doc, "Minha Transação"))
                {
                    transaction.Start();
                    var overWriteOption = new OverLoadOption();

                    // Tenta carregar a família
                    if (!context.Doc.LoadFamily(FamilyPath, overWriteOption,out family))
                    {
                        family = FindElementByName(context.Doc, typeof(Family), FamilyName) as Family;
                        if (family == null)
                        {
                            TaskDialog.Show("Aviso", "Não foi possível carregar a família.");
                            return;
                        }
                    }
                    context.Doc.Regenerate();
                        
                    FamilySymbol familySymbol = family.GetFamilySymbolIds()
                                                    .Select(id => context.Doc.GetElement(id) as FamilySymbol)
                                                    .FirstOrDefault();

                    
                        // Ativa o símbolo se necessário
                        if (!familySymbol.IsActive)
                        {
                            familySymbol.Activate();
                            context.Doc.Regenerate();
                        }

                        transaction.Commit();

                        try
                        {
                            context.UiDoc.PromptForFamilyInstancePlacement(familySymbol);
                        }
                        catch (Autodesk.Revit.Exceptions.OperationCanceledException ex)
                        {
                            if (ex.Message == "The user aborted the pick operation.")
                                return;
                        }
                    
                }               
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new NotImplementedException();
            }
        }

        public static Element FindElementByName(Document doc, Type targetType, string targetName)
        {
            return new FilteredElementCollector(doc).OfClass(targetType).FirstOrDefault(e => e.Name.Equals(targetName));
        }


        public void SetFamilyPath(string familyName, string familyPath)
        {
            FamilyPath = familyPath;
            FamilyName = familyName;
        }

    }
}