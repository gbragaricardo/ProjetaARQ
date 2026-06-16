using Autodesk.Revit.DB;
using ProjetaARQ.Commands.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetaARQ.Commands.Shared.Services
{
    public class ViewTemplateService : IViewTemplateService
    {
        public IEnumerable<RevitViewTemplateItem> GetTemplates(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate)
                .Select(v => new RevitViewTemplateItem(v.Name, v.Id))
                .OrderBy(t => t.Name)
                .ToList();
        }
    }
}
