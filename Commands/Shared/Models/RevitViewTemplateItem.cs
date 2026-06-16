using Autodesk.Revit.DB;

namespace ProjetaARQ.Commands.Shared.Models
{
    public record RevitViewTemplateItem(string Name, ElementId Id)
    {
        public override string ToString() => Name;
    }
}
