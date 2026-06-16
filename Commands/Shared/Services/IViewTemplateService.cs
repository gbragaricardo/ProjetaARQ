using Autodesk.Revit.DB;
using ProjetaARQ.Commands.Shared.Models;
using System.Collections.Generic;

namespace ProjetaARQ.Commands.Shared.Services
{
    public interface IViewTemplateService
    {
        IEnumerable<RevitViewTemplateItem> GetTemplates(Document doc);
    }
}
