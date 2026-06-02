using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.WordExport.Models
{
    internal class PresetModel
    {
        public string PresetName { get; set; }
        public string WordTemplatePath { get; set; }
        public List<RuleCardModel> RuleCards { get; set; } = new List<RuleCardModel>();
    }
}
