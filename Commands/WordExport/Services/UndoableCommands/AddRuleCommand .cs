using ProjetaARQ.Commands.WordExport.Interfaces;
using ProjetaARQ.Commands.WordExport.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.WordExport.Services.UndoableCommands
{
    internal class AddRuleCommand : IUndoableCommand
    {
        private readonly ObservableCollection<RuleCardViewModel> _rulesList;
        private readonly RuleCardViewModel _newRule;

        public AddRuleCommand(ObservableCollection<RuleCardViewModel> rulesList, RuleCardViewModel newRule)
        {
            _rulesList = rulesList;
            _newRule = newRule;
        }
        public void Execute() => _rulesList.Add(_newRule);
        public void Unexecute() => _rulesList.Remove(_newRule);
    }
}
