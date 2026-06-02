using ProjetaARQ.Commands.WordExport.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Commands.WordExport.Services.UndoableCommands
{
    internal class ChangePropertyCommand<T> : IUndoableCommand
    {
        private readonly Action<T> _setter; // Uma referência ao "setter" da propriedade
        private readonly Action _propertyChangedNotifier; // Ação para notificar a UI
        private readonly T _oldValue;       // O valor antigo que guardamos
        private readonly T _newValue;       // O novo valor

        public ChangePropertyCommand(Action<T> setter, Action propertyChangedNotifier, T oldValue, T newValue)
        {
            _setter = setter;
            _propertyChangedNotifier = propertyChangedNotifier;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        // Executar = aplicar o novo valor
        public void Execute()
        {
            _setter(_newValue);
            _propertyChangedNotifier?.Invoke(); // Notifica a UI após desfazer
        }



        // Desfazer = aplicar o valor antigo
        public void Unexecute()
        {
            _setter(_oldValue);
            _propertyChangedNotifier?.Invoke(); // Notifica a UI após desfazer
        }
    }
}
