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
    internal class DeleteRuleCommand : IUndoableCommand
    {
        ObservableCollection<RuleCardViewModel> _rulesList;
        RuleCardViewModel _ruleToDelete;
        int _oldIndex;

        public DeleteRuleCommand(ObservableCollection<RuleCardViewModel> rulesList, RuleCardViewModel ruleToDelete)
        {
            _rulesList = rulesList;
            _ruleToDelete = ruleToDelete;
            _oldIndex = _rulesList.IndexOf(ruleToDelete); // Para garantir a inserção no local antigo
        }

        public void Execute() => _rulesList.Remove(_ruleToDelete);

        public void Unexecute() => _rulesList.Insert(_oldIndex,_ruleToDelete);
    }
}
