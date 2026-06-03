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
    internal class MoveRuleCommand : IUndoableCommand
    {
        ObservableCollection<RuleCardViewModel> _rulesList;
        int _oldIndex;
        int _newIndex;

        public MoveRuleCommand(ObservableCollection<RuleCardViewModel> rulesList, int oldIndex, int newIndex)
        {
            _rulesList = rulesList;
            _oldIndex = oldIndex;
            _newIndex = newIndex;
        }

        public void Execute() => _rulesList.Move(_oldIndex, _newIndex);

        public void Unexecute() => _rulesList.Move(_newIndex, _oldIndex);
    }
}
