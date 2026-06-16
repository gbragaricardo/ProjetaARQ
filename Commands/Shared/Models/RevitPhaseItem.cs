using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.Shared.Models
{
    public record RevitPhaseItem(string Name, ElementId Id)
    {
        public override string ToString() => Name;
    }
}
